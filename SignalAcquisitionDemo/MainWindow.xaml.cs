using SignalAcquisitionDemo.Helper;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LoggerHelp;

namespace SignalAcquisitionDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            LogHelper.Info("Application Start.");
            InitializeComponent();
        }


        /// <summary>
        /// 窗口可移动
        /// </summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnMAX_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
                btn.SetResourceReference(Button.StyleProperty, "NormalButtonStyle");
            }
            else
            {
                this.WindowState = WindowState.Normal;
                btn.SetResourceReference(Button.StyleProperty, "MaxButtonStyle");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Info("UpperWaveform");
            //增加处理
        }
        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnQuit(object sender, RoutedEventArgs e)
        {

           // this.MainView.timer?.Dispose();
            //this.MainView.sendControlTimer?.Dispose();
            Application.Current.Shutdown();
        }

    }
}
