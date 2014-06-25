using System.Web;
using System.Web.Http;

namespace Login.Services.Extention
{
   public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
       protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
       {
           var profile = new Profile(HttpContext.Current);
           if (profile.IsAuthenticated)
           {
               return true;
           }
           return false;
       }        
    }
}
