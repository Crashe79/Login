using System;
using System.Web;
using Profile.Repository;
using Profile.Repository.Entity;

namespace Profile.Business.Authorization
{
    public class ContextUserProvider : IContextUserProvider
    {
        private readonly SqlProfileRepository repository;
        private HttpContext _context;

        public ContextUserProvider(SqlProfileRepository repository, HttpContext context)
        {
            this._context = context;
            this.repository = repository;
        }

        public UserAccount ContextUser()
        {
            return ContextUser(true);
        }

        public UserAccount ContextUser(bool shouldThrow)
        {
            CustomIdentity identity = null;
            if(this._context.User!= null)
            {
                identity = this._context.User.Identity as CustomIdentity;
            }
            if (identity == null)
            {
                if (shouldThrow)
                    throw new NotSupportedException();
                return null;
            }
            if(string.IsNullOrEmpty(identity.Id))
            {
                return null;
            }
            return repository.GetUserBy(identity.Id);
        }
    }
}
