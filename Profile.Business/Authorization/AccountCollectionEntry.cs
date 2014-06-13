using System;
using System.Collections.Generic;
using System.IO;
using Profile.Repository.Entity;
using System.Text;
using System.Xml.Serialization;

namespace Profile.Business.Authorization
{
    [Serializable]
    public class AccountCollectionEntry
    {
        [Obsolete("Only for serializations")]
        public AccountCollectionEntry()
        {
        }

        public RoleType RoleType { get; set; }
        public Account GetCurrent()
        {
            foreach (var account in AccountCollection)
            {
                if (account.Id.Equals(CurrentAccountId))
                {
                    return account;
                }
            }
            return null;
        }

        public bool DeleteBy(string id)
        {
            int index = 0;
            foreach (var account in AccountCollection)
            {
                if (account.Id.Equals(id))
                {
                    AccountCollection.RemoveAt(index);
                    this.CurrentAccountId = string.Empty;
                    return true;
                }
                index++;
            }
            return false;
        }

        public Account GetCurrent(string accountId)
        {
            foreach (var account in AccountCollection)
            {
                if (account.Id.Equals(accountId))
                {
                    return account;
                }
            }
            return null;
        }

        public string CurrentAccountId { get; set; }

        public void AddAccount(UserAccount user, bool staySignedIn)
        {
            var currentAccount = new Account(user, staySignedIn);
            CurrentAccountId = currentAccount.Id;
            AccountCollection.Add(currentAccount);
        }

        private List<Account> _accountCollection;
        public List<Account> AccountCollection
        {
            get
            {
                if(null == _accountCollection)
                {
                    _accountCollection = new List<Account>();
                }
                return _accountCollection;
            }
            set { _accountCollection = value; }
        }

        public static string Serialize(AccountCollectionEntry accountColle)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new XmlSerializer(typeof(AccountCollectionEntry));
                formatter.Serialize(stream, accountColle);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static AccountCollectionEntry Deserialize(string value)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                var formatter = new XmlSerializer(typeof(AccountCollectionEntry));

                return (AccountCollectionEntry)formatter.Deserialize(stream);
            }
        }
    }
}
