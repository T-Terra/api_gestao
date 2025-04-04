namespace Expenses.Serializers;

public static class ConvertTimeZone
{
    private static readonly TimeZoneInfo TimeZoneBr = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

    public static DateTime Convert(DateTime utcNow)
    {
        var nowBr = TimeZoneInfo.ConvertTimeFromUtc(utcNow, TimeZoneBr);
        return nowBr;
    }
}