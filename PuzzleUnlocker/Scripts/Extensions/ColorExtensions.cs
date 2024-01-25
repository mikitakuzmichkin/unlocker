using UnityEngine;

namespace PuzzleUnlocker.Scripts.Extensions
{
    public static class ColorExtensions
    {
        public static float[] ColorToFloat(this Color color)
        {
            float[] array = new float[4];
            for (int i = 0; i < 4; ++i)
            {
                array[i] = color[i];
            }

            return array;
        }
        
        public static Color FloatToColor(this float[] array)
        {
            if (array == null)
            {
                return Color.clear;
            }
            
            Color color = new Color();
            for (int i = 0; i < 4; ++i)
            {
                color[i] = array[i];
            }

            return color;
        }
        
        public static Color XorColor(this Color first, Color second)
        {
            first.r = Mathf.Abs(first.r - second.r);
            first.g = Mathf.Abs(first.g - second.g);
            first.b = Mathf.Abs(first.b - second.b);
            return first;
        }
        
        public static Color AndColor(this Color first, Color second)
        {
            first.r = Mathf.Abs(first.r * second.r);
            first.g = Mathf.Abs(first.g * second.g);
            first.b = Mathf.Abs(first.b * second.b);
            return first;
        }
    }
}