using System.Security.Cryptography;
using System.Text;

namespace UserAccount.PassManager
{
    public class Md5Manager
    {
        public static string EncryptToString(string stringToEncrypt, Encoding encoding)
        {
            if (stringToEncrypt == null)
                return (string)null;
            byte[] numArray = Md5Manager.EncryptToBytes(stringToEncrypt, encoding);
            var stringBuilder = new StringBuilder();
            for (int index = 0; index < numArray.Length; ++index)
                stringBuilder.Append(numArray[index].ToString("X2"));
            return ((object)stringBuilder).ToString();
        }

        private static byte[] EncryptToBytes(string stringToEncrypt, Encoding encoding)
        {
            if (stringToEncrypt == null)
                return (byte[])null;
            else
                return MD5.Create().ComputeHash(encoding.GetBytes(stringToEncrypt));
        }
    }
}
