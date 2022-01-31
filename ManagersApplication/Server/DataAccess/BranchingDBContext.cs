using ManagersApplication.Shared;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;

namespace ManagersApplication.Server.DataAccess
{
    public class BranchingDBContext
    {
        public readonly IConfiguration _config;
       

        public BranchingDBContext(IConfiguration configuration)
        {
            _config = configuration;
            _config.GetValue<string>("ConnectionStrings:OracleConnection");

        }

        private OracleConnection GetOracleConnection()
        {
            return new OracleConnection(_config.ToString());
        }

        public async Task<List<Branching>> GetAllAsync()
        {
            List<Branching> list = new List<Branching>();
            using (OracleConnection conn = GetOracleConnection())
            {
                var cmdtext = "select RQID,RQST_DATE,ACTV_DESC from adf_task where rqtp_code=9 and sub_sys=1 and actv_name ='Cntd'";
                OracleCommand cmd = new OracleCommand(cmdtext, conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Branching()
                        {
                            RQID = await reader.GetFieldValueAsync<string>(0)
                        });
                    }
                }
                return list;
            }
            
        }
    }
}
