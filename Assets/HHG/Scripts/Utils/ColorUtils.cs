using UnityEngine;
using static UnityEngine.Random;

namespace HHG.Scripts.Utils
{
    public static class ColorUtils
    {
        public static Color Random()
        {
            return new Color(value, value, value, 1);
        }
    }
}
