using System.Web.Mvc;
using CustomOAuthClient;
using Profile.Repository;
using Profile.Repository.Entity;
using Profile.Repository.ModelInterfaces;

namespace Profile.Business.Authorization
{
    public interface IUserService
    {
        UserAccount GetCurrentUser();
        SqlProfileRepository Repository { get; }
        AccountFacade AccountFacade { get; }
        UserAccount GetUserByIdentity(string userIdentity);
        UserAccount GetUserById(string userIdentity);
        UserAccount Login(string email, string password, bool rememberMe);
        bool LoginViaOauth(OAuthProvider provider, JsonResult json, string accesToken, UserAccount userByOauthId);
        bool SignOut();
        bool IsUserRegistered(string email, string password);
        UserAccount RegistrateUser(IRegistrationModel model);
        void DeleteCurrentUser();
        void DeleteUserBy(string id);
        void DeleteUserBy(UserAccount userEntity);
    }
}