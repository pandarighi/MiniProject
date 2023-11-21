using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using TestApp.Models;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CustomerController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task <ResponseBase> GetCustomerAsync()
        {
            var res = new List<Customer>();
            var response = new ResponseBase(); 
            var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

            SqlConnection conn = new SqlConnection(connstr);
            var query = " SELECT A.AP_REGNO, A.CU_NAME, B.GENDERDESC 'CU_GENDER', A.CU_ALAMAT, A.CU_KTP, C.PROPERTYNAME 'CU_PROP', D.AREANAME 'CU_AREA' " +
                        " FROM CUSTOMER A "+
                        " LEFT JOIN RFGENDER B ON(B.GENDERCODE = A.CU_GENDER) "+
                        " LEFT JOIN RFPROPERTY C ON(C.PROPERTYID = A.CU_PROP) "+
                        " LEFT JOIN RFAREA D ON(D.AREAID = A.cu_AREA) ";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var res2 = new Customer
                {
                    ApRegno = dt.Rows[i]["AP_REGNO"].ToString(),
                    Name = dt.Rows[i]["CU_NAME"].ToString(),
                    Gender = dt.Rows[i]["CU_GENDER"].ToString(),
                    Alamat = dt.Rows[i]["CU_ALAMAT"].ToString(),
                    Ktp = dt.Rows[i]["CU_NAME"].ToString(),
                    Prop = dt.Rows[i]["CU_PROP"].ToString(),
                    Area = dt.Rows[i]["CU_AREA"].ToString()
                };
                res.Add(res2);
            }
            response.Data = res;

            return response;
        }

        [HttpGet("{regno}")]
        public async Task<Customer> GetCustomerByIdAsync(string regno)
        {
            var res = new Customer();
            var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

            SqlConnection conn = new SqlConnection(connstr);
            var query = "SELECT * FROM CUSTOMER WHERE AP_REGNO = '"+regno+"'";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            res.ApRegno = dt.Rows[0]["AP_REGNO"].ToString();
            res.Name = dt.Rows[0]["NAMA"].ToString();
            
            return res;
        }

        [HttpDelete("{regno}")]
        public async Task<string> DeleteCustomerAsync(string regno)
        {
            try
            {

                var res = "";
                var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

                SqlConnection conn = new SqlConnection(connstr);
                var query = "DELETE FROM CUSTOMER WHERE AP_REGNO = '" + regno + "'";

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
        public async Task<string> UpdateCustomerAsync(Customer model)
        {
            try
            {

                var res = "";
                var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

                SqlConnection conn = new SqlConnection(connstr);
                var query = "UPDATE CUSTOMER SET NAMA='"+model.Name+"' WHERE AP_REGNO = '" + model.ApRegno + "'";

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

        [HttpPost("{nama}")]
        public async Task<string> AddCustomerAsync(string nama)
        {
            try
            {

                var res = "";
                var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

                SqlConnection conn = new SqlConnection(connstr);
                var query = "INSERT INTO CUSTOMER VALUES('" + Guid.NewGuid().ToString() + "','" + nama + "')";

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
