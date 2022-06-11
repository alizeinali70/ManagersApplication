using System.ComponentModel.DataAnnotations;

namespace ManagersApplication.Shared
{
    public class Branching
    {

        [Key]
        public int ID { get; set; }
        public long RQID { get; set; }
        public string UPDT_DATE { get; set; }
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
        //public Int16 Rqtt_Code { get; set; }
        public string Rqtt_Desc { get; set; }
        public string Brnc_Type { get; set; }
        //public Int16 Loct_Row_No { get; set; }
        public string Loct_Desc { get; set; }
        public string Inst_Supr { get; set; }
        public Int16 Admn_Numb { get; set; }
        //public Int16 Rqtp_Code { get; set; }
        public string Rqtp_Desc { get; set; }
        public string Use_Type { get; set; }
        public Int32 Ampr { get; set; }
        public string Phas { get; set; }
        public double Powr { get; set; }
        public string Volt_Type { get; set; }
        public List<string> Reject_Reason { get; set; }
    }

    public class Contract_Item
    {
        //[Key]
        //public int ID { get; set; }

        /// <summary>
        /// //Cont_Date,View_Date,Resp_Inst_Equp,Resp_Dlve_Powr,Comt_Aplr,Comt_Comp
        /// </summary>
        public string Cont_Date { get; set; }
        public string View_Date { get; set; }
        public Int16 Resp_Inst_Equp { get; set; }
        public Int16 Resp_Dlve_Powr { get; set; }
        public string Comt_Aplr { get; set; }
        public string Comt_Comp { get; set; }
    }

    public class Sas_Image_Document
    {
        public object Image_Type { get; set; }
        public object Image_Type_Desc { get; set; }
        public byte[] Image { get; set; }
    }
}













