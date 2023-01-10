using DistortionAss.Interfaces;
using oops.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistortionAss
{


    public class AssInfo : IAssSerializable<AssInfo>, IAssSerializableWriter
    {
        public string AssComment { get; set; } = "";
        public TrackableDictionary<string, string> assValues = new TrackableDictionary<string, string>();
        private AssVersion _version = AssVersion.UNKNOWN;
        public AssVersion version { 
            get {
                if (_version != AssVersion.UNKNOWN)
                {
                    return _version;
                }
                if (assValues.TryGetValue("ScriptVersion", out string scriptVersion))
                {
                    if (scriptVersion.ToLowerInvariant() == "v4.00+")
                    {
                        _version = AssVersion.ASS;
                    }
                    else if (scriptVersion.ToLowerInvariant() == "v4.00++")
                    {
                        _version = AssVersion.ASSv5;
                    }
                }
                return _version;
            }
            set
            {
                _version = value;
                switch (_version)
                {
                    case AssVersion.ASS:
                        assValues["ScriptVersion"] = "v4.00+";
                        break;
                    case AssVersion.ASSv5:
                        assValues["ScriptVersion"] = "v4.00++";
                        break;
                    case AssVersion.UNKNOWN: // Save as v4+ any since it should be the default.
                        assValues["ScriptVersion"] = "v4.00+";
                        break;
                    default:
                        break;
                }
            }
        }

        public static AssInfo FromAssString(string assString, AssVersion version)
        {
            throw new AssException("AssInfo is special. It should not be created from an AssString.");
        }

        string formatComment()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var line in AssComment.Split("\n"))
            {
                sb.AppendLine(";" + line);
            }
            return sb.ToString();
        }
        public string ToAssString(AssVersion version)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[Script Info]");
            builder.Append(formatComment());
            foreach (var keyValue in assValues)
            {
                builder.AppendLine($"{keyValue.Key}: {keyValue.Value}");
            }
            return builder.ToString();
        }

        public bool ToAssString(StringBuilder builder)
        {
            builder.Append("[Script Info]");
            builder.Append(formatComment());
            foreach (var keyValue in assValues)
            {
                builder.AppendLine($"{keyValue.Key}: {keyValue.Value}");
            }
            return true;
        }
    }
}
