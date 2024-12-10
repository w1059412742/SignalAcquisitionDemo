using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalAcquisitionDemo.Helper;
using SignalAcquisitionDemo.Properties;

namespace SignalAcquisitionDemo.Models
{
    public class AnalysisData
    {
        /*
        public static List<double> GetData(byte[] data, out DataType dataType)
        {
            dataType = DataType.Original;
            try
            {
                var array = DataHelper.UnpackData(data);
                if (array == null) return null;
                if (array[0] == 0x02 && array[1] == 0x01)//原始数据包
                {
                    dataType = DataType.Original;
                    return GetOriginalData(array);
                }
                else if (array[0] == 0x02 && array[1] == 0x02)//FFT包
                {
                    dataType = DataType.FFT;
                    return GetFFTData(array);
                }
                else if (array[0] == 0x02 && array[1] == 0x03)//FIR包
                {
                    dataType = DataType.IIR;
                    return GetFIRData(array);
                }
                else if (array[0] == 0x02 && array[1] == 0x04)//控制回复包
                {
                    dataType = DataType.ControlReply;
                    return GetControlData(array);
                }
                else if (array[0] == 0x02 && array[1] == 0x05)//峰峰值包
                {
                    dataType = DataType.VPP;
                    return GetVPPData(array);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogHelper.Error($"解析数据包失败！请开启更精细的log输出来抓取信息。R:{ex.Message}");
                return null;
            }
        }

        public static byte[] SendData(SendDataType dataType, List<byte> data = null)
        {
            try
            {
                switch (dataType)
                {
                    case SendDataType.Control:
                        return DataHelper.PackData(SetControlData(data).ToArray());

                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error($"组装发送数据包失败！发送类型：{dataType}。R:{ex.Message}");
                return null;
            }

        }



        /// <summary>
        /// 解析原始数据包
        /// </summary>
        /// <param name="hexArray"></param>
        public static List<double> GetOriginalData(byte[] hexArray)
        {
            return GetDataBase(hexArray, Settings.Default.bitNum_1, Settings.Default.PointCount_1, 10.0 / Math.Pow(10, Settings.Default.bitNum_1), true);// 0.001
        }

        public static List<double> GetFFTData(byte[] hexArray)
        {
            return GetDataBase(hexArray, Settings.Default.bitNum_2, Settings.Default.PointCount_2, 10.0 / Math.Pow(10, Settings.Default.bitNum_2));// 2, 1024, 0.1);
        }

        public static List<double> GetFIRData(byte[] hexArray)
        {
            return GetDataBase(hexArray, Settings.Default.bitNum_4, Settings.Default.PointCount_4, 10.0 / Math.Pow(10, Settings.Default.bitNum_4));
        }

        private static List<double> GetVPPData(byte[] hexArray)
        {
            return GetDataBase(hexArray, Settings.Default.bitNum_3, Settings.Default.PointCount_3, 10.0 / Math.Pow(10, Settings.Default.bitNum_3), true);
        }

        public static List<double> GetControlData(byte[] hexArray)
        {

            if (hexArray == null || hexArray.Length <= 8)// 检查数组长度是否符合要求
                return null;
            // 提取包长度
            UInt16 packageLength = BitConverter.ToUInt16(hexArray, 2);

            // 提取CRC32校验值
            UInt32 crc32 = BitConverter.ToUInt32(hexArray, packageLength - 4);
            UInt32 c = DataHelper.GetCrc32(hexArray, 0, packageLength - 4);
            if (c != crc32)
            {
                LogHelper.Trace("校验Crc32失败！data：{0}", hexArray);
                return null;
            }
            var value = BitConverter.ToUInt16(hexArray, 4) & 0x00FF;
            if (value == 0x55)
                return new List<double> { value };
            else
            {
                LogHelper.Trace("接收到的控制回复包标志位不为0x55！data：{0}", hexArray);
                return null;
            }

        }



        public static List<double> GetDataBase(byte[] hexArray, int bitNum, int length, double unit = 0.001, bool isHadNegative = false)
        {
            if (hexArray == null || hexArray.Length <= 4)// 检查数组长度是否符合要求
                return null;

            // 提取包类型和包扩展类型
            byte packageType = hexArray[0];
            byte packageExtensionType = hexArray[1];

            // 提取包长度
            UInt16 packageLength = BitConverter.ToUInt16(hexArray, 2);

            // 提取CRC32校验值
            UInt32 crc32 = BitConverter.ToUInt32(hexArray, packageLength - 4);
            UInt32 c = DataHelper.GetCrc32(hexArray, 0, packageLength - 4);
            if (c != crc32)
            {
                LogHelper.Trace("校验Crc32失败！data：{0}", hexArray);
                return null;
            }

            //解析数据
            List<double> list = new List<double>();
            int startValue = 4;
            int endValue = bitNum * length + startValue;

            for (int i = startValue; i < endValue; i += bitNum)
            {
                var f = (hexArray[i + bitNum - 1] & 0x80) == 0 ? 1 : -1;

                double value = 0;
                if (isHadNegative)
                    hexArray[i + bitNum - 1] &= 0x7F;
                value = ParseByteArrayToIntArray(hexArray, i, bitNum) * unit;
                //value = (bitNum == 2 ? BitConverter.ToUInt16(hexArray, i) : BitConverter.ToUInt32(hexArray, i)) * unit;
                if (isHadNegative)
                    value *= f;
                list.Add(value);
            }
            return list;
        }
        static int ParseByteArrayToIntArray(byte[] bytes, int index, int bitNum)
        {
            if (bytes.Length % bitNum != 0)
                throw new ArgumentException("The byte array length must be a multiple of 3.");
            int result = bytes[index];
            for (int i = 1; i < bitNum; i++)
            {
                result = result | (bytes[index + i] << (8 * i));
            }

            return result;
        }


        /// <summary>
        /// 封装控制包数据
        /// </summary>
        /// <returns></returns>
        public static List<byte> SetControlData(List<byte> data)
        {
            List<byte> binaryData = new List<byte>();

            // 设置包类型和包扩展类型
            binaryData.Add(0x01); // 包类型
            binaryData.Add(0x01); // 包扩展类型

            // 设置包长度
            binaryData.Add(0x0C); // 包长度高字节
            binaryData.Add(0x00); // 包长度低字节

            binaryData.Add(0x55);
            binaryData.Add(0x00);

            binaryData.Add(data[0]);
            binaryData.Add(data[1]);
            //binaryData.Add((byte)(value & 0x00FF));
            //binaryData.Add((byte)((value & 0xFF00) >> 8));

            // 计算CRC32校验
            UInt32 crc32 = DataHelper.GetCrc32(binaryData.ToArray());
            binaryData.AddRange(BitConverter.GetBytes(crc32));
            return binaryData;
        }  */
    }

    public enum DataType
    {
        Original = 0,
        FFT = 1,
        IIR = 2,
        ControlReply = 3,
        VPP = 4
    }

    public enum SendDataType
    {
        Control = 0
    }
      
}
