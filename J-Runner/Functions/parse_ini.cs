using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JRunner
{
    class parse_ini
    {
        public static string[] parse(string filename)
        {
            List<string> options = new List<string>();
            if (File.Exists(filename))
            {
                TextReader tr = new StreamReader(filename);

                int i = 0;

                string temp;
                while ((temp = tr.ReadLine()) != null)
                {
                    if (temp == "") continue;
                    else if (temp[0] == ';') continue;
                    else
                    {
                        options.Add(temp.Replace(" ", string.Empty));
                        i++;
                    }
                }
                tr.Close();
            }
            return options.ToArray();
        }

        public static bool write_ini(string filename, string[] options)
        {
            int counter = 0;
            TextWriter tw = new StreamWriter(filename);
            try
            {
                foreach (string option in options)
                {
                    if (option == "")
                    {
                        tw.WriteLine("");
                        counter++;
                    }
                    else if (option[0] == '[' && options[0] != option)
                    {
                        while (2 - counter <= 0)
                        {
                            tw.WriteLine("");
                        }
                        tw.WriteLine(option);
                        counter = 0;
                    }
                    else if (option[0] == ';' && options[0] != option)
                    {
                        tw.WriteLine(option);
                        counter = 0;
                    }
                    else
                    {
                        counter = 0;
                        option.Replace(" ", "");
                        option.Replace("=", " = ");
                        tw.WriteLine(option);
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            tw.Close();
            return true;
        }

        public static bool edit_ini(string filename, string[] edit, string[] delete)
        {
            if (File.Exists(filename))
            {
                TextReader TR = File.OpenText(filename);
                List<string> lines = new List<string>();
                //List<string> lines = (List<string>)File.ReadLines(filename);
                string temp;
                while ((temp = TR.ReadLine()) != null)
                {
                    lines.Add(temp);
                }
                TR.Close();

                foreach (string del in delete)
                {
                    lines.Remove(del);
                }

                foreach (string tochange in edit)
                {
                    bool found = false;
                    int index = 0;
                    foreach (string line in lines)
                    {
                        if (line == "" || line[0] == ';') continue;
                        if (line.Contains('='))
                        {
                            if (line.Substring(0, line.IndexOf('=')).Contains(tochange.Substring(0, tochange.IndexOf('='))))
                            {
                                //Console.WriteLine(line.Substring(0, line.IndexOf('=')));
                                //Console.WriteLine(tochange.Substring(0, tochange.IndexOf('=')));
                            }
                            if (line.Substring(0, line.IndexOf('=')).Replace(" ", "") == (tochange.Substring(0, tochange.IndexOf('=')).Replace(" ", "")))
                            {
                                //Console.WriteLine("!");
                                //Console.WriteLine("{0} - {1} || 2", line, (tochange));
                                found = true;
                                index = lines.IndexOf(line);
                            }
                            else
                            {
                                //Console.WriteLine("{0} - {1} || 3", line, (tochange));
                            }
                        }
                        else
                        {
                            if (line == (tochange))
                            {
                                //Console.WriteLine("{0} - {1} || 4", line, (tochange));
                                found = true;
                                index = lines.IndexOf(line);
                            }
                        }

                    }

                    if (!found)
                    {
                        //Console.WriteLine("{0}", (tochange));
                        lines.Add(tochange);
                    }
                    else
                    {
                        lines.RemoveAt(index);
                        lines.Insert(index, tochange.Replace("=", " = "));
                    }
                }

                write_ini(filename, lines.ToArray());

            }
            else
            {
                write_ini(filename, edit);
            }
            return true;
        }

        public static List<string> getlabels(string filename)
        {
            List<string> labels = new List<string>();
            if (File.Exists(filename))
            {
                TextReader tr = new StreamReader(filename);

                int i = 0;

                string temp;
                while ((temp = tr.ReadLine()) != null)
                {
                    if (temp == "") continue;
                    else if (temp[0] == ';') continue;
                    else if (temp[0] == '[' && temp.Contains("]"))
                    {
                        labels.Add(temp.Replace(" ", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty));
                        i++;
                    }
                }
                tr.Close();
            }
            return labels;
        }

        public static string[] parselabel(string filename, string label)
        {
            List<string> labels = new List<string>();
            if (File.Exists(filename))
            {
                TextReader tr = new StreamReader(filename);

                string temp;
                bool record = false;
                while ((temp = tr.ReadLine()) != null)
                {
                    if (temp == "") continue;
                    else if (temp[0] == ';') continue;
                    else if (temp[0] == '[' && temp.Contains("]"))
                    {
                        if (temp.Contains(label)) record = !record;
                        else record = false;
                    }
                    else if (record)
                    {
                        labels.Add(temp.Replace(" ", string.Empty));
                    }
                }
                tr.Close();
            }
            return labels.ToArray();
        }

    }
}

