using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace load_air_data
{
    class postgres_conection
    {

        public static void exec_procedure_cmd (string str_conn, string cmd, string file_name_log)
        {

            // Connect to a PostgreSQL database

            NpgsqlConnection conn = new NpgsqlConnection(str_conn);
            conn.Open();
            // Define a query

            NpgsqlCommand command = new NpgsqlCommand(cmd, conn);
            // Execute the query and obtain a result set

            NpgsqlDataReader dr = command.ExecuteReader();
            dr.Close();

            conn.Close();

        }

    }
}
