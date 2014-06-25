using System;
using System.Text;
using System.Web;
using Login.Business;
using Login.Business.Authorization;
using Login.Business.Cryptography;
using Login.Repository;
using Login.Repository.ModelInterfaces;

namespace Login.Services
{
    public class UserAccountService : IUserService
    {
        public UserAccountService(HttpContextBase context)
        {
            _context = context.ApplicationInstance.Context;
        }

        public UserAccountService(HttpContext context)
        {
            _context = context;
        }

        public UserAccountService()
        {
        }

        private readonly HttpContext _context ;

        private IProfileRepository _repository;

        public IProfileRepository Repository
        {
            get { return _repository ?? (_repository = new SqlProfileRepository()); }
        }

        public IUserAccount GetCurrentUser()
        {
            var curUser = new ContextUserProvider(Repository, _context).ContextUser(false) ??
                          Repository.CreateEmptyUserAccount();
            return curUser;
        }


        private AccountFacade accountFacade;

        public AccountFacade AccountFacade
        {
            get { return accountFacade ?? (accountFacade = new AccountFacade()); }
        }

        public IUserAccount GetUserByIdentity(string email)
        {
            return this.Repository.GetUsersBy(email);
        }

        public IUserAccount GetUserById(string userIdentity)
        {
            return this.Repository.GetUserBy(userIdentity);
        }

        public IUserAccount Login(IUserAccount user, bool rememberMe = true)
        {
            user.LoginDate = DateTime.UtcNow;
            new FormsAuthenticationService(_context).SignIn(user, rememberMe);
            var customIdentity = _context.User.Identity as CustomIdentity;
            if (customIdentity != null)
                user.SessionId = customIdentity.Id;
            Repository.UpdateUser(user);
            return user;
        }

        public IUserAccount Login(string email, string password, bool rememberMe = true)
        {
            var userAcc = Repository.GetUsersBy(email);
            if (userAcc != null)
            {
                if (AccountFacade.IsPasswordCorrect(userAcc, password))
                {
                    userAcc.LoginDate = DateTime.UtcNow;
                    new FormsAuthenticationService(_context).SignIn(userAcc, rememberMe);
                    var customIdentity = _context.User.Identity as CustomIdentity;
                    if (customIdentity != null)
                        userAcc.SessionId = customIdentity.Id;
                    Repository.UpdateUser(userAcc);
                    return userAcc;
                }
            }
            return null;
        }
        public void ClearCookiesAndSession()
        {
            new FormsAuthenticationService(_context).SignOut(this);
        }
        public bool SignOut()
        {
            if (!_context.User.Identity.IsAuthenticated) return false;
            var user = GetCurrentUser();
            ClearCookiesAndSession();
            user.SessionId = string.Empty;
            Repository.UpdateUser(user);
            return true;
        }

        public void DeleteUserFromContext(string id)
        {
            new FormsAuthenticationService(_context).DeleteUserFromContext(id);
        }

        public bool IsUserRegistered(string email, string password)
        {
            var userByEmail = Repository.GetUsersBy(email);
            if (null != userByEmail)
            {
                bool isPasswordCorrect = AccountFacade.IsPasswordCorrect(userByEmail, password);
                if (isPasswordCorrect) return true;
            }
            return false;
        }

        public IUserAccount RegistrateUser(IRegistrationModel model)
        {
            string guid = Guid.NewGuid().ToString();

            model.Id = Guid.NewGuid().ToString();

            DateTime date = DateTime.UtcNow;

            string salt = Md5Manager.EncryptToString(string.Concat(guid, date), Encoding.UTF8);

            string hash = AccountFacade.GenerateHash(model.Password, salt);

            IUserAccount userEntity = Repository.SaveUser(date, salt, hash, model);

            if (userEntity == null)
            {
                throw new Exception("Unable  to registrate user");
            }

            return userEntity;
        }

        public void DeleteCurrentUser()
        {
            var user = GetCurrentUser();
            DeleteUserBy(user.Id);
        }
        public void DeleteCurrentUser(IUserAccount user)
        {
            DeleteUserBy(user.Id);
        }

        public void DeleteUserBy(string id)
        {
            Repository.Delete(id);
        }
       
        public void DeleteUserBy(IUserAccount userEntity)
        {
            Repository.Delete(userEntity.Id);
        }

        public IUserAccount UpdateUserPassword(IRegistrationModel regmodel)
        {
            var curUser = Repository.GetUsersBy(regmodel.Email) ;
            if (curUser != null)
            {
                DateTime date = DateTime.UtcNow;
                string guid = Guid.NewGuid().ToString();
                string salt = Md5Manager.EncryptToString(string.Concat(guid, date), Encoding.UTF8);
                string hash = AccountFacade.GenerateHash(regmodel.Password, salt);
                Repository.UpdateUser(salt, hash, curUser);
                curUser.PasswordHash = hash;
                curUser.PasswordSalt = salt;
                return curUser;
            }
            return null;
        }

        public bool ConfirmationPassword(IUserAccount user, string password)
        {
            return AccountFacade.IsPasswordCorrect(user, password);
        }

        public IUserAccount UpdateUser(IRegistrationModel model)
        {
            string guid = Guid.NewGuid().ToString();

            var curUser = GetCurrentUser();

            model.Id = curUser.Id;

            DateTime date = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (!string.IsNullOrEmpty(model.Old_Password))
                {
                    if (!ConfirmationPassword(curUser, model.Old_Password))
                    {
                        return curUser;
                    }
                }
                string salt = Md5Manager.EncryptToString(string.Concat(guid, date), Encoding.UTF8);
                string hash = AccountFacade.GenerateHash(model.Password, salt);
                Repository.UpdateUser(date, salt, hash, model);
            }
            else
            {
                Repository.UpdateUser(date, curUser.PasswordSalt, curUser.PasswordHash, model);
            }
           return Repository.GetUserBy(model.Id);
        }

        public void OneClickSignUp(IUserAccount currentUser)
        {
            currentUser.Id = Guid.NewGuid().ToString();
            currentUser.RegistrationDate = DateTime.UtcNow;
            currentUser.NickName = "Anonimous";
            currentUser.Description = "Main profile";
            currentUser.Email = Guid.NewGuid().ToString();
            Repository.SaveUser(currentUser);
            Login(currentUser);
        }

        public bool IsEmailExist(string email)
        {
            return Repository.IsEmailExist(email);
        }
    }
}
