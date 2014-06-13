using BLToolkit.Data;

namespace Profile.Repository.DataProvider
{
        public class ProfileDbManager : DbManager
        {
            public ProfileDbManager(string connectionString)
                : base(new Profile.Repository.DataProvider.MySqlDataProvider.MySqlDataProvider(), connectionString)
            {
            }
        }
}
