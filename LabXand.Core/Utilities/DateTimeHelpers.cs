using System;
using System.Globalization;

namespace LabXand.Core
{
    public static class DateTimeHelpers
    {
        public static string ToPersianDate(this DateTime? date, bool rightToLeft = false, bool showClock = false)
        {
            try
            {
                if (!date.HasValue) return string.Empty;
                PersianCalendar persianCalendar = new PersianCalendar();
                string returnValue;
                if (showClock)
                {
                    returnValue = string.Format(rightToLeft ? "{5}:{4}:{3}   {2}/{1}/{0}" : "{0}/{1}/{2}   {3}:{4}:{5}",
                                                persianCalendar.GetYear(date.Value).ToString("00"),
                                                persianCalendar.GetMonth(date.Value).ToString("00"),
                                                persianCalendar.GetDayOfMonth(date.Value).ToString("00"),
                                                persianCalendar.GetHour(date.Value).ToString("00"),
                                                persianCalendar.GetMinute(date.Value).ToString("00"),
                                                persianCalendar.GetSecond(date.Value).ToString("00"));
                }
                else
                    returnValue = string.Format(rightToLeft ? "{5}:{4}:{3}" : "{0}/{1}/{2}",
                                                persianCalendar.GetYear(date.Value).ToString("00"),
                                                persianCalendar.GetMonth(date.Value).ToString("00"),
                                                persianCalendar.GetDayOfMonth(date.Value).ToString("00"));
                return returnValue.ToPersianNumber();
            }
            catch
            {
                return "";
            }
        }
        public static string ToPersianDate(this DateTime date, bool rightToLeft = false, bool showClock = true)
        {
            try
            {
                PersianCalendar persianCalendar = new PersianCalendar();
                string returnValue;
                if (showClock)
                {
                    returnValue = string.Format(rightToLeft ? "{5}:{4}:{3}   {2}/{1}/{0}" : "{0}/{1}/{2}   {3}:{4}:{5}",
                                                persianCalendar.GetYear(date).ToString("00"),
                                                persianCalendar.GetMonth(date).ToString("00"),
                                                persianCalendar.GetDayOfMonth(date).ToString("00"),
                                                persianCalendar.GetHour(date).ToString("00"),
                                                persianCalendar.GetMinute(date).ToString("00"),
                                                persianCalendar.GetSecond(date).ToString("00"));
                }
                else
                    returnValue = string.Format(rightToLeft ? "{2}/{1}/{0}" : "{0}/{1}/{2}",
                                                persianCalendar.GetYear(date).ToString("00"),
                                                persianCalendar.GetMonth(date).ToString("00"),
                                                persianCalendar.GetDayOfMonth(date).ToString("00"));
                return returnValue.ToPersianNumber();
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// تبدیل تاریخ شمسی به میلادی
        /// </summary>
        public static DateTime? ToChristianDate(this string persianDate, bool setTimeWithNowIfItsZero = true)
        {
            if (string.IsNullOrWhiteSpace(persianDate)) return null;
            persianDate = persianDate.Trim().Replace(" ", "").ToEnglishNumber();

            int year = Int32.Parse(persianDate.Substring(0, persianDate.IndexOf('/')));
            int month = Int32.Parse(persianDate.Substring(persianDate.IndexOf('/') + 1, 2));
            int day = Int32.Parse(persianDate.Substring(persianDate.LastIndexOf('/') + 1, 2));

            int hour = 0;
            int minute = 0;
            int second = 0;

            try
            {
                hour = Int32.Parse(persianDate.Substring(10, 2)) == 0 && setTimeWithNowIfItsZero ? DateTime.Now.Hour : Int32.Parse(persianDate.Substring(10, 2));
                minute = Int32.Parse(persianDate.Substring(13, 2)) == 0 && setTimeWithNowIfItsZero ? DateTime.Now.Minute : Int32.Parse(persianDate.Substring(13, 2));
                second = Int32.Parse(persianDate.Substring(16, 2)) == 0 && setTimeWithNowIfItsZero ? DateTime.Now.Second : Int32.Parse(persianDate.Substring(16, 2));
            }
            catch { }

            if (month > 12 || month < 01 || day > 31 || day < 01 || hour > 24 || hour < 00 || minute > 60 || minute < 00 || second > 60 || second < 00)
                throw new Exception("فرمت تاریخ اشتباه است");

            PersianCalendar pCalendar = new PersianCalendar();
            return pCalendar.ToDateTime(year, month, day, hour, minute, second, 0);
        }

        public static string ToQueryStringFormat(this DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMddHHmmss");
        }
        public static DateTime? ParseToDateTime(this string dateTimeString)
        {
            if (string.IsNullOrWhiteSpace(dateTimeString)) return null;
            dateTimeString = dateTimeString.ToEnglishNumber();
            DateTime dateTime;
            DateTime.TryParseExact(dateTimeString, "yyyyMMddHHmmss", new CultureInfo("en-US"), DateTimeStyles.None, out dateTime);
            return dateTime;
        }
    }
}