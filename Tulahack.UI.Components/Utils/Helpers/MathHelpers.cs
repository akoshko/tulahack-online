namespace Tulahack.UI.Components.Utils.Helpers;

public static class MathHelpers
{
    private static (double min, double max) GetMinMax(double a, double b) =>
        a >= b ? (b, a) : (a, b);

    private static (float min, float max) GetMinMax(float a, float b) =>
        a >= b ? (b, a) : (a, b);

    private static (decimal min, decimal max) GetMinMax(decimal a, decimal b) =>
        a >= b ? (b, a) : (a, b);

    private static (int min, int max) GetMinMax(int a, int b) =>
        a >= b ? (b, a) : (a, b);

    public static double SafeClamp(double value, double min, double max)
    {
        (min, max) = GetMinMax(min, max);

        if (value < min)
        {
            return min;
        }

        return value > max ? max : value;
    }

    public static decimal SafeClamp(decimal value, decimal min, decimal max)
    {
        (min, max) = GetMinMax(min, max);

        if (value < min)
        {
            return min;
        }

        return value > max ? max : value;
    }

    public static int SafeClamp(int value, int min, int max)
    {
        (min, max) = GetMinMax(min, max);

        if (value < min)
        {
            return min;
        }

        return value > max ? max : value;
    }

    public static float SafeClamp(float value, float min, float max)
    {
        (min, max) = GetMinMax(min, max);

        if (value < min)
        {
            return min;
        }

        return value > max ? max : value;
    }
}
