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
            get { return userService ?? (userService = new UserAccountService(context, conn)); }
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

        public UserPublicProfile GetUserPublicProfile()
        {
            if (context.User != null && context.User.Identity != null)
            {
                var customIdentity = context.User.Identity as CustomIdentity;
                if (customIdentity != null)
                {
                    return new UserPublicProfile
                        {
                            Id = customIdentity.Account.UserAccountGuid,
                            Nick =
                                customIdentity.Account.Nick.Equals("Unknown")
                                    ? customIdentity.Account.FirstName + " " + customIdentity.Account.LastName
                                    : customIdentity.Account.Nick,
                            Avatar = customIdentity.Account.Avatar,
                            Description = customIdentity.Account.Description,
                            Email = customIdentity.Account.Email,
                            SigInProvider = customIdentity.Account.SigInProvider
                        };
                }
            }
            return null;
        }

        public string LoginBySocialNT(RegistrationTokenModel registrationTokenModel)
        {
            if (registrationTokenModel.Account == null || registrationTokenModel.Provider == null)
                return string.Empty;

            var id = UserService.SignIn(registrationTokenModel, CurrentCulture);
            return id;
        }

        public bool LogOut(Guid userGuid)
        {
            return UserService.SignOut();
        }

        public string GetUserAccountId()
        {
            var account = GetUserPublicProfile();
            return account != null ? account.Id : string.Empty;
        }
    }
}
