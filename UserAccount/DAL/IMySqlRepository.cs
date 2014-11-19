namespace UserAccount.DAL
{
    public interface IMySqlRepository
    {
        UserAccount CreateUserAccount(UserAccount model);
        UserAccount GetUserAccountByGuid(string accountGuid);
        UserAccount GetUserAccountModelBySocialNet(string socialNet, string account);
        UserAccount AddedSocialNet(string userAccountGuid, string socialNet, string account);
        bool DeleteUserAccount(string accountGuid);
        void UpdateLoginUser(UserAccount userAccount);
        bool IsSocialNtAccountExsist(string provider, string account);
    }
}
