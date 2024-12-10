using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SignalAcquisitionDemo.Helper;

namespace SignalAcquisitionDemo.Models
{
    public class SerialPortHelper
    {
        static SerialPort serialPort;
        static Action<DataType, List<double>> ReceiveDataAction;
        static List<byte[]> bytes = new List<byte[]>();
        static object lockObj = new object();

        public static void Init(Action<DataType, List<double>> receiveDataAction, string portName = "COM1", int baudRate = 921600)
        {
            try
            {
                if (serialPort != null)
                {
                    if (serialPort.IsOpen)
                        serialPort.Close();
                    serialPort.DataReceived -= SerialPort_DataReceived;
                }

                ReceiveDataAction = receiveDataAction;
                serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
                // 开始监听串口数据
                serialPort.DataReceived += SerialPort_DataReceived;
                LogHelper.Trace("串口初始化完成。");
            }
            catch (Exception e)
            {
                LogHelper.Error($"初始化串口失败！R:{e.Message}");
                MessageBox.Show("初始化串口失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void Open()
        {
            try
            {
                serialPort.Open();
                LogHelper.Trace("串口打开成功。");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"串口打开异常! R: {ex.Message}。");
            }
        }

        public static void Close()
        {
            try
            {
                // 关闭串口
                serialPort?.Close();
                LogHelper.Trace("串口关闭。");
            }
            catch (Exception ex)
            {
                LogHelper.Error($"串口关闭异常! R: {ex.Message}。");
            }
        }

        public static void SentData(SendDataType dataType, List<byte> data = null)
        {
           /* try
            {
                var sendData = AnalysisData.SendData(dataType, data);
                // 发送数据
                serialPort?.Write(sendData, 0, sendData.Length);
                LogHelper.Trace("发送数据:{0}", sendData);
            }
            catch (Exception ex)
            {
                LogHelper.Error($"数据发送异常! R: {ex.Message}。");
            }*/
        }


        public static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
          /*  try
            {
                // 读取串口数据
                int bytesToRead = serialPort.BytesToRead;
                byte[] buffer = new byte[bytesToRead];
                serialPort.Read(buffer, 0, bytesToRead);
                LogHelper.Trace("收到数据数据:{0}", buffer);

                lock (lockObj)
                {
                    try
                    {
                        int headerIndex = Array.LastIndexOf(buffer, (byte)0x10);
                        if (headerIndex < 0 
                            || headerIndex + 1 >= buffer.Length 
                            || buffer[headerIndex + 1] != 0x03 
                            || buffer[headerIndex + 1] == 0x03 && (headerIndex>0 && buffer[headerIndex - 1] == 0x10 
                                                                   || headerIndex == 0 && bytes != null && bytes.Count > 0 && bytes.LastOrDefault() is byte[] bs && bs.Length > 0 && bs[bs.Length - 1] == (byte)0x10))
                        {
                            bytes.Add(buffer);
                            return;
                        }
                        else
                        {

                            var b = new byte[headerIndex + 2];
                            Array.Copy(buffer, b, b.Length);
                            bytes.Add(b);

                            if (bytes.Count <= 0)
                                return;
                            var length = bytes.Sum(_ => _.Length);
                            byte[] result = bytes.SelectMany(array => array).ToArray();
                            bytes = new List<byte[]>();
                            if (headerIndex < buffer.Length - 2)
                            {
                                var bb = new byte[buffer.Length - 2 - headerIndex];
                                Array.Copy(buffer, headerIndex + 2, bb, 0, bb.Length);
                                bytes.Add(bb);
                            }

                            DataType type;
                            // 解析收到的数据
                            var data = AnalysisData.GetData(result, out type);
                            if (data == null)
                            {
                                LogHelper.Trace("解析收到的包失败！ 包内容为：{0}", result);
                                return;
                            }
                            ReceiveDataAction?.BeginInvoke(type, data, null, null);
                            
                        }
                    }
                    catch (Exception err)
                    {
                        LogHelper.Error($"接收数据时发生异常。R:{err}");
                    }
                }
            }
            catch (Exception err)
            {
                LogHelper.Error($"接收数据时发生异常。R:{err}");
            }*/
        }

        public static void test(byte[] buffer)
        {
           /* try
            {

                lock (lockObj)
                {
                    try
                    {
                        int headerIndex = Array.LastIndexOf(buffer, (byte)0x10);
                        if (headerIndex < 0
                            || headerIndex + 1 >= buffer.Length
                            || buffer[headerIndex + 1] != 0x03
                            || buffer[headerIndex + 1] == 0x03 && (headerIndex > 0 && buffer[headerIndex - 1] == 0x10
                                                                   || headerIndex == 0 && bytes != null && bytes.Count > 0 && bytes.LastOrDefault() is byte[] bs && bs.Length > 0 && bs[bs.Length - 1] == (byte)0x10))
                        {
                            bytes.Add(buffer);
                            return;
                        }
                        else
                        {

                            var b = new byte[headerIndex + 2];
                            Array.Copy(buffer, b, b.Length);
                            bytes.Add(b);

                            if (bytes.Count <= 0)
                                return;
                            var length = bytes.Sum(_ => _.Length);
                            byte[] result = bytes.SelectMany(array => array).ToArray();
                            bytes = new List<byte[]>();
                            if (headerIndex < buffer.Length - 2)
                            {
                                var bb = new byte[buffer.Length - 2 - headerIndex];
                                Array.Copy(buffer, headerIndex + 2, bb, 0, bb.Length);
                                bytes.Add(bb);
                            }

                            DataType type;
                            // 解析收到的数据
                            var data = AnalysisData.GetData(result, out type);
                            if (data == null)
                            {
                                LogHelper.Trace("解析收到的包失败！ 包内容为：{0}", result);
                                return;
                            }
                            ReceiveDataAction?.BeginInvoke(type, data, null, null);

                        }
                    }
                    catch (Exception err)
                    {
                        LogHelper.Error($"接收数据时发生异常。R:{err}");
                    }
                }
            }
            catch (Exception err)
            {
                LogHelper.Error($"接收数据时发生异常。R:{err}");
            }*/
        }






    }

}
