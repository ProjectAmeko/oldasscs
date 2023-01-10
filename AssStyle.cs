using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistortionAss.Interfaces;

namespace DistortionAss
{
    public class AssStyle: IAssSerializable<AssStyle>
    {
        public enum AssBorderStyle
        {
            OUTLINE = 1, // Treat border as outline
            OPAQUE = 3, // Draw an Opaque box for each line. This can cause overdraw issues.
            LIBASSOPAQUE = 4, // Opaque box but box is drawn across all the lines. Libass specfic. (?)

        }

        public enum AssAlignment
        {
            BOTTOMLEFT = 1,
            BOTTOM,
            BOTTOMRIGHT,
            MIDDLELEFT,
            MIDDLE,
            MIDDLERIGHT,
            TOPLEFT,
            TOP,
            TOPRIGHT
        }

        public string Name = "Default";
        public string FontName = "Arial";
        public float FontSize = 20;
        public AssColor ForegroundColor = new AssColor(Color.White);
        public AssColor KaraokeColor = new AssColor(Color.Red);
        public AssColor OutlineColor = new AssColor(Color.Black);
        public AssColor ShadowColor = new AssColor(Color.FromArgb(255 / 2, 0, 0, 0));
        private bool[] fontParams = { false, false, false, false };
        private float[] fontScale = { 100, 100};
        public float FontSpacing = 0.0f;
        public float FontAngle = 0.0f;
        public AssBorderStyle BorderStyle = AssBorderStyle.OUTLINE;
        public AssAlignment Alignment = AssAlignment.BOTTOM;
        private float[] bordshad = { 0.0f, 0.0f};
        private int[] margin = { 0, 0, 0, 0};

        // accessors
        // fontParams
        public bool Bold { get { return fontParams[0]; } set { fontParams[0] = value; } }
        public bool Italic { get { return fontParams[1]; } set { fontParams[1] = value; } }
        public bool Underline { get { return fontParams[2]; } set { fontParams[2] = value; } }
        public bool StrikeOut { get { return fontParams[2]; } set { fontParams[2] = value; } }
        // fontScale
        public float ScaleX { get { return fontScale[0]; } set { fontScale[0] = value; } }
        public float ScaleY { get { return fontScale[1]; } set { fontScale[1] = value; } }
        // Border & Shadow
        public float Border { get { return bordshad[0]; } set { bordshad[0] = value; } }
        public float Shadow { get { return bordshad[1]; } set { bordshad[1] = value; } }

        // margins
        public int MarginLeft { get { return margin[0]; } set { margin[0] = value; } }
        public int MarginRight { get { return margin[1]; } set { margin[1] = value; } }
        public int MarginBottom { get { return margin[2]; } set { margin[2] = value; } }
        public int MarginTop { get { return margin[3]; } set { margin[3] = value; } }


        public static AssStyle FromAssString(string assString, AssVersion version)
        {

        }

        public string ToAssString(AssVersion version)
        {
            // Math.Round(a, 2, MidpointRounding.ToEven)

            switch (version)
            {
                case AssVersion.ASS:
                    return 
                        $"Style: {this.Name},{this.FontName}," +
                        $"{AssKit.ToAssFloat(FontSize)}," +
                        $"{ForegroundColor.ToAssString(AssColor.ColorType.ASSA)}," +
                        $"{KaraokeColor.ToAssString(AssColor.ColorType.ASSA)}," +
                        $"{OutlineColor.ToAssString(AssColor.ColorType.ASSA)}," +
                        $"{ShadowColor.ToAssString(AssColor.ColorType.ASSA)}," +
                        $"{(fontParams[0] ? '1': '0')},{(fontParams[1] ? '1' : '0')},{(fontParams[2] ? '1' : '0')},{(fontParams[3] ? '1' : '0')}," +
                        $"{AssKit.ToAssFloat(fontScale[0])}," +
                        $"{AssKit.ToAssFloat(fontScale[1])}," +
                        $"{AssKit.ToAssFloat(FontSpacing)}," +
                        $"{AssKit.ToAssFloat(FontAngle)}," +
                        $"{(int)BorderStyle}," +
                        $"{AssKit.ToAssFloat(bordshad[0])}," +
                        $"{AssKit.ToAssFloat(bordshad[1])}," +
                        $"{(int)Alignment}," +
                        $"{margin[0]},{margin[1]},{margin[2]},1";
                case AssVersion.ASSv5:
                    return
                        $"Style: {this.Name},{this.FontName}," +
                        $"{AssKit.ToAssFloat(FontSize)}," +
                        $"{ForegroundColor.ToAssString(AssColor.ColorType.ASSA)}," +
                        $"{KaraokeColor.ToAssString(AssColor.ColorType.ASSA)}," +
                        $"{OutlineColor.ToAssString(AssColor.ColorType.ASSA)}," +
                        $"{ShadowColor.ToAssString(AssColor.ColorType.ASSA)}," +
                        $"{(fontParams[0] ? '1' : '0')},{(fontParams[1] ? '1' : '0')},{(fontParams[2] ? '1' : '0')},{(fontParams[3] ? '1' : '0')}," +
                        $"{AssKit.ToAssFloat(fontScale[0])}," +
                        $"{AssKit.ToAssFloat(fontScale[1])}," +
                        $"{AssKit.ToAssFloat(FontSpacing)}," +
                        $"{AssKit.ToAssFloat(FontAngle)}," +
                        $"{(int)BorderStyle}," +
                        $"{AssKit.ToAssFloat(bordshad[0])}," +
                        $"{AssKit.ToAssFloat(bordshad[1])}," +
                        $"{(int)Alignment}," +
                        $"{margin[0]},{margin[1]},{margin[2]},{margin[3]},1";
                default:
                    break;
            }
            throw new NotImplementedException();
        }
    }
}
