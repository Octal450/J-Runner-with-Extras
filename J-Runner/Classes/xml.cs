using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace JRunner
{
    class xml
    {
        XElement sitemap;
        Stream io;
        XmlWriter xw;
        public void load(string text)
        {
            try
            {
                TextReader t = new StringReader(text);
                sitemap = XElement.Load(t);
                t.Close();
            }
            catch (Exception) { }
        }

        public void create(string file)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(file))) Directory.CreateDirectory(Path.GetDirectoryName(file));
                io = new FileStream(file, FileMode.Create);
                xw = XmlWriter.Create(io);
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
        }
        public void close()
        {
            try
            {
                xw.Close();
                io.Close();
            }
            catch (Exception ex) { if (variables.debugme) Console.WriteLine(ex.ToString()); }
        }
        public void start()
        {
            xw.WriteStartDocument();
            xw.WriteRaw(Environment.NewLine);
            xw.WriteStartElement("JRunner");
            xw.WriteRaw(Environment.NewLine);
        }
        public void end()
        {
            xw.WriteEndElement();
            xw.WriteRaw(Environment.NewLine);
            xw.WriteEndDocument();
        }

        public string readsetting(string what)
        {
            if (sitemap != null)
            {
                XName setting = XName.Get("setting");
                XName name = XName.Get("name");
                XName value = XName.Get("value");
                foreach (var urlElement in sitemap.Elements(setting))
                {
                    if (urlElement.Element(name).Value == what)
                    {
                        return urlElement.Element(value).Value;
                    }
                }
            }
            return "";
        }
        public string read(string what)
        {
            if (sitemap != null)
            {
                return sitemap.Element(what).Value;
            }
            return "";
        }

        public void write(string name, string value)
        {
            xw.WriteRaw("\t");
            xw.WriteStartElement("setting");
            xw.WriteRaw(Environment.NewLine);
            xw.WriteRaw("\t\t"); xw.WriteElementString("name", name);
            xw.WriteRaw(Environment.NewLine);
            xw.WriteRaw("\t\t"); xw.WriteElementString("value", value);
            xw.WriteRaw(Environment.NewLine);
            xw.WriteRaw("\t");
            xw.WriteEndElement();
            xw.WriteRaw(Environment.NewLine);
        }
    }
}
