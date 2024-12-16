using SignalAcquisitionDemo.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SignalAcquisitionDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }


        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (e.Exception is TaskCanceledException || e.Exception is ObjectDisposedException)
                LogHelper.Warn("线程取消异常，已忽略。"+e.Exception.Message);
            else
            {
                LogHelper.Error("未处理的异常："+e.Exception.Message+"stack:"+e.Exception.StackTrace);
                MessageBox.Show("发生错误(一般是未安装PCI-1730的驱动)，请重启软件！！！","错误",MessageBoxButton.OK,MessageBoxImage.Error);
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
