using System;
using System.Collections.Generic;

namespace Server.Helpers
{
    public class HolidayService : IHolidayService
    {
        private List<Holiday> _holidays;

        public HolidayService()
        {
            CurrentYear = DateTime.Now.Year;
            Initialize();
        }

        /// <summary>
        /// Beschreibung: 
        /// </summary>
        public int CurrentYear { get; set; }

        /// <summary>
        /// Beschreibung: Gibt variable Feiertage zurueck
        /// </summary>
        public List<Holiday> VariableFeiertage
        {
            get
            {
                return _holidays.FindAll(f => !f.IsFix);
            }
        }

        public bool IsHoliday()
        {
            DateTime value = DateTime.Now;
            return _holidays.Find(f => f.Datum.Date == value.Date) != null;
        }

        public bool IsHoliday(DateTime dt)
        {
            if (CurrentYear != dt.Year)
            {
                CurrentYear = dt.Year;
                Initialize();
            }
            return _holidays.Find(f => f.Datum.Date == dt.Date) != null;
        }

        public Holiday GetFeiertagName(DateTime value)
        {
            return _holidays.Find(f => f.Datum.Date == value.Date);
        }
        /// <summary>
        /// Beschreibung: gibt feste Feiertage zurueck
        /// </summary>
        public List<Holiday> FesteFeiertage
        {
            get
            {
                return _holidays.FindAll(f => f.IsFix);
            }
        }

        private void Initialize()
        {
            #region fillList

            _holidays = new List<Holiday>
         {
            new Holiday(true, new DateTime(CurrentYear, 1, 1), "Neujahr"),
            new Holiday(true, new DateTime(CurrentYear, 1, 2), "Berchtoldstag"),
            new Holiday(true, new DateTime(CurrentYear, 5, 1), "Tag der Arbeit"),
            new Holiday(true, new DateTime(CurrentYear, 8, 1), "Bundesfeier"),
            new Holiday(true, new DateTime(CurrentYear, 12, 25), "1. Weihnachtstag"),
            new Holiday(true, new DateTime(CurrentYear, 12, 26), "2. Weihnachtstag")
         };
            DateTime osterSonntag = GetOsterSonntag();
            _holidays.Add(new Holiday(false, osterSonntag, "Ostersonntag"));
            _holidays.Add(new Holiday(false, osterSonntag.AddDays(-2), "Karfreitag"));
            _holidays.Add(new Holiday(false, osterSonntag.AddDays(1), "Ostermontag"));
            _holidays.Add(new Holiday(false, osterSonntag.AddDays(39), "Auffahrt"));
            _holidays.Add(new Holiday(false, osterSonntag.AddDays(49), "Pfingstsonntag"));
            _holidays.Add(new Holiday(false, osterSonntag.AddDays(50), "Pfingstmontag"));
            #endregion
        }

        private DateTime GetOsterSonntag()
        {
            var g = CurrentYear % 19;
            var c = CurrentYear / 100;
            var h = (c - c / 4 - (8 * c + 13) / 25 + 19 * g + 15) % 30;
            var i = h - h / 28 * (1 - 29 / (h + 1) * ((21 - g) / 11));
            var j = (CurrentYear + CurrentYear / 4 + i + 2 - c + c / 4) % 7;
            var l = i - j;
            var month = 3 + (l + 40) / 44;
            var day = l + 28 - 31 * (month / 4);
            return new DateTime(CurrentYear, month, day);
        }
    }

    public class Holiday : IComparable<Holiday>
    {
        public Holiday(bool isFix, DateTime datum, string name)
        {
            IsFix = isFix;
            Datum = datum;
            Name = name;
        }

        /// <summary>
        /// Beschreibung: 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Beschreibung: 
        /// </summary>
        public DateTime Datum { get; set; }

        /// <summary>
        /// Beschreibung: 
        /// </summary>
        public bool IsFix { get; set; }

        #region IComparable<Feiertag> Member
        public int CompareTo(Holiday other)
        {
            return Datum.Date.CompareTo(other.Datum.Date);
        }
        #endregion
    }
}