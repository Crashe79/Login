namespace Profile.Business
{
    using Cryptography;
    using Repository.Entity;

    public class AccountFacade
    {
        public AccountFacade()
        {
            passwordManager = new PasswordManager();
        }

        private PasswordManager passwordManager;

        
        public bool IsPasswordCorrect(UserAccount userEntity, string enteredPassword)
        {
            string salt = userEntity.PasswordSalt;
            string hash = userEntity.PasswordHash;
            bool isCorrect = passwordManager.IsHashCorrect(enteredPassword, salt, hash);
            return isCorrect;
        }

        public string GenerateHash(string password, string salt)
        {
            string hash = passwordManager.GenerateHash(password, salt);
            return hash;
        }
    }
}