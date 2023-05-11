using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestCase_WPF_RSS.Settings
{
    // https://metanit.com/sharp/tutorial/16.2.php
    internal class ApplicationSettings
    {

        public ApplicationSettings(string fileName)
        {
            FileName = fileName;
        }

        public ApplicationSettings() : this("config/config.xml") {}


        public string FileName { get; set; }

        public DataBaseSettings? DataBaseSettings { get; set; }

        public void LoadApplicationSettings()
        {
            CreateFieldsIfNoExists();
            ParseXml();
        }

        private void ParseXml()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(FileName);
            // получим корневой элемент
            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                // Jбход всех узлов в корневом элементе
                foreach (XmlElement xnode in xRoot)
                {
                    // получаем атрибут name
                    //XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                    //Console.WriteLine(attr?.Value);

                    if (xnode.Name.Equals("Application"))
                    { 
                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            if (childnode.Name.Equals("Database"))
                            {
                                foreach (XmlNode childnodeInner in childnode.ChildNodes)
                                {
                                    if (!childnodeInner.Name.Equals("ConnectionString"))
                                    {
                                        if(DataBaseSettings != null && DataBaseSettings.ConnectionStringSettings != null)
                                            DataBaseSettings.ConnectionStringSettings.ConnectionString = childnodeInner.InnerText;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        private void CreateFieldsIfNoExists()
        {
            if (DataBaseSettings == null)
            {
                DataBaseSettings = new DataBaseSettings();

                if (DataBaseSettings.ConnectionStringSettings == null)
                {
                    DataBaseSettings.ConnectionStringSettings = new ConnectionStringSettings();
                }
            }
        }

        public void SaveSettings()
        {
            if (DataBaseSettings != null)
            {
                if (DataBaseSettings.ConnectionStringSettings != null)
                {
                    if(DataBaseSettings.ConnectionStringSettings.ConnectionString != null)
                    {
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.Load(FileName);
                        // получим корневой элемент
                        XmlElement? xRoot = xDoc.DocumentElement;
                        if (xRoot != null)
                        {
                            // Jбход всех узлов в корневом элементе
                            foreach (XmlElement xnode in xRoot)
                            {
                                // получаем атрибут name
                                //XmlNode? attr = xnode.Attributes.GetNamedItem("name");
                                //Console.WriteLine(attr?.Value);

                                if (xnode.Name.Equals("Application"))
                                {
                                    foreach (XmlNode childnode in xnode.ChildNodes)
                                    {
                                        if (childnode.Name.Equals("Database"))
                                        {
                                            foreach (XmlNode childnodeInner in childnode.ChildNodes)
                                            {
                                                if (!childnodeInner.Name.Equals("ConnectionString"))
                                                { 
                                                        childnodeInner.InnerText = DataBaseSettings.ConnectionStringSettings.ConnectionString;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }

                        xDoc.Save(FileName);
                    }
                }
            }
        }
    }
}
