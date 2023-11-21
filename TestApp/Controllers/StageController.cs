using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using TestApp.Models;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StageController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StageController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ResponseStage> GetStageAsync()
        {
            var res = new List<Stage>();
            var response = new ResponseStage();
            var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

            SqlConnection conn = new SqlConnection(connstr);
            var query = " SELECT * FROM ENUMSTAGE ";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var res2 = new Stage
                {
                    StageId = dt.Rows[i]["STAGE"].ToString(),
                    StageDesc = dt.Rows[i]["STAGEDESC"].ToString()
                };
                res.Add(res2);
            }
            response.Data = res;

            return response;
        }

        [HttpGet("{stage}")]
        public async Task<Stage> GetStageByIdAsync(string stage)
        {
            var res = new Stage();
            var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

            SqlConnection conn = new SqlConnection(connstr);
            var query = " SELECT * FROM ENUMSTAGE " +
                        " WHERE STAGE = '" + stage + "' ";

            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            res.StageId = dt.Rows[0]["STAGE"].ToString();
            res.StageDesc = dt.Rows[0]["STAGEDESC"].ToString();

            return res;
        }

        [HttpDelete("{stage}")]
        public async Task<string> DeleteStageAsync(string stage)
        {
            try
            {

                var res = "";
                var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

                SqlConnection conn = new SqlConnection(connstr);
                var query = "DELETE FROM ENUMSTAGE WHERE STAGE = '" + stage + "'";

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
        public async Task<string> UpdateStageAsync(Stage model)
        {
            try
            {

                var res = "";
                var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

                SqlConnection conn = new SqlConnection(connstr);
                var query = "UPDATE ENUMSTAGE SET STAGEDESC='" + model.StageDesc + "' WHERE STAGE = '" + model.StageId + "'";

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

        [HttpPost]
        public async Task<string> AddStageAsync(Stage model)
        {
            try
            {

                var res = "";
                var connstr = _config.GetValue<string>("ConnectionStrings:DefaultConnection");

                SqlConnection conn = new SqlConnection(connstr);
                var query = "INSERT INTO CUSTOMER VALUES('" + model.StageId + "','" + model.StageDesc + "')";

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
