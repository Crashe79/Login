using System;
using Login.Repository.ModelInterfaces;

namespace Login.Repository
{
    public interface IProfileRepository
    {
        IUserAccount GetUserBy(string id);
        IUserAccount GetUsersBy(string email);
        IUserAccount GetUserId(string userIdentity);
        void UpdateUser(IUserAccount user);
        void UpdateUser(DateTime date, string salt, string hash, IRegistrationModel model);
        void UpdateUser(string salt,string hash,IUserAccount curUser);
        void SaveUser(IUserAccount user);
        IUserAccount SaveUser(DateTime date, string salt, string hash, IRegistrationModel model, string facebookId = null, string googleId = null, string twitterId = null);
        void Delete(string id);
        bool IsEmailExist(string email);

        IUserAccount CreateEmptyUserAccount();
    }
}