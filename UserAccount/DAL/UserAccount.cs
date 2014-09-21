using System;
using System.Collections.Generic;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UserAccount.DAL
{
    [TableName("core_UserAccounts")]
    public class UserAccount
    {
        [PrimaryKey, Identity]
        public int Id { get; set; }
        [NotNull]
        public string UserAccountGuid { get; set; }
        [NotNull]
        public string FirstName { get; set; }
        [NotNull]
        public string LastName { get; set; }
        [NotNull]
        public string Nick { get; set; }
        [NotNull]
        public string Email { get; set; }
        [NotNull]
        public string PasswordSalt { get; set; }
        [NotNull]
        public string PasswordHash { get; set; }
        [NotNull]
        public string Avatar { get; set; }
        [NotNull]
        public string Description { get; set; }
        [NotNull]
        public DateTime RegistrationDate { get; set; }
        [NotNull]
        public DateTime LastUpdateDate { get; set; }
        [NotNull]
        public DateTime LoginDate { get; set; }
        [Nullable]
        public string SessionId { get; set; }
        [Nullable]
        public byte[] Session { get; set; }
        [NotNull]
        public string Culture { get; set; }
        [NotNull]
        public string Role { get; set; }

        [Association(ThisKey = "UserAccountGuid", OtherKey = "UserAccountGuid")]
        public List<SocialNetModel> SocialNetModels { get; set; }
    }
}
