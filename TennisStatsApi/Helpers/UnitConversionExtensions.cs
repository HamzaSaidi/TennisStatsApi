namespace TennisStatsApi.Helpers;

public static class UnitConversionExtensions
{
    public static float ToMeters(this int centimeters)
    {
        return centimeters / 100f;
    }

    public static float ToKilograms(this int grams)
    {
        return grams / 1000f;
    }
}