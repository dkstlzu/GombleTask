namespace GombleTask.Extensions
{
    public static class MathUtility
    {
        public static float EulerAngleDiff(float from, float target)
        {
            float positiveFromEuler = ToEuler(from);
            float positiveTargetEuler = ToEuler(target);

            float diff = positiveTargetEuler - positiveFromEuler;

            if (diff >= 0) return diff;
            else return diff + 360;
        }

        public static float AngleDiff(float from, float target)
        {
            float eulerDiff = EulerAngleDiff(from, target);

            if (eulerDiff > 180)
            {
                eulerDiff -= 360;
            }

            return eulerDiff;
        }

        public static float ToEuler(float value)
        {
            float oneRound = 360;
            
            while (value < 0 || value >= oneRound)
            {
                if (value < 0) value += oneRound;
                else if (value >= oneRound) value -= oneRound;
            }

            return value;
        }
    }
}