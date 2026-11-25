import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.nio.file.InvalidPathException;
import java.nio.file.Path;
import java.security.CodeSigner;
import java.security.GeneralSecurityException;
import java.security.MessageDigest;
import java.security.cert.Certificate;
import java.util.Enumeration;
import java.util.HashSet;
import java.util.HexFormat;
import java.util.Locale;
import java.util.Set;
import java.util.jar.JarEntry;
import java.util.jar.JarFile;

/** Read-only pre-upload check against a SHA-256 obtained independently from Play Console. */
public final class VerifyUploadBundle {
    public static void main(String[] args) {
        if (args.length != 2 || !args[1].matches("(?i)([0-9a-f]{64}|[0-9a-f]{2}(:[0-9a-f]{2}){31})")) {
            System.err.println("Usage: java VerifyUploadBundle.java candidate.aab PLAY_UPLOAD_CERT_SHA256");
            System.exit(2);
        }
        try {
            byte[] expected = HexFormat.of().parseHex(args[1].replace(":", ""));
            int count = verify(Path.of(args[0]), expected);
            System.out.println("Verified " + count + " signed bundle files against the supplied upload certificate.");
        } catch (IOException | GeneralSecurityException | SecurityException | InvalidPathException ex) {
            System.err.println("Bundle verification failed: " + ex.getMessage());
            System.exit(1);
        }
    }

    private static int verify(Path file, byte[] expected) throws IOException, GeneralSecurityException {
        MessageDigest digest = MessageDigest.getInstance("SHA-256");
        Set<String> names = new HashSet<>();
        int count = 0;
        try (JarFile jar = new JarFile(file.toFile(), true)) {
            Enumeration<JarEntry> entries = jar.entries();
            while (entries.hasMoreElements()) {
                JarEntry entry = entries.nextElement();
                String name = entry.getName();
                if (!names.add(name)) throw new IOException("Duplicate archive entry.");
                if (entry.isDirectory()) {
                    if (entry.getSize() != 0) throw new IOException("Unexpected directory payload.");
                    continue;
                }
                if (isSigningMetadata(name)) continue;
                // JarFile verifies the entry while it is read. Signers are available only at EOF.
                try (InputStream data = jar.getInputStream(entry)) {
                    data.transferTo(OutputStream.nullOutputStream());
                }
                CodeSigner[] signers = entry.getCodeSigners();
                if (signers == null || signers.length != 1) {
                    throw new GeneralSecurityException("Expected one signer on every payload file.");
                }
                Certificate leaf = signers[0].getSignerCertPath().getCertificates().get(0);
                if (!MessageDigest.isEqual(expected, digest.digest(leaf.getEncoded()))) {
                    throw new GeneralSecurityException("Upload certificate fingerprint mismatch.");
                }
                count++;
            }
            if (!names.contains("BundleConfig.pb") || !names.contains("base/manifest/AndroidManifest.xml")) {
                throw new IOException("Missing bundle configuration or base module manifest.");
            }
        }
        return count;
    }

    private static boolean isSigningMetadata(String name) {
        String upper = name.toUpperCase(Locale.ROOT);
        if (!upper.startsWith("META-INF/") || upper.indexOf('/', "META-INF/".length()) >= 0) return false;
        return upper.equals("META-INF/MANIFEST.MF") || upper.endsWith(".SF")
                || upper.endsWith(".RSA") || upper.endsWith(".DSA") || upper.endsWith(".EC");
    }
}
