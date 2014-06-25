using System;
using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Login.Repository
{
    public interface IUserAccount
    {
        [PrimaryKey, Identity]
        string Id { get; set; }

        [NullValue]
        DateTime RegistrationDate { get; set; }

        [NullValue]
        DateTime LoginDate { get; set; }

        [NullValue]
        string Email { get; set; }

        [NullValue]
        string NickName { get; set; }

        [NullValue]
        string PasswordSalt { get; set; }

        [NullValue]
        string PasswordHash { get; set; }

        [NullValue]
        string Hash { get; set; }

        [NullValue]
        string GoogleProfile { get; set; }

        [NullValue]
        string FacebookProfile { get; set; }

        [NullValue]
        string TwitterProfile { get; set; }

        [NullValue]
        string SessionId { get; set; }

        [NullValue]
        byte[] Session { get; set; }

        [NullValue]
        string Culture { get; set; }

        [NullValue]
        string Avatar { get; set; }

        [NullValue]
        string Description { get; set; }

        [NullValue]
        string Provider { get; set; }
    }
}