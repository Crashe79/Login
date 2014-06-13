using System;
using Profile.Repository.Entity;

namespace Profile.Business.Authorization
{
    [Serializable]
    public class Account
    {
        [Obsolete("Only for serializations")]
        public Account()
        {
        }

        public Account(UserAccount account, bool staySignedIn)
        {
            Name = account.NickName;
            Id = account.Id;
            LoginDate = account.LoginDate;
            Culture = account.Culture;
            Description = account.Description;
            Email = account.Email;
            RegistrationDate = account.RegistrationDate;
            SessionId = account.SessionId;
            GoogleProfile = account.GoogleProfile;
            TwitterProfile = account.TwitterProfile;
            Avatar = account.Avatar;
            Provider = account.Provider;
            StaySignedIn = staySignedIn;
        }

        public string Id { get; set; }

        public DateTime RegistrationDate { get; set; }

        public string Name { get; set; }

        public string GoogleProfile { get; set; }

        public string FacebookProfile { get; set; }

        public string TwitterProfile { get; set; }

        public string SessionId { get; set; }

        public string Avatar { get; set; }

        public string Provider { get; set; }

        public DateTime LoginDate { get; set; }

        public string Culture { get; set; }

        public string Email { get; set; }

        public string Description { get; set; }

        public RoleType RoleType { get; set; }

        public bool StaySignedIn { get; set; }

        public bool IsExpired()
        {
            if (LoginDate != null)
            {
                var curentDate = DateTime.UtcNow;
                var days = curentDate.Subtract(curentDate).TotalDays;
                if (days > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
