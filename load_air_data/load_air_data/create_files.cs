using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.NetworkInformation;

namespace load_air_data
{
    public class create_files
    {

        static void write_log(string file_log, string text)
        {
            using (StreamWriter w = File.AppendText(file_log))
            {
                w.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " : " + text);
            }
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " : " + text);

        }

        public static bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        public static int Main(string v_country, string v_st_load)
        {   
            //Parametros
            string v_sub_dir = "\\";
            string v_param01 = "";
            string v_list_dir = ".\\" + Properties.Settings.Default.dir_list_coutry +  "\\" + v_country + ".txt ";

            if (v_st_load.ToUpper() == "DEPARTURES")
            {
                v_sub_dir = v_sub_dir + v_st_load.ToUpper();
                v_param01 = " -d " + v_list_dir ; //DEPARTURES
            }

            if (v_st_load.ToUpper() == "ARRIVALS")
            {
                v_sub_dir = v_sub_dir + v_st_load.ToUpper();
                v_param01 = " -a "  + v_list_dir ; //ARRIVALS
            }

            if (v_param01 != "")
            {
                string drive = Properties.Settings.Default.file_drive;

                string v_dir_log = drive + "data\\source\\log"; //Properties.Settings.Default.diretorio_orig;
                string file_log = v_dir_log + "\\log_create_files_" + v_country + "_" + v_st_load + (DateTime.Now.ToString("yyyyMMdd")) + ".txt";
                write_log(file_log, "Start create file - " + v_country + "_" + v_st_load + ".");
                string v_dir_incoming = drive + "data" + v_sub_dir + "\\incoming\\";
                string v_web = "www.google.com.br";

                write_log(file_log, "Testing ping on " + v_web + "...");

                if (PingHost("www.google.com.br") == false)
                {
                    write_log(file_log, "Ping on " + v_web + " fail. <---------------- ");
                    return 1;
                }
                else
                {
                    write_log(file_log, "Ping on " + v_web + " sucess.");

                    string file_dest = v_dir_incoming + v_country + "_" + (DateTime.Now.ToString("yyyyMMddHHmmss")); //+ ".txt.tmp");

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.StartInfo = startInfo;
                    startInfo.FileName = "run_py.bat";
                    startInfo.Arguments = v_param01 + " " + file_dest;

                    process.Start();
                    process.WaitForExit();

                    write_log(file_log, "File created : " + file_dest);
                }
                return 0;
            }
            else
            { return 1; }
        }

    }

}
