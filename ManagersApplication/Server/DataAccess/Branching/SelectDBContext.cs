using ManagersApplication.Shared;
using Oracle.ManagedDataAccess.Client;


namespace ManagersApplication.Server.DataAccess
{
    public class SelectDBContext
    {
        // public readonly IConfiguration _config;
        string _conn;

        public SelectDBContext(IConfiguration configuration)
        {
            _conn = configuration.GetValue<string>("ConnectionStrings:OracleConnection");
        }

        private OracleConnection GetOracleConnection()
        {
            return new OracleConnection(_conn);

        }

        public async Task<List<Branching>> GetAllContractAsync()
        {
            int i = 0;
            try
            {
                List<Branching> list = new List<Branching>();
                using (OracleConnection conn = GetOracleConnection())
                {
                    var cmdtext = "select RQID,to_char(UPDT_DATE,'yyyy/MM/dd'),ACTV_DESC from adf_task where rqtp_code=9 and sub_sys=1 and actv_name ='Cntd'";
                    OracleCommand cmd = new OracleCommand(cmdtext, conn);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new Branching()
                            {
                                ID = ++i,
                                RQID = await reader.GetFieldValueAsync<Int64>(0),
                                UPDT_DATE = await reader.GetFieldValueAsync<string>(1),
                                ACTV_DESC = await reader.GetFieldValueAsync<string>(2),
                            });
                        }
                    }
                    conn.Close();
                    return list;
                }
            }
            catch (Exception exp)
            {
                throw;
            }

        }


        public async Task<Branching_Item> GetNameAsync(string RQID)
        {
            try
            {
                List<Branching_Item> list = new List<Branching_Item>();
                Branching_Item _Item = new Branching_Item();
                using (OracleConnection conn = GetOracleConnection())
                {
                    var cmdtext = "select Name from adm_requester where rqst_rqid=" + RQID;
                    OracleCommand cmd = new OracleCommand(cmdtext, conn);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            _Item.Requster_Name = await reader.GetFieldValueAsync<string>(0);
                        }
                    }
                    conn.Close();
                    return _Item;
                }
            }
            catch (Exception exp)
            {
                throw;
            }

        }


        public async Task<List<Branching_Item>> GetRquestRowAsync(string RQID)
        {
            int i = 0;
            try
            {
                List<Branching_Item> list = new List<Branching_Item>();
                using (OracleConnection conn = GetOracleConnection())
                {
                    var cmdtext = "select Gnrt,Serv_Type,Rqtt_Desc,Brnc_Type,Loct_Desc,Inst_Supr,Admn_Numb,Rqtp_Desc,Use_Type," +
                        " Ampr,Phas,Powr,Volt_Type from request_row " +
                        " left join requester_type on request_row.Rqtt_Code = requester_type.CODE " +
                        " left join adm_location on request_row.Loct_Row_No = adm_location.ROW_NO " +
                        " left join request_type on request_row.RQTP_CODE = request_type.Code" +
                        " where rqst_rqid = " + RQID;

                    OracleCommand cmd = new OracleCommand(cmdtext, conn);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            list.Add(new Branching_Item()
                            {
                                ID = ++i,
                                Gnrt = await reader.GetFieldValueAsync<string>(0),
                                Serv_Type = await reader.GetFieldValueAsync<string>(1),
                                Rqtt_Desc = await reader.GetFieldValueAsync<string>(2),
                                Brnc_Type = await reader.GetFieldValueAsync<string>(3),
                                Loct_Desc = await reader.GetFieldValueAsync<string>(4),
                                Inst_Supr = await reader.GetFieldValueAsync<string>(5),
                                Admn_Numb = await reader.GetFieldValueAsync<Int16>(6),
                                Rqtp_Desc = await reader.GetFieldValueAsync<string>(7),
                                Use_Type = await reader.GetFieldValueAsync<string>(8),
                                Ampr = await reader.GetFieldValueAsync<Int32>(9),
                                Phas = await reader.GetFieldValueAsync<string>(10),
                                Powr = await reader.GetFieldValueAsync<double>(11),
                                Volt_Type = await reader.GetFieldValueAsync<string>(12),
                            });
                        }
                    }
                    conn.Close();
                    return list;
                }
            }
            catch (Exception exp)
            {
                throw;
            }

        }
        public async Task<Contract_Item> GetAdmContract(string RQID)
        {
            try
            {
                Contract_Item contract_Item = new Contract_Item();
                using (OracleConnection conn = GetOracleConnection())
                {
                    var cmdtext = "select Cont_Date,View_Date,Resp_Inst_Equp,Resp_Dlve_Powr,Comt_Aplr,Comt_Comp from adm_contract " +
                        "where rqro_rqst_rqid=" + RQID;
                    OracleCommand cmd = new OracleCommand(cmdtext, conn);
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (!reader.IsDBNull(0))
                                contract_Item.Cont_Date = reader.GetString(0);
                            if (!reader.IsDBNull(1))
                                contract_Item.View_Date = reader.GetString(1);
                            if (!reader.IsDBNull(2))
                                contract_Item.Resp_Inst_Equp = reader.GetInt16(2);
                            if (!reader.IsDBNull(3))
                                contract_Item.Resp_Dlve_Powr = reader.GetInt16(3);
                            if (!reader.IsDBNull(4))
                                contract_Item.Comt_Aplr = reader.GetString(4);
                            if (!reader.IsDBNull(5))
                                contract_Item.Comt_Comp = reader.GetString(5);
                        }
                    }
                }
                return contract_Item;
            }
            catch (Exception exp)
            {

                throw;
            }
        }
    }
}
