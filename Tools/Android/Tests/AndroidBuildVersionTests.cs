using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// Compile together with Assets/Scripts/Editor/AndroidBuildVersion.cs. No Editor or signing key is required.
internal static class AndroidBuildVersionTests
{
    private static void Check(bool condition)
    {
        if (!condition) throw new Exception("Android version reservation assertion failed.");
    }

    private static void RefusesWithoutWriting<T>(string path, Action attempt) where T : Exception
    {
        byte[] previous = File.ReadAllBytes(path);
        try { attempt(); }
        catch (T) { Check(File.ReadAllBytes(path).SequenceEqual(previous)); return; }
        throw new Exception("Expected " + typeof(T).Name);
    }

    private static void Run(string root)
    {
        string local = Path.Combine(root, "checkout.txt");
        string shared = Path.Combine(root, "authoritative.txt");
        string output = Path.Combine(root, "builds");
        File.WriteAllText(local, "196,2,34");
        File.WriteAllText(shared, "200,22,60");

        var next = AndroidBuildVersion.Reserve(local, shared, 5, null, 200, output, true);
        Check(next.Code == 61 && next.Name == "1.0.200" && Path.GetFileName(next.OutputPath) == "1.0.200.23.aab");
        Check(File.ReadAllText(shared) == "200,23,61" && File.ReadAllText(local) == "196,2,34");

        RefusesWithoutWriting<InvalidDataException>(shared, () => AndroidBuildVersion.Reserve(local, shared, 65, "65", 200, output, true));
        var explicitCode = AndroidBuildVersion.Reserve(local, shared, 65, "70", 201, output, false);
        Check(explicitCode.Code == 70 && Path.GetFileName(explicitCode.OutputPath) == "1.0.201.0.apk");
        Check(File.ReadAllText(shared) == "201,0,70");

        File.WriteAllText(local, "202,7,85");
        var laterCheckout = AndroidBuildVersion.Reserve(local, shared, 9, null, 201, output, true);
        Check(laterCheckout.Code == 86 && File.ReadAllText(shared) == "202,8,86");
        string occupied = Path.Combine(output, "1.0.202.9.aab");
        Directory.CreateDirectory(occupied);
        RefusesWithoutWriting<InvalidDataException>(shared, () => AndroidBuildVersion.Reserve(local, shared, 9, null, 201, output, true));
        Directory.Delete(occupied);
        File.WriteAllText(occupied, "retained build");
        RefusesWithoutWriting<InvalidDataException>(shared, () => AndroidBuildVersion.Reserve(local, shared, 9, null, 201, output, true));
        Check(File.ReadAllText(occupied) == "retained build");

        var legacy = AndroidBuildVersion.Reserve(local, null, 88, null, 202, output, false);
        Check(legacy.Code == 89 && File.ReadAllText(local) == "202,8,89");
        File.WriteAllText(shared, "invalid");
        RefusesWithoutWriting<InvalidDataException>(shared, () => AndroidBuildVersion.Reserve(local, shared, 0, null, 202, output, false));
        File.WriteAllText(shared, "202,8,2100000000");
        RefusesWithoutWriting<InvalidDataException>(shared, () => AndroidBuildVersion.Reserve(local, shared, 0, null, 202, output, false));

        File.WriteAllText(shared, "202,8,89");
        bool refused = false;
        using (File.Open(shared, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
        {
            try { AndroidBuildVersion.Reserve(local, shared, 0, null, 202, output, false); }
            catch (IOException) { refused = true; }
        }
        Check(refused && File.ReadAllText(shared) == "202,8,89");

        int[] codes = new int[8];
        Parallel.For(0, codes.Length, i =>
        {
            var deadline = DateTime.UtcNow.AddSeconds(10);
            while (true)
            {
                try { codes[i] = AndroidBuildVersion.Reserve(local, shared, 0, null, 202, output, false).Code; break; }
                catch (IOException)
                {
                    if (DateTime.UtcNow >= deadline) throw;
                    Thread.Sleep(5);
                }
            }
        });
        Check(codes.OrderBy(code => code).SequenceEqual(Enumerable.Range(90, codes.Length)));
        Check(File.ReadAllText(shared) == "202,16,97" && Directory.GetFiles(root, "*.tmp").Length == 0);
    }

    public static void Main()
    {
        string temp = Path.GetFullPath(Path.GetTempPath());
        string root = Path.Combine(temp, "SuddenForce-AndroidVersion-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try { Run(root); Console.WriteLine("Android build version checks passed."); }
        finally
        {
            if (Path.GetDirectoryName(Path.GetFullPath(root)) == temp.TrimEnd(Path.DirectorySeparatorChar))
                Directory.Delete(root, true);
        }
    }
}