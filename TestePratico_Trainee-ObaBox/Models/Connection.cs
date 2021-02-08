using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using MySql.Data.MySqlClient;
using TestePratico_Trainee_ObaBox.App_Start;


namespace TestePratico_Trainee_ObaBox.Models
{
    public abstract class Connection : ConnectionConfig
    {
        protected MySqlConnection conn;
        protected void Conectar()
        {
            conn = new MySqlConnection($"server={db_host};uid={db_user};pwd={db_pass};database={db_name};Character Set=utf8");
            conn.Open();
        }

        protected DataTable Consulta(string sql, MySqlConnection conn)
        {
            DataTable dt = new DataTable();
            new MySqlDataAdapter(sql, conn).Fill(dt);
            return dt;

        }
        protected int Comando(string sql, MySqlConnection conn) => new MySqlCommand(sql, conn).ExecuteNonQuery();
    }
}