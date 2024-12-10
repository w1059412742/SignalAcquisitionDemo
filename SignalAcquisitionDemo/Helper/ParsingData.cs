using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SignalAcquisitionDemo.Helper
{
    public class ParsingData
    {
        public static float ParseFloat(byte[] data)
        {

            ValReg val = new ValReg();

            // 将16进制值分别放入
            val.data3 = data[0];
            val.data2 = data[1];
            val.data1 = data[2];
            val.data0 = data[3];

            // 输出浮点值
            Console.WriteLine("fval十进制的值为={0}", val.fval);
            return val.fval;
        }
    }


    [StructLayout(LayoutKind.Explicit)]
        public struct ValReg
    {
        [FieldOffset(0)]
        public byte data0;
        [FieldOffset(1)]
        public byte data1;
        [FieldOffset(2)]
        public byte data2;
        [FieldOffset(3)]
        public byte data3;

        [FieldOffset(0)]
        public float fval;
    }
}
