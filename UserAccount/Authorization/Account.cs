using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using UserAccount.DAL;

namespace UserAccount.Authorization
{
    [Serializable]
    public class Account
    {
        public string UserAccountGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nick { get; set; }
        public string Email { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public DateTime LoginDate { get; set; }
        public string SessionId { get; set; }
        public byte[] Session { get; set; }
        public string Culture { get; set; }
        public RoleType RoleType { get; set; }
        public bool StaySignedIn { get; set; }
        public string SigInAccount { get; set; }
        public string SigInProvider { get; set; }

        [Obsolete("Only for serializations")]
        public Account()
        {
        }

        public Account(DAL.UserAccount account, bool staySignedIn)
        {
            UserAccountGuid = account.UserAccountGuid;
            FirstName = account.FirstName;
            LastName = account.LastName;
            Nick = account.Nick;
            Email = account.Email;
            Avatar = account.Avatar;
            Description = account.Description;
            RegistrationDate = account.RegistrationDate;
            LastUpdateDate = account.LastUpdateDate;
            LoginDate = account.LoginDate;
            SessionId = account.SessionId;
            Session = account.Session;
            Culture = account.Culture;
            RoleType = (RoleType) Enum.Parse(typeof(RoleType), account.Role);
            StaySignedIn = staySignedIn;
        }

        public static string Serialize(Account account)
        {
            using (var memoryStream = new MemoryStream())
            {
                new XmlSerializer(typeof(Account)).Serialize(memoryStream, account);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public static Account Deserialize(string value)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                var account = (Account)new XmlSerializer(typeof(Account)).Deserialize(memoryStream);
                return account;
            }
        }
    }
}
