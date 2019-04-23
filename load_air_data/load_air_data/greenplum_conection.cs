using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

//To do
// [] add write log about queries running

namespace load_air_data
{
    public class greenplum_conection
    {
        public static void exec_gpdb_cmd (string str_conn, string cmd)
        {
            NpgsqlConnection my_conection = new NpgsqlConnection(str_conn);

            DataTable gpdb_dt = new DataTable();
            my_conection.Open();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd, str_conn);
            da.Fill(gpdb_dt);
            my_conection.Close();

        }

        //static void write_log(string file_log, string text)
        //{
        //    using (StreamWriter w = File.AppendText(file_log))
        //    {
        //        w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " : " + text);
        //    }
        //}
    }


}
