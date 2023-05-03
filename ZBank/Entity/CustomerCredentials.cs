using SQLite;

namespace ZBank.Entities
{
    [Table("CustomerCredentials")]
    public class CustomerCredentials
    {
        [PrimaryKey]
        public string ID { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }
    }
}
