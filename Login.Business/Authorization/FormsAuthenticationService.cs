using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using Login.Repository;
using Login.Repository.Entity;

namespace Login.Business.Authorization
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        private HttpContext _context;

        public FormsAuthenticationService(HttpContext context)
        {
            _context = context;
        }

        public void SignIn(IUserAccount user, bool createPersistentCookie)
        {
            var accountEntry = new AccountCollectionEntry();
            accountEntry.AddAccount(user, createPersistentCookie);
            var authTicket = new FormsAuthenticationTicket(1,
                                                          user.Id,
                                                          DateTime.Now,
                                                          DateTime.Now.AddYears(5),
                                                          createPersistentCookie,
                                                          AccountCollectionEntry.Serialize(accountEntry));
            UpdateContextResponse(authTicket, accountEntry);
        }

        private void UpdateContextResponse(FormsAuthenticationTicket authTicket, AccountCollectionEntry accountEntry)
        {
            var authCookie = new HttpCookie("webrunes", FormsAuthentication.Encrypt(authTicket))
                                 {
                                     Expires = DateTime.UtcNow.AddDays(30)
                                 };
            _context.Response.Cookies.Add(authCookie);
            var identity = new CustomIdentity(accountEntry, authTicket.Name);
            var principal = new GenericPrincipal(identity, identity.GetRoles());
            _context.User = principal;
        }

        public bool SignOut(IUserService userService)
        {
            if (!_context.User.Identity.IsAuthenticated) return false;
            HttpCookie currentUserCookie = _context.Request.Cookies["webrunes"];
            
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(currentUserCookie.Value);
            var collectionentry = AccountCollectionEntry.Deserialize(ticket.UserData);
            collectionentry.CurrentAccountId = null;

            var authTicket = new FormsAuthenticationTicket(1,
                                                          "",
                                                          DateTime.Now,
                                                          DateTime.Now.AddDays(-1),
                                                          false,
                                                          AccountCollectionEntry.Serialize(collectionentry));

            UpdateContextResponse(authTicket, collectionentry);
            return true;
        }

        public void DeleteUserFromContext(string id)
        {
            HttpCookie currentUserCookie = _context.Request.Cookies["webrunes"];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(currentUserCookie.Value);
            var collectionentry = AccountCollectionEntry.Deserialize(ticket.UserData);
            collectionentry.DeleteBy(id);
            collectionentry.CurrentAccountId = null;

            var authTicket = new FormsAuthenticationTicket(1,
                                                          "",
                                                          DateTime.Now,
                                                          DateTime.Now.AddDays(-1),
                                                          false,
                                                          AccountCollectionEntry.Serialize(collectionentry));
            UpdateContextResponse(authTicket, collectionentry);
        }
    }
}
