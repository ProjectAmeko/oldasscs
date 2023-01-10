using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DistortionAss.Interfaces;

namespace DistortionAss
{
    public class AssEvent : IAssSerializable<AssEvent>
    {

        public EventType eventType { get; set; } = EventType.Dialogue;
        public int Layer { get; set; } = 0;
        public AssTime Start { get; set; } = new AssTime();
        public AssTime End { get; set; } = new AssTime();
        public string Style { get; set; } = string.Empty;
        public string Actor { get; set; } = string.Empty;
        private int[] margins = new int[] { 0,0,0,0};
        public string Effect { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;

        public static Regex eventRegex = new(@"(.+): ([\d]+),([\d:.]+),([\d:.]+),(.*?),(\d*?),(\d*?),(\d*?),(\d*?),(.*?),(.*)");
        public static Regex eventRegexASSv5 = new(@"(.+): ([\d]+),([\d:.]+),([\d:.]+),(.*?),(\d*?),(\d*?),(\d*?),(\d*?),(\d*?),(.*?),(.*)");
        public enum EventType
        {
            Comment,
            Dialogue,
            Picture, // Not used used ASS, Deprecated
            Sound, // Not used used ASS, Deprecated
            Movie, // Not used used ASS, Deprecated
            Command // Not used used ASS, Deprecated
        }
        public static AssEvent FromAssString(string assString, AssVersion version)
        {
            var matches = eventRegex.Matches(assString);
            if (matches.Count > 0)
            {
                EventType matchType = EventType.Dialogue;
                switch (matches[0].Value)
                {
                    case "Dialogue":
                        matchType = EventType.Dialogue;
                        break;
                    case "Comment":
                        matchType = EventType.Comment;
                        break;
                    default:
                        throw new AssException("String matches regex but is not a valid event type.");
                }
                AssEvent? finalEvent = null;
                switch (version)
                {
                    case AssVersion.SSA:
                        throw new NotImplementedException();
                    case AssVersion.ASS:
                        {
                            AssEvent @event = new AssEvent()
                            {
                                eventType = matchType,
                                Layer = int.Parse(matches[1].Value.Trim()),
                                Start = AssTime.FromAssString(matches[2].Value.Trim()),
                                End = AssTime.FromAssString(matches[3].Value.Trim()),
                                Style = matches[4].Value,
                                Actor = matches[5].Value,
                                margins = new int[] {
                                    int.Parse(matches[6].Value),
                                    int.Parse(matches[7].Value),
                                    int.Parse(matches[8].Value),
                                    int.Parse(matches[8].Value)
                                },
                                Effect = matches[9].Value,
                                Text = matches[10].Value,
                            };
                            return @event;
                        }
                    case AssVersion.ASSv5:
                        {
                            matches = eventRegexASSv5.Matches(assString);
                            AssEvent @event = new AssEvent()
                            {
                                eventType = matchType,
                                Layer = int.Parse(matches[1].Value.Trim()),
                                Start = AssTime.FromAssString(matches[2].Value.Trim()),
                                End = AssTime.FromAssString(matches[3].Value.Trim()),
                                Style = matches[4].Value,
                                Actor = matches[5].Value,
                                margins = new int[] {
                                    int.Parse(matches[6].Value),
                                    int.Parse(matches[7].Value),
                                    int.Parse(matches[8].Value),
                                    int.Parse(matches[9].Value)
                                },
                                Effect = matches[10].Value,
                                Text = matches[11].Value,
                            };
                            return @event;
                        }
                    case AssVersion.UNKNOWN:
                        throw new AssException($"No Version provided?");
                    default:
                        throw new AssException($"No Version provided?");
                }
            }
            else
            {
                throw new AssException($"\"{assString}\" is not a valid assString");
            }
        }
        public string ToAssString(AssVersion version)
        {
            throw new NotImplementedException();
        }
    }
}
