namespace Login.Business.Cryptography
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Manages SHA 256 hashing.
    /// </summary>
    public class Sha256Manager
    {
        #region Public Methods and Operators

        public static bool AreHashesEqual(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length != hash2.Length)
            {
                return false;
            }
            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool AreHashesEqual(string hash1, string hash2)
        {
            return string.Compare(hash1, hash2, true) == 0;
        }

        public static byte[] EncodeToBytes(string stringToEncode)
        {
            if (stringToEncode == null)
            {
                return null;
            }
            return SHA256.Create().ComputeHash(Encoding.Default.GetBytes(stringToEncode));
        }

        public static string EncodeToString(string stringToEncode)
        {
            if (stringToEncode == null)
            {
                return null;
            }

            byte[] encoded = EncodeToBytes(stringToEncode);
            var sb = new StringBuilder();
            for (int i = 0; i < encoded.Length; i++)
            {
                sb.Append(encoded[i].ToString("X2"));
            }
            return sb.ToString();
        }

        #endregion
    }
}