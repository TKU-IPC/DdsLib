using System;
using System.Collections.Specialized;

namespace DdsLib
{

    public class SsoAPI
    {
        /// <summary>
        /// 在學學生
        /// </summary>
        /// <param name="role">角色碼</param>
        /// <returns></returns>
        public static bool IsInStudent(string role)
        {
            if (role == "3")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsEmpolyee(string role)
        {
            if (role == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsTeacher(string role)
        {
            if (role == "2")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsTeaOrEmp(string role)
        {
            string[] ary = { "1", "2" };
            if (Array.IndexOf(ary, role) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static bool IsStudent(string role)
        {
            string[] ary = { "3", "4", "5", "6" };
            if (Array.IndexOf(ary, role) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsInStudentNow(string role)
        {
            //string[] ary = { "3", "4", "5", "6" };
            if (role=="3")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }


    // 在學學生

    
        


    // 在職老師

    // 在職職員



    public class SsoObj
    {
        public string iv_user = "";//應該是登入id
        public string sso_birthday_y = "";//出生年
        public string iv_server_name = "";//不知道是什麼的伺服器名稱
        public string sso_unit1 = "";//一級單位
        public string sso_clsid = "";//班級代碼 A . B . C .D
        public string sso_stucode = "";//聘任別：如專任；學生班級簡稱
        public string sso_futmtype = "";//(即職員的本職)  0-9,A-F
        public string sso_chname = "";//中文姓名
        public string sso_birthday_m = "";//出生月
        public string sso_roletype = "";//身份：char(1) 1:職員,2:教師(含教官)  	　  	使用者身分(3:在學,4:休學,5:退學,6:畢業)
        public string sso_gradeid = "";//年級 
        public string sso_name = "";//一級單位名稱、學生則是學院
        public string sso_userid = "";//學號、職員代號
        public string sso_parttype = "";//不知道
        public string sso_deptcd = "";//單位代號學生五碼、職員四碼
        public string sso_email = "";//校內Email
        public string sso_deptname = "";//單位名稱
        public string sso_birthday_d = "";//出生日
        public string sso_titlename = "";//學生學制、職員是職稱
        public string sso_parttitle = "";//兼任吧
        public string sso_id = "";//身份證代號
        public string sso_sex = "";//性別
        public string sso_ip = "";//IP
        public string sso_x_forwarded_for = "";//IP
        public NameValueCollection coll;

        private string getHeaderValue(string par)
        {
            string val = string.Empty;
            try
            {
                val = coll.Get(par);
            }
            catch
            {
            }
            return val;
        }

        private void setSexVal()
        {
            if (this.sso_sex == "1")
            {
                this.sso_sex = "M";
            }
            if (this.sso_sex == "2")
            {
                this.sso_sex = "F";
            }            
        }

        private void init()
        {
            this.coll = System.Web.HttpContext.Current.Request.Headers;
            this.iv_user = getHeaderValue("iv-user");            
            this.iv_server_name = getHeaderValue("iv_server_name");
            this.sso_birthday_d = getHeaderValue("sso_birthday_d");
            this.sso_birthday_m = getHeaderValue("sso_birthday_m");
            this.sso_birthday_y = getHeaderValue("sso_birthday_y");
            this.sso_chname = getHeaderValue("sso_chname");
            this.sso_clsid = getHeaderValue("sso_clsid");
            this.sso_deptcd = getHeaderValue("sso_deptcd");
            this.sso_deptname = getHeaderValue("sso_deptname");
            this.sso_email = getHeaderValue("sso_email");
            this.sso_futmtype = getHeaderValue("sso_futmtype");
            this.sso_gradeid = getHeaderValue("sso_gradeid");
            this.sso_id = getHeaderValue("sso_id");
            this.sso_name = getHeaderValue("sso_name");
            this.sso_parttitle = getHeaderValue("sso_parttitle");
            this.sso_parttype = getHeaderValue("sso_parttype");
            this.sso_roletype = getHeaderValue("sso_roletype");
            this.sso_stucode = getHeaderValue("sso_stucode");
            this.sso_titlename = getHeaderValue("sso_titlename");
            this.sso_unit1 = getHeaderValue("sso_unit1");
            this.sso_userid = getHeaderValue("sso_userid");
            this.sso_sex = getHeaderValue("sso_sex");
            this.sso_ip = getHeaderValue("x-forwarded-for");
            this.sso_x_forwarded_for = this.sso_ip;
            setSexVal();
        }
        public SsoObj()
        {
            init();
        }
        /// <summary>
        /// 建構子：解碼
        /// </summary>
        /// <param name="FlgDec"></param>
        public SsoObj(bool FlgDec)
        {
            if (FlgDec)
            {
                this.coll = System.Web.HttpContext.Current.Request.Headers;
                this.iv_user = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("iv-user"));
                this.iv_server_name = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("iv_server_name"));
                this.sso_birthday_d = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_birthday_d"));
                this.sso_birthday_m = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_birthday_m"));
                this.sso_birthday_y = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_birthday_y"));
                this.sso_chname = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_chname"));
                this.sso_clsid = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_clsid"));
                this.sso_deptcd = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_deptcd"));
                this.sso_deptname = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_deptname"));
                this.sso_email = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_email"));
                this.sso_futmtype = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_futmtype"));
                this.sso_gradeid = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_gradeid"));
                this.sso_id = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_id"));
                this.sso_name = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_name"));
                this.sso_parttitle = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_parttitle"));
                this.sso_parttype = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_parttype"));
                this.sso_roletype = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_roletype"));
                this.sso_stucode = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_stucode"));
                this.sso_titlename = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_titlename"));
                this.sso_unit1 = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_unit1"));
                this.sso_userid = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("sso_userid"));
                this.sso_sex = getHeaderValue("sso_sex");
                this.sso_ip = System.Web.HttpContext.Current.Server.UrlDecode(getHeaderValue("x-forwarded-for"));
                this.sso_x_forwarded_for = this.sso_ip;
                setSexVal();
            }
            else
            {
                init();                
            }
        }

        public string OutputString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int i = 0; i < coll.Count; i++)
            {
                sb.Append(coll.Keys[i] + "===>" + System.Web.HttpContext.Current.Server.UrlDecode(coll.Get(i)) + "<br />");
            }
            return sb.ToString();
        }

        public string getTwBirthDay(string sp)
        {
            string s = string.Empty;
            
            int y =0;
            try
            {
                y = Convert.ToInt16(this.sso_birthday_y);
            }
            catch
            {
            }
            
            int m = 0;
            try
            {
                m = Convert.ToInt16(this.sso_birthday_m);
                if (m>12)
                {
                    m = 0;
                }
                else if (m < 0)
                {
                    m = 0;
                }

            }
            catch
            {
            }

            int d = 0;
            try
            {
                d = Convert.ToInt16(this.sso_birthday_d);
                if (d > 31)
                {
                    d = 0;
                }
                else if (m < 0)
                {
                    d = 0;
                }
            }
            catch
            {
            }

            s = string.Format("{0:00}{1}{2:00}{3}{4:00}", y, sp, m, sp, d);
            return s;
        }

        public string getBirthDay(string sp)
        {
            string s = string.Empty;

            int y = 0;
            try
            {
                y = Convert.ToInt16(this.sso_birthday_y);
                y += 1911;
            }
            catch
            {
            }

            int m = 0;
            try
            {
                m = Convert.ToInt16(this.sso_birthday_m);
                if (m > 12)
                {
                    m = 0;
                }
                else if (m < 0)
                {
                    m = 0;
                }

            }
            catch
            {
            }

            int d = 0;
            try
            {
                d = Convert.ToInt16(this.sso_birthday_d);
                if (d > 31)
                {
                    d = 0;
                }
                else if (m < 0)
                {
                    d = 0;
                }
            }
            catch
            {
            }

            s = string.Format("{0:0000}{1}{2:00}{3}{4:00}", y, sp, m, sp, d);
            return s;
        }


    }
}
