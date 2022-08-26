using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;


namespace Web.BongaCC.Codes
{
    public class Computations
    {
        public static string sVariance(decimal POValue, decimal PRValue)
        {
            var dVariance = (POValue == 0) ? 0.ToString() + " %" : (Math.Round(((PRValue / POValue) * 100), 2)).ToString() + " %";
            return dVariance;
        }


        //public IEnumerable<string> WeeksInYear()
        //{
        //    var calendar = CultureInfo.CurrentCulture.Calendar;
        //    var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        //    var weekPeriods =
        //    Enumerable.Range(1, calendar.GetDaysInMonth(year, month))
        //              .Select(d =>
        //              {
        //                  var date = new DateTime(year, month, d);
        //                  var weekNumInYear = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, firstDayOfWeek);
        //                  return new { date, weekNumInYear };
        //              })
        //              .GroupBy(x => x.weekNumInYear)
        //              .Select(x => new { DateFrom = x.First().date, To = x.Last().date })
        //              .ToList();

        //    return weekPeriods;
        //}


        //TODO: Source=> https://stackoverflow.com/questions/7363978/get-a-list-of-weeks-for-a-year-with-dates
        public static IEnumerable<object> GetWeeksInYear()
        {
            DateTime jan1 = new DateTime(DateTime.Today.Year, 1, 1);
            //beware different cultures, see other answers
            DateTime startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));
            var weeks =
                Enumerable.Range(1, 54)
                    .Select(i => new
                    {
                        weekStart = startOfFirstWeek.AddDays(i * 7)
                    })
                    .TakeWhile(x => x.weekStart.Year <= jan1.Year)
                    .Select(x => new
                    {
                        x.weekStart,
                        weekFinish = x.weekStart.AddDays(4)
                    })
                    .SkipWhile(x => x.weekFinish.Year < jan1.Year)
                    .Select((x, i) => new
                    {
                        WeekStart = x.weekStart.ToString("ddd, d, MMM"),
                        WeekFinish = x.weekFinish.ToString("ddd, d, MMM"),
                        WeekNum = i + 1,
                        Description = string.Format("Week {0},  {1}", i + 1, x.weekStart.ToString("MMM d, yyyy"))
                        //Description = string.Format("{0}  Between  {1}  and  {2}", i + 1, x.weekStart.ToString("ddd, d, MMM"), x.weekFinish.ToString("ddd, d, MMM"))
                    });

            return weeks;
        }

        public static IEnumerable<object> GetMonths()
        {
            var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames
                 .TakeWhile(m => m != String.Empty)
                 .Select((m, i) => new
                 {
                     Month = i + 1,
                     MonthName = m
                 })
                 .ToList();
            return months;
        }


        //https://stackoverflow.com/questions/19901666/get-date-of-first-and-last-day-of-week-knowing-week-number
        public static DateTime FirstDateOfWeek(int? year, int? weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new DateTime((int)year, 1, 1);
            int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(((int)weekOfYear * 7) + 1);
        }

        //public static DateTime FirstDateOfWeek(int? year, int? weekOfYear, CultureInfo ci)
        //{
        //    DateTime jan1 = new DateTime((int)year, 1, 1);
        //    DateTime startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));

        //    //int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
        //    int daysOffset = (int)ci.DateTimeFormat.FirstDayOfWeek - (int)startOfFirstWeek.DayOfWeek;

        //    DateTime firstWeekDay = jan1.AddDays(daysOffset);
        //    int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, ci.DateTimeFormat.FirstDayOfWeek);
        //    if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
        //    {
        //        weekOfYear -= 1;
        //    }
        //    return firstWeekDay.AddDays(((int)weekOfYear * 7) - 1);
        //}

        public static DateTime FirstDateOfWeek(int? year, int? weekOfYear)
        {
            var firstDate = new DateTime((int)year, 1, 1);
            //first thursday of the week defines the first week (https://en.wikipedia.org/wiki/ISO_8601)
            //Wiki: the 4th of january is always in the first week
            while (firstDate.DayOfWeek != DayOfWeek.Monday)
                firstDate = firstDate.AddDays(-1);

            return firstDate.AddDays(((int)weekOfYear - 1) * 7);
        }

        //public DateTime FirstDateOfWeek(int year, int weekOfYear)
        //{
        //    var newYear = new DateTime(year, 1, 1);
        //    var weekNumber = newYear.GetIso8601WeekOfYear();

        //    DateTime firstWeekDate;

        //    if (weekNumber != 1)
        //    {
        //        var dayNumber = (int)newYear.DayOfWeek;
        //        firstWeekDate = newYear.AddDays(7 - dayNumber + 1);
        //    }
        //    else
        //    {
        //        var dayNumber = (int)newYear.DayOfWeek;
        //        firstWeekDate = newYear.AddDays(-dayNumber + 1);
        //    }

        //    if (weekOfYear == 1)
        //    {
        //        return firstWeekDate;
        //    }

        //    return firstWeekDate.AddDays(7 * (weekOfYear - 1));
        //}

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }


        //public static IEnumerable<object> GetExportTypes()
        //{
        //    Enumerable.Range(0, 2).Select(i => new
        //    {
        //        text1 = exportType.A3,
        //        text2 = exportType.A4,
        //        num1 = (int)exportType.A3,
        //        num2 = (int)exportType.A4,
        //    });





        //    DateTime jan1 = new DateTime(DateTime.Today.Year, 1, 1);
        //    //beware different cultures, see other answers
        //    DateTime startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));
        //    var weeks =
        //        Enumerable.Range(0, 54)
        //            .Select(i => new
        //            {
        //                weekStart = startOfFirstWeek.AddDays(i * 7)
        //            })
        //            .TakeWhile(x => x.weekStart.Year <= jan1.Year)
        //            .Select(x => new
        //            {
        //                x.weekStart,
        //                weekFinish = x.weekStart.AddDays(4)
        //            })
        //            .SkipWhile(x => x.weekFinish.Year < jan1.Year)
        //            .Select((x, i) => new
        //            {
        //                WeekStart = x.weekStart.ToString("ddd, d, MMM"),
        //                WeekFinish = x.weekFinish.ToString("ddd, d, MMM"),
        //                WeekNum = i + 1,
        //                Description = string.Format("{0}  Between  {1}  and  {2}", i + 1, x.weekStart.ToString("ddd, d, MMM"), x.weekFinish.ToString("ddd, d, MMM"))
        //            });

        //    return weeks;
        //}
    }
}
