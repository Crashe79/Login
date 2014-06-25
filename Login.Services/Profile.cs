using System.Collections.Generic;
using System.Web;
using Login.Business.Authorization;
using Login.Repository;
using Login.Repository.Entity;

namespace Login.Services
{
    public class Profile
    {
        private HttpContext _context;
        public Profile(HttpContext context)
        {
            _context = context;
        }

        private string _currentCulture;
        public string CurrentCulture
        {
            get
            {
                if (string.IsNullOrEmpty(_currentCulture))
                {
                    _currentCulture = CurrentUser.Culture;
                    if (string.IsNullOrEmpty(_currentCulture))
                    {
                        _currentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture.IetfLanguageTag; 
                        CurrentUser.Culture = _currentCulture;
                    }
                }
                return _currentCulture;
            }
        }

        private List<Account> _accountCollection; 
        public List<Account> AccountCollection
        {
            get
            {
                if(_accountCollection == null)
                {
                    _accountCollection = GetAccountCollection();
                }
                return _accountCollection;
            }
        }

        public Account GetCurrentAccount()
        {
            var customIdentity = _context.User.Identity as CustomIdentity;
            if (customIdentity != null) return customIdentity.AccountCollectionEntry.GetCurrent();
            return new Account();
        }

        private List<Account> GetAccountCollection()
        {
            if (_context.User != null && _context.User.Identity != null)
            {
                var customIdentity = _context.User.Identity as CustomIdentity;
                if (customIdentity!=null)return customIdentity.AccountCollectionEntry.AccountCollection;
            }
            return new List<Account>();
        }

        public IUserAccount CurrentUser
        {
            get
            {
                IUserAccount contextUser =
                    new ContextUserProvider(new SqlProfileRepository(), _context).ContextUser(false);
                if (contextUser == null)
                {
                    return new UserAccount();
                }
                return contextUser;
            }
        }

        public bool VerifiedPassword(string password)
        {
           return UserService.ConfirmationPassword(CurrentUser, password);
        }

        public bool IsAuthenticated
        {
            get
            {
                if (null != _context.User)
                {
                    return _context.User.Identity.IsAuthenticated;
                }
                return false;
            }
        }

        private UserAccountService _userService;

        public UserAccountService UserService
        {
            get
            {
                return this._userService ?? (this._userService = new UserAccountService(_context));
            }
        }

        public void OneClickSignUp()
        {
            var curUser = CurrentUser;
            curUser.Culture = CurrentCulture;
            UserService.OneClickSignUp(CurrentUser);
        }

        public Account GetContextAccountBy(string email)
        {
            var accountCollection = GetAccountCollection();
            foreach (var account in accountCollection)
            {
                if(account.Email.Equals(email))
                {
                    return account;
                }
            }
            return null;
        }
    }
}