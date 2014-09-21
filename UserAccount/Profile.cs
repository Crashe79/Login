using System;
using System.Threading;
using System.Web;
using UserAccount.Authorization;

namespace UserAccount
{
    public class Profile : IProfile
    {
        private HttpContext context;
        private readonly string conn;

        public Profile(HttpContext context, string conn)
        {
            this.context = context;
            this.conn = conn;
        }

        private IUserAccountService userService;
        public IUserAccountService UserService
        {
            get
            {
                if (userService == null)
                {
                    userService = new UserAccountService(context, conn);
                }
                return userService;
            }
        }

        private string currentCulture;
        public string CurrentCulture
        {
            get
            {
                if (string.IsNullOrEmpty(currentCulture))
                {
                    currentCulture = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
                }
                return currentCulture;
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return context.User != null && context.User.Identity.IsAuthenticated;
            }
        }

        private Account account;
        public Account Account
        {
            get
            {
                if (account == null)
                {
                    var customIdentity = context.User.Identity as CustomIdentity;
                    account = customIdentity != null ? customIdentity.Account : null;
                }
                return account;
            }
        }

        public string GetUserAccountGuid()
        {
            return Account != null ? Account.UserAccountGuid : "";
        }

        public UserPublicProfile GetUserPublicProfile()
        {
            return Account != null
                       ? new UserPublicProfile
                           {
                               Id = Account.UserAccountGuid,
                               Nick = Account.Nick.Equals("Unknown") ? Account.FirstName + " " + Account.LastName : Account.Nick,
                               Avatar = Account.Avatar,
                               Description = Account.Description,
                               Email = Account.Email
                           }
                       : null;
        }

        public string LoginBySocialNT(RegistrationTokenModel registrationTokenModel)
        {
            if (registrationTokenModel.Account == null || registrationTokenModel.Provider == null)
                return string.Empty;

            if (!UserService.IsSocialNtAccountExsist(registrationTokenModel.Provider, registrationTokenModel.Account))
                return UserService.RegistrationUserBySocialNT(registrationTokenModel, CurrentCulture);

            var accaunt = UserService.LoginBySocialNt(registrationTokenModel.Provider, registrationTokenModel.Account);
            return accaunt != null ? accaunt.UserAccountGuid : string.Empty;
        }

        public bool LogOut(Guid userGuid)
        {
            return UserService.SignOut();
        }
    }
}
