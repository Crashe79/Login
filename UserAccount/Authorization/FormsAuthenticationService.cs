using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace UserAccount.Authorization
{
    public class FormsAuthenticationService
    {
        private HttpContext context;

        public FormsAuthenticationService(HttpContext context)
        {
            this.context = context;
        }

        public void SignIn(DAL.UserAccount user, bool createPersistentCookie)
        {
            var account = new Account(user, createPersistentCookie);
            var ticket = new FormsAuthenticationTicket(
                1,
                user.UserAccountGuid,
                DateTime.Now,
                DateTime.Now.AddYears(5),
                createPersistentCookie,
                Account.Serialize(account));
            UpdateContextResponse(ticket, account);
        }

        public bool SignOut(UserAccountService userAccountService)
        {
            if (!context.User.Identity.IsAuthenticated)
                return false;
            HttpCookie userCookie = context.Request.Cookies["webrunes_polity"];

            FormsAuthenticationTicket ticke = FormsAuthentication.Decrypt(userCookie.Value);
            var account = Account.Deserialize(ticke.UserData);
            account.UserAccountGuid = string.Empty;
            var ticket = new FormsAuthenticationTicket(
                1,
                "",
                DateTime.Now,
                DateTime.Now.AddDays(-1.0),
                false,
                Account.Serialize(account));
            UpdateContextResponse(ticket, account);
            return true;
        }

        private void UpdateContextResponse(FormsAuthenticationTicket authTicket, Account account)
        {
            var encript = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie("webrunes_polity", encript)
                {
                    Expires = DateTime.UtcNow.AddDays(3)
                };
            context.Response.Cookies.Add(cookie);
            var customIdentity = new CustomIdentity(account, authTicket.Name);
            var principal = new GenericPrincipal(customIdentity, customIdentity.GetRoles());
            context.User = principal;
        }
    }
}
