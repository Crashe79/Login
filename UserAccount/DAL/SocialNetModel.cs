using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace UserAccount.DAL
{
    [TableName("core_SocialNets")]
    public class SocialNetModel
    {
        [PrimaryKey, Identity]
        public int Id { get; set; }
        [NotNull]
        public string UserAccountGuid { get; set; }
        [NotNull]
        public string SocialNetName { get; set; }
        [NotNull]
        public string Account { get; set; }
    }
}
