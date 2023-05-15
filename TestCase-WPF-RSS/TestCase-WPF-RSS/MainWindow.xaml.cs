using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestCase_WPF_RSS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static MainWindow? _MainWindow { get; set; } = null;
        private const int maxStackSize = 2 * 1024 * 1024;
        public static ApplicationWindow? applicationWindow { get; set; } = null;
        public MainWindow()
        {
            InitializeComponent();
            _MainWindow = this;
            applicationWindow = new ApplicationWindow();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var width = LoadingText.ActualWidth;
            DoubleAnimation textAnimation = new DoubleAnimation();
            textAnimation.From = width;
            textAnimation.To = 200;
            textAnimation.Duration = TimeSpan.FromSeconds(3);
            textAnimation.RepeatBehavior = new RepeatBehavior(2);

            textAnimation.Completed += (sender, eArgs) =>
            {
                ShowApplicationWindow();
                this.Hide();
            };
            LoadingText.BeginAnimation(Label.WidthProperty, textAnimation, HandoffBehavior.Compose);
        }

        private void ShowApplicationWindow()
        {
            applicationWindow?.Show();
        }
    }
}
