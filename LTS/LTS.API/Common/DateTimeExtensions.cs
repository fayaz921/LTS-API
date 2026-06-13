namespace LTS.API.Common;

public static class DateTimeExtensions
{
    public static DateTime ToUtc(this DateTime dt) =>
        dt.Kind switch
        {
            DateTimeKind.Utc => dt,
            DateTimeKind.Local => dt.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
        };

    public static DateTime? ToUtc(this DateTime? dt) =>
        dt.HasValue ? dt.Value.ToUtc() : null;
}
