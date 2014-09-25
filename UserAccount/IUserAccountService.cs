using UserAccount.Authorization;

namespace UserAccount
{
    public interface IUserAccountService
    {
        DAL.UserAccount GetUserAccountBySocialNet(string socialNet, string account);
        DAL.UserAccount AddedSocialNet(string userAccountGuid, string socialNet, string account);
        DAL.UserAccount GetUserAccountByGuid(string userGuid);
        string SignIn(RegistrationTokenModel registrationTokenModel, string culture);
        bool SignOut();
    }
}
