using System;

namespace LabXand.Core
{
    public static class PersianCalendarHelper
    {
        public static int Today
        {
            get
            {
                System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
                return persianCalendar.GetDayOfMonth(DateTime.Now);
            }
        }

        public static string CurrentMonthName
        {
            get
            {
                System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
                var month = persianCalendar.GetMonth(DateTime.Now);
                switch (month)
                {
                    case 1 :
                        return "فروردین";
                    case 2:
                        return "اردیبهشت";
                    case 3:
                        return "خرداد";
                    case 4:
                        return "تیر";
                    case 5:
                        return "مرداد";
                    case 6:
                        return "شهریور";
                    case 7:
                        return "مهر";
                    case 8:
                        return "آبان";
                    case 9:
                        return "آذر";
                    case 10:
                        return "دی";
                    case 11:
                        return "بهمن";
                    case 12:
                        return "اسفند";
                    default:
                        return string.Empty;
                }
            }
        }

        public static int CurrentMonth
        {
            get
            {
                System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
                return persianCalendar.GetMonth(DateTime.Now);
            }
        }

        public static int CurrentYear
        {
            get
            {
                System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
                return persianCalendar.GetYear(DateTime.Now);
            }
        }

        public static int GetYearNumber(DateTime dateTime)
        {
            System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
            return persianCalendar.GetYear(dateTime);
        }

        public static int GetMonthNumber(DateTime dateTime)
        {
            System.Globalization.PersianCalendar persianCalendar = new System.Globalization.PersianCalendar();
            return persianCalendar.GetMonth(dateTime);
        }
    }
}