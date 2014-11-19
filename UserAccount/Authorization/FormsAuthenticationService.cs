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

        public bool SignIn(DAL.UserAccount user, string provider, string socNt, bool createPersistentCookie)
        {
            var account = new Account(user, createPersistentCookie) {SigInProvider = provider, SigInAccount = socNt};
            var ticket = new FormsAuthenticationTicket(
                1,
                user.UserAccountGuid,
                DateTime.Now,
                DateTime.Now.AddYears(5),
                createPersistentCookie,
                Account.Serialize(account));
            return UpdateContextResponse(ticket, account);
        }

        public bool SignOut(UserAccountService userAccountService)
        {
            if (!context.User.Identity.IsAuthenticated)
                return false;
            HttpCookie userCookie = context.Request.Cookies["webrunes_dev"];

            FormsAuthenticationTicket ticke = FormsAuthentication.Decrypt(userCookie.Value);
            var ticket = new FormsAuthenticationTicket(
                1,
                "",
                DateTime.Now,
                DateTime.Now.AddDays(-1.0),
                false,
                Account.Serialize(null));

            return UpdateContextResponse(ticket, null);
        }

        private bool UpdateContextResponse(FormsAuthenticationTicket authTicket, Account account)
        {
            var encript = FormsAuthentication.Encrypt(authTicket);
            var cookie = new HttpCookie("webrunes_dev", encript)
                {
                    Expires = DateTime.UtcNow.AddDays(3.0)
                };
            context.Response.SetCookie(cookie);
            var customIdentity = new CustomIdentity(account, authTicket.Name);
            var principal = new GenericPrincipal(customIdentity, customIdentity.GetRoles());
            context.User = principal;
            return true;
        }
    }
}
