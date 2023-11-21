using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using TestApp.Models;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerProcessController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomerProcessController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ResponseCustProcess> GetCustomerProcessAsync()
        {
            var res = new List<CustomerProcess>();
            var response = new ResponseCustProcess();
            var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

            SqlConnection conn = new SqlConnection(connstr);
            var query = " SELECT A.AP_REGNO, B.STAGEDESC 'STAGE', A.CU_NAME, C.PROPERTYNAME 'CU_PROP' " +
                        " FROM CUSTOMER_PROCESS A " +
                        " LEFT JOIN ENUMSTAGE B ON (B.STAGE = A.STAGE) " +
                        " LEFT JOIN RFPROPERTY C ON (C.PROPERTYID = A.CU_PROP) ";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var res2 = new CustomerProcess
                {
                    ApRegno = dt.Rows[i]["AP_REGNO"].ToString(),
                    Stage = dt.Rows[i]["STAGE"].ToString(),
                    Name = dt.Rows[i]["CU_NAME"].ToString(),
                    Prop = dt.Rows[i]["CU_PROP"].ToString()
                };
                res.Add(res2);
            }
            response.Data = res;

            return response;
        }

        [HttpGet("{regno}")]
        public async Task<CustomerProcess> GetCustomerProcessByIdAsync(string regno)
        {
            var res = new CustomerProcess();
            var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

            SqlConnection conn = new SqlConnection(connstr);
            var query = " SELECT A.AP_REGNO, B.STAGEDESC 'STAGE', A.CU_NAME, C.PROPERTYNAME 'CU_PROP' " +
                        " FROM CUSTOMER_PROCESS A " +
                        " LEFT JOIN ENUMSTAGE B ON (B.STAGE = A.STAGE) " +
                        " LEFT JOIN RFPROPERTY C ON (C.PROPERTYID = A.CU_PROP) " +
                        " WHERE A.AP_REGNO = '" + regno + "' ";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            res.ApRegno = dt.Rows[0]["AP_REGNO"].ToString();
            res.Stage = dt.Rows[0]["STAGE"].ToString();
            res.Name = dt.Rows[0]["CU_NAME"].ToString();
            res.Prop = dt.Rows[0]["CU_PROP"].ToString();

            return res;
        }

        [HttpDelete("{regno}")]
        public async Task<string> DeleteCustomerProcessAsync(string regno)
        {
            try
            {

                var res = "";
                var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

                SqlConnection conn = new SqlConnection(connstr);
                var query = "DELETE FROM CUSTOMER_PROCESS WHERE AP_REGNO = '" + regno + "'";

                //var conn = new SqlConnection(connstr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                return "Sukses";
            }
            catch
            {

                return "false";
            }
        }

        [HttpPut]
        public async Task<string> UpdateCustomerProcessAsync(CustomerProcess model)
        {
            try
            {

                var res = "";
                var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

                SqlConnection conn = new SqlConnection(connstr);
                var query = "UPDATE CUSTOMER_PROCESS SET STAGE='" + model.Stage + "', CU_NAME='" + model.Name + "', CU_PROP='" + model.Prop + "' WHERE AP_REGNO = '" + model.ApRegno + "'";

                //var conn = new SqlConnection(connstr);
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
                conn.Close();

                return "Sukses";
            }
            catch
            {

                return "false";
            }
        }
    }
}
