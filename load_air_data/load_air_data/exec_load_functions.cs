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

        public static int Main(string v_st_load)
        {

            //Parametros
            string v_sub_dir_gpfdist = "/";
            string v_sub_dir_files = "D:\\data\\";

            if (v_st_load.ToUpper() == "DEPARTURES")
            {
                v_sub_dir_gpfdist = v_sub_dir_gpfdist + v_st_load.ToUpper();
                v_sub_dir_files = v_sub_dir_files + v_st_load.ToUpper();
                //v_param01 = " -d "; //DEPARTURES
            }
            else
            {
                v_sub_dir_gpfdist = v_sub_dir_gpfdist + v_st_load.ToUpper();
                v_sub_dir_files = v_sub_dir_files + v_st_load.ToUpper();
                //v_param01 = " -a "; //ARRIVALS
            }

            string file_log = "D:\\data\\source\\log" + "\\log_exec_load_functions_" + v_st_load + (DateTime.Now.ToString("yyyyMMdd")) + ".txt";
            write_log(file_log, "Start create file - " + v_st_load + ".");

            string v_dir_processing_gpfdist = v_sub_dir_gpfdist + "/PROCESSING/";
            string v_dir_processing = v_sub_dir_files + "\\PROCESSING\\";
            string v_dir_finished = v_sub_dir_files + "\\FINISHED\\";
            string v_ip_gpfdist = "192.168.110.1";
            string v_port_gpfdist = "3131";

            //LOOP DE ARQUIVOS ".dat"
            string file_name = "";
            string file_dest = "";
            string v_comand = "";
            string str_conn = "";
            string Tex_Server_GPDB_IP = "192.168.110.131";
            str_conn = "Server=" + Tex_Server_GPDB_IP + ";Port=5432;UserId=gpadmin;Password='pivotal';CommandTimeout=0;Database=airdata;";

            //file_dest = "arrivals_20190418182148.dat";

            string[] ListFiles = Directory.GetFiles(v_dir_processing,"*.dat");
            for (int cont_arq = 0; cont_arq < ListFiles.Length; cont_arq++)
            {
                Console.WriteLine("File found: " + ListFiles[cont_arq]);
                write_log(file_log, "File found: " + ListFiles[cont_arq]);

                file_name = ListFiles[cont_arq].Replace(v_dir_processing, "");
                file_dest = ListFiles[cont_arq].Replace(".dat", "").Replace(v_dir_processing,"");

                List<string> ListCmds = new List<string>();

                ListCmds = list_cmd();
                for (int cont_cmd = 0; cont_cmd < ListCmds.Count; cont_cmd++)
                {
                    string fn = ListCmds[cont_cmd];

                    if (fn.ToUpper() == "FN_LOAD_EXT_AIR_DATA") //ADD NEW EXTERNAL TABLE, DEPENDS MORE PARAMETERS
                    {
                        v_comand = "SELECT WORK_AD." + fn +"('" + file_dest + "','1','" + v_ip_gpfdist + "','" + v_port_gpfdist + "','" + v_dir_processing_gpfdist + "')";
                    }
                    else
                    {
                        v_comand = "SELECT WORK_AD." + fn +"('" + file_dest + "','1')";
                    }

                    greenplum_conection.exec_gpdb_cmd(str_conn, v_comand);
                    Console.WriteLine("Command: " + v_comand);
                    write_log(file_log, "Command: " + v_comand);

                }

                File.Move(v_dir_processing + file_name, v_dir_finished + file_name);

            }

            return 0;
        }




        static List<string> list_cmd()
        {
            List <string> list_cmd = new List<string>();

            list_cmd.Add("FN_LOAD_EXT_AIR_DATA");
            list_cmd.Add("FN_LOAD_STG_AIR_DATA");
            list_cmd.Add("FN_LOAD_WRK_AIR_DATA");

            return list_cmd;
        }

        static void write_log(string file_log, string text)
        {
            using (StreamWriter w = File.AppendText(file_log))
            {
                w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " : " + text);
            }
        }
    }
}
