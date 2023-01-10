using oops.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DistortionAss
{
    public enum AssVersion
    {
        SSA, // v4.00
        ASS, // v4.00+
        ASSv5, // v4.00++
        UNKNOWN
    }
    public class AssFile
    {
        public AssInfo assInfo;
        public TrackableCollection<AssStyle> Styles;
        public TrackableCollection<AssEvent> Events;
        public string filepath = "";


        public AssFile()
        {
            assInfo = new AssInfo();
            Styles = new TrackableCollection<AssStyle>();
            Events = new TrackableCollection<AssEvent>();
            assInfo.version = AssVersion.ASS;
            assInfo.assValues["x-capabilities"] = "x-capable, extradata, garbage";
        }

        public AssFile(string filepath)
        {

        }

        private enum ParseState
        {
            INITAL,
            SCRIPT_INFO,
            DATA
        }


        #region Saving
        public void Save(string filepath)
        {
            using FileStream fs = File.Open(filepath, FileMode.Create);
            using StreamWriter sw = new StreamWriter(fs);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[script info]");
            this.assInfo.ToAssString(sb);
            sw.Write(sb);
            sb.Clear();
            AssVersion version = this.assInfo.version;

            // Setup pagination
            uint pagination = 5000;

            // Write Styles
            switch (version)
            {
                case AssVersion.SSA:
                    throw new AssException("Not Impl");
                case AssVersion.ASS:
                    sb.AppendLine("[v4+ styles]");
                    break;
                case AssVersion.ASSv5:
                    sb.AppendLine("[v4++ styles]");
                    break;
                case AssVersion.UNKNOWN:
                    throw new AssException("Unable to determine ScriptType. This should not happen?");
                default:
                    break;
            }
            foreach (var style in Styles)
            {
                sb.AppendLine(style.ToAssString(version));
                pagination -= 1;
                if (pagination <= 0 )
                {
                    pagination = 5000;
                    sw.WriteLine(sb);
                    sb.Clear();
                }
            }

            sb.AppendLine();
            sw.WriteLine(sb);

            // Write events
            sb.AppendLine("[events]");
            sb.Clear();
            foreach (var assEvent in Events)
            {
                assEvent.ToAssString(version);
            }
        }
        #endregion


        #region Parsing
        public void Parse()
        {

            // Don't track changes when unloading or loading
            this.Events.TrackChanges = this.assInfo.assValues.TrackChanges = this.Styles.TrackChanges = false;

            ParseFunc parseState = ParseUnknown;
            if (File.Exists(this.filepath))
            {
                using FileStream fs = File.Open(this.filepath, FileMode.Open);
                using StreamReader rd = new StreamReader(fs, true);
                while (rd.ReadLine() is { } cLine)
                {
                    // Skip empty strings
                    if (cLine.Equals(string.Empty)) continue;

                    // Section Headers
                    if (cLine.StartsWith('[') && cLine.EndsWith(']'))
                    {
                        var low = cLine.ToLower();
                        parseState = low switch
                        {
                            "[v4+ styles]" => ParseStyle,
                            "[events]" => ParseEvent,
                            "[script info]" => ParseScript,
                            //"[aegisub project garbage]" => ParseMetadataLine,
                            //"[aegisub extradata]" => ParseExtradataLine,
                            _ => ParseUnknown
                        };
                    }

                    // Parse the line based on the current state
                    parseState(cLine);
                }
            }
        }

        private delegate void ParseFunc(string line);

        private void ParseScript(string lineData)
        {
            if (lineData.StartsWith(";"))
            {
                this.assInfo.AssComment += lineData.Trim(';').Trim(' ') + "\n";
            }
            else
            {
                string[] spl = lineData.Split(":");
                this.assInfo.assValues[spl[0]] = string.Join(":", spl[0].Trim(' ') + spl[2..]);
            }
        }
        private void ParseEvent(string lineData)
        {
            this.Events.Add(AssEvent.FromAssString(lineData, this.assInfo.version));
        }

        private void ParseStyle(string lineData)
        {
            this.Styles.Add(AssStyle.FromAssString(lineData));
        }

        public void ParseUnknown(string lineData)
        {

        }
        #endregion
    }
}
