using Login.Repository;

namespace Login.Business.Authorization
{
    public interface IAuthenticationService
    {
        void SignIn(IUserAccount user, bool createPersistentCookie);
        bool SignOut(IUserService userService);
    }
}
