using ManagersApplication.Shared;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ManagersApplication.Server.DataAccess
{
    public class DBContext
    {
        // public readonly IConfiguration _config;
        string _conn;
        OracleTransaction transection = null;

        public DBContext(IConfiguration configuration)
        {
            _conn = configuration.GetValue<string>("ConnectionStrings:OracleConnection");
        }

        private OracleConnection GetOracleConnection()
        {
            return new OracleConnection(_conn);

        }

        public async Task<List<Branching>> GetAllContractAsync(string regn_code)
        {
            int i = 0;
            string cmdtext = "";
            try
            {
                List<Branching> list = new List<Branching>();
                using (OracleConnection conn = GetOracleConnection())
                {
                    if (regn_code == "999")
                    {
                        cmdtext = "select RQID,to_char(UPDT_DATE,'yyyy/MM/dd'),ACTV_DESC from adf_task where " +
                           "rqtp_code=9 and sub_sys=1 and actv_name ='Cntd'";
                    }
                    else
                    {
                        cmdtext = "select RQID,to_char(UPDT_DATE,'yyyy/MM/dd'),ACTV_DESC from adf_task where " +
                             "rqtp_code=9 and sub_sys=1 and actv_name ='Cntd' and Regn_Code = " + regn_code;
                    }
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
      
        public async Task<int> CountAllContractAsync(string regn_code)
        {
            int count = 0;
            try
            {
                List<Branching> list = new List<Branching>();
                using (OracleConnection conn = GetOracleConnection())
                {
                    var cmdtext = "";
                    if (regn_code == "999")
                    {
                        cmdtext = "select RQID,to_char(UPDT_DATE,'yyyy/MM/dd'),ACTV_DESC from adf_task where " +
                           "rqtp_code=9 and sub_sys=1 and actv_name ='Cntd'";
                    }
                    else
                    {
                        cmdtext = "select RQID,to_char(UPDT_DATE,'yyyy/MM/dd'),ACTV_DESC from adf_task where " +
                             "rqtp_code=9 and sub_sys=1 and actv_name ='Cntd' and Regn_Code = " + regn_code;
                    }
                    OracleCommand cmd = new OracleCommand(cmdtext, conn);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();

                    // count = int.Parse(ds.Tables[0].Rows[0].ItemArray[0].ToString());
                    count = ds.Tables[0].Rows.Count;
                    return count;
                }
            }
            catch (Exception exp)
            {
                throw;
            }
        }
     
        public async Task<int> ConfirmContract(string RQID)
        {
            ////ADFA_RCPT_RQST.ACPT_CNTA_U(P_RQID NUMBER) RETURN NUMBER 
            ///

            var res = "";

            using (OracleConnection conn = GetOracleConnection())
            {
                conn.Open();
                transection = conn.BeginTransaction();
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "ADFA_RCPT_RQST.ACPT_CNTA_U";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;

                //out
                OracleParameter result = new OracleParameter("RSLT", OracleDbType.Int32, 2);
                result.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(result);
                
                //in
                cmd.Parameters.Add(new OracleParameter("P_RQID", OracleDbType.Long, 10)).Value = long.Parse(RQID);

                cmd.ExecuteNonQuery();
                res = result.Value.ToString();
               if (res == "0")
                  transection.Commit();
               else
                   transection.Rollback();


                conn.Close();
            }

            return int.Parse(res);
        }

        public async Task<int> RejectContract(string RQID,string Desc)
        {
            ////ADFA_RCPT_RQST.ACPT_CNTA_U(P_RQID NUMBER) RETURN NUMBER 
            ///

            var res = "";

            using (OracleConnection conn = GetOracleConnection())
            {
                conn.Open();
                transection = conn.BeginTransaction();
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "ADFA_RCPT_RQST.DGRE_CNTA_U";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;

                //out
                OracleParameter result = new OracleParameter("RSLT", OracleDbType.Int32, 2);
                result.Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(result);

                //in
                cmd.Parameters.Add(new OracleParameter("P_RQID", OracleDbType.Long, 10)).Value = long.Parse(RQID);
                cmd.Parameters.Add(new OracleParameter("P_DESC", OracleDbType.Varchar2, 200)).Value = Desc;

                cmd.ExecuteNonQuery();
                res = result.Value.ToString();
                if (res == "0")
                    transection.Commit();
                else
                    transection.Rollback();


                conn.Close();
            }

            return int.Parse(res);
        }

        public async Task<List<object>> View_Img(string RQID)
        {
            ////ADFA_APPS_PACK.GETL_SCAN_P(P_RQID NUMBER , P_RESULT OUT sys_refcursor) IS
            ///          
            List<object> list = new List<object>();
            using (OracleConnection conn = GetOracleConnection())
            {
                conn.Open();
                transection = conn.BeginTransaction();
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "ADFA_APPS_PACK.GETL_SCAN_P";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.BindByName=true;
                //out
                OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                result.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(result);

                //in
                cmd.Parameters.Add(new OracleParameter("P_RQID", OracleDbType.Long, 10)).Value = long.Parse(RQID);

                using (var reader = cmd.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {                       
                            list.Add(new Sas_Image_Document
                            {
                                Image_Type = reader.GetValue(2),
                                Image_Type_Desc = reader.GetValue(3),
                                Image =(byte[])reader.GetValue(4)
                            });                        
                    }
                    
                }
              
                //if (res == "0")
                //    transection.Commit();
                //else
                //    transection.Rollback();


                conn.Close();
            }

            return list;
        }

    }
}
