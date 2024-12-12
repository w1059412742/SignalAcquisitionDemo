using SignalAcquisitionDemo.Helper;
using SignalAcquisitionDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Markup;

namespace SignalAcquisitionDemo.Helper
{
    public class DataHelper
    {

        /// <summary>
        /// 解包数据
        /// </summary>
        /// <param name="receivedData"></param>
        /// <returns></returns>
        public static DataModel UnpackData(byte[] receivedData)
        {
            if (receivedData.Length < 2)
                return null;

            if (receivedData[1] == 0x03)
            {
                if (BitConverter.ToUInt16(receivedData,receivedData.Length-2) != Crc(receivedData.Take(receivedData.Length - 2).ToArray(), (byte)(receivedData.Length - 2)))
                {
                    return null;
                }
                var dataModel = new PCI1622C();
                dataModel.DataType = DataType.PCI1622C;
                dataModel.DeviceNumber = receivedData[0];
                dataModel.DataLength = receivedData[2];
                //dataModel.Data = GetPCI1622CData(receivedData.Skip(3).Take(dataModel.DataLength).ToArray());
                dataModel.Data = GetPCI1622CData(receivedData.Skip(3).Take((receivedData[0]==1?64:16)*4).ToArray());
                return dataModel;
            }
            return null;
        }

        private static List<float> GetPCI1622CData(byte[] bytes)
        {
            var data = new List<float>();
            for (int i = 0; i < bytes.Length; i += 4)
            {
                data.Add(ParsingData.ParseFloat(bytes.Skip(i).Take(4).ToArray()));
            }
            return data;
        }
        public static uint Crc(byte[] data, byte length)
        {
            uint check = 0;
            uint CRCreg = 0xFFFF;

            for (int i = 0; i < length; i++)
            {
                CRCreg ^= data[i];

                for (int j = 1; j <= 8; j++)
                {
                    if ((CRCreg & 0x01) != 0)
                    {
                        CRCreg = (CRCreg >> 1) ^ 0xA001;
                    }
                    else
                    {
                        CRCreg = CRCreg >> 1;
                    }
                }
            }

            return CRCreg;
        }

        public static byte[] PackDeviceData(int deviceNumber)
        {
            var data = new List<byte>();
            if (deviceNumber == 1)
                data.AddRange(new byte[] { 0x01, 0x03, 0x00, 0x00, 0x00, 0x7F });
            else if (deviceNumber == 2)
                data.AddRange(new byte[] { 0x02, 0x03, 0x00, 0x00, 0x00, 0x1F });

            if (data.Count > 0)
            {
                var crc = Crc(data.ToArray(), (byte)data.Count);
                data.Add((byte)(crc & 0xFF));
                data.Add((byte)((crc >> 8) & 0xFF));
            }
            return data.ToArray();
        }
    }
}
