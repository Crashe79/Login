using System;

namespace UserAccount.Authorization
{
    [Serializable]
    public class AccountSocNt
    {
        public int Id { get; set; }
        public string UserAccountGuid { get; set; }
        public string SocialNetName { get; set; }
        public string Account { get; set; }

        [Obsolete("Only for serializations")]
        public AccountSocNt()
        {
        }

        public AccountSocNt(DAL.SocialNetModel model)
        {
            Id = model.Id;
            UserAccountGuid = model.UserAccountGuid;
            SocialNetName = model.SocialNetName;
            Account = model.Account;
        }
    }
}
