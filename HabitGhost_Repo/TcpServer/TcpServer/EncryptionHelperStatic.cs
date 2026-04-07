using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelperStatic
{
    private static readonly byte[] Key;
    private static readonly byte[] IV = new byte[16];

    static EncryptionHelperStatic()
    {
        using (SHA256 sha = SHA256.Create())
        {
            Key = sha.ComputeHash(Encoding.UTF8.GetBytes("GhostTapSecureKeyAES256"));
        }
    }

    public static string Decrypt(string encryptedText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;
            ICryptoTransform decryptor = aes.CreateDecryptor();
            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
            using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (StreamReader reader = new StreamReader(cs))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
