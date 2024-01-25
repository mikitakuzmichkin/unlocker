using System;
using UnityEngine;

namespace PuzzleUnlocker.Scripts.Extensions
{
    public static class IntegerExtensions
    {
        public static int LoopCrop(this int value, int min, int max)
        {
            if (value < 0)
            {
                value = max - Mathf.Abs(min - value);
            }

            if (value >= max)
            {
                value %= max;
            }

            return value;
        }

        public static int LoopShortDistance(this int first, int second, int min, int max)
        {
            if (first > second)
            {
                var temp = first;
                first = second;
                second = temp;
            }

            var firstDistance = second - first;
            var secondDistance = Mathf.Abs(min - (max - second) - first);
            return firstDistance < secondDistance ? firstDistance : secondDistance;
        }
    }
}