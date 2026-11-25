using System;
using System.Globalization;
using System.IO;
using System.Text;

internal sealed class AndroidBuildVersion
{
    private const int MaximumVersionCode = 2100000000;
    private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

    internal readonly int Code;
    internal readonly string Name;
    internal readonly string OutputPath;

    private AndroidBuildVersion(int code, int week, int build, string directory, bool appBundle)
    {
        Code = code;
        Name = "1.0." + week.ToString(Invariant);
        OutputPath = Path.Combine(directory, Name + "." + build.ToString(Invariant) + (appBundle ? ".aab" : ".apk"));
    }

    internal static AndroidBuildVersion Reserve(string localPath, string sharedPath, int settingsCode,
        string requestedCode, int currentWeek, string outputDirectory, bool appBundle)
    {
        string ledgerPath = Path.GetFullPath(string.IsNullOrEmpty(sharedPath) ? localPath : sharedPath);
        if (!File.Exists(ledgerPath))
            throw new InvalidDataException("The configured Android build ledger is missing.");

        if ((File.GetAttributes(ledgerPath) & FileAttributes.ReparsePoint) != 0)
            throw new InvalidDataException("Use the original Android build ledger file, not a link.");

        // Keep a stable lock identity across replacement of the ledger itself.
        // The ledger handle also coordinates with existing Windows builders that use FileShare.None.
        using (var reservationLock = new FileStream(ledgerPath + ".reserve.lock", FileMode.OpenOrCreate,
            FileAccess.ReadWrite, FileShare.None))
        using (var ledger = new FileStream(ledgerPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read | FileShare.Delete))
        using (var reader = new StreamReader(ledger))
        {
            var shared = ReadLedger(reader.ReadToEnd());
            var comparison = Path.DirectorySeparatorChar == '\\' ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            var local = string.Equals(ledgerPath, Path.GetFullPath(localPath), comparison)
                ? shared : ReadLedger(File.ReadAllText(localPath));
            int known = Math.Max(settingsCode, Math.Max(shared[2], local[2]));
            if (known >= MaximumVersionCode)
                throw new InvalidDataException("Android versionCode has reached Play's limit.");
            int code = known + 1;
            if (!string.IsNullOrEmpty(requestedCode) && (!int.TryParse(requestedCode, NumberStyles.None, Invariant, out code)
                || code <= known || code > MaximumVersionCode))
                throw new InvalidDataException("SUDDEN_FORCE_VERSION_CODE must exceed the ledgers and PlayerSettings code, within Play's limit.");

            int week = Math.Max(currentWeek, Math.Max(shared[0], local[0]));
            int lastBuild = Math.Max(week == shared[0] ? shared[1] : -1, week == local[0] ? local[1] : -1);
            int build = checked(lastBuild + 1);
            var version = new AndroidBuildVersion(code, week, build, outputDirectory, appBundle);
            if (File.Exists(version.OutputPath) || Directory.Exists(version.OutputPath))
                throw new InvalidDataException("The expected Android output path already exists; build cancelled to preserve it.");
            Directory.CreateDirectory(outputDirectory);

            // Publish a complete record in the same directory, leaving the previous ledger intact on failure.
            string temp = ledgerPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
            byte[] bytes = Encoding.ASCII.GetBytes(string.Format(Invariant, "{0},{1},{2}", week, build, code));
            var file = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            try
            {
                using (file)
                {
                    file.Write(bytes, 0, bytes.Length);
                    file.Flush(true);
                }
                File.Replace(temp, ledgerPath, null);
            }
            finally { File.Delete(temp); }
            return version;
        }
    }

    private static int[] ReadLedger(string text)
    {
        string[] fields = text.Trim().Split(',');
        var values = new int[3];
        if (fields.Length != values.Length)
            throw new InvalidDataException("The build ledger must contain week,build,versionCode.");
        for (int i = 0; i < fields.Length; i++)
            if (!int.TryParse(fields[i], NumberStyles.None, Invariant, out values[i]) || values[i] < 0)
                throw new InvalidDataException("Build ledger values must be nonnegative integers.");
        if (values[2] > MaximumVersionCode)
            throw new InvalidDataException("Build ledger versionCode exceeds Play's limit.");
        return values;
    }
}