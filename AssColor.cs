using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistortionAss.Interfaces;

namespace DistortionAss
{
    public class AssColor: IAssSerializable<AssColor>
    {
        public enum ColorType
        {
            ASS,
            ASSA,
            HTML
        }
        public Color rawColor { get; private set; }

        public AssColor(Color color)
        {
            this.rawColor = color;
        }

        public AssColor()
        {
            rawColor = Color.White;
        }

        public static AssColor FromAssString(string assString)
        {
            if (assString.StartsWith("&H"))
            {
                assString = assString[2..];
            }
            else if (assString.StartsWith("&"))
            {
                assString = assString[1..];
            }
            if (assString.Length == 8)
            {
                Color color = Color.FromArgb(
                    Convert.ToInt32(assString[..2], 16),
                    Convert.ToInt32(assString[2..2], 16),
                    Convert.ToInt32(assString[4..2], 16),
                    Convert.ToInt32(assString[6..2], 16)
                    );
                return new AssColor(color);
            }
            else if (assString.Length == 6)
            {
                Color color = Color.FromArgb(
                    Convert.ToInt32(assString[..2], 16),
                    Convert.ToInt32(assString[2..2], 16),
                    Convert.ToInt32(assString[4..2], 16)
                        );
                return new AssColor(color);
            }
            else
            {
                throw new AssException($"Unknown/Incomplete assColor {assString}");
            }
        }

        public string ToAssString(ColorType colorType = ColorType.ASS)
        {
            switch (colorType)
            {
                case ColorType.ASS:
                    return $"&{Convert.ToHexString(new byte[]{rawColor.B, rawColor.G, rawColor.R})}";
                case ColorType.ASSA:
                    return $"&{Convert.ToHexString(new byte[] { rawColor.A, rawColor.B, rawColor.G, rawColor.R })}";
                case ColorType.HTML:
                    return ColorTranslator.ToHtml(rawColor);
                default:
                    throw new AssException("Not Implemented lol");
            }
            
        }

        public string ToAssString(AssVersion version)
        {
            return ToAssString(ColorType.ASS);
        }
    }
}
