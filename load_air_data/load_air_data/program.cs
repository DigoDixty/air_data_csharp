using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace load_air_data
{
    class program
    {
        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.WriteLine("Please enter with parameters.");
                System.Console.WriteLine("Necessary 2 at least.");
                System.Console.WriteLine("1 -  applicattion.");
                System.Console.WriteLine("2 -  departure os arrivals.");
                System.Console.WriteLine("...");
               // return 1;
            }

            string v_01 = ""; // country
            string v_02 = ""; // arrival or departure
            string v_03 = ""; // process

            v_01 = "brazil";
            v_02 = "arrivals";
            v_03 = "create_files";

            //v_01 = args[0];
            //v_02 = args[1];
            //v_03 = args[2];

            System.Console.WriteLine("System load running...");
            System.Console.WriteLine("type: " + v_01);
            System.Console.WriteLine("execution: " + v_02);

            //v_03 = "create_files";
            if (v_03 == "create_files")
            {
                create_files.Main(v_01, v_02);
            }

            //v_03 = "aggregate_files";
            if (v_03 == "aggregate_files")
            {
                aggregate_files.Main(v_01);
            }

            //v_03 = "exec_load_functions";
            if (v_03 == "exec_load_functions")
            {
                exec_load_functions.Main(v_01);
            }
            
            return 0;
        }
    }
}
