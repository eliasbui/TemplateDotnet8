namespace Template.Contract.Extensions;

public static class DatetimeExtension
{
    public static string ToUnixTimeMilliseconds(this DateTime dateTime)
    {
        DateTimeOffset dto = new(dateTime.ToUniversalTime());
        return dto.ToUnixTimeMilliseconds().ToString();
    }
}
