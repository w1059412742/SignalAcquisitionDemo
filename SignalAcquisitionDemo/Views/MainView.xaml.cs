using InteractiveDataDisplay.WPF;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IO.Ports;

namespace SignalAcquisitionDemo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : UserControl
    {
        public List<Point> Data1 = new List<Point>();
        public List<Point> Data2 = new List<Point>();
        public List<Point> Data3 = new List<Point>();
        public List<Point> Data4 = new List<Point>();
        private List<string> Items = new List<string>() { "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9" };
        public Timer timer = null;
        public Timer sendControlTimer = null;
        private bool isConnect;



        public bool IsConnect
        {
            get { return isConnect; }
            set
            {
                isConnect = value;
                this.SL_Status.Value = value;
            }
        }


        public MainView()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Items= SerialPort.GetPortNames().ToList();
            Cmb_Com.ItemsSource = Items;
            this.Cmb_Com.SelectedIndex = Items.IndexOf(Settings.Default.COM);
            IsConnect = false;
            CreateChannel(8, 8);
            CreateSwitchRead(4, 4);
            CreateSwitchWrite(4, 4);

        }

        private void CreateSwitchWrite(int rows, int columns)
        {

            for (int i = 0; i < rows; i++)
            {
                Grid_SwitchWrite.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columns; j++)
            {
                Grid_SwitchWrite.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            var random = new Random();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var groupBox = new GroupBox
                    {
                        Margin = new Thickness(5),
                        Header = new TextBlock
                        {
                            Text = $"1-CH{row * columns + col + 1}",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 14
                        },
                        Content = new TextBlock
                        {
                            Text = (random.Next(0, 100000) / 1000.0).ToString("0.000"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 18
                        }
                    };

                    Grid.SetRow(groupBox, row);
                    Grid.SetColumn(groupBox, col);
                    Grid_SwitchWrite.Children.Add(groupBox);
                }
            }
        }

        private void CreateSwitchRead(int rows, int columns)
        {

            for (int i = 0; i < rows; i++)
            {
                Grid_SwitchRead.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columns; j++)
            {
                Grid_SwitchRead.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            var random = new Random();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var groupBox = new GroupBox
                    {
                        Margin = new Thickness(5),
                        Header = new TextBlock
                        {
                            Text = $"1-CH{row * columns + col + 1}",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 14
                        },
                        Content = new TextBlock
                        {
                            Text = (random.Next(0, 100000) / 1000.0).ToString("0.000"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 18
                        }
                    };

                    Grid.SetRow(groupBox, row);
                    Grid.SetColumn(groupBox, col);
                    Grid_SwitchRead.Children.Add(groupBox);
                }
            }
        }

        private void CreateChannel(int rows, int columns)
        {
            for (int i = 0; i < rows; i++)
            {
                Grid_PCI1622C.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < columns; j++)
            {
                Grid_PCI1622C.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            var random = new Random();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    var groupBox = new GroupBox
                    {
                        Margin = new Thickness(5),
                        Header = new TextBlock
                        {
                            Text = $"1-CH{row * columns + col + 1}",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 14
                        },
                        Content = new TextBlock
                        {
                            Text = (random.Next(0, 100000) / 1000.0).ToString("0.000"),
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 18
                        }
                    };

                    Grid.SetRow(groupBox, row);
                    Grid.SetColumn(groupBox, col);
                    Grid_PCI1622C.Children.Add(groupBox);
                }
            }
        }


        /* Cmb_Com.ItemsSource = Items;
this.Cmb_Com.SelectedIndex = Items.IndexOf(Settings.Default.COM);
IsConnect = false;
SerialPortHelper.Init(ReceiveDataAction, Settings.Default.COM, Settings.Default.BaudRate);// portName: "COM2");
timer = new Timer();
timer.Interval = 10 * 1000;
timer.Elapsed += Timer_Elapsed;

sendControlTimer = new Timer(3 * 1000);
sendControlTimer.Elapsed += SendControTimerCallBack;
// InitLinG();
}

private void InitLinG()
{
LineG1.PlotOriginX = 0;
LineG1.PlotOriginY = -20;
LineG1.PlotWidth = 2 * Settings.Default.period_1;
LineG1.PlotHeight = 40;
}

private void Timer_Elapsed(object sender, ElapsedEventArgs e)
{
this.Dispatcher.Invoke(() =>
{
    timer.Stop();
    IsConnect = false;
});
LogHelper.Error("与下位机断开连接！");
MessageBox.Show("与下位机断开连接！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
}

private void ReceiveDataAction(DataType dataType, List<double> data)
{

this.Dispatcher.Invoke(() =>
{
    timer.Stop();
    IsConnect = true;
    timer.Start();
    if (BB_Show.IsChecked != true)
        return;
    try
    {
        switch (dataType)
        {
            case DataType.Original:
                SaveDataToFile(data, savePath1);
                Data1 = SetOriginalData(data, Data1, LineG1);
                break;
            case DataType.IIR:
                SaveDataToFile(data, savePath4);
                Data4 = SetOriginalData(data, Data4, LineG4);
                break;
            case DataType.ControlReply:
                sendControlTimer.Stop();
                this.Btn_Calibration.IsEnabled = true;
                MessageBox.Show("校准成功。");
                break;
            case DataType.VPP:
                Data3 = SetData3(data, Data3, LineG3);
                SaveDataToFile(new List<double>() { data[0], (double)this.Lbl_Value3.Content }, savePath3);
                var data2 = SetOriginalData2(Data3, LineG2);
                SaveDataToFile(data2, savePath2);
                break;
            default:
                break;
        }

    }
    catch (Exception err)
    {
        MessageBox.Show(err.Message);
    }
});

}

private void SaveDataToFile(List<double> data, string savePath)
{
try
{
    if (!string.IsNullOrEmpty(savePath))
    {
        var path = Path.GetDirectoryName(savePath);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        File.AppendAllLines(savePath, new string[] { string.Join(",", data) });
    }
}
catch (Exception err)
{
    LogHelper.Error($"保存数据失败！R:{err.Message}");
}

}


private List<Point> SetData3(List<double> data, List<Point> lastData, LineGraph lineG)
{
const int width = 1024;
double value = 0;
var originValue = data[0];
if (originValue <= Settings.Default.threshold_1)
    value = 0;
else if (originValue <= Settings.Default.threshold_2)
    value = Settings.Default.k_1 * originValue + Settings.Default.b_1;
else
    value = Settings.Default.k_2 * originValue + Settings.Default.b_2;

this.Lbl_Value3.Content = value;
lastData.Add(new Point(lastData.Count, value));

var xs = lastData.Select(_ => _.X).ToList();
var ys = lastData.Select(_ => _.Y).ToList();
lineG.Plot(xs, ys);


return lastData;
}

private List<Point> SetData4(List<double> data, List<Point> data4, LineGraph lineG)
{
const int width = 1024;
if (Settings.Default.index_2 < data.Count)
{
    var value = Settings.Default.k_2 * data[Settings.Default.index_2] + Settings.Default.b_2;
    data4.Add(new Point(data4.Count, value));
    SaveDataToFile(new List<double>() { value }, savePath4);
}

var xs = data4.Select(_ => _.X).ToList();
var ys = data4.Select(_ => _.Y).ToList();
lineG.Plot(xs, ys);


var max = data4.Max(_ => _.Y);
var min = data4.Min(_ => _.Y);
var h = (max - min) > 1 ? (max - min) : 1;
lineG.PlotHeight = h;
lineG.PlotOriginY = (max - min) == 0 ? (min - 1.0 / 2) : min;
lineG.PlotWidth = width;
lineG.PlotOriginX = data4.Count > width ? data4.Count - width : 0;

return data4;
}


private List<Point> SetData4(List<double> data, List<Point> data4, LineGraph lineG)
{
var max = double.MaxValue - UInt16.MaxValue;
if (data4.Any(_ => _.Y > max))
    data4 = new List<Point>();
var count = data.Count;
for (int i = 0; i < count; i++)
{
    if (i < data4.Count)
        data4[i] = new Point(data4[i].X, data4[i].Y + data[i]);
    else
        data4.Add(new Point((i + 1) / 4.096, data[i]));
}
var xs = data4.Select(_ => _.X).ToList();
var ys = data4.Select(_ => _.Y).ToList();
lineG.Plot(xs, ys);

return data4;
}

private List<Point> SetOriginalData(List<double> data, List<Point> points, LineGraph lineG)
{
double maxY = data[0];
double minY = data[0];
var count = data.Count;
var originX = points.Count * 2.0 / count;
for (int i = 0; i < count; i++)
{
    points.Add(new Point(2.0 * (i + 1) / count + originX, data[i]));
}

if (points.Count > Settings.Default.period_1 * count)
{
    points.RemoveRange(0, points.Count - Settings.Default.period_1 * count);
    for (int i = 0; i < points.Count; i++)
        points[i] = new Point(2.0 * (i + 1) / count, points[i].Y);
}


var xs = points.Select(_ => _.X).ToList();
var ys = points.Select(_ => _.Y).ToList();

lineG.Plot(xs, ys);


if (maxY < 0.5 && minY > -0.5)
{
    lineG.PlotHeight = maxY - minY; //(maxY - minY) * (1 + 0.1 * 2);
    lineG.PlotOriginY = minY; //minY - (maxY - minY)*0.1;
}
else
{
    lineG.PlotHeight = 5;
    lineG.PlotOriginY = -2.5;
}

lineG.PlotOriginX = 0;
lineG.PlotWidth = 2;
//lineG.PlotWidth += lineG.PlotWidth * 0.1;
return points;
}


private List<double> SetOriginalData2(List<Point> points, LineGraph lineG)
{
//double maxY = points[0].Y;
//double minY = points[0].Y;
var count = points.Count;
var point2 = new List<Point>();
var data2 = new List<double>();
double lastValue = 0;
for (int i = 0; i < count; i++)
{
    var y = points[i].Y * Settings.Default.e / Settings.Default.f;
    point2.Add(new Point(points[i].X, (int)(y + lastValue)));
    lastValue = y - (int)y;
    data2.Add(y);
}

if (point2.Any())
{
    var lastPoint = point2.LastOrDefault();
    point2[point2.Count - 1] = new Point(lastPoint.X, Math.Round((data2.LastOrDefault() + lastValue), 0));
}

var xs = point2.Select(_ => _.X).ToList();
var ys = point2.Select(_ => _.Y).ToList();

lineG.Plot(xs, ys);


if (maxY < 0.5 && minY > -0.5)
{
    lineG.PlotHeight = maxY - minY; //(maxY - minY) * (1 + 0.1 * 2);
    lineG.PlotOriginY = minY; //minY - (maxY - minY)*0.1;
}
else
{
    lineG.PlotHeight = 5;
    lineG.PlotOriginY = -2.5;
}

lineG.PlotOriginX = 0;
lineG.PlotWidth = 2;
//lineG.PlotWidth += lineG.PlotWidth * 0.1;
return data2;
}


private List<Point> SetData(List<double> data, List<Point> lastData, LineGraph lineG)
{

var count = data.Count;
var points = new List<Point>();
for (int i = 0; i < count; i++)
{
    points.Add(new Point((i + 1) / 4.096, data[i]));
}


var xs = points.Select(_ => _.X).ToList();
var ys = points.Select(_ => _.Y).ToList();
lineG.Plot(xs, ys);

//lineG.PlotWidth += lineG.PlotWidth * 0.1;
return points;
}

private void TB_Start_Click(object sender, RoutedEventArgs e)
{
try
{
    if (this.TB_Start.IsChecked == true)
    {
        SerialPortHelper.Open();
        timer.Start();
        this.Btn_Calibration.IsEnabled = true;
        Cmb_Com.IsEnabled = false;
        //InitLinG();
    }
    else
    {
        Cmb_Com.IsEnabled = true;
        SerialPortHelper.Close();
        timer.Stop();
        sendControlTimer.Stop();
        this.Btn_Calibration.IsEnabled = false;
        IsConnect = false;
    }
}
catch (Exception err)
{
    LogHelper.Error($"开始点击开始按钮异常！R:{err.Message}");
    MessageBox.Show("发生未知异常！请重启软件。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
}

}


private void Button_Calibration_Click(object sender, RoutedEventArgs e)
{
try
{
    SerialPortHelper.SentData(SendDataType.Control,new List<byte>() { (byte)(Settings.Default.control_threshold & 0x00FF) , (byte)((Settings.Default.control_threshold & 0xFF00) >> 8) });
    //计时
    sendControlTimer.Start();
    this.Btn_Calibration.IsEnabled = false;
}
catch (Exception err)
{
    LogHelper.Error($"Button_Calibration_Click异常！R:{err.Message}");
    MessageBox.Show("发生未知异常！请重启软件。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
}
}

private void SendControTimerCallBack(object sender, ElapsedEventArgs e)
{
this.Dispatcher.Invoke(() =>
{
    try
    {
        sendControlTimer.Stop();
        timer.Stop();
        IsConnect = false;
        this.Btn_Calibration.IsEnabled = true;
        LogHelper.Error("校准失败！与下位机通信异常！");
        MessageBox.Show("校准失败！与下位机通信异常！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
    }
    catch (Exception err)
    {
        LogHelper.Error($"SendControTimerCallBack异常！R:{err.Message}");
        MessageBox.Show("发生未知异常！请重启软件。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
    }
});
}


private void BB_Save_Click(object sender, RoutedEventArgs e)
{
if (this.BB_Save.IsChecked == true)
{
    var timePath = DateTime.Now.ToString("yyyyMMdd-HHmmss");
    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", timePath, timePath);
    this.savePath1 = path + "_Channel1.csv";
    this.savePath2 = path + "_Channel2.csv";
    this.savePath3 = path + "_Channel3.csv";
    var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data", timePath, "Channel");
    this.savePath1 = path + "1.csv";
    this.savePath2 = path + "2.csv";
    this.savePath3 = path + "3.csv";
    this.savePath4 = path + "4.csv";

    var path3 = Path.GetDirectoryName(savePath3);
    if (!Directory.Exists(path3))
        Directory.CreateDirectory(path3);
    File.AppendAllLines(savePath3, new string[] { "Peak-to-peak,Y" });
}
else
{
    this.savePath1 = null;
    this.savePath2 = null;
    this.savePath3 = null;
    this.savePath4 = null;
}

}



private void Button_Click(object sender, RoutedEventArgs e)
{
var data = new List<double>();
Random random = new Random();

for (int i = 0; i < 50; i++)
{
    data.Add(random.Next(0, 500) / 10.0);
}
ReceiveDataAction(DataType.Original, data);
//ReceiveDataAction(DataType.Original, data);
//ReceiveDataAction(DataType.Original, data);
//ReceiveDataAction(DataType.Original, data);

ReceiveDataAction(DataType.FFT, data);
//ReceiveDataAction(DataType.FFT, data);
//ReceiveDataAction(DataType.FFT, data);
//ReceiveDataAction(DataType.FFT, data);

ReceiveDataAction(DataType.IIR, data);
//ReceiveDataAction(DataType.FIR, data);
//ReceiveDataAction(DataType.FIR, data);
//ReceiveDataAction(DataType.FIR, data);
}

private void LineG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
{
if (sender is LineGraph lineG && lineG.Parent is Grid gd && gd.Parent is Chart chart && chart.Title is TextBlock tb)
{
    Point mousePosition = e.GetPosition(lineG);
    var x = Math.Round(lineG.XFromLeft(mousePosition.X), 2);
    var y = Math.Round(lineG.YFromTop(mousePosition.Y), 2);
    tb.Text = tb.Text.Substring(0, 3) + $" (x:{x}, y:{y})";
}
}

private void Chart_PreviewMouseWheel(object sender, MouseButtonEventArgs e)
{
e.Handled = true;//不让控件相应滚轮事件
}

private void Chart_PreviewMouseWheel(object sender, MouseEventArgs e)
{
e.Handled = true;//不让控件相应滚轮事件
}


private void Chart1_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
{
InitLinG();
e.Handled = true;
}

private void UserControl_Loaded(object sender, RoutedEventArgs e)
{
Task.Delay(100).ContinueWith(t =>
{
    this.Dispatcher.Invoke(() =>
    {
        InitLinG();
    });
});
Button_Calibration_Click(null, null);
}

private void SL_Status_MouseDoubleClick(object sender, MouseButtonEventArgs e)
{
/*var data = new List<double>();
for (int i = 0; i < 500; i++)
{
    data.Add(i);
}
Data1 = SetOriginalData(data, Data1, LineG1);*/
        /* //var path = "C:\\Users\\DYJ\\Documents\\WeChat Files\\wxid_tex8nak4r31922\\FileStorage\\File\\2024-06\\new8.txt";
         var path = "C:\\Users\\DYJ\\Documents\\WeChat Files\\wxid_tex8nak4r31922\\FileStorage\\File\\2024-06\\new10.txt";
         var context = File.ReadAllText(path);
         var bytes = context
         .Split(' ')
         .Select(hex => Convert.ToByte(hex, 16))
         .ToArray();
         SerialPortHelper.test(bytes);*/



        private void Cmb_Com_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           /* try
            {
                if (Cmb_Com.SelectedIndex < 0 || Cmb_Com.SelectedIndex >= Items.Count)
                    return;
                Settings.Default.COM = Items[Cmb_Com.SelectedIndex];
                Settings.Default.Save();
                SerialPortHelper.Init(ReceiveDataAction, Settings.Default.COM, Settings.Default.BaudRate);// portName: "COM2");
            }
            catch (Exception err)
            {
                LogHelper.Error($"Cmb_Com_SelectionChanged异常！R:{err.Message}");
                MessageBox.Show("发生未知异常！请重启软件。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
*/
    }

        private void TB_Start_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
