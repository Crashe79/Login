namespace Profile.Business.Cryptography
{
    public class PasswordManager
    {
        public string GenerateHash(string password, string salt)
        {
            string hashed = BCrypt.Net.BCrypt.HashPassword(password + salt, BCrypt.Net.BCrypt.GenerateSalt(12));

            return hashed;
        }
       
        public bool IsHashCorrect(string hashToVerify)
        {
            string hashedPassword = string.Empty;
            //ToDo hashed password to take from DB
            bool matches = BCrypt.Net.BCrypt.Verify(hashToVerify, hashedPassword);
            return matches;
        }

        /// <summary>
        /// Temp test method.
        /// </summary>
        public bool IsHashCorrect(string userPassword, string saltEncrypted, string hashedPassword)
        {
            bool matches = BCrypt.Net.BCrypt.Verify(userPassword + saltEncrypted, hashedPassword);
            return matches;
        }  
    }
}
