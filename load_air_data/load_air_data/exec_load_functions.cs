using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace load_air_data
{
    public class exec_load_functions
    {

        public static void run_console_psql(string v_tp_load, string dir_file_dest, string file_log)
        {
            string pwd = "PostAdmin@123";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo = startInfo;
            startInfo.FileName = "run_load_stg_psql.bat";
            startInfo.Arguments = v_tp_load + " " + dir_file_dest + " " + pwd;
            write_log(file_log, "v_tp_load : " + v_tp_load);
            write_log(file_log, "dir_file_dest: " + dir_file_dest);
            write_log(file_log, "password: *******" );

            process.Start();
            process.WaitForExit();

            write_log(file_log, "run_load_stg_psql.bat executed sucess...");
        }

        public static int Main(string v_tp_load)
        {

            //Parametros
            string v_sub_dir_files = "C:\\data\\";

            if (v_tp_load.ToUpper() == "DEPARTURES")
            {
                v_sub_dir_files = v_sub_dir_files + v_tp_load.ToUpper();
                //v_param01 = " -d "; //DEPARTURES
            }
            else
            {
                v_sub_dir_files = v_sub_dir_files + v_tp_load.ToUpper();
                //v_param01 = " -a "; //ARRIVALS
            }

            string file_log = "C:\\data\\source\\log" + "\\log_exec_load_functions_" + v_tp_load + (DateTime.Now.ToString("yyyyMMdd")) + ".txt";
            write_log(file_log, "Start load functions, type: - " + v_tp_load + ".");

            string v_dir_processing = v_sub_dir_files + "\\PROCESSING\\";
            string v_dir_finished = v_sub_dir_files + "\\FINISHED\\";

            //LOOP DE ARQUIVOS ".dat"
            string file_name = "";
            string file_name_full = "";
            string v_ref = "";
            string v_comand = "";
            string str_conn = "";
            string Tex_Server_Postgres_IP = "13.78.133.227";
            str_conn = "Server=" + Tex_Server_Postgres_IP + ";Port=5432;UserId=postgres;Password='PostAdmin@123';CommandTimeout=0;Database=airdata;";

            //file_dest = "arrivals_20190418182148.dat";
            string[] ListFiles = Directory.GetFiles(v_dir_processing,"*.dat");
            for (int cont_arq = 0; cont_arq < ListFiles.Length; cont_arq++)
            {
                write_log(file_log, "");
                write_log(file_log, "File found: " + ListFiles[cont_arq]);

                file_name_full = ListFiles[cont_arq];

                file_name = ListFiles[cont_arq].Replace(v_dir_processing, "");
                v_ref = ListFiles[cont_arq].Replace(".dat", "").Replace(v_dir_processing,"");

                List<string> ListCmds = new List<string>();

                //EXECUTE CONSOLE PSQL (BEST PERFORMANCE I HAVE TRIED;
                run_console_psql(v_tp_load, file_name_full, file_log);

                ListCmds = list_cmd();
                for (int cont_cmd = 0; cont_cmd < ListCmds.Count; cont_cmd++)
                {
                    string fn = ListCmds[cont_cmd];
                    v_comand = "SELECT WORK_AD." + fn +"('" + v_ref + "');";
                    
                    write_log(file_log, "Execute function: " + v_comand);
                    postgres_conection.exec_procedure_cmd(str_conn, v_comand, file_log);
                }

                File.Move(v_dir_processing + file_name, v_dir_finished + file_name);
                write_log(file_log, "File : " + v_dir_processing + file_name);
                write_log(file_log, "Moved to : " + v_dir_finished + file_name);
            }

            write_log(file_log, "End load functions.");

            return 0;
            
        }

        static List<string> list_cmd()
        {
            List <string> list_cmd = new List<string>();

            //list_cmd.Add("fn_load_stg_air_data"); // LOAD STAGE
            list_cmd.Add("fn_load_wrk_air_data"); // LOAD WORK AND AGGREGATE
            list_cmd.Add("fn_load_tgt_air_data"); // LOAD TARGET

            return list_cmd;
        }

        static void write_log(string file_log, string text)
        {
            using (StreamWriter w = File.AppendText(file_log))
            {
                w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " : " + text);
            }
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " : " + text);
        }
    }
}
