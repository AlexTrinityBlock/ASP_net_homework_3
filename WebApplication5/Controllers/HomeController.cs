using WebApplication5.Models;
using MySql.Data.MySqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication5.Controllers
{
    public class HomeController : Controller
    {
        string connString = "server=127.0.0.1;port=3306;user id=root;password=root;database=mvcdb;charset=utf8;";
        MySqlConnection conn = new MySqlConnection();
        public ActionResult Index()
        {
            DateTime date = DateTime.Now;
            ViewBag.Date = date;
            Item data = new Item("A009", "BRC001", 80);
            return View(data);
        }

        public ActionResult Transcripts(string id, string name, int number)
        {

            Item data = new Item(id, name, number);
            return View(data);
        }

        [HttpPost]
        public ActionResult Transcripts(Item model)
        {
            string id = model.id;
            string name = model.name;
            int number = model.number;
            Item data = new Item(id, name, number);
            string number1 = number.ToString();
            string name1 = name.ToString();
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string sql = @"INSERT INTO item (id, name , number ) VALUES (@ID,@Name,@Number)";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("@ID", MySqlDbType.VarChar);
            cmd.Parameters["@ID"].Value = id;

            cmd.Parameters.Add("@Name", MySqlDbType.VarChar);
            cmd.Parameters["@Name"].Value = name1;

            cmd.Parameters.Add("@Number", MySqlDbType.Int32);
            cmd.Parameters["@Number"].Value = number1;

            int index = cmd.ExecuteNonQuery();
            bool success = false;
            if (index > 0)
                success = true;
            else
                success = false;
            ViewBag.Success = success;
            conn.Close();

            var jsonData = new { Status = "Success" };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        //https://localhost:44304/Home/getItemsList
        [HttpGet]
        public ActionResult getItemsList()
        {
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string sql = @"SELECT `ID`,`Name`,`Number` FROM `item`";

            DataTable dataTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            adapter.Fill(dataTable);

            string responseText = "{\"data\":[";

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                responseText += "{";
                responseText += "\"ID\":\"" + dataTable.Rows[i]["ID"].ToString() + "\",";
                responseText += "\"Name\":\"" + dataTable.Rows[i]["Name"].ToString() + "\",";
                responseText += "\"Number\":\"" + dataTable.Rows[i]["Number"].ToString() + "\"";
                if (i == (dataTable.Rows.Count - 1))
                {
                    responseText += "}";
                }
                else
                {
                    responseText += "},";
                }
            }

            responseText += "]}";

            return Content(responseText);
        }

        //搜尋結果
        //https://localhost:44304/Home/getItemsSearchList
        [HttpGet]
        public ActionResult getItemsSearchList(Search model)
        {
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            string search = model.search;
            //string sql = @"SELECT `ID`,`Name`,`Number` FROM `item` WHERE " + "ID LIKE '%" + search + "%' OR" + " Name LIKE '%" + search + "%'";
            string sql = @"SELECT `ID`,`Name`,`Number` FROM `item` WHERE ID LIKE @Search OR Name LIKE @Search";

            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("@Search", MySqlDbType.VarChar);
            cmd.Parameters["@Search"].Value = "%" + search + "%";

            DataTable dataTable = new DataTable();
            //MySqlDataAdapter adapter = new MySqlDataAdapter(sql, conn);
            MySqlDataReader sdr = cmd.ExecuteReader();


            string responseText = "{\"data\":[";

            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    responseText += "{";
                    responseText += "\"ID\":\"" + sdr["ID"].ToString() + "\",";
                    responseText += "\"Name\":\"" + sdr["Name"].ToString() + "\",";
                    responseText += "\"Number\":\"" + sdr["Number"].ToString() + "\"";

                    responseText += "},";
                }
                responseText = responseText.Substring(0, responseText.Length - 1);
            }
            responseText += "]}";

            return Content(responseText);
        }

        [HttpPost]
        public ActionResult DeleteByID(DeleteData model)
        {
            conn.ConnectionString = connString;
            if (conn.State != ConnectionState.Open)
                conn.Open();
            //string sql = @"DELETE FROM `item` WHERE ID='" + model.ID + "';";
            string sql = @"DELETE FROM `item` WHERE ID=@ID;";
            MySqlCommand cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.Add("@ID", MySqlDbType.VarChar);
            cmd.Parameters["@ID"].Value = model.ID;

            int index = cmd.ExecuteNonQuery();

            conn.Close();

            string responseText = sql;
            return Content(responseText);
        }
    }
}