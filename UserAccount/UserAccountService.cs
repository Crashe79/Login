using System;
using System.Collections.Generic;
using System.Web;
using UserAccount.Authorization;
using UserAccount.DAL;

namespace UserAccount
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IMySqlRepository repository;
        private HttpContext context;

        public UserAccountService(HttpContext context, string conn)
        {
            repository = new MySqlRepository(conn);
            this.context = context;
        }

        public UserAccountService(HttpContextBase context, string conn)
        {
            repository = new MySqlRepository(conn);
            this.context = context.ApplicationInstance.Context;
        }

        public DAL.UserAccount RegistrationUserAccountBySocialNet(DAL.UserAccount userAccount)
        {
            return repository.CreateUserAccount(userAccount);
        }

        public DAL.UserAccount GetUserAccountBySocialNet(string socialNet, string account)
        {
            return repository.GetUserAccountModelBySocialNet(socialNet, account);
        }

        public DAL.UserAccount AddedSocialNet(string userAccountGuid, string socialNet, string account)
        {
            return repository.AddedSocialNet(userAccountGuid, socialNet, account);
        }

        public DAL.UserAccount GetUserAccountByGuid(string userGuid)
        {
            return repository.GetUserAccountByGuid(userGuid);
        }

        public DAL.UserAccount GetCurrentUser()
        {
            return repository.GetUserAccountByGuid(context.User.Identity.Name);
        }

        public bool SignOut()
        {
            if (!context.User.Identity.IsAuthenticated)
                return false;
            var currentUser = GetCurrentUser();
            ClearCookiesAndSession();
            currentUser.SessionId = string.Empty;
            repository.UpdateLoginUser(currentUser);
            return true;
        }

        private void ClearCookiesAndSession()
        {
            new FormsAuthenticationService(context).SignOut(this);
        }

        public DAL.UserAccount LoginBySocialNt(string socialNtName, string socialNtAccount, bool rememberMe = true)
        {
            var user = repository.GetUserAccountModelBySocialNet(socialNtName, socialNtAccount);
            if (user == null)
                return null;

            user.LoginDate = DateTime.UtcNow;
            new FormsAuthenticationService(context).SignIn(user, rememberMe);
            var customIdentity = context.User.Identity as CustomIdentity;
            if (customIdentity != null)
                user.SessionId = customIdentity.Id;
            repository.UpdateLoginUser(user);
            return user;
        }

        public bool IsSocialNtAccountExsist(string provider, string account)
        {
            return repository.IsSocialNtAccountExsist(provider, account);
        }

        public string RegistrationUserBySocialNT(RegistrationTokenModel registrationTokenModel, string culture)
        {
            var userAccount = new DAL.UserAccount
                {
                    UserAccountGuid = Guid.NewGuid().ToString(),
                    FirstName = registrationTokenModel.FirstName ?? string.Empty,
                    LastName = registrationTokenModel.LastName ?? string.Empty,
                    Nick = "Unknown",
                    Email = string.Empty,
                    PasswordSalt = string.Empty,
                    PasswordHash = string.Empty,
                    Avatar = string.Empty,
                    Description = string.Empty,
                    RegistrationDate = DateTime.UtcNow,
                    LastUpdateDate = DateTime.UtcNow,
                    LoginDate = DateTime.UtcNow,
                    Culture = culture,
                    Role = RoleType.None.ToString()
                };
            userAccount.SocialNetModels = new List<SocialNetModel>
                {
                    new SocialNetModel
                        {
                            UserAccountGuid = userAccount.UserAccountGuid,
                            Account = registrationTokenModel.Account,
                            SocialNetName = registrationTokenModel.Provider
                        }
                };

            var model = RegistrationUserAccountBySocialNet(userAccount);
            if (model == null)
                return string.Empty;

            if (model.Id > 0 && model.SocialNetModels[0].Id > 0)
            {
                var accaunt = LoginBySocialNt(registrationTokenModel.Provider, registrationTokenModel.Account);
                return accaunt.UserAccountGuid;
            }

            return string.Empty;
        }
    }
}
