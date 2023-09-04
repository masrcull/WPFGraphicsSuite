using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GraphicsCommon
{
    public class ColorHelper
    {
        public static SolidColorBrush CreateColorBrush(byte red, byte green, byte blue)
        {
            Color specifiedColor = Color.FromRgb(red, green, blue);
            return new SolidColorBrush(specifiedColor);
        }

        public static byte IncrementRgbByte(byte rgbByte, byte incrementAmount, ref bool isIncrease)
        {
            return (byte)(rgbByte + (isIncrease ? (rgbByte < 240 ? incrementAmount : ((isIncrease = false) ? -incrementAmount : 0)) : (rgbByte > 15 ? -incrementAmount : ((isIncrease = true) ? incrementAmount : 0))));
        }
    }
}
