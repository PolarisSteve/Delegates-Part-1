using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerClass
{
    /// <summary>
    /// Simple Logger
    /// </summary>
    public class Logger : ILogger
    {

        LogLevel _levelset;
        string Pathname;
        string Filename;

        public Logger(LogLevel levelset,string pathname, string filename)
        {

            _levelset = levelset;
            Pathname = pathname;
            Filename = filename;
        }

        public enum LogLevel
        {

            Error = 0,
            Warning = 1,
            Info = 2

        }


        public void Log(string Message, string module, LogLevel level)
        {

            if (level <= _levelset)
            {

                DateTime dt = DateTime.Now;

                string dayfolder = dt.ToString("yyyyMM"); 

                dayfolder = Path.Combine(Pathname, dayfolder);

                //create a folder for the month
                if (!Directory.Exists(dayfolder))
                    Directory.CreateDirectory(dayfolder);


                //Create a file for the day, append if exists
                using (StreamWriter sw = new StreamWriter(Path.Combine(dayfolder,Path.GetFileNameWithoutExtension(Filename) + dt.ToString("yyyyMMdd") + ".txt"), true))
                {
                    sw.WriteLine("{0}\t{1}\t{2}\t{3}", dt.ToLocalTime(), Enum.GetName(typeof(LogLevel),level) ,module, Message);

                }

            }
        }

    }
}
