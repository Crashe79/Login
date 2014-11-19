using System;
using System.Security.Principal;

namespace UserAccount.Authorization
{
    [Serializable]
    public class CustomIdentity : MarshalByRefObject, IIdentity
    {
        public Account Account { get; private set; }
        public string Id
        {
            get { return Account != null ? Account.UserAccountGuid : string.Empty; }
        }
        public string AuthenticationType
        {
            get
            {
                return "Custom";
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(Id);
            }
        }
        public string Name { get; private set; }

        public CustomIdentity(Account account, string name)
        {
            Account = account;
            Name = name;
        }

        public string[] GetRoles()
        {
            return new[] { "None" };
        }
    }
}
