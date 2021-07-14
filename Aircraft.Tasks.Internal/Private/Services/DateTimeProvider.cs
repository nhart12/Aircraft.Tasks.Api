using System;
namespace Aircraft.Tasks.Internal.Private.Services
{
    internal interface IDateTimeProvider
    {
        DateTime Now();
        DateTime UtcNow();
    }
    internal class DateTimeProvider:IDateTimeProvider
    {
        public DateTimeProvider()
        {
        }

        public DateTime Now()
        {
            return DateTime.Now;
        }

        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
