using System;
using System.Text;

namespace DdsLib
{
    public class SchoolAPI
    {
        public static int getCurrYr()
        {
            int y = DateTime.Now.Year;
            int m = DateTime.Now.Month;
            if (m >= 8)
            {
                return y - 1911;
            }
            else
            {
                return y - 1912;
            }
        }

        public static int getYr(string d)
        {

            int y = Convert.ToInt16(DateAPI.GetYearStr(d));
            int m = Convert.ToInt16(DateAPI.GetMonthStr(d));
            if (m >= 8)
            {
                return y - 1911;
            }
            else
            {
                return y - 1912;
            }
        }

        public static int getCurrSem()
        {
            int y = DateTime.Now.Year;
            int m = DateTime.Now.Month;
            if (m < 8 && m >= 2)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        public static int getSem(string d)
        {

            int y = Convert.ToInt16(DateAPI.GetYearStr(d));
            int m = Convert.ToInt16(DateAPI.GetMonthStr(d));
            if (m < 8 && m >= 2)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }
        public static string getChWeekDayName(int d)
        {
            switch (d)
            {
                case 1:
                    return "星期一";
                case 2:
                    return "星期二";
                case 3:
                    return "星期三";
                case 4:
                    return "星期四";
                case 5:
                    return "星期五";
                case 6:
                    return "星期六";
                case 7:
                    return "星期日";
                default:
                    return "";
            }
        }

        public static string getWeekWorkTime(int d)
        {
            switch (d)
            {
                case 1:
                    return "08:10-09:00";
                case 2:
                    return "09:10-10:00";
                case 3:
                    return "10:10-11:00";
                case 4:
                    return "11:10-12:00";
                case 5:
                    return "12:10-13:00";
                case 6:
                    return "13:10-14:00";
                case 7:
                    return "14:10-15:00";
                case 8:
                    return "15:10-16:00";
                case 9:
                    return "16:10-17:00";
                case 10:
                    return "17:10-18:00";
                case 11:
                    return "18:10-19:00";
                case 12:
                    return "19:10-20:00";
                case 13:
                    return "20:10-21:00";
                case 14:
                    return "21:10-22:00";


                default:
                    return "";
            }
        }


        public static string getSess(string s1, string s2, string s3)
        {

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(s2))
            {
                sb.Append(s1);
            }

            if (!string.IsNullOrEmpty(s2))
            {
                sb.Append(" , ");
                sb.Append(s2);
            }

            if (!string.IsNullOrEmpty(s3))
            {
                sb.Append(" , ");
                sb.Append(s3);
            }

            if (sb.Length > 0)
            {
                return string.Concat("第 ", sb.ToString(), " 節");
            }
            else
            {
                return "";
            }
        }
    }
}
