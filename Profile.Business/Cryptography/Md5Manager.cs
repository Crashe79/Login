namespace Profile.Business.Cryptography
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Manages Md5 hashing.
    /// </summary>
    public class Md5Manager
    {
        #region Public Methods and Operators

        public static string EncryptToString(string stringToEncrypt, Encoding encoding)
        {
            if (stringToEncrypt == null)
            {
                return null;
            }
            byte[] encoded = EncryptToBytes(stringToEncrypt, encoding);

            var sb = new StringBuilder();
            for (int i = 0; i < encoded.Length; i++)
            {
                sb.Append(encoded[i].ToString("X2"));
            }
            return sb.ToString();
        }

        #endregion

        #region Methods

        private static byte[] EncryptToBytes(string stringToEncrypt, Encoding encoding)
        {
            if (stringToEncrypt == null)
            {
                return null;
            }
            MD5 md5 = MD5.Create();

            return md5.ComputeHash(encoding.GetBytes(stringToEncrypt));
        }

        #endregion
    }
}