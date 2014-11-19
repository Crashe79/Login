namespace UserAccount.Authorization
{
    public class RegistrationTokenModel
    {
        public string Provider { get; set; }
        public bool Oauth2 { get; set; }
        public string Token { get; set; }
        public string Secret { get; set; }
        public string Access { get; set; }
        public int ExpiresIn { get; set; }
        public string Account { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
