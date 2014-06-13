using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Profile.Repository.Entity
{
    [TableName("PaymentHistory")]
    public class UserAccount
    {
        [PrimaryKey, Identity]
        public string Id { get; set; }

        [NullValue]
        public DateTime RegistrationDate { get; set; }

        [NullValue]
        public DateTime LoginDate { get; set; }

        [NullValue]
        public string Email { get; set; }

        [NullValue]
        public string NickName { get; set; }

        [NullValue]
        public string PasswordSalt { get; set; }

        [NullValue]
        public string PasswordHash { get; set; }

        [NullValue]
        public string Hash { get; set; }

        [NullValue]
        public string GoogleProfile { get; set; }

        [NullValue]
        public string FacebookProfile { get; set; }

        [NullValue]
        public string TwitterProfile { get; set; }

        [NullValue]
        public string SessionId { get; set; }

        [NullValue]
        public byte[] Session { get; set; }

        [NullValue]
        public string Culture { get; set; }

        [NullValue]
        public string Avatar { get; set; }

        [NullValue]
        public string Description { get; set; }

        [NullValue]
        public string Provider { get; set; }
    }
}
