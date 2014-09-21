using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;

namespace UserAccount.Authorization
{
    public class CustomAutentificationModule : IHttpModule
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += OnAuthenticateRequest;
        }

        private static void OnAuthenticateRequest(object sender, EventArgs e)
        {
            var httpApplication = (HttpApplication)sender;
            var context = httpApplication.Context;
            if (context.User != null && context.User.Identity.IsAuthenticated)
                return;

            var httpCookie = httpApplication.Request.Cookies["webrunes_polity"];
            if (httpCookie == null)
                return;

            try
            {
                var ticket = FormsAuthentication.Decrypt(httpCookie.Value);
                var account = Account.Deserialize(ticket.UserData);
                var customIdentity = new CustomIdentity(account, ticket.Name);
                var principal = new GenericPrincipal((IIdentity) customIdentity, customIdentity.GetRoles());
                context.User = principal;
                Thread.CurrentPrincipal = (IPrincipal) principal;
            }
            catch
            {
            }
        }
    }
}
