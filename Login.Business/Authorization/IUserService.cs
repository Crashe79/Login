using Login.Repository;
using Login.Repository.Entity;
using Login.Repository.ModelInterfaces;

namespace Login.Business.Authorization
{
    public interface IUserService
    {
        IUserAccount GetCurrentUser();
        IProfileRepository Repository { get; }
        AccountFacade AccountFacade { get; }
        IUserAccount GetUserByIdentity(string userIdentity);
        IUserAccount GetUserById(string userIdentity);
        IUserAccount Login(string email, string password, bool rememberMe);
        bool IsUserRegistered(string email, string password);
        IUserAccount RegistrateUser(IRegistrationModel model);
        void DeleteCurrentUser();
        void DeleteUserBy(string id);
        void DeleteUserBy(IUserAccount userEntity);
    }
}