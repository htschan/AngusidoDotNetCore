using System;

namespace Server.Helpers
{
   public interface IHolidayService
   {
      bool IsHoliday();
      bool IsHoliday(DateTime dt);
   }
}
