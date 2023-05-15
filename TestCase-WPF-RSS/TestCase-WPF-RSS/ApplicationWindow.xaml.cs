using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using TestCase_WPF_RSS.EntityFramework;
using TestCase_WPF_RSS.Settings;
using static System.Net.Mime.MediaTypeNames;
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
        public string? connectionString;

        // https://metanit.com/sharp/tutorial/16.2.php
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            appsets.LoadApplicationSettings();
            ConnectionString.Text = appsets.DataBaseSettings?.ConnectionStringSettings?.ConnectionString;
            connectionString = ConnectionString.Text;
        }

        private void ConnectionStringSaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (appsets.DataBaseSettings != null && appsets.DataBaseSettings.ConnectionStringSettings != null)
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

        bool isSettingsLocked = false;
        private void BlockSettings_Click(object sender, RoutedEventArgs e)
        {
            isSettingsLocked = !isSettingsLocked;
            if (isSettingsLocked)
            {
                ConnectionString.IsEnabled = false;
                ConnectionStringLabel.IsEnabled = false;
                ConnectionStringSaveButton.IsEnabled = false;
                ConnectionStringCancel.IsEnabled = false;
                ConnectionStringTestConnectionButton.IsEnabled = false;
                ConnectionStringTestConnectionLabel.IsEnabled = false;
                ConnectionStringTestGroupBox.IsEnabled = false;
                BlockSettings.Content = "Разблокировать настройки";
                //CreateObjectTab.IsEnabled = true;
            }
            else
            {
                ConnectionString.IsEnabled = true;
                ConnectionStringLabel.IsEnabled = true;
                ConnectionStringSaveButton.IsEnabled = true;
                ConnectionStringCancel.IsEnabled = true;
                ConnectionStringTestConnectionButton.IsEnabled = true;
                ConnectionStringTestConnectionLabel.IsEnabled = true;
                ConnectionStringTestGroupBox.IsEnabled = true;
                BlockSettings.Content = "Блокировать настройки на изменение";
                //CreateObjectTab.IsEnabled = false;
            }
        }

        #endregion

        // Создание объектов

        #region DB Object Creation

        private void ConnectionStringTestConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            if (connectionString is not null)
            {
                ApplicationContext actxt = new ApplicationContext(connectionString);
                if (actxt.TestConnection(connectionString))
                {
                    MessageBox.Show("Соединение удалось установить!");
                }
                else
                {
                    MessageBox.Show("Соединение НЕ удалось установить!");
                }
            }

            else
            {
                MessageBox.Show("Соединение НЕ удалось установить!");
            }
        }


        #endregion

        #region Received Tab

        public void FillDataGrid()

        {

            string conString = connectionString ?? "";

            string cmdString = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(conString))

                {

                    cmdString = "SELECT S.Id as 'ИД', S.Status as 'Статус', SS.StatusText AS 'Текст Статуса' FROM Shipments as S LEFT JOIN ShipmentStatuses as SS ON S.Status = SS.StatusId WHERE S.Status = 0";

                    SqlCommand cmd = new SqlCommand(cmdString, con);

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable("Shipments");

                    sda.Fill(dt);

                    ReceivedGrid_DataGrid.ItemsSource = dt.AsDataView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось подключиться к БД!");
            }
        }

        public void FillDataGrid_ToWarehouse()

        {

            string conString = connectionString ?? "";

            string cmdString = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(conString))

                {

                    cmdString = "SELECT S.Id as 'ИД', S.Status as 'Статус', SS.StatusText AS 'Текст Статуса' FROM Shipments as S LEFT JOIN ShipmentStatuses as SS ON S.Status = SS.StatusId WHERE S.Status = 1";

                    SqlCommand cmd = new SqlCommand(cmdString, con);

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable("Shipments");

                    sda.Fill(dt);

                    ToWarehouseGrid_DataGrid.ItemsSource = dt.AsDataView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось подключиться к БД!");
            }
        }

        public void FillDataGrid_Sold()

        {

            string conString = connectionString ?? "";

            string cmdString = string.Empty;

            try
            {
                using (SqlConnection con = new SqlConnection(conString))

                {

                    cmdString = "SELECT S.Id as 'ИД', S.Status as 'Статус', SS.StatusText AS 'Текст Статуса' FROM Shipments as S LEFT JOIN ShipmentStatuses as SS ON S.Status = SS.StatusId WHERE S.Status = 2";

                    SqlCommand cmd = new SqlCommand(cmdString, con);

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable("Shipments");

                    sda.Fill(dt);

                    SoldGrid_DataGrid.ItemsSource = dt.AsDataView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось подключиться к БД!");
            }
        }

        private void CreateShipmentButton_Click(object sender, RoutedEventArgs e)
        {
            Window_CreateShipment window_CreateShipment = new Window_CreateShipment();
            window_CreateShipment.connectionString = connectionString;
            window_CreateShipment.applicationWindow = this;
            window_CreateShipment.Show();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (e.Source is not null && ((TabControl)e.Source).SelectedIndex == 1)
                {
                    FillDataGrid();
                }
                else if (e.Source is not null && ((TabControl)e.Source).SelectedIndex == 2)
                {
                    FillDataGrid_ToWarehouse();
                }
                else if (e.Source is not null && ((TabControl)e.Source).SelectedIndex == 3)
                {
                    FillDataGrid_Sold();
                }
            }
        }

        private void ReceivedGrid_DataGrid_ContextMenu_ToWarehouse_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = ReceivedGrid_DataGrid.SelectedIndex;
            System.Data.DataRowView shipment = (System.Data.DataRowView)ReceivedGrid_DataGrid.SelectedItem;
            if (selectedIndex >= 0)
            {
                if (shipment != null)
                {
                    var selectedId = (int)shipment.Row[0];

                    try
                    {
                        using (ApplicationContext db = new ApplicationContext(connectionString ?? ""))
                        {
                            var s = db.Shipments.FirstOrDefault(x => x.Id == selectedId);
                            s.Status = ShipmentStatusEnum.ToWarehouse;
                            s.Modified = DateTimeOffset.Now;
                            db.Shipments.Update(s);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception) { MessageBox.Show("Нет соединения с БД!"); }

                    FillDataGrid();
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите строку данных...");
            }
        }

        private void ToWarehouseGrid_DataGrid_ContextMenu_ToWarehouse_Click(object sender, RoutedEventArgs e)
        {
            var selectedIndex = ToWarehouseGrid_DataGrid.SelectedIndex;
            System.Data.DataRowView shipment = (System.Data.DataRowView)ToWarehouseGrid_DataGrid.SelectedItem;
            if (selectedIndex >= 0)
            {
                if (shipment != null)
                {
                    var selectedId = (int)shipment.Row[0];

                    try
                    {
                        using (ApplicationContext db = new ApplicationContext(connectionString ?? ""))
                        {
                            var s = db.Shipments.FirstOrDefault(x => x.Id == selectedId);
                            s.Status = ShipmentStatusEnum.Sold;
                            s.Modified = DateTimeOffset.Now;
                            db.Shipments.Update(s);
                            db.SaveChanges();
                        }
                    }
                    catch (Exception) { MessageBox.Show("Нет соединения с БД!"); }

                    FillDataGrid_ToWarehouse();
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите строку данных...");
            }
        }

        #endregion

    }
}
