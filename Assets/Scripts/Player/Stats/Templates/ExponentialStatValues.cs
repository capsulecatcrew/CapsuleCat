using System;

namespace Stats.Templates
{
    public static class ExponentialStatValues
    {
        public static float GetNextLevelValue(float current, float changeValue)
        {
            return current *= changeValue;
        }

        public static float GetSetLevelValue(int level, int maxLevel, float baseValue, float changeValue)
        {
            if (level >= maxLevel) level = maxLevel;
            return baseValue * (float)Math.Pow(changeValue, level - 1);
        }
    }
}