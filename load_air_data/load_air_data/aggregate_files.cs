using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace load_air_data
{
    public class aggregate_files
    {
        public static int Main(string v_st_load)
        {

            //Parametros
            string v_sub_dir = "\\";
            string v_param01 = "";


            string drive = Properties.Settings.Default.file_drive;

            v_sub_dir = v_sub_dir + v_st_load.ToUpper();
            v_param01 = v_st_load + "_" + (DateTime.Now.ToString("yyyyMMddhhmmss")); //DEPARTURES

            string v_dir_log = drive + "data\\source\\log"; //Properties.Settings.Default.diretorio_orig;
            string file_log = v_dir_log + "\\log_aggregate_files_" + v_st_load + (DateTime.Now.ToString("yyyyMMdd")) + ".txt";
            write_log(file_log, "Start create file - " + v_st_load + ".");

            string v_dir_incoming = drive + "DATA" + v_sub_dir + "\\INCOMING\\";
            string v_dir_processing = drive + "DATA" + v_sub_dir + "\\PROCESSING\\";

            Console.WriteLine("Directory incoming: " + v_dir_incoming + ".");
            Console.WriteLine("Directory processing: " + v_dir_processing + ".");
            write_log(file_log, "Directory incoming: " + v_dir_incoming + ".");
            write_log(file_log, "Directory processing: " + v_dir_processing + ".");
            
            List<String> ListFiles = Directory.GetFiles(v_dir_incoming, "*.txt").ToList();

            foreach (string file in ListFiles)
            {
                FileInfo name_file = new FileInfo(file);
                // if to remove name collisions
                if (new FileInfo(v_dir_processing + name_file.Name).Exists == false)
                {
                    name_file.MoveTo(v_dir_processing + name_file.Name);
                    write_log(file_log, "Arquivo: " + name_file.Name + " movido para " + v_dir_processing + name_file.Name);
                    Console.WriteLine("Arquivo: " + name_file.Name + " movido para " + v_dir_processing + name_file.Name);
                }
            }

            //string air_iata = "GRU";
            Console.WriteLine("");

            // executa script criando aquivos
            //string strCmdText = "python script_get_air_data.py " + air_iata + " > ";
            string file_dest = v_dir_processing + v_st_load + "_" + (DateTime.Now.ToString("yyyyMMddHHmmss")) + ".dat";  //file_dest = "arrivals_20190418182148.dat";

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo = startInfo;
            startInfo.FileName = "run_findstr.bat";
            startInfo.Arguments = v_dir_processing + " " + file_dest;
            process.Start();
            process.WaitForExit();
            write_log(file_log, "Fim da execução para: " + file_dest);

            Console.WriteLine(v_dir_incoming + " was moved to " + v_dir_processing + ".");
            write_log(file_log, v_dir_incoming + " was moved to " + v_dir_processing + ".");

            return 0;
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
