using BLToolkit.Data;

namespace Login.Repository.DataProvider
{
        public class ProfileDbManager : DbManager
        {
            public ProfileDbManager(string connectionString)
                : base(new Login.Repository.DataProvider.MySqlDataProvider.MySqlDataProvider(), connectionString)
            {
            }
        }
}
