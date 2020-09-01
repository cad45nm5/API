using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

using System.Net;
using System.Net.Http;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API
{
   // [Route("api/[controller]")]
    public class MacController : Controller
    {
       const string dbHost = "119.8.101.21";//資料庫位址
       const string dbUser = "samchou";//資料庫使用者帳號
       const string dbPass = "1234";//資料庫使用者密碼
       string dbName = "sms";//資料庫名稱
        string news = "HEY";
        public string GetInformation(string MSG)
        {
            string connStr = "server=" + dbHost + ";uid=" + dbUser + ";pwd=" + dbPass + ";database=" + dbName;
            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = conn.CreateCommand();
            conn.Open();
            command.CommandText = MSG;//Is MAC avalible?
            string R_MSG = Convert.ToString(command.ExecuteScalar());
            command.ExecuteNonQuery();
            conn.Close();
            dbName = "sms";
            return R_MSG;
        }
        public bool accountCheck(string user,string password)
        {
          dbName = "smsdb";
          return (GetInformation("SELECT password FROM users WHERE user_name= '" + user + "'") == password);
        }

        // GET: api/getDevice
        [Route("api/Device/status")]
        public IEnumerable<string> Get(string MAC)
        {
            string status = GetInformation("SELECT active_status FROM device WHERE mac= '" + MAC + "'");
            if (status == "Y")
            {
                return new string[] { "Device " + MAC + " is available " };
            }
            else
            {
                return new string[] { "Device " + MAC + " is unavailable " };
            }

        }
        [Route("api/Device/id")]
        public IEnumerable<string> GetId(string MAC)
        {
            string id = GetInformation("SELECT device_id FROM device WHERE mac= '" + MAC + "'");
            return new string[] { MAC + " id is " + id };
        }
        [Route("api")]
        public IEnumerable<string> AP(string MAC)
        {
            return new string[] {"API"};
        }

        [Route("api/check")]
        public IEnumerable<string> ACCcheck(string user, string password)
        {
            if(accountCheck(user, password))
            {
                return new string[] { "SUCCESS" };
            }
            else
            {
                return new string[] { "FAILED"};

            }


        }
        [Route("api/superior")]
        public IEnumerable<string> superior(string user,string password,string user_id)
        {
            if(accountCheck(user, password))
            {
                string sup = "default";

                return new string[] { sup };
            }
            else return new string[] {"Failed!"};
        }

        [Route("api/inferior")]
        public IEnumerable<string> inferior(string user, string password, string user_id)
        {
            if (accountCheck(user, password))
            {
                int len = 1;
                string []ary =new string[len];
                string inf = "";
                for (int i = 0; i < len; i++)
                {
                    inf += ary[i];
                    if (i != len - 1)
                    {
                        inf += ",";
                    }
                }

                return new string[] { inf };
            }
            else return new string[] { "Failed!" };
        }

        [Route("api/news/post")]
        public IEnumerable<string> po(string user, string password, string content)
        {
            if (accountCheck(user, password))
            {
                news = GetInformation("UPDATE NEWS SET CONTENT = '"+content+"'");

                return new string[] { "ack"};
            }
            else return new string[] { "Failed!" };
        }
        [Route("api/news/update")]
        public IEnumerable<string> up(string user, string password)
        {
            if (accountCheck(user, password))
            {
                string content = GetInformation("SELECT CONTENT FROM NEWS ");

                return new string[] { content };
            }
            else return new string[] { "Failed!" };
        }
        [Route("api/commission")]
        public IEnumerable<string> commission(string user, string password)
        {
            if (accountCheck(user, password))
            {
                string commission = "default 10000000";
                string hours = "default 1000";

                return new string[] { commission+","+hours };
            }
            else return new string[] { "Failed!" };
        }
        [Route("api/battery")]
        public IEnumerable<string> battery(string user, string password,string mac)
        {
            if (accountCheck(user, password))
            {
                string battery=GetInformation("SELECT battery FROM device WHERE mac= '" + mac + "'");

                return new string[] { battery+"%"};
            }
            else return new string[] { "Failed!" };
        }
        [Route("api/simcheck")]
        public IEnumerable<string> balance(string user, string password)
        {
            if (accountCheck(user, password))
            {
                MySqlConnection conn = new MySqlConnection("server=127.0.0.1;user=root;database=test;port=3306;password=1111;");
                conn.Open();

                //建立 DataSet
                DataSet dataSet = new DataSet();

                //使用 MySqlDataAdapter 查詢資料，並將結果存回 DataSet 當做名為 test1 的 DataTable
                string sql = "SELECT * FROM device WHERE 1";
                MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, conn);
                dataAdapter1.Fill(dataSet, "set1");

                return new string[] {"%" };
            }
            else return new string[] { "Failed!" };
        }





        // POST api/others/5/books (用 ~ 可以覆寫 RoutePrefix 設定)
        [Route("~/api/others/{other:int}/books")]
        public void Post([FromBody]string value)
        {
        }
    }
}
