﻿using System;
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
        public long RQID { get; set; }
        public DateTime UPDT_DATE { get; set; }
        public string ACTV_DESC { get; set; }

    }
    public class Branching_Item
    {
        [Key]
        public int ID { get; set; }
        public string RQID { get; set; }
        public string Requster_Name { get; set; }
        public string Gnrt { get; set; }
        public string Serv_Type { get; set; }
        public Int16 Rqtt_Code { get; set; }
        public string Brnc_Type { get; set; }
        public Int16 Loct_Row_No { get; set; }
        public string Inst_Supr { get; set; }
        public Int16 Admn_Numb { get; set; }
        public Int16 Rqtp_Code { get; set; }
        public string Use_Type { get; set; }
        public Int32 Ampr { get; set; }
        public string Phas { get; set; }
        public double Powr { get; set; }
        public string Volt_Type { get; set; }       
    }
    

}

    
    
    
    
    
    
    
    
    
    
    
    
    