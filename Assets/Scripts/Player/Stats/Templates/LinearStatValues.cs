namespace Stats.Templates
{
    public static class LinearStatValues
    {
        public static float GetNextLevelValue(float current, float changeValue)
        {
            current += changeValue;
            return current;
        }

        public static float GetSetLevelValue(int level, int maxLevel, float baseValue, float changeValue)
        {
            if (level >= maxLevel) level = maxLevel;
            return baseValue + (level - 1) * changeValue;
        }
    }
}