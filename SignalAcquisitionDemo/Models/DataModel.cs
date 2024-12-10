using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalAcquisitionDemo.Models
{
    public abstract class DataModel
    {
        public DataType DataType { get; set; }
    }

    public class PCI1622C : DataModel
    {
        public int DeviceNumber { get; set; }
        public int DataLength { get; set; }
        public List<float> Data { get; set; }
    }


    public enum DataType
    {
        Unknown = 0,
        PCI1622C = 1,
        PCI1730U = 2,
    }

    public enum SendDataType
    {
        Device1 = 1,
        Device2 = 2
    }

}
