using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestCase_WPF_RSS.Settings
{
    internal class ApplicationSettings
    {

        public ApplicationSettings(string fileName)
        {
            FileName = fileName;
        }

        public ApplicationSettings() : this("..\\config\\config.xml") {}


        public string FileName { get; set; }

        DataBaseSettings? DataBaseSettings { get; set; }

        public void LoadApplicationSettings()
        {
            CreateFieldsIfNoExists();
            ParseXml();
        }

        private static void ParseXml()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("config/config.xml");
            // получим корневой элемент
            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {/*
                // обход всех узлов в корневом элементе
                foreach (XmlElement xnode in xRoot)
                {
                    // получаем атрибут name
                    //XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                    //Console.WriteLine(attr?.Value);
                    ConnectionString.Text += $"\nxnode.Name={xnode.Name}";
                    // обходим все дочерние узлы элемента user
                    foreach (XmlNode childnode in xnode.ChildNodes)
                    {
                        //Console.WriteLine($"Company: {childnode.InnerText}");
                        ConnectionString.Text += $"\nchildnode.Name={childnode.Name}";

                        foreach (XmlNode childnodeInner in childnode.ChildNodes)
                        {
                            //Console.WriteLine($"Company: {childnode.InnerText}");
                            ConnectionString.Text += $"\nchildnodeInner.Name={childnodeInner.Name}";

                        }
                    }
                    Console.WriteLine();
                }*/
            }
        }

        private void CreateFieldsIfNoExists()
        {
            if (DataBaseSettings == null)
            {
                DataBaseSettings = new DataBaseSettings();

                if (DataBaseSettings.ConnectionStringSettings != null)
                {
                    DataBaseSettings.ConnectionStringSettings = new ConnectionStringSettings();
                }
            }
        }
    }
}
