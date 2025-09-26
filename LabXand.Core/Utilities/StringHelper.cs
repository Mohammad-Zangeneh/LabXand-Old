using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace LabXand.Core
{
    public static class StringHelper
    {
        /// <summary>
        /// متدی برای تبدیل اعداد انگلیسی به فارسی
        /// </summary>
        public static string ToPersianNumber(this string inputString, bool extractEnglishNumber = false)
        {
            if (string.IsNullOrWhiteSpace(inputString) || inputString.Trim() == "") return "";

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            inputString = inputString.Replace("0", "۰");
            inputString = inputString.Replace("1", "۱");
            inputString = inputString.Replace("2", "۲");
            inputString = inputString.Replace("3", "۳");
            inputString = inputString.Replace("4", "۴");
            inputString = inputString.Replace("5", "۵");
            inputString = inputString.Replace("6", "۶");
            inputString = inputString.Replace("7", "۷");
            inputString = inputString.Replace("8", "۸");
            inputString = inputString.Replace("9", "۹");

            // خارج کردن attribute های
            // تگ ها
            MatchCollection htmlAttributes = Regex.Matches(inputString, @"(\S+)=[""']?((?:.(?![""']?\s+(?:\S+)=|[>""']))+.)[""']?", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            inputString = htmlAttributes.Cast<Match>().Aggregate(inputString, (current, match) => current.Replace(match.Value, match.Value.CorrectEnglishNumber()));

            //خارج کردن تگهای pre
            MatchCollection preTags = Regex.Matches(inputString, @"<pre[^>]*>(.*?)</pre>", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            inputString = preTags.Cast<Match>().Aggregate(inputString, (current, match) => current.Replace(match.Value, match.Value.CorrectEnglishNumber()));

            //خارج کردن تگهای code
            MatchCollection codeTags = Regex.Matches(inputString, @"<code[^>]*>(.*?)</code>", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            inputString = codeTags.Cast<Match>().Aggregate(inputString, (current, match) => current.Replace(match.Value, match.Value.CorrectEnglishNumber()));

            if (!extractEnglishNumber) return inputString;

            // خارج کردن اعدادی که قبل از آنها حروف انگلیسی هست
            inputString = Regex.Replace(inputString, @"(?<=(\W*[a-z]\W*))[\d.]+|[\d.,]+(?=(\W*[a-z$]\W*))|[\d.,]+(?=(\W*[a-z$]\W*))", "{--$&--}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            MatchCollection flaged = Regex.Matches(inputString, @"\{--[^-]*--\}", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            foreach (Match match in flaged)
            {
                var number = Regex.Replace(match.Value, @"\{--|--\}", "", RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
                inputString = inputString.Replace(match.Value, number.CorrectEnglishNumber());
            }

            return inputString;
        }
        public static string ToPersianNumber(this int input)
        {
            string inputAsString = input.ToString();
            if (string.IsNullOrWhiteSpace(inputAsString) || inputAsString.Trim() == "") return "";

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            inputAsString = inputAsString.Replace("0", "۰");
            inputAsString = inputAsString.Replace("1", "۱");
            inputAsString = inputAsString.Replace("2", "۲");
            inputAsString = inputAsString.Replace("3", "۳");
            inputAsString = inputAsString.Replace("4", "۴");
            inputAsString = inputAsString.Replace("5", "۵");
            inputAsString = inputAsString.Replace("6", "۶");
            inputAsString = inputAsString.Replace("7", "۷");
            inputAsString = inputAsString.Replace("8", "۸");
            inputAsString = inputAsString.Replace("9", "۹");
            return inputAsString;
        }
        public static string ToPersianNumber(this int? input)
        {
            if (!input.HasValue) return string.Empty;
            string inputAsString = input.Value.ToString();
            if (string.IsNullOrWhiteSpace(inputAsString) || inputAsString.Trim() == "") return string.Empty;

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            inputAsString = inputAsString.Replace("0", "۰");
            inputAsString = inputAsString.Replace("1", "۱");
            inputAsString = inputAsString.Replace("2", "۲");
            inputAsString = inputAsString.Replace("3", "۳");
            inputAsString = inputAsString.Replace("4", "۴");
            inputAsString = inputAsString.Replace("5", "۵");
            inputAsString = inputAsString.Replace("6", "۶");
            inputAsString = inputAsString.Replace("7", "۷");
            inputAsString = inputAsString.Replace("8", "۸");
            inputAsString = inputAsString.Replace("9", "۹");
            return inputAsString;
        }
        public static string ToPersianNumber(this double input)
        {
            string inputAsString = input.ToString();
            if (string.IsNullOrWhiteSpace(inputAsString) || inputAsString.Trim() == "") return "";

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            inputAsString = inputAsString.Replace("0", "۰");
            inputAsString = inputAsString.Replace("1", "۱");
            inputAsString = inputAsString.Replace("2", "۲");
            inputAsString = inputAsString.Replace("3", "۳");
            inputAsString = inputAsString.Replace("4", "۴");
            inputAsString = inputAsString.Replace("5", "۵");
            inputAsString = inputAsString.Replace("6", "۶");
            inputAsString = inputAsString.Replace("7", "۷");
            inputAsString = inputAsString.Replace("8", "۸");
            inputAsString = inputAsString.Replace("9", "۹");
            return inputAsString;
        }

        /// <summary>
        /// متدی برای تبدیل اعداد فارسی به انگلیسی
        /// </summary>
        public static string ToEnglishNumber(this string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Trim() == "") return "0";

            input = input.Replace(",", "");

            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            input = input.Replace("۰", "0");
            input = input.Replace("۱", "1");
            input = input.Replace("۲", "2");
            input = input.Replace("۳", "3");
            input = input.Replace("۴", "4");
            input = input.Replace("۵", "5");
            input = input.Replace("۶", "6");
            input = input.Replace("۷", "7");
            input = input.Replace("۸", "8");
            input = input.Replace("۹", "9");
            return input;
        }
        public static char ToEnglishNumber(this char input)
        {
            if (string.IsNullOrWhiteSpace(input.ToString()) || input == '\0') return '0';
            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            switch (input)
            {
                case '۹':
                    return '9';
                case '۸':
                    return '8';
                case '۷':
                    return '7';
                case '۶':
                    return '6';
                case '۵':
                    return '5';
                case '۴':
                    return '4';
                case '۳':
                    return '3';
                case '۲':
                    return '2';
                case '۱':
                    return '1';
                case '۰':
                    return '0';
                default:
                    return '0';
            }
        }
        static string CorrectEnglishNumber(this string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Trim() == "") return "";
            //۰ ۱ ۲ ۳ ۴ ۵ ۶ ۷ ۸ ۹
            input = input.Replace("۰", "0");
            input = input.Replace("۱", "1");
            input = input.Replace("۲", "2");
            input = input.Replace("۳", "3");
            input = input.Replace("۴", "4");
            input = input.Replace("۵", "5");
            input = input.Replace("۶", "6");
            input = input.Replace("۷", "7");
            input = input.Replace("۸", "8");
            input = input.Replace("۹", "9");
            return input;
        }

        const string Format = "{0:#,0.####}";

        /// <summary>
        /// متدی برای تبدیل یک رشته به فرمت پول
        /// </summary>
        public static string ToMoneyFormat(this string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return string.Empty;
            str = str.Trim().Replace(" ", "").ToEnglishNumber();
            if (str == "") return "";
            string output = String.Format(Format, double.Parse(str));
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output.ToPersianNumber();
        }
        public static string ToMoneyFormat(this int input)
        {
            string output = String.Format(Format, input);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output.ToPersianNumber();
        }
        public static string ToMoneyFormat(this int? input)
        {
            if (!input.HasValue) return "";
            string output = String.Format(Format, input.Value);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output.ToPersianNumber();
        }
        public static string ToMoneyFormat(this short input)
        {
            string output = String.Format(Format, input);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output.ToPersianNumber();
        }
        public static string ToMoneyFormat(this short? input)
        {
            if (!input.HasValue) return "";
            string output = String.Format(Format, input.Value);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output.ToPersianNumber();
        }
        public static string ToMoneyFormat(this byte input)
        {
            string output = String.Format(Format, input);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output.ToPersianNumber();
        }
        public static string ToMoneyFormat(this uint input)
        {
            string output = String.Format(Format, input);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output;
        }
        public static string ToMoneyFormat(this long input)
        {
            string output = String.Format(Format, input);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output.ToPersianNumber();
        }
        public static string ToMoneyFormat(this decimal input)
        {
            string output = String.Format(Format, input);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output;
        }
        public static string ToMoneyFormat(this double input)
        {
            string output = String.Format(Format, input);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output;
        }
        public static string ToMoneyFormat(this ulong input)
        {
            string output = String.Format(Format, input);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output;
        }
        public static string ToMoneyFormat(this float input)
        {
            string output = String.Format(Format, input);
            if (output.Contains("-"))
                output = output.Replace("-", "") + "-";
            return output;
        }

        public static string ReplaceMultipleLinesWithOneLine(this string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? string.Empty
                : Regex.Replace(input, @"(\r\n)+|\n+", Environment.NewLine, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
        }

        /// <summary>
        /// حذف کاراکترهای عمومی غیر مجاز برای نام یک فایل
        /// </summary>
        public static string RemoveIllegalChars(this string inputString)
        {
            // / ? < > \ : * | ” 
            if (inputString == "") return "";
            inputString = Regex.Replace(inputString.Trim(), @"^\s*|\s*$", "").ToEnglishNumber();
            inputString = Regex.Replace(inputString.Trim(), @"(?:\W(?<!\.))+", "-").ToEnglishNumber();
            inputString = Regex.Replace(inputString.Trim(), @"-*$|^-*", "").ToEnglishNumber();
            return inputString;
        }
        public static string RemoveIllegalCharsForUrl(this string inputString)
        {
            // / ? < > \ : * | ” 
            if (string.IsNullOrWhiteSpace(inputString)) return "";
            inputString = Regex.Replace(inputString.Trim(), @"^\s*|\s*$", "").ToEnglishNumber();
            inputString = Regex.Replace(inputString.Trim(), @"\W+", "-").ToEnglishNumber();
            inputString = Regex.Replace(inputString.Trim(), @"-*$|^-*", "").ToEnglishNumber();
            inputString = inputString.Replace("--", "-");
            return inputString;
        }
    }
}
