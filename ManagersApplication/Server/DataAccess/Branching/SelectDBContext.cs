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
           _conn= configuration.GetValue<string>("ConnectionStrings:OracleConnection");
        }

        private OracleConnection GetOracleConnection()
        {
           return new OracleConnection(_conn);
            
        }

        public async Task<List<Branching>> GetAllAsync()
        {
            List<Branching> list = new List<Branching>();
            using (OracleConnection conn = GetOracleConnection())
            {
                var cmdtext = "select RQID,RQST_DATE,ACTV_DESC from adf_task where rqtp_code=9 and sub_sys=1 and actv_name ='Cntd'";
                OracleCommand cmd = new OracleCommand(cmdtext, conn);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Branching()
                        {
                            RQID = await reader.GetFieldValueAsync<Int64>(0)
                        });
                    }
                }
                conn.Close();
                return list;
            }
            
        }
    }
}
