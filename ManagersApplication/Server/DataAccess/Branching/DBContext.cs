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
        public static List<Branching> list_Contract = new List<Branching>();
        public static List<Branching> list_PriceAnnounce = new List<Branching>();
        public static List<Branching> list_Installment = new List<Branching>();

        public DBContext(IConfiguration configuration)
        {
            _conn = configuration.GetValue<string>("ConnectionStrings:OracleConnection");
        }

        private OracleConnection GetOracleConnection()
        {
            return new OracleConnection(_conn);

        }
        public async Task<string> LoginAsync(string username)
        {
            // Branching branching = new Branching();
            using (OracleConnection conn = GetOracleConnection())
            {
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "ADFA_MGNT_APPL.CHK_USER_EXIST";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                //in
                // cmd.Parameters.Add(new OracleParameter("P_USERNAME", OracleDbType.Varchar2, 200)).Value = username;

                cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200);
                cmd.Parameters["P_USERNAME"].Direction = ParameterDirection.Input;
                cmd.Parameters["P_USERNAME"].Value = username;

                //out
                OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.Varchar2, 200);
                result.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(result);
                cmd.BindByName = true;

                conn.Open();
                cmd.ExecuteNonQuery();
                var resultValue = cmd.Parameters[1].Value.ToString();
                return resultValue;
            }

        }

        #region Public
        public async Task<Branching_Item> GetNameAsync(string RQID)
        {
            try
            {
                List<Branching_Item> list = new List<Branching_Item>();
                Branching_Item _Item = new Branching_Item();
                using (OracleConnection conn = GetOracleConnection())
                {
                    //var cmdtext = "select Name from adm_requester where rqst_rqid = " + RQID +
                    //    " AND CODE = (SELECT MAX(CODE) FROM ADM_REQUESTER WHERE RQST_RQID = " + RQID + ")";
                    conn.Open();
                    transection = conn.BeginTransaction();
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "ADFA_MGNT_APPL.NAME_RQST_P";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    //in
                    // cmd.Parameters.Add(new OracleParameter("P_USERNAME", OracleDbType.Varchar2, 200)).Value = username;

                    cmd.Parameters.Add("P_RQID", OracleDbType.Int64, 10);
                    cmd.Parameters["P_RQID"].Direction = ParameterDirection.Input;
                    cmd.Parameters["P_RQID"].Value = Int64.Parse(RQID);

                    //out
                    OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                    result.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(result);
                    cmd.BindByName = true;


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
                    conn.Open();
                    transection = conn.BeginTransaction();
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "ADFA_MGNT_APPL.RQRO_LIST_P";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    //in
                    // cmd.Parameters.Add(new OracleParameter("P_USERNAME", OracleDbType.Varchar2, 200)).Value = username;

                    cmd.Parameters.Add("P_RQID", OracleDbType.Int64, 10);
                    cmd.Parameters["P_RQID"].Direction = ParameterDirection.Input;
                    cmd.Parameters["P_RQID"].Value = Int64.Parse(RQID);

                    //out
                    OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                    result.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(result);
                    cmd.BindByName = true;

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
        #endregion


        #region Contract
        public async Task<List<Branching>> GetAllContractAsync(string username)
        {
            try
            {
                list_Contract = new List<Branching>();
                using (OracleConnection conn = GetOracleConnection())
                {
                    int i = 0;
                    conn.Open();
                    transection = conn.BeginTransaction();

                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "ADFA_MGNT_APPL.ADML_LIST_P";
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //in
                    cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200);
                    cmd.Parameters["P_USERNAME"].Direction = ParameterDirection.Input;
                    cmd.Parameters["P_USERNAME"].Value = username;

                    cmd.Parameters.Add("P_ACTVNAME", OracleDbType.Varchar2, 10);
                    cmd.Parameters["P_ACTVNAME"].Direction = ParameterDirection.Input;
                    cmd.Parameters["P_ACTVNAME"].Value = "Cntd";

                    //out
                    OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                    result.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(result);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            list_Contract.Add(new Branching()
                            {
                                ID = ++i,
                                RQID = await reader.GetFieldValueAsync<Int64>(0),
                                UPDT_DATE = await reader.GetFieldValueAsync<string>(1),
                                ACTV_DESC = await reader.GetFieldValueAsync<string>(2),
                            });
                        }
                    }
                    conn.Close();
                }
                return list_Contract;
            }
            catch (Exception exp)
            {
                throw;
            }
        }
        public async Task<int> CountAllContractAsync(string username)
        {
            try
            {
                if (list_Contract.Count == 0)
                {
                    list_Contract = new List<Branching>();
                    using (OracleConnection conn = GetOracleConnection())
                    {
                        int i = 0;
                        conn.Open();
                        transection = conn.BeginTransaction();

                        OracleCommand cmd = new OracleCommand();
                        cmd.CommandText = "ADFA_MGNT_APPL.ADML_LIST_P";
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //in
                        cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200);
                        cmd.Parameters["P_USERNAME"].Direction = ParameterDirection.Input;
                        cmd.Parameters["P_USERNAME"].Value = username;

                        cmd.Parameters.Add("P_ACTVNAME", OracleDbType.Varchar2, 10);
                        cmd.Parameters["P_ACTVNAME"].Direction = ParameterDirection.Input;
                        cmd.Parameters["P_ACTVNAME"].Value = "Cntd";

                        //out
                        OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                        result.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(result);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                list_Contract.Add(new Branching()
                                {
                                    ID = ++i,
                                    RQID = await reader.GetFieldValueAsync<Int64>(0),
                                    UPDT_DATE = await reader.GetFieldValueAsync<string>(1),
                                    ACTV_DESC = await reader.GetFieldValueAsync<string>(2),
                                });
                            }
                        }
                        conn.Close();
                    }
                    return list_Contract.Count;
                }
                return list_Contract.Count;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<Contract_Item> GetAdmContractAsync(string RQID)
        {
            try
            {
                Contract_Item contract_Item = new Contract_Item();
                using (OracleConnection conn = GetOracleConnection())
                {
                    conn.Open();
                    transection = conn.BeginTransaction();
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "ADFA_MGNT_APPL.CONT_INFO_P";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    //in
                    // cmd.Parameters.Add(new OracleParameter("P_USERNAME", OracleDbType.Varchar2, 200)).Value = username;

                    cmd.Parameters.Add("P_RQID", OracleDbType.Int64, 10);
                    cmd.Parameters["P_RQID"].Direction = ParameterDirection.Input;
                    cmd.Parameters["P_RQID"].Value = Int64.Parse(RQID);

                    //out
                    OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                    result.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(result);
                    cmd.BindByName = true;

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
        public async Task<int> ConfirmContractAsync(string RQID)
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
        public async Task<int> RejectContractAsync(string RQID, string Desc)
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
        public async Task<List<object>> View_ImgAsync(string RQID)
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
                cmd.BindByName = true;
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
                            Image = (byte[])reader.GetValue(4)
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
        public async Task<List<string>> GetRejectReasonAsync(string RQID)
        {
            try
            {
                List<string> Reasons = new List<string>();
                using (OracleConnection conn = GetOracleConnection())
                {
                    conn.Open();
                    transection = conn.BeginTransaction();
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "ADFA_MGNT_APPL.DSGE_RESN_P";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    //in
                    cmd.Parameters.Add("P_RQID", OracleDbType.Int64, 10);
                    cmd.Parameters["P_RQID"].Direction = ParameterDirection.Input;
                    cmd.Parameters["P_RQID"].Value = Int64.Parse(RQID);

                    //out
                    OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                    result.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(result);
                    cmd.BindByName = true;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            Reasons.Add(reader.GetString(2));
                        }
                    }
                }
                return Reasons;
            }
            catch (Exception exp)
            {

                throw;
            }
        }

        #endregion

        #region PriceAnnounce
        public async Task<List<Branching>> GetAllPriceAnnounceAsync(string username)
        {
            try
            {
                if (list_PriceAnnounce.Count == 0)
                {
                    list_PriceAnnounce = new List<Branching>();
                    using (OracleConnection conn = GetOracleConnection())
                    {
                        int i = 0;
                        conn.Open();
                        transection = conn.BeginTransaction();

                        OracleCommand cmd = new OracleCommand();
                        cmd.CommandText = "ADFA_MGNT_APPL.ADML_LIST_P";
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //in
                        cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200);
                        cmd.Parameters["P_USERNAME"].Direction = ParameterDirection.Input;
                        cmd.Parameters["P_USERNAME"].Value = username;

                        cmd.Parameters.Add("P_ACTVNAME", OracleDbType.Varchar2, 10);
                        cmd.Parameters["P_ACTVNAME"].Direction = ParameterDirection.Input;
                        cmd.Parameters["P_ACTVNAME"].Value = "Annp";

                        //out
                        OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                        result.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(result);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                list_PriceAnnounce.Add(new Branching()
                                {
                                    ID = ++i,
                                    RQID = await reader.GetFieldValueAsync<Int64>(0),
                                    UPDT_DATE = await reader.GetFieldValueAsync<string>(1),
                                    ACTV_DESC = await reader.GetFieldValueAsync<string>(2),
                                });
                            }
                        }
                        conn.Close();
                    }
                    return list_PriceAnnounce;
                }
                return list_PriceAnnounce;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<int> CountAllPriceAnnounceAsync(string username)
        {
            try
            {
                if (list_PriceAnnounce.Count == 0)
                {
                    list_PriceAnnounce = new List<Branching>();
                    using (OracleConnection conn = GetOracleConnection())
                    {
                        int i = 0;
                        conn.Open();
                        transection = conn.BeginTransaction();

                        OracleCommand cmd = new OracleCommand();
                        cmd.CommandText = "ADFA_MGNT_APPL.ADML_LIST_P";
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //in
                        cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200);
                        cmd.Parameters["P_USERNAME"].Direction = ParameterDirection.Input;
                        cmd.Parameters["P_USERNAME"].Value = username;

                        cmd.Parameters.Add("P_ACTVNAME", OracleDbType.Varchar2, 10);
                        cmd.Parameters["P_ACTVNAME"].Direction = ParameterDirection.Input;
                        cmd.Parameters["P_ACTVNAME"].Value = "Annp";

                        //out
                        OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                        result.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(result);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                list_PriceAnnounce.Add(new Branching()
                                {
                                    ID = ++i,
                                    RQID = await reader.GetFieldValueAsync<Int64>(0),
                                    UPDT_DATE = await reader.GetFieldValueAsync<string>(1),
                                    ACTV_DESC = await reader.GetFieldValueAsync<string>(2),
                                });
                            }
                        }
                        conn.Close();
                    }
                    return list_PriceAnnounce.Count;
                }
                return list_PriceAnnounce.Count;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Confirm Devided Price Announce

        public async Task<List<Branching>> GetAllInstallmentAsync(string username)
        {
            try
            {
                list_Installment = new List<Branching>();
                using (OracleConnection conn = GetOracleConnection())
                {
                    int i = 0;
                    conn.Open();
                    transection = conn.BeginTransaction();

                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "ADFA_MGNT_APPL.ADML_LIST_P";
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    //in
                    cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200);
                    cmd.Parameters["P_USERNAME"].Direction = ParameterDirection.Input;
                    cmd.Parameters["P_USERNAME"].Value = username;

                    cmd.Parameters.Add("P_ACTVNAME", OracleDbType.Varchar2, 10);
                    cmd.Parameters["P_ACTVNAME"].Direction = ParameterDirection.Input;
                    cmd.Parameters["P_ACTVNAME"].Value = "Insc";

                    //out
                    OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                    result.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(result);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            list_Installment.Add(new Branching()
                            {
                                ID = ++i,
                                RQID = await reader.GetFieldValueAsync<Int64>(0),
                                UPDT_DATE = await reader.GetFieldValueAsync<string>(1),
                                ACTV_DESC = await reader.GetFieldValueAsync<string>(2),
                            });
                        }
                    }
                    conn.Close();
                }
                return list_Installment;
            }
            catch (Exception exp)
            {
                throw;
            }
        }
        public async Task<int> CountAllInstallmentAsync(string username)
        {
            try
            {
                if (list_Installment.Count == 0)
                {
                    list_Installment = new List<Branching>();
                    using (OracleConnection conn = GetOracleConnection())
                    {
                        int i = 0;
                        conn.Open();
                        transection = conn.BeginTransaction();

                        OracleCommand cmd = new OracleCommand();
                        cmd.CommandText = "ADFA_MGNT_APPL.ADML_LIST_P";
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        //in
                        cmd.Parameters.Add("P_USERNAME", OracleDbType.Varchar2, 200);
                        cmd.Parameters["P_USERNAME"].Direction = ParameterDirection.Input;
                        cmd.Parameters["P_USERNAME"].Value = username;

                        cmd.Parameters.Add("P_ACTVNAME", OracleDbType.Varchar2, 10);
                        cmd.Parameters["P_ACTVNAME"].Direction = ParameterDirection.Input;
                        cmd.Parameters["P_ACTVNAME"].Value = "Insc";

                        //out
                        OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                        result.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(result);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (await reader.ReadAsync())
                            {
                                list_Installment.Add(new Branching()
                                {
                                    ID = ++i,
                                    RQID = await reader.GetFieldValueAsync<Int64>(0),
                                    UPDT_DATE = await reader.GetFieldValueAsync<string>(1),
                                    ACTV_DESC = await reader.GetFieldValueAsync<string>(2),
                                });
                            }
                        }
                        conn.Close();
                    }
                    return list_Installment.Count;
                }
                return list_Installment.Count;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<Installment_Item>> GetAdmInstallmentAsync(string RQID)
        {
            try
            {
                List<Installment_Item> installment_Item = new List<Installment_Item>();
                using (OracleConnection conn = GetOracleConnection())
                {
                    conn.Open();
                    transection = conn.BeginTransaction();
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandText = "ADFA_MGNT_APPL.PYMT_RQST_P";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;

                    //in
                    // cmd.Parameters.Add(new OracleParameter("P_USERNAME", OracleDbType.Varchar2, 200)).Value = username;

                    cmd.Parameters.Add("P_RQID", OracleDbType.Int64, 10);
                    cmd.Parameters["P_RQID"].Direction = ParameterDirection.Input;
                    cmd.Parameters["P_RQID"].Value = Int64.Parse(RQID);

                    //out
                    OracleParameter result = new OracleParameter("P_RESULT", OracleDbType.RefCursor);
                    result.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(result);
                    cmd.BindByName = true;

                    OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (await reader.ReadAsync())
                        {
                            installment_Item.Add(new Installment_Item
                            {                                
                                EXTP_DESC = await reader.GetFieldValueAsync<string>(1),
                                INST_AMNT = await reader.GetFieldValueAsync<Int64>(4),
                                INST_PRCN = await reader.GetFieldValueAsync<Int16>(10),
                                
                            });
                        }
                    }
                }
                return installment_Item;
            }
            catch (Exception exp)
            {

                throw;
            }
        }
        #endregion
    }
}
