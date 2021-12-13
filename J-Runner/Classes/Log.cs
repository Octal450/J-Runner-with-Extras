using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JRunner
{
    public static class Log
    {
        private static string logtext = "";
        private static string file = Path.Combine(variables.pathforit, "Log.txt");
        public static int debuglevel = 0;

        public static void savelog()
        {
            //string file = Path.Combine(variables.pathforit, "Log.txt");
            if (File.Exists(file))
            {
                try
                {
                    string[] data = File.ReadAllLines(file);
                    int last = 0;
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i].Contains("===================================================") && i != data.Length - 1) { last = i + 1; }
                    }
                    DateTime convertedDate = DateTime.Parse(data[last]);
                    if ((DateTime.Now - convertedDate).Days > 0)
                    {
                        string logdir = Path.Combine(variables.pathforit, "Logs");
                        if (!Directory.Exists(logdir)) Directory.CreateDirectory(logdir);
                        string newfile = Path.Combine(logdir, "Log-" + convertedDate.ToShortDateString() + ".txt");
                        if (!File.Exists(newfile)) File.Move(file, newfile);
                    }
                }
                catch (Exception) { }
            }
            //File.AppendAllText(file, "\n" + text);
        }

        public static void log(string text, int level = 0)
        {
            if (level <= debuglevel && level > 0)
            {
                Console.WriteLine(text);
                return;
            }

            logtext += text;
            if (logtext.Contains(Environment.NewLine))
            {
                try
                {
                    File.AppendAllText(file, logtext);
                }
                catch (Exception) { }

                logtext = "";
            }
        }



    }
}
