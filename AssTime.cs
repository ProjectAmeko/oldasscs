using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DistortionAss.Interfaces;

namespace DistortionAss
{
    public class AssTime: IAssSerializable<AssTime>
    {
        TimeSpan timePoint;

        public AssTime(AssTime other)
        {
            timePoint = new TimeSpan(other.timePoint.Ticks);
        }

        public AssTime()
        {
            timePoint = new TimeSpan(0);
        }

        private AssTime(TimeSpan timePoint)
        {
            this.timePoint = timePoint;
        }

        public static AssTime FromAssString(string assString)
        {
            if (float.TryParse(assString, out float floatTime))
            {
                return new AssTime(TimeSpan.FromSeconds(floatTime));
            }
            else
            {
                string[] segmented = assString.Split(":");
                if (segmented.Length == 3)
                {
                    long ms = 0;
                    int[] multiplier = { 60 * 60 * 1000, 60 * 1000, 1000, 1 };
                    for (int i = 0; i < segmented.Length && i < multiplier.Length; i++)
                        ms += Convert.ToInt64(segmented[i]) * multiplier[i];
                    return new AssTime(TimeSpan.FromMilliseconds(ms));
                }
                else
                {
                    throw new AssException($"Unable to determine {assString} time format");
                }
            }
        }

        public string ToAssString(AssVersion version)
        {
            return $"{timePoint.Days * 24 + timePoint.Hours}:{timePoint.Minutes}:{timePoint.Seconds}.{timePoint.Seconds}.{timePoint:ff}";
        }
    }
}
