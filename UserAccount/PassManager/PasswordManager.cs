namespace UserAccount.PassManager
{
    internal static class PasswordManager
    {
        public static string GenerateHash(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password + salt, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        public static bool IsHashCorrect(string hashToVerify)
        {
            return BCrypt.Net.BCrypt.Verify(hashToVerify, string.Empty);
        }

        public static bool IsHashCorrect(string userPassword, string saltEncrypted, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(userPassword + saltEncrypted, hashedPassword);
        }
    }
}
