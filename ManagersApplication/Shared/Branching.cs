using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagersApplication.Shared
{
    public class Branching
    {
        [Key]
        public int ID { get; set; }
        public Int64 RQID { get; set; }
        public DateTime UPDT_DATE { get; set; }
        public string ACTV_DESC { get; set; }
    }
}
