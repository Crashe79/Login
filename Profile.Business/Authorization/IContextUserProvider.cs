using Profile.Repository.Entity;

namespace Profile.Business.Authorization
{
    public interface IContextUserProvider
    {
        UserAccount ContextUser();

        UserAccount ContextUser(bool shouldThrow);
    }
}