using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabXand.Core.Extensions
{
    public static class MathExtensions
    {
        public static int ConvertByteToKB(this long value)
        {
            return (int)(value / (Math.Pow(10, 3)));
        }
        public static int ConvertByteToMB(this long value)
        {
            return (int)(value / (Math.Pow(10, 6)));
        }
        public static int ConvertByteToGB(this long value)
        {
            return (int)(value / (Math.Pow(10, 9)));
        }

    }
}
