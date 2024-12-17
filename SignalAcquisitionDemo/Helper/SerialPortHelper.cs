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
using SignalAcquisitionDemo.Models;

namespace SignalAcquisitionDemo.Helper
{
    public class SerialPortHelper
    {
        static SerialPort serialPort;
        static Action<DataType, DataModel> ReceiveDataAction;
        static List<byte[]> bytes = new List<byte[]>();
        static object lockObj = new object();

        public static void Init(Action<DataType, DataModel> receiveDataAction, string portName = "COM1", int baudRate = 921600)
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
                LogHelper.Error("初始化串口失败！R:"+e.Message);
                MessageBox.Show("初始化串口失败！" + e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                LogHelper.Error("串口打开异常! R: "+ex.Message);
            }
        }

        public static void Close()
        {
            try
            {
                // 关闭串口
                if(serialPort!=null)
                serialPort.Close();
                LogHelper.Trace("串口关闭。");
            }
            catch (Exception ex)
            {
                LogHelper.Error("串口关闭异常! R: "+ex.Message);
            }
        }

        public static void SendData(SendDataType dataType, List<byte> data = null)
        {
            try
            {
                byte[] sendData = null;
                switch (dataType)
                {
                    case SendDataType.Device1:
                        sendData = AnalysisData.SendDeviceData(1);
                        break;
                    default:
                        break;
                }

                // 发送数据
                if (sendData != null && sendData.Length > 0)
                    serialPort.Write(sendData, 0, sendData.Length);
                LogHelper.Trace("发送数据:{0}", sendData);
            }
            catch (Exception ex)
            {
                LogHelper.Error("数据发送异常! R: "+ex.Message);
            }
        }


        public static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
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
                        DataType type;
                        // 解析收到的数据
                        var data = AnalysisData.GetData(buffer, out type);
                        if (data == null)
                        {
                            LogHelper.Trace("解析收到的包失败！ 包内容为：{0}", buffer);
                            return;
                        }
                        if(ReceiveDataAction!=null)
                        ReceiveDataAction.BeginInvoke(type, data, null, null);
                    }
                    catch (Exception err)
                    {
                        LogHelper.Error("接收数据时发生异常。R:"+err);
                    }
                }
            }
            catch (Exception err)
            {
                LogHelper.Error("接收数据时发生异常。R:"+err);
            }
        }





    }

}
