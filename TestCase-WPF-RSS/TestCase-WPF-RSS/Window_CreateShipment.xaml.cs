using System;
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
using TestCase_WPF_RSS.EntityFramework;

namespace TestCase_WPF_RSS
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window_CreateShipment : Window
    {
        public string? connectionString;

        public ApplicationWindow applicationWindow;

        public Window_CreateShipment()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShipmentStatusEnum shipmentStatus = (ShipmentStatusEnum)StatusComboBox.SelectedIndex;
                using (ApplicationContext db = new ApplicationContext(connectionString??""))
                {
                    
                        db.Shipments.Add(new EntityFramework.Shipments() { Status = shipmentStatus });
                        db.SaveChanges();
                }

                if (e.Source is not null && shipmentStatus == ShipmentStatusEnum.Received)
                {
                    applicationWindow.FillDataGrid();
                }
                else if (e.Source is not null && shipmentStatus == ShipmentStatusEnum.ToWarehouse)
                {
                    applicationWindow.FillDataGrid_ToWarehouse();
                }
                else if (e.Source is not null && shipmentStatus == ShipmentStatusEnum.Sold)
                {
                    applicationWindow.FillDataGrid_Sold();
                }

            }
            catch (Exception) { MessageBox.Show("Нет соединения с БД!"); }

        }
    }
}
