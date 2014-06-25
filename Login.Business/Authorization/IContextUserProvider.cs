using Login.Repository;

namespace Login.Business.Authorization
{
    public interface IContextUserProvider
    {
        IUserAccount ContextUser();

        IUserAccount ContextUser(bool shouldThrow);
    }
}