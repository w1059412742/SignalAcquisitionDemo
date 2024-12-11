using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using SignalAcquisitionDemo.Helper;
using SignalAcquisitionDemo.Models;
using SignalAcquisitionDemo.Properties;

namespace SignalAcquisitionDemo.Helper
{
    public class AnalysisData
    {
        
        public static DataModel GetData(byte[] data, out DataType dataType)
        {
            dataType = DataType.Unknown;
            try
            {
                return DataHelper.UnpackData(data); 
            }
            catch (Exception ex)
            {
                LogHelper.Error("解析数据包失败！请开启更精细的log输出来抓取信息。R:"+ex.Message);
                return null;
            }
        }



        public static byte[] SendDeviceData(int deviceNumber)
        {
            return DataHelper.PackDeviceData(deviceNumber);
        }
    }

}
