using UserAccount.Authorization;

namespace UserAccount
{
    public interface IUserAccountService
    {
        string RegistrationUserBySocialNT(RegistrationTokenModel registrationTokenModel, string culture);
        DAL.UserAccount GetUserAccountBySocialNet(string socialNet, string account);
        DAL.UserAccount AddedSocialNet(string userAccountGuid, string socialNet, string account);
        DAL.UserAccount GetUserAccountByGuid(string userGuid);
        bool IsSocialNtAccountExsist(string provider, string account);
        DAL.UserAccount LoginBySocialNt(string socialNtName, string socialNtAccount, bool rememberMe = true);
        bool SignOut();
    }
}
