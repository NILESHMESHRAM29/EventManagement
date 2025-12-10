using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EventManagement.Data
{
    public static class ValueConverters
    {
        public static readonly ValueConverter<DateTime, DateTime> UtcToUnspecifiedConverter =
            new ValueConverter<DateTime, DateTime>(
                // Convert to DB: If it's UTC, change the Kind to Unspecified before saving
                v => v.Kind == DateTimeKind.Utc ? DateTime.SpecifyKind(v, DateTimeKind.Unspecified) : v,

                // Convert from DB: When reading back, return the value as-is (Unspecified)
                v => v
            );
    }
}
