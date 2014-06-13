using System;
using System.Security.Principal;

namespace Profile.Business.Authorization
{
    [Serializable]
    public class CustomIdentity : MarshalByRefObject, IIdentity
    {
        public AccountCollectionEntry AccountCollectionEntry { get; private set; }

        public CustomIdentity(AccountCollectionEntry accountEntry, string name)
        {
            Name = name;
            this.AccountCollectionEntry = accountEntry;
        }

        public string Id
        {
            get { return AccountCollectionEntry.CurrentAccountId; }
        }

        public RoleType RoleType
        {
            get { return AccountCollectionEntry.RoleType; }
        }

        public string AuthenticationType
        {
            get { return "Custom"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(Id); }
        }

        public string Name { get; private set; }

        public string[] GetRoles()
        {
            return new[] { RoleType.ToString() };
        }
    }
}
