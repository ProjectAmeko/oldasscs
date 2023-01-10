using System.Text;

namespace DistortionAss.Interfaces
{
    internal interface IAssSerializableWriter
    {
        public bool ToAssString(StringBuilder builder);
    }
}