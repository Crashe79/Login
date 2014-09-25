using System;
using System.Collections.Generic;
using System.Linq;
using BLToolkit.Data.Linq;

namespace UserAccount.DAL
{
    public class MySqlRepository : IMySqlRepository
    {
        private readonly string conn;

        public MySqlRepository(string conn)
        {
            this.conn = conn;
        }

        public UserAccount CreateUserAccount(UserAccount model)
        {
            using (var db = new MySqlDb(conn))
            {
                var id = db.InsertWithIdentity<UserAccount>(model);
                var idd = Convert.ToInt32(id);
                if (idd > 0)
                {
                    model.Id = idd;
                    foreach (var item in model.SocialNetModels)
                    {
                        var idc = db.InsertWithIdentity<SocialNetModel>(item);
                        item.Id = Convert.ToInt32(idc);
                    }
                    return model;
                }

                return null;
            }
        }

        public bool DeleteUserAccount(string accountGuid)
        {
            using (var db = new MySqlDb(conn))
            {
                var model = GetUserAccountByGuid(db, accountGuid);
                if (model == null)
                    return true;
                foreach (var item in model.SocialNetModels)
                {
                    db.SocialNets.Delete(d => d.Id == item.Id);
                }
                return db.UserAccounts.Delete(d => d.UserAccountGuid.Equals(model.UserAccountGuid)) == 1;
            }
        }

        public UserAccount GetUserAccountByGuid(string accountGuid)
        {
            using (var db = new MySqlDb(conn))
            {
                return db.UserAccounts.FirstOrDefault(d => d.UserAccountGuid.Equals(accountGuid));
            }
        }

        private List<SocialNetModel> GetUserSocialNets(MySqlDb db, string userAccountGuid)
        {
            return db.SocialNets.Where(d => d.UserAccountGuid.Equals(userAccountGuid)).ToList();
        }

        public UserAccount GetUserAccountModelBySocialNet(string socialNet, string account)
        {
            using (var db = new MySqlDb(conn))
            {
                var social =
                    db.SocialNets.FirstOrDefault(d => d.SocialNetName.Equals(socialNet) && d.Account.Equals(account));
                if (social != null)
                {
                    return GetUserAccountByGuid(db, social.UserAccountGuid);
                }

                return null;
            }
        }

        private UserAccount GetUserAccountByGuid(MySqlDb db, string accountGuid)
        {
            var account = db.UserAccounts.FirstOrDefault(d => d.UserAccountGuid.Equals(accountGuid));
            if (account != null)
            {
                account.SocialNetModels = GetUserSocialNets(db, account.UserAccountGuid);

                return account;
            }

            return null;
        }

        public UserAccount AddedSocialNet(string userAccountGuid, string socialNet, string account)
        {
            using (var db = new MySqlDb(conn))
            {
                var social =
                    db.SocialNets.FirstOrDefault(
                        d =>
                        d.SocialNetName.Equals(socialNet) && d.Account.Equals(account) &&
                        d.UserAccountGuid.Equals(userAccountGuid));
                if (social == null)
                {
                    db.InsertWithIdentity<SocialNetModel>(new SocialNetModel
                        {
                            UserAccountGuid = userAccountGuid,
                            SocialNetName = socialNet,
                            Account = account
                        });
                }

                return GetUserAccountByGuid(db, userAccountGuid);
            }
        }

        public void UpdateLoginUser(UserAccount userAccount)
        {
            using (var db = new MySqlDb(conn))
            {
                db.UserAccounts
                  .Where(e => e.UserAccountGuid.Equals(userAccount.UserAccountGuid))
                  .Update(e => new UserAccount()
                      {
                          LoginDate = userAccount.LoginDate,
                          SessionId = userAccount.SessionId
                      });
            }
        }

        public bool IsSocialNtAccountExsist(string provider, string account)
        {
            using (var db = new MySqlDb(conn))
            {
                return db.SocialNets.FirstOrDefault(d => d.SocialNetName.Equals(provider) && d.Account.Equals(account)) != null;
            }
        }
    }
}
