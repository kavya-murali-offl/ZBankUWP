using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZBankManagement.Entity.DTOs
{
    [Table("KYCDocuments")]
    public class KYCDocuments
    {
        public string ID { get; set; }  

        public byte[] File {  get; set; }   

        public string FileName { get; set; }

        public string AccountID { get; set; }

        public DateTime UploadedOn { get; set; }
    }

}

