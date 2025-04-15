namespace Expenses.Serializers;

public static class ConvertTimeZone
{
    private static readonly TimeZoneInfo TimeZoneBr = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

    public static DateTime Convert(DateTime utcNow, int addMinutes = 0)
    {
        var nowBr = TimeZoneInfo.ConvertTimeFromUtc(utcNow.AddMinutes(addMinutes), TimeZoneBr);
        return nowBr;
    }
    
    public static DateTimeOffset Convert(DateTimeOffset utcNow, int addMinutes = 0)
    {
        var nowBr = TimeZoneInfo.ConvertTime(utcNow.AddMinutes(addMinutes), TimeZoneBr);
        return nowBr;
    }
}