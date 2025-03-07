using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domain.EntityFramework
{
    public class DateTimeWithTimeZoneConverter : ValueConverter<DateTime, DateTime>
    {
        public DateTimeWithTimeZoneConverter() : base(
            dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc),
            dateTime => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc))
        {
        }
    }
}
