using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistortionAss
{
    public static class AssKit
    {
        public static string ToAssFloat(float floatNumber)
        {
            return Math.Round(floatNumber, 2, MidpointRounding.ToEven).ToString("0.##");
        }
    }
}
