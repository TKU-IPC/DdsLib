using System;
using System.Globalization;

namespace DdsLib
{
    public class DateAPI
    {
        /// <summary>
        /// 取得時間（年月日）格式化字串
        /// </summary>
        /// <param name="obj">DateTime物件</param>
        /// <returns>時間格式化字串[string]</returns>
        public static string GetDateStr(Object obj)
        {
            try
            {
                string s = ((DateTime)obj).ToString("yyyy/MM/dd");
                if (s == "1900/01/01")
                {
                    s = "";
                }
                return s;
            }
            catch 
            {
                return "0000/00/00";
            }
        }

        /// <summary>
        /// 取得時間（年月日）格式化字串
        /// </summary>
        /// <param name="obj">DateTime物件</param>
        /// <returns>時間格式化字串[string]</returns>
        public static string GetDateTimeStr(Object obj)
        {
            try
            {
                return ((DateTime)obj).ToString("yyyy/MM/dd HH:mm:ss");
            }
            catch 
            {
                return "0000/00/00 00:00:00";
            }
        }

        /// <summary>
        /// 取得目前時間（年月日時分秒）格式化字串
        /// </summary>
        /// <returns>時間格式化字串[string]</returns>
        public static string GetCurrTimeStr()
        {
            try
            {
                return (DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            }
            catch 
            {
                return "0000/00/00 00:00:00";
            }
        }

        /// <summary>
        /// 取得目前時間（年月日）格式化字串
        /// </summary>
        /// <returns>時間格式化字串[string]</returns>
        public static string GetCurrDateStr()
        {
            try
            {
                return (DateTime.Now).ToString("yyyy/MM/dd");
            }
            catch 
            {
                return "0000/00/00";
            }
        }

        /// <summary>
        /// 取得月
        /// </summary>
        /// <param name="DateTimeStr">時間格式化字串</param>
        /// <returns>月[string]</returns>
        public static string GetMonthStr(string DateTimeStr)
        {
            try
            {
                return DateTimeStr.Substring(5, 2);
            }
            catch 
            {
                return "00";
            }
        }

        /// <summary>
        /// 取得年
        /// </summary>
        /// <param name="DateTimeStr">時間格式化字串</param>
        /// <returns>年[string]</returns>
        public static string GetYearStr(string DateTimeStr)
        {
            try
            {
                return DateTimeStr.Substring(0, 4);
            }
            catch 
            {
                return "0000";
            }
        }

        /// <summary>
        /// 取得日
        /// </summary>
        /// <param name="DateTimeStr">時間格式化字串</param>
        /// <returns>日[string]</returns>
        public static string GetDayStr(string DateTimeStr)
        {
            try
            {
                return DateTimeStr.Substring(8, 2);
            }
            catch 
            {
                return "00";
            }
        }

        public static int CalculateAge(DateTime bday)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - bday.Year;
            if (bday > now.AddYears(-age)) age--;
            return age;
        }

        public static int CalculateAge(DateTime bday , DateTime cday)
        {            
            int age = cday.Year - bday.Year;
            if (bday > cday.AddYears(-age)) age--;
            return age;
        }

        public static string GetCurrentTimeStamp()
        {
            ////產生1970 - Now 的span
            TimeSpan span = DateTime.Now.ToLocalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

            ////回傳TotalSecond
            return Convert.ToInt32(span.TotalSeconds).ToString();
        }

        public static string GetTimeStamp(DateTime dt)
        {
            ////產生1970 - Now 的span
            TimeSpan span = dt.ToLocalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();

            ////回傳TotalSecond
            return Convert.ToInt32(span.TotalSeconds).ToString();
        }


        public static int CalculateAge(string bdaystr, string cdaystr)
        {
            string[] DateTimeList = { 
                            "yyyy/M/d tt hh:mm:ss", 
                            "yyyy/MM/dd tt hh:mm:ss", 
                            "yyyy/MM/dd HH:mm:ss", 
                            "yyyy/M/d HH:mm:ss", 
                            "yyyy/M/d", 
                            "yyyy/MM/dd",
                            "yyyy-M-d tt hh:mm:ss", 
                            "yyyy-MM-dd tt hh:mm:ss", 
                            "yyyy-MM-dd HH:mm:ss", 
                            "yyyy-M-d HH:mm:ss", 
                            "yyyy-M-d", 
                            "yyyy-MM-dd"
 
                        };
            DateTime bday = DateTime.ParseExact(bdaystr, DateTimeList, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
            DateTime cday = DateTime.ParseExact(cdaystr, DateTimeList, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
            int age = cday.Year - bday.Year;
            if (bday > cday.AddYears(-age)) age--;
            return age;
        }

        public static DateTime StrToDate(string _date)
        {
            DateTime dt = new DateTime(0);
            try
            {
                dt = Convert.ToDateTime(_date);
            }
            catch
            {
            }
            return dt;
        }

        /// 將字串日期轉整數
        /// 日期字串
        /// 整數日期格式
        public static DateTime ParseDate(string _date)
        {
            //這裡定義所有日期格式 
            string[] dateFormats = { "yyyy/MM/dd", "yyy/MM/dd", "yy/MM/dd", "y/MM/dd", 
           "yyyy-MM-dd", "yyy-MM-dd", "yy-MM-dd", "y-MM-dd", 
           "yyyy/M/dd","yyy/M/dd","yy/M/dd","y/M/dd", 
           "yyyy-M-dd","yyy-M-dd","yy-M-dd","y-M-dd", 
           "yyyy/MM/d","yyy/MM/d","yy/MM/d","y/MM/d", 
           "yyyy-MM-d","yyy-MM-d","yy-MM-d","y-MM-d", 
           "yyyy/M/d","yyy/M/d","yy/M/d","y/M/d", 
           "yyyy-M-d","yyy-M-d","yy-M-d","y-M-d"};
            try
            {
                //這裡來處理傳入的格式是否為日期格式，只需要簡單一行 
                DateTime datetime = DateTime.ParseExact(_date, dateFormats, null, DateTimeStyles.AllowWhiteSpaces);
                return datetime;
                /*
                int iToken = _date.IndexOf("-", 0);
                if (iToken == -1)
                    iToken = _date.IndexOf("/", 0);
                string iYear = _date.Substring(0, iToken);
                switch (iYear.Length)
                {
                    ///這裡是利用datetime.ToString(IFormatProvider)的方法傳出自己想要的統一格式 
                    case 1:
                        return Int32.Parse(datetime.ToString("yMMdd"));
                    case 2:
                        return Int32.Parse(datetime.ToString("yyMMdd"));
                    case 3:
                        return Int32.Parse(datetime.ToString("yyyMMdd"));
                    case 4:
                        return Int32.Parse(datetime.ToString("yyyyMMdd"));
                }
                return 0;
                 */ 
            }
            catch (Exception)
            {
                return new DateTime(0);
            }
        }
        /// 將字串時間轉換為整數時間
        /// 時間字串(HH:mm:ss
        /// HHmmss整數格式 
        public static DateTime ParseTime(string _time)
        {
            string[] timeFormats = { "HH:mm:ss", "HH:mm:s","HH:m:ss", "HH:m:s", 
           "H:mm:ss","H:mm:s","H:m:ss","H:m:s"};
            try
            {
                DateTime datetime = DateTime.ParseExact(_time, timeFormats, null, DateTimeStyles.AllowWhiteSpaces);
                //return Int32.Parse(datetime.ToString("HHmmss"));
                return datetime;
            }
            catch (Exception)
            {
                return new DateTime(0);
            }
        }


    }
}
