using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Xml.Linq;
using SignalAcquisitionDemo.Helper;
using SignalAcquisitionDemo.Models;
using SignalAcquisitionDemo.Properties;

using System.IO.Ports;
using System.Windows.Shapes;
using System.Threading;
using SignalAcquisitionDemo.Styles;
using Automation.BDaq;

namespace SignalAcquisitionDemo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : UserControl
    {
        private List<string> Items = new List<string>() { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9" };
        private static bool isConnect;


        public static bool isDevice1Receive = false;
        public static bool isDevice1TimerEnd = false;
        public static bool isDevice2Receive = false;
        public static bool isDevice2TimerEnd = false;
        public readonly static int ChannelInterval = (int)(1000 / Settings.Default.frq);// 20Hz
        public readonly static int ChannelShowCount = (int)(Settings.Default.frq / 2);
        private static System.Timers.Timer device1Timer;
        private static System.Timers.Timer device2Timer;
        private static System.Timers.Timer diTimer;

        string deviceDescription = Settings.Default.deviceDescription;//"DemoDevice,BID#0";
        string profilePath = Settings.Default.profilePath;
        int startPort = 0;
        int portCount = 2;
        ErrorCode errorCode = ErrorCode.Success;
        InstantDiCtrl instantDiCtrl;
        InstantDoCtrl instantDoCtrl;
        static List<List<float>> device1Data = new List<List<float>>();
        static List<List<float>> device2Data = new List<List<float>>();

        int index = 0;

        #region 属性
        public bool IsConnect
        {
            get { return isConnect; }
            set
            {
                isConnect = value;
                this.SL_Status.Value = value;
            }
        }

        private List<ChannelPropertyData> _ChannelData = new List<ChannelPropertyData>();
        public List<ChannelPropertyData> ChannelData
        {
            get { return _ChannelData; }
            set { _ChannelData = value; }
        }


        private List<SwitchPropertyData> _SwitchReadData = new List<SwitchPropertyData>();
        public List<SwitchPropertyData> SwitchReadData
        {
            get { return _SwitchReadData; }
            set { _SwitchReadData = value; }
        }


        private List<SwitchPropertyData> _SwitchWriteData = new List<SwitchPropertyData>();
        public List<SwitchPropertyData> SwitchWriteData
        {
            get { return _SwitchWriteData; }
            set { _SwitchWriteData = value; }
        }


        #endregion



        public MainView()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            try
            {
                Items = SerialPort.GetPortNames().ToList();
                Cmb_Com.ItemsSource = Items;
                IsConnect = false;
                device1Timer = new System.Timers.Timer(ChannelInterval);
                device1Timer.Elapsed += Device1Timer_Elapsed;
                device2Timer = new System.Timers.Timer(ChannelInterval);
                device2Timer.Elapsed += Device2Timer_Elapsed;
                diTimer = new System.Timers.Timer(Settings.Default.diIntervalMs);
                diTimer.Elapsed += DiTimer_Elapsed;
                CreateChannel(10, 8, 80);
                CreateSwitchRead(4, 4, 16);
                CreateSwitchWrite(4, 4, 16);
                this.Cmb_Com.SelectedIndex = Items.IndexOf(Settings.Default.COM);
                instantDiCtrl = new InstantDiCtrl();
                instantDoCtrl = new InstantDoCtrl();
                (new Task(() => StartDevice1Timer())).Start();
                Thread.Sleep(25);
                (new Task(() => StartDevice2Timer())).Start();
                diTimer.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("初始化失败！\r\n" + e.Message);
            }
        }

        private void DiTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ReadDi();
        }

        private void Device1Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            //isDevice1TimerEnd = true;
            //if (isDevice1Receive)
            StartDevice1Timer();
        }

        private void Device2Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StartDevice2Timer();
        }

        private static void StartDevice1Timer()
        {
            while (!isConnect)
            {
                Thread.Sleep(ChannelInterval);
            }
            SerialPortHelper.SendData(SendDataType.Device1);
            isDevice1Receive = false;
            isDevice1TimerEnd = false;
        }

        private static void StartDevice2Timer()
        {
            while (!isConnect)
            {
                Thread.Sleep(ChannelInterval);
            }
            SerialPortHelper.SendData(SendDataType.Device2);
            isDevice2Receive = false;
            isDevice2TimerEnd = false;
        }


        private void CreateSwitchRead(int rows, int columns, int max)
        {

            for (int i = 0; i < rows; i++)
            {
                Grid_SwitchRead.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columns; j++)
            {
                Grid_SwitchRead.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }


            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var index = row * columns + col + 1;
                    if (index > max)
                        break;
                    var groupData = new SwitchPropertyData() { Value = false };
                    SwitchReadData.Add(groupData);
                    var groupBox = new GroupBox
                    {
                        Margin = new Thickness(5),
                        BorderBrush = new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)),//设置为#4CAF50
                        Header = new TextBlock
                        {
                            Text = "CH" + index,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 14
                        },
                        Content = new StatusLight()
                        {
                            Width = 55,
                            Height = 55,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                        }
                    };
                    var statusLight = (StatusLight)groupBox.Content;
                    statusLight.SetBinding(StatusLight.ValueProperty, new System.Windows.Data.Binding("Value")
                    {
                        Source = groupData,
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    Grid.SetRow(groupBox, row);
                    Grid.SetColumn(groupBox, col);
                    Grid_SwitchRead.Children.Add(groupBox);
                }
            }
        }
        private void CreateSwitchWrite(int rows, int columns, int max)
        {
            for (int i = 0; i < rows; i++)
            {
                Grid_SwitchWrite.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columns; j++)
            {
                Grid_SwitchWrite.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var index = row * columns + col + 1;
                    if (index > max)
                        break;
                    var groupData = new SwitchPropertyData() { Value = false };
                    SwitchWriteData.Add(groupData);
                    var groupBox = new GroupBox
                    {
                        Margin = new Thickness(5),
                        BorderBrush = new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)),//设置为#4CAF50
                        Header = new TextBlock
                        {
                            Text = "CH" + index,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 14
                        },
                        Content = new StatusLight()
                        {
                            Width = 55,
                            Height = 55,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                        }
                    };
                    var statusLight = (StatusLight)groupBox.Content;
                    statusLight.SetBinding(StatusLight.ValueProperty, new System.Windows.Data.Binding("Value")
                    {
                        Source = groupData,
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    });
                    statusLight.Click += StatusLightButtonClickHandler;// StatusLightMouseLeftButtonUpHandler;
                    Grid.SetRow(groupBox, row);
                    Grid.SetColumn(groupBox, col);
                    Grid_SwitchWrite.Children.Add(groupBox);
                }
            }
        }

        private void StatusLightButtonClickHandler(object sender, RoutedEventArgs e)
        {
            var statusLight = sender as StatusLight;
            statusLight.Value = !statusLight.Value;
            WriteDi();//发送数据
        }

        /*
        private void CreateSwitchRead()//int rows, int columns, int max)
        {
            var grid1 = AddSwitchChannel(0);
            Grid.SetColumn(grid1, 0);
            Grid_SwitchRead.Children.Add(grid1);
            var grid2 = AddSwitchChannel(8);
            Grid.SetColumn(grid2, 1);
            Grid_SwitchRead.Children.Add(grid2);
        }

        private void CreateSwitchWrite(int rows, int columns, int max)
        {
            var grid1 = AddSwitchChannel(0, true);
            Grid.SetColumn(grid1, 0);
            Grid_SwitchWrite.Children.Add(grid1);
            var grid2 = AddSwitchChannel(8, true);
            Grid.SetColumn(grid2, 1);
            Grid_SwitchWrite.Children.Add(grid2);
        }

        private Grid AddSwitchChannel(int startIndex = 0, bool canClick = false)
        {
            // 创建嵌套Grid
            Grid nestedGrid = new Grid
            {
                Margin = new Thickness(10),
                //Background = new SolidColorBrush(Colors.LightGray)
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            // 定义行和列
            for (int i = 0; i < 9; i++)
            {
                nestedGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < 11; j++)
            {
                double w = 2;

                if (j == 10)
                    w = 4;
                else if (j == 5)
                    w = 0.5;
                else
                    w = 2;
                nestedGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(w, GridUnitType.Star) });
            }

            // 添加标题行内容
            AddTextBlock(nestedGrid, "通道", 0, 0);

            for (int j = 1; j <= 4; j++)
            {
                AddTextBlock(nestedGrid, (9 - j).ToString(), 0, j);
            }

            for (int j = 6; j <= 9; j++)
            {
                AddTextBlock(nestedGrid, (9 - j + 1).ToString(), 0, j);
            }

            AddTextBlock(nestedGrid, "Hex", 0, 10);

            // 添加其余8行内容
            for (int i = 1; i <= 8; i++)
            {
                // 添加通道编号
                AddTextBlock(nestedGrid, (startIndex + i).ToString(), i, 0);

                // 添加状态灯
                for (int j = 1; j <= 8; j++)
                {
                    StatusLight statusLight = new StatusLight()
                    {
                        Width = 30,
                        Height = 30,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };
                    if (canClick)
                        statusLight.MouseDoubleClick += StatusLightMouseLeftButtonDownHandler;
                    Grid.SetRow(statusLight, i);
                    Grid.SetColumn(statusLight, j >= 5 ? j + 1 : j);
                    nestedGrid.Children.Add(statusLight);
                }

                // 添加十六进制数值
                AddTextBlock(nestedGrid, "0x" + (i * 10).ToString("X2"), i, 10);
            }

            return nestedGrid;
        }*/

        private void AddTextBlock(Grid grid, string text, int row, int column)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = text,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0)
            };

            Grid.SetRow(textBlock, row);
            Grid.SetColumn(textBlock, column);
            grid.Children.Add(textBlock);
        }


        private void CreateChannel(int rows, int columns, int max)
        {
            for (int i = 0; i < rows; i++)
            {
                Grid_PCI1622C.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columns; j++)
            {
                Grid_PCI1622C.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            //var random = new Random();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var index = row * columns + col + 1;
                    if (index > max)
                        break;
                    var groupData = new ChannelPropertyData() { Value = 0 };
                    ChannelData.Add(groupData);
                    var groupBox = new GroupBox
                    {
                        Margin = new Thickness(5),
                        BorderBrush = new SolidColorBrush(Color.FromArgb(255, 76, 175, 80)),//设置为#4CAF50
                        Header = new TextBlock
                        {
                            Text = ((index - 1) / 64 + 1) + "-CH" + index,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 16
                        },
                        Content = new TextBlock
                        {
                            //Text = (random.Next(0, 100000) / 1000.0).ToString("0.000"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 22,
                            Foreground = new SolidColorBrush(Colors.Green)
                        }
                    };
                    // 绑定TextBlock的Text属性
                    var textBlock = (TextBlock)groupBox.Content;
                    textBlock.SetBinding(TextBlock.TextProperty, new System.Windows.Data.Binding("Value")
                    {
                        Source = groupData
                    });
                    Grid.SetRow(groupBox, row);
                    Grid.SetColumn(groupBox, col);
                    Grid_PCI1622C.Children.Add(groupBox);
                }
            }
        }


        private void ReceiveDataAction(DataType dataType, DataModel data)
        {
            try
            {
                switch (data.DataType)
                {
                    case DataType.PCI1622C:
                        var pic1622CData = data as PCI1622C;
                        if (pic1622CData == null)
                            break;
                        
                        if (pic1622CData.DeviceNumber == 1)
                        {
                            device1Data.Add(pic1622CData.Data);
                            if (device1Data.Count >= ChannelShowCount)
                            {
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    isDevice1Receive = true;
                                    for (int i = 0; i < 64; i++)
                                    {
                                        ChannelData[i].Value = device1Data.Select(row => row[i]).Average();
                                    }
                                }));
                                device1Data.Clear();
                            }
                        }
                        else if (pic1622CData.DeviceNumber == 2)
                        {
                            device2Data.Add(pic1622CData.Data);
                            if (device2Data.Count >= ChannelShowCount)
                            {

                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    isDevice2Receive = true;
                                    for (int i = 64; i < ChannelData.Count; i++)
                                    {
                                        ChannelData[i].Value = device2Data.Select(row => row[i - 64]).Average();
                                    }
                                }));
                                device2Data.Clear();
                            }
                        }
                        break;
                    case DataType.PCI1730U:
                        break;
                    case DataType.Unknown:
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                LogHelper.Trace("更新界面数据失败！DataType:" + data.DataType + "R:" + e.Message);
            }


        }


        private void TB_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.TB_Start.IsChecked == true)
                {
                    this.Grb_AnalogRead.IsEnabled = true;
                    Cmb_Com.IsEnabled = false;
                    SerialPortHelper.Open();
                    IsConnect = true;
                    device1Timer.Start();
                    device2Timer.Start();
                }
                else
                {
                    this.Grb_AnalogRead.IsEnabled = false;
                    Cmb_Com.IsEnabled = true;
                    SerialPortHelper.Close();
                    device1Timer.Stop();
                    device2Timer.Stop();
                    IsConnect = false;

                }
            }
            catch (Exception err)
            {
                LogHelper.Error("开始点击开始按钮异常！R:" + err.Message);
                MessageBox.Show("发生未知异常！请重启软件。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Cmb_Com_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Cmb_Com.SelectedIndex < 0 || Cmb_Com.SelectedIndex >= Items.Count)
                    return;
                Settings.Default.COM = Items[Cmb_Com.SelectedIndex];
                Settings.Default.Save();
                SerialPortHelper.Init(ReceiveDataAction, Settings.Default.COM, Settings.Default.BaudRate);// portName: "COM2");
            }
            catch (Exception err)
            {
                LogHelper.Error("Cmb_Com_SelectionChanged异常！R:" + err.Message);
                MessageBox.Show("发生未知异常！请重启软件。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static bool BioFailed(ErrorCode err)
        {
            return err < ErrorCode.Success && err >= ErrorCode.ErrorHandleNotValid;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            instantDiCtrl.SelectedDevice = new DeviceInformation(deviceDescription);
            instantDoCtrl.SelectedDevice = new DeviceInformation(deviceDescription);
            //if (BioFailed(errorCode))
            //{
            //    MessageBox.Show("设备连接失败！请检查设备并重启软件。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            //ReadDi();

        }

        private void ReadDi()
        {
            byte[] buffer = new byte[64];
            errorCode = instantDiCtrl.Read(startPort, portCount, buffer);
            //errorCode = instantDiCtrl.ReadBit(startPort, bit, out data);
            if (!BioFailed(errorCode))
            {
                for (int i = 0; i < 8; i++)
                {
                    SwitchReadData[i].Value = (buffer[0] & (1 << i)) != 0;
                    SwitchReadData[i + 8].Value = (buffer[1] & (1 << i)) != 0;
                }
            }
            else
                LogHelper.Trace("ReadDi失败！R:" + errorCode.ToString());
        }

        private void WriteDi()
        {
            byte[] buffer = new byte[64];
            for (int i = 0; i < 8; i++)
            {
                if (SwitchReadData[i].Value)
                    buffer[0] |= (byte)(1 << i);
                if (SwitchReadData[i + 8].Value)
                    buffer[1] |= (byte)(1 << i);
            }
            errorCode = instantDoCtrl.Write(startPort, portCount, buffer);
            if (BioFailed(errorCode))
                LogHelper.Trace("WriteDi失败！R:" + errorCode.ToString());
        }

    }

}
