using Profile.Repository.Entity;

namespace Profile.Business.Authorization
{
    public interface IAuthenticationService
    {
        void SignIn(UserAccount user, bool createPersistentCookie);
        bool SignOut(IUserService userService);
    }
}
