package bajitumop.clinic.services.security;

import java.security.MessageDigest;

public class Sha256HashAlgorithm {
    private final static char[] hexArray = "0123456789ABCDEF".toCharArray();
    private static Sha256HashAlgorithm instance;
    private MessageDigest md;

    private Sha256HashAlgorithm(){
        try {
            md = MessageDigest.getInstance("SHA-256");
        } catch (Exception e) {
            // ignore
        }
    }

    public static Sha256HashAlgorithm Create(){
        if (instance == null) {
            instance = new Sha256HashAlgorithm();
        }

        return instance;
    }

    public String hash(String text) {
        byte[] bytes = md.digest(text.getBytes());
        char[] hexChars = new char[bytes.length * 2];
        for ( int j = 0; j < bytes.length; j++ ) {
            int v = bytes[j] & 0xFF;
            hexChars[j * 2] = hexArray[v >>> 4];
            hexChars[j * 2 + 1] = hexArray[v & 0x0F];
        }
        return new String(hexChars);
    }
}
