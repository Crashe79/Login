using System;
using System.Web;
using Login.Repository;
using Login.Repository.Entity;

namespace Login.Business.Authorization
{
    public class ContextUserProvider : IContextUserProvider
    {
        private readonly IProfileRepository repository;
        private HttpContext _context;

        public ContextUserProvider(IProfileRepository repository, HttpContext context)
        {
            this._context = context;
            this.repository = repository;
        }

        public IUserAccount ContextUser()
        {
            return ContextUser(true);
        }

        public IUserAccount ContextUser(bool shouldThrow)
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
