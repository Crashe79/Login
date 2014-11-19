using System;
using UserAccount.Authorization;

namespace UserAccount
{
    public interface IProfile
    {
        bool IsAuthenticated { get; }
        string GetUserAccountId();
        string LoginBySocialNT(RegistrationTokenModel registrationTokenModel);
        bool LogOut(Guid userGuid);
        UserPublicProfile GetUserPublicProfile();
    }
}
