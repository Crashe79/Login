using BLToolkit.Data.Linq;
using MySqlDataProvider;

namespace UserAccount.DAL
{
    public class MySqlDb : MySqlDbManager
    {
        public MySqlDb(string connectionString) : base(connectionString)
        {
        }

        public Table<UserAccount> UserAccounts { get { return GetTable<UserAccount>(); } }
        public Table<SocialNetModel> SocialNets { get { return GetTable<SocialNetModel>(); } }
    }
}
