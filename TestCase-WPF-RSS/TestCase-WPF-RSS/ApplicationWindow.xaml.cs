﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;

namespace TestCase_WPF_RSS
{
    /// <summary>
    /// Логика взаимодействия для ApplicationWindow.xaml
    /// </summary>
    public partial class ApplicationWindow : Window
    {
        public ApplicationWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow._MainWindow?.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("config/config.xml");
            // получим корневой элемент
            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
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
                }
            }
        }
    }
}
