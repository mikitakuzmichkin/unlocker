using UnityEngine;

namespace PuzzleUnlocker.Scripts.UI
{
    public static class PuColor
    {
        public static Color Red => (Color) (new Color32(249, 67, 67, 255));

        public static Color Blue => (Color) (new Color32(113, 161, 253, 255));
        
        public static Color Green => (Color) (new Color32(89, 203, 167, 255));
        
        public static Color Yellow => (Color) (new Color32(254, 200, 90, 255));

        public static Color Purple => (Color) (new Color32(220, 108, 225, 255));
        
        public static Color White => (Color) (new Color32(255, 255, 255, 255));
        
        public static Color Cyan => (Color) new Color32(62,138,249,255);
        
        public static Color RedForBall => (Color) new Color32(249,67,67,255);
        
        public static Color BlueForBall => (Color) new Color32(62,138,249,255);

        public static Color PuColorFromColor(Color color)
        {
            if (color == Color.white)
            {
                return White;
            }
            
            if (color == Color.red)
            {
                return Red;
            }
            
            if (color == Color.blue)
            {
                return Blue;
            }
            
            if (color == Color.green)
            {
                return Green;
            }
            
            if (color == Color.yellow)
            {
                return Yellow;
            }
            
            if (color == new Color(1,0,1,1))
            {
                return Purple;
            }
            
            if (color == Color.cyan)
            {
                return Cyan;
            }

            return color;
        }
    }
}