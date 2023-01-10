using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistortionAss.Interfaces
{
    internal interface IAssSerializable<T>
    {
        public static T FromAssString(string assString, AssVersion version) => throw new NotImplementedException();
        public string ToAssString(AssVersion version);
    }
}
