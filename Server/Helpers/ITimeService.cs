using System;

namespace Server.Helpers
{
    public interface ITimeService
    {
        decimal GetDecimalHour(DateTime dt);
        int GetWeekNumber(DateTime dt);
    }
}