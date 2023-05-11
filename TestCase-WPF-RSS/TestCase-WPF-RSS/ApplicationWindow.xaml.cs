using System;
using System.Collections.Generic;
using System.Drawing;
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
using TestCase_WPF_RSS.Settings;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Drawing.Color;

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

        // Настройки строки подключения к БД
        #region Application Settings

        internal ApplicationSettings appsets = new ApplicationSettings();
        string? connectionString;

        // https://metanit.com/sharp/tutorial/16.2.php
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            appsets.LoadApplicationSettings();
            ConnectionString.Text = appsets.DataBaseSettings?.ConnectionStringSettings?.ConnectionString;
            connectionString = ConnectionString.Text;
        }

        private void ConnectionStringSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(appsets.DataBaseSettings != null && appsets.DataBaseSettings.ConnectionStringSettings != null)
            {
                appsets.DataBaseSettings.ConnectionStringSettings.ConnectionString = ConnectionString.Text;
                appsets.SaveSettings();
                connectionString = appsets.DataBaseSettings.ConnectionStringSettings.ConnectionString;
                ResetBrush();
            } 
        }

        private void ResetBrush()
        {
            var converter = new System.Windows.Media.BrushConverter();
            var brush = (System.Windows.Media.Brush?)converter.ConvertFromString("#FFDDDDDD");
            ConnectionStringSaveButton.Background = brush;
        }

        private void ConnectionString_KeyUp(object sender, KeyEventArgs e)
        {
            if (ConnectionString.Text != connectionString)
            {
                ConnectionStringSaveButton.Background = Brushes.LightGreen;
            }
            else
            {
                ResetBrush();
            }
        }

        private void ConnectionStringCancel_Click(object sender, RoutedEventArgs e)
        {
            ConnectionString.Text = connectionString;
            ResetBrush();
        }

        #endregion

        // Создание объектов
    }
}
