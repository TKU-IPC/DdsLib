using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace DdsLib
{
    /// <summary>
    /// 常用功能
    /// </summary>
    public class Common
    {

        /// <summary>
        /// 把Email除了Domain name之外的字串只保留第一個字，其他用replacestring替代。
        /// </summary>
        /// <param name="orgstring">原始字串</param>
        /// <param name="replacestring">替代字串</param>
        /// <returns>字串</returns>
        public static string getEmailFilter(string orgstring, string replacestring)
        {
            StringBuilder sb = new StringBuilder();
            string emailid = "";
            string emaildomain = "";
            string[] s = null;
            s = orgstring.Split('@');
            try
            {
                emailid = s[0];
            }
            catch
            {

            }

            try
            {
                emaildomain = s[1];
            }
            catch
            {

            }

            sb.Append(Common.getChNameFilter(emailid, replacestring, ""));
            sb.Append("@");
            sb.Append(emaildomain);
            return sb.ToString();
        }
        /// <summary>
        /// 把中文姓名只保留第一個字，其他以替代字串呈現，並可自行加入稱呼。
        /// </summary>
        /// <param name="orgstring">原始字串</param>
        /// <param name="replacestring">替代字串</param>
        /// <param name="title">稱呼</param>
        /// <example>
        /// <code>
        /// Common.getChNameFilter("王大毛", "?", "先生");
        /// </code>
        /// </example>
        /// <returns>字串</returns>
        public static string getChNameFilter(string orgstring, string replacestring, string title)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < orgstring.Length; i++)
            {
                if (i == 0)
                {
                    sb.Append(orgstring.Substring(0, 1));
                }
                else
                {
                    sb.Append(replacestring);
                }

            }
            sb.Append(title);
            return sb.ToString();
        }

        /// <summary>
        /// 依據Key取得AppSetting的Value
        /// </summary>
        /// <param name="key">AppSettingKey</param>
        /// <returns>字串</returns>
        public static string ConfAppsVal(string key)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[key];
            }
            catch
            {
                return "";
            }
        }

        public static void DelFile(string destinationFile)
        {
            destinationFile = MakeFoldernameValid(destinationFile);
            if (File.Exists(destinationFile))
            {
                FileInfo fi = new FileInfo(destinationFile);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(destinationFile);
            }
        }

        
        #region G to L (13) 
        /// <summary>
        /// 特定編碼SubString(不建議使用)
        /// </summary>
        /// <param name="a_SrcStr"></param>
        /// <param name="a_StartIndex"></param>
        /// <param name="a_Cnt"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string getByteSubStr(string a_SrcStr, int a_StartIndex, int a_Cnt, string encoding)
        {
            Encoding l_Encoding = Encoding.GetEncoding(encoding, new EncoderExceptionFallback(), new DecoderReplacementFallback(""));
            byte[] l_byte = l_Encoding.GetBytes(a_SrcStr);
            if (a_Cnt <= 0)
                return "";
            //例若長度10 
            //若a_StartIndex傳入9 -> ok, 10 ->不行 
            if (a_StartIndex + 1 > l_byte.Length)
                return "";
            else
            {
                //若a_StartIndex傳入9 , a_Cnt 傳入2 -> 不行 -> 改成 9,1 
                if (a_StartIndex + a_Cnt > l_byte.Length)
                    a_Cnt = l_byte.Length - a_StartIndex;
            }
            return l_Encoding.GetString(l_byte, a_StartIndex, a_Cnt);
        }
        /// <summary>
        /// 純數字字串轉換，失敗則回傳空字串。
        /// </summary>
        /// <param name="str">原始字串</param>
        /// <returns>字串</returns>
        public static string getDgt(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Length = 0;
            string s = "";
            for (int i = 0; i < str.Length; i++)
            {
                try
                {
                    s = str.Substring(i, 1);
                    if (IsNumeric(s))
                    {
                        sb.Append(s);
                    }
                }
                catch
                {
                    s = "";
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 取得Hash字串
        /// </summary>
        /// <param name="key1">key1</param>
        /// <param name="key2">key2</param>
        /// <returns>Hash字串[string]</returns>
        public static string getHashStr(string key1, string key2)
        {
            int k1, k2, k=0;
            // 略
            return Md5Str(k.ToString()).Substring(0, 5);
        }

        /// <summary>
        /// 取得Hash字串，與MIS WEB SERVICE用。
        /// </summary>
        /// <param name="key1">key1</param>
        /// <param name="key2">key2</param>
        /// <returns>Hash字串[string]</returns>
        public static string getHashStrMis(string key1, string key2)
        {
            double k1, k2, k=0;
            // 略           
            return Md5Str(k.ToString()).Substring(0, 5);
        }

        /// <summary>
        /// 下載檔案到特地路徑
        /// </summary>
        /// <param name="url">網址</param>
        /// <param name="path">路徑</param>
        /// <returns>bool</returns>
        public static bool GetHttpFile(string url, string path)
        {
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadFile(url, path);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 取得某網址的文字內容
        /// </summary>
        /// <param name="url">網址</param>
        /// <returns>字串</returns>
        public static string GetUrlString(string url)
        {
            string s = "";
            try
            {
                WebClient wc = new WebClient();
                s = wc.DownloadString(url);
            }
            catch
            {
                return "";
            }
            return s;
        }

        /// <summary>
        /// 下載某網址的內容到MemorySteam中
        /// </summary>
        /// <param name="url">下載網址</param>
        /// <returns>MemoryStream</returns>
        public static MemoryStream GetUrlData(string url)
        {
            MemoryStream ms = null;
            try
            {
                WebClient wc = new WebClient();
                ms = new MemoryStream(wc.DownloadData(url));
            }
            catch
            {
                return null;
            }
            return ms;
        }

        /// <summary>
        /// 取得 Client Ip
        /// </summary>
        /// <returns>IP[string]</returns>
        public static string GetIpAddress()
        {
            string strIpAddr = string.Empty;

            if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null || HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IndexOf("unknown") > 0)
            {
                strIpAddr = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IndexOf(",") > 0)
            {
                strIpAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Substring(1, HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IndexOf(",") - 1);
            }
            else if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IndexOf(";") > 0)
            {
                strIpAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Substring(1, HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].IndexOf(";") - 1);
            }
            else
            {
                strIpAddr = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (Regexpr.isIpFmt(strIpAddr))
            {
                return strIpAddr; ;
            }
            else
            {
                return "0.0.0.0"; ;
            }
        }
        /// <summary>
        /// 取得百分比字串
        /// </summary>
        /// <param name="s">分子</param>
        /// <param name="p">分母</param>
        /// <returns>字串</returns>
        public static string GetPercent(string s, string p)
        {
            string str = string.Empty;
            s = string.IsNullOrEmpty(s) ? "0" : s;
            p = string.IsNullOrEmpty(p) ? "0" : p;
            double r = 0;

            if (p == "0")
            {
                str = "N/A";
            }
            else
            {
                r = Convert.ToDouble(s) / Convert.ToDouble(p) * 100;
                str = string.Format("{2}%", s, p, r.ToString("F2"));
            }

            return str;

        }

        /// <summary>
        /// 只保留原字串包含_與其他英文、中文或全形字串的部分
        /// </summary>
        /// <param name="s">原字串</param>
        /// <returns>字串</returns>
        public static string getPureLetter(string s)
        {
            //string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
            string pt = @"[^\w]";
            string rs = Regex.Replace(s, pt, "");
            return rs;
        }
        /// <summary>
        /// 原字串是否符合_與其他英文、中文或全形字串的原則
        /// </summary>
        /// <param name="s"></param>
        /// <returns>bool</returns>
        public static bool getPureLetter(ref string s)
        {
            //string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
            string pt = @"[^\w]";
            bool b = Regex.IsMatch(s, pt);
            s = Regex.Replace(s, pt, "");
            return b;
            //return rs;
        }
        /// <summary>
        /// 取得Utf8字串長度(不建議使用)        
        /// </summary>
        /// <param name="str">輸入字串</param>
        /// <returns>int</returns>
        public static int getUtf8MixStringLength(string str)
        {
            byte[] strbytes;
            int tmpcnt = 0;

            for (int i = 0; i < str.Length; i++)
            {
                strbytes = Encoding.UTF8.GetBytes(str.Substring(i, 1));
                if (strbytes.Length >= 3)
                {
                    tmpcnt += 2;
                }
                else
                {
                    tmpcnt += 1;
                }
            }
            return tmpcnt;
        }
        /// <summary>
        /// 取得Utf8 Substring
        /// </summary>
        /// <param name="str">輸入字串</param>
        /// <param name="len">長度</param>
        /// <returns>字串</returns>
        public static string getUtf8MixSubString(string str, int len)
        {
            Encoding l_Encoding = Encoding.GetEncoding("UTF-8", new EncoderExceptionFallback(), new DecoderReplacementFallback(""));
            byte[] l_byte = l_Encoding.GetBytes(str);
            byte[] strbytes;
            int tmpcnt = 0;
            StringBuilder sb = new StringBuilder();
            sb.Length = 0;
            for (int i = 0; i < str.Length; i++)
            {
                strbytes = Encoding.UTF8.GetBytes(str.Substring(i, 1));
                if (strbytes.Length >= 3)
                {
                    tmpcnt += 2;
                }
                else
                {
                    tmpcnt += 1;

                }
                if (tmpcnt <= len)
                {
                    sb.Append(l_Encoding.GetString(strbytes));
                    //return l_Encoding.GetString(l_byte, 0, a_Cnt);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Html轉純文字(你應該不會想用)
        /// </summary>
        /// <param name="str">輸入字串</param>
        /// <returns>字串</returns>
        public static string html2text(string str)
        {
            str = Regex.Replace(str, "(?is)<.+?>", "");
            return str;
        }

        // IsNumeric Function
        /// <summary>
        /// 檢查是否為數值
        /// </summary>
        /// <param name="Expression">輸入物件</param>
        /// <returns>bool</returns>
        public static bool IsNumeric(object Expression)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        #endregion G to L 
        #region M to R (11) 
        /// <summary>
        /// 過濾不當的檔名
        /// </summary>
        /// <param name="filename">檔案名稱</param>
        /// <returns>字串</returns>
        public static string MakeFilenameValid(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException();
            if (filename.EndsWith("."))
                filename = Regex.Replace(filename, @"\.+$", "");
            if (filename.Length == 0)
                throw new ArgumentException();
            if (filename.Length > 245)
                throw new PathTooLongException();
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '_');
            }

            return filename;
        }
        /// <summary>
        /// 過濾不當的路徑
        /// </summary>
        /// <param name="filename">路徑名稱</param>
        /// <returns>字串</returns>
        public static string MakeFoldernameValid(string foldername)
        {
            if (foldername == null)
                throw new ArgumentNullException();
            if (foldername.EndsWith("."))
                foldername = Regex.Replace(foldername, @"\.+$", "");
            if (foldername.Length == 0)
                throw new ArgumentException();

            if (foldername.Length > 245)
                throw new PathTooLongException();

            foreach (char c in System.IO.Path.GetInvalidPathChars())
            {
                foldername = foldername.Replace(c, '_');
            }
            return foldername;
        }

        /// <summary>
        /// 取得md5字串
        /// </summary>
        /// <param name="str">未加密內容</param>
        /// <returns>加密字串</returns>
        public static string Md5Str(string str)
        {
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(str));
            string ret = "";
            for (int i = 0; i < data.Length; i++)
            {
                ret += data[i].ToString("x2");
            }
            return ret;
        }

        /// <summary>
        /// 取得new line轉換成 <br> 的字串
        /// </summary>
        /// <param name="str">欲轉換的字串</param>
        /// <returns>轉換後字串</returns>
        public static string nl2br(string str)
        {
            str = str.Replace("\n", "<br>");
            return str;
        }

        /// <summary>
        /// 取得new line轉換成 <br> 的字串
        /// </summary>
        /// <param name="str">欲轉換的字串</param>
        /// <returns>轉換後字串</returns>
        public static string nl2p(string str)
        {
            str = str.Replace("\n", "</p><p>");
            str = "<p>" + str + "</p>";
            return str;
        }
        /// <summary>
        /// 忘了有機會再查
        /// </summary>
        /// <param name="Plo"></param>
        /// <returns></returns>
        public static string PloToRowXml(ParamListObj Plo)
        {
            return PloToRowXml(Plo, "ParamObjList", "Item");
        }
        /// <summary>
        /// 忘了有機會再查
        /// </summary>
        /// <param name="Plo"></param>
        /// <returns></returns>
        public static string PloToRowXml(ParamListObj Plo, string NodeNmae, string ItemName)
        {
            string xmlstr = string.Empty;
            System.IO.StringWriter sw = null;
            System.Xml.XmlTextWriter textWriter = null;
            try
            {
                sw = new System.IO.StringWriter();
                textWriter = new System.Xml.XmlTextWriter(sw);
                textWriter.WriteStartDocument();

                textWriter.WriteStartElement(NodeNmae);
                for (int i = 0; i < Plo.poLst.Count; i++)
                {
                    textWriter.WriteStartElement(ItemName, "");
                    textWriter.WriteStartElement(Plo.poLst[i].Name, "");
                    textWriter.WriteString(Plo.poLst[i].Value.ToString());
                    textWriter.WriteEndElement();
                    textWriter.WriteEndElement();
                }
                textWriter.WriteEndElement();
                textWriter.WriteEndDocument();
                xmlstr = sw.ToString();
                textWriter.Flush();
                textWriter.Close();
                sw.Close();
                sw.Dispose();
            }
            catch (Exception ex)
            {
                xmlstr = ex.ToString();
            }
            finally
            {
                textWriter.Flush();
                textWriter.Close();
                sw.Close();
                sw.Dispose();
            }
            return xmlstr;
        }
        /// <summary>
        /// ParamListObj 轉 Xml 字串
        /// </summary>
        /// <param name="Plo">ParamListObj物件</param>
        /// <returns>字串</returns>
        public static string PloToXml(ParamListObj Plo)
        {
            return PloToXml(Plo, "ParamObjList");
        }

        /// <summary>
        /// ParamListObj 轉 Xml 字串，可自訂。
        /// </summary>
        /// <param name="Plo">ParamListObj物件</param>
        /// <param name="NodeName">NodeName</param>
        /// <returns>字串</returns>

        public static string PloToXml(ParamListObj Plo, string NodeName)
        {
            string xmlstr = string.Empty;
            System.IO.StringWriter sw = null;
            System.Xml.XmlTextWriter textWriter = null;
            try
            {
                sw = new System.IO.StringWriter();
                textWriter = new System.Xml.XmlTextWriter(sw);
                textWriter.WriteStartDocument();
                textWriter.WriteStartElement(NodeName);

                for (int i = 0; i < Plo.poLst.Count; i++)
                {
                    textWriter.WriteStartElement(Plo.poLst[i].Name, "");

                    textWriter.WriteString(Plo.poLst[i].Value.ToString());
                    textWriter.WriteEndElement();
                }
                textWriter.WriteEndElement();
                xmlstr = sw.ToString();
                textWriter.Flush();
                textWriter.Close();
                sw.Close();
                sw.Dispose();
            }
            catch
            {
            }
            finally
            {
                textWriter.Flush();
                textWriter.Close();
                sw.Close();
                sw.Dispose();
            }
            return xmlstr;
        }

        /// <summary>
        /// 讀取文字檔的內容到字串 (DdsResult.Msg)
        /// </summary>
        /// <param name="txtfn">文字檔路徑</param>
        /// <returns>DDS結果物件</returns>
        public static DdsResult ReadTextFile(string txtfn)
        {
            DdsResult Drt = new DdsResult();
            Drt.Flg = true;
            try
            {
                StreamReader sr = File.OpenText(txtfn);
                Drt.Msg = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception ex)
            {
                Drt.Flg = false;
                Drt.Msg = ex.ToString();
            }
            return Drt;
        }

        /// <summary>
        /// 輸出檔案下載，範例：ResponseFile(this.Request, this.Response, fn, filepath, 1024000);
        /// </summary>
        /// <param name="_Request">Request物件</param>
        /// <param name="_Response">Response物件</param>
        /// <param name="_fileName">檔案名稱</param>
        /// <param name="_fullPath">檔案路徑</param>        
        /// <returns>bool</returns>
        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath)
        {
            return ResponseFile(_Request, _Response, _fileName, _fullPath, 102400);
        }
        /// <summary>
        /// 輸出檔案下載，範例：ResponseFile(this.Request, this.Response, fn, filepath, 1024000);
        /// </summary>
        /// <param name="_Request">Request物件</param>
        /// <param name="_Response">Response物件</param>
        /// <param name="_fileName">檔案名稱</param>
        /// <param name="_fullPath">檔案路徑</param>
        /// <param name="_speed">下載速度，建議102400。</param>
        /// <returns>bool</returns>
        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {

                FileStream myFile = new FileStream(MakeFoldernameValid(_fullPath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    int pack = 10240; //10K bytes
                    //int sleep = 200;   //每秒5次   即5*10K bytes每秒
                    double d = 1000 * pack / _speed;
                    int sleep = (int)Math.Floor(d) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", Common.getPureLetter((fileLength - startBytes).ToString()));
                    if (startBytes != 0)
                    {
                        _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", Common.getPureLetter(startBytes.ToString()), Common.getPureLetter((fileLength - 1).ToString()), Common.getPureLetter(fileLength.ToString())));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(MakeFilenameValid(_fileName), System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    d = (fileLength - startBytes) / pack;
                    int maxCount = (int)Math.Floor(d) + 1;



                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(pack));
                            System.Threading.Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }

                    }
                }
                catch //(Exception ex)
                {
                    //HttpContext.Current.Response.Write(ex.ToString());
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion M to R 
        #region S to Z (8) 

        /// <summary>
        /// 把分隔字串的資料用PureLetter的規則過濾
        /// </summary>
        /// <param name="str">輸入</param>
        /// <param name="c">分隔符號</param>
        /// <returns></returns>
        public static string StringSplitFilter(string str, char c)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                string[] sarj = str.Split(c);
                if (sarj.Length > 0)
                {
                    int i = 0;
                    foreach (string s in sarj)
                    {

                        if (!string.IsNullOrEmpty(s))
                        {
                            if (i > 0)
                            {
                                sb.Append(c);
                            }
                            sb.Append(Common.getPureLetter(s));
                            i++;

                        }

                    }
                }
                return sb.ToString();
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 減輕input的XSS(不建議使用)
        /// </summary>
        /// <param name="input">輸入</param>
        /// <returns>文字</returns>
        public static string StripXss(string input)
        {
            string output = null;

            if (!string.IsNullOrEmpty(input))
            {
                // Encode the input string
                output = HttpUtility.HtmlEncode(input);

                // Selectively allow certain tags (note unsafe object tag added to allow flash movies)
                output = output.Replace("&lt;b&gt;", "<b>");
                output = output.Replace("&lt;/b&gt;", "</b>");
                output = output.Replace("&lt;strong&gt;", "<strong>");
                output = output.Replace("&lt;/strong&gt;", "</strong>");
                output = output.Replace("&lt;i&gt;", "<i>");
                output = output.Replace("&lt;/i&gt;", "</i>");
                output = output.Replace("&lt;em&gt;", "<em>");
                output = output.Replace("&lt;/em&gt;", "</em>");
                output = output.Replace("&lt;u&gt;", "<u>");
                output = output.Replace("&lt;/u&gt;", "</u>");
                output = output.Replace("&lt;hr /&gt;", "<hr />");
                output = output.Replace("&lt;br&gt;", "<br />");
                output = output.Replace("&lt;br/&gt;", "<br />");
                output = output.Replace("&lt;br /&gt;", "<br />");
                output = Regex.Replace(output, "&lt;img(.*?)/&gt;", "<img$1/>", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&lt;(div|span|table|tbody|thead|tr|th|td|font|ol|ul|li|a|p)(.*?)&gt;", "<$1$2>", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&lt;/(div|span|table|tbody|thead|tr|th|td|font|ol|ul|li|a|p)&gt;", "</$1>", RegexOptions.IgnoreCase);
                output = Regex.Replace(output, @"&lt;!--(.*?)--&gt;", "");
                output = output.Replace("&quot;", "\"");
                output = output.Replace("&amp;", "&");
            }

            return output;
        }
        /// <summary>
        /// 取得到第一個分隔符號的SubString
        /// </summary>
        /// <param name="orgstr">輸入</param>
        /// <param name="brk">分隔</param>
        /// <returns></returns>
        public static string SubString(string orgstr, string[] brk)
        {
            //int intLen = orgstr.Length;
            string strReturn = string.Empty;
            string[] s = null;
            try
            {
                s = orgstr.Split(brk, StringSplitOptions.RemoveEmptyEntries);
                strReturn = s[0];
            }
            catch
            {
            }
            return strReturn;
        }



        /// <summary>
        /// 把&#的字轉換成Unicode的字
        /// </summary>
        /// <param name="content">輸入</param>
        /// <returns>字串</returns>
        public static string UnicodeDecode(string content)
        {
            Regex reg = new Regex(@"&\#([0-9]{1,5});", RegexOptions.Compiled);
            return reg.Replace(content, m => Convert.ToChar(Convert.ToInt32(m.Groups[1].Value)).ToString());
        }
        /// <summary>
        /// 把Link文字轉成連結
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ConvertUrlsToLinks(string msg)
        {
            string open_new_window_msg = "另開新視窗";
            return ConvertUrlsToLinks(msg, open_new_window_msg);
        }
        /// <summary>
        /// 把Link文字Email文字轉成連結
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="open_new_window_msg"></param>
        /// <returns></returns>
        public static string ConvertContentToLinks(string msg, string open_new_window_msg)
        {
            return ConvertEmailsToMailToLinks(ConvertUrlsToLinks(msg, open_new_window_msg));
        }
        /// <summary>
        /// 把Link文字Email文字轉成連結
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ConvertContentToLinks(string msg)
        {
            return ConvertEmailsToMailToLinks(ConvertUrlsToLinks(msg));
        }
        /// <summary>
        /// 把Email文字轉成連結
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string ConvertEmailsToMailToLinks(string msg)
        {
            Regex emailregex = new Regex(@"([a-zA-Z_0-9.-]+\@[a-zA-Z_0-9.-]+\.\w+)",
                                   RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return emailregex.Replace(msg, "<a href='mailto:$1'>$1</a>");
        }
        /// <summary>
        /// 把Link文字轉成連結
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="open_new_window_msg"></param>
        /// <returns></returns>
        public static string ConvertUrlsToLinks(string msg, string open_new_window_msg)
        {
            //string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[_.a-z0-9-]+\.[a-z0-9\/_:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
            string regex = @"((www\.|(http|https|ftp|news|file)+\:\/\/)[_.a-z0-9-]+\.[a-z0-9\/_:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\)])";
            Regex r = new Regex(regex, RegexOptions.IgnoreCase);
            return r.Replace(msg, "<a href='$1' title='" + open_new_window_msg + "' target='_blank'>$1</a>").Replace("href='www", "href='http://www");
        }


        /// <summary>
        /// Substring替代功能
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubString(string strData, int startIndex, int length)
        {
            int intLen = strData.Length;
            int intSubLen = intLen - startIndex;
            string strReturn;
            if (length == 0)
                strReturn = "";
            else
            {
                if (intLen <= startIndex)
                    strReturn = "";
                else
                {
                    if (length > intSubLen)
                        length = intSubLen;

                    strReturn = strData.Substring(startIndex, length);
                }
            }
            return strReturn;
        }

        /// <summary>
        /// 寫入字串至文字檔
        /// </summary>
        /// <param name="ctnt">字串內容</param>
        /// <param name="txtfn">文字檔路徑</param>
        /// <returns>DDS結果物件</returns>
        public static DdsResult WriteTextFile(string ctnt, string txtfn)
        {
            DdsResult Drt = new DdsResult();
            Drt.Flg = true;
            Drt.Msg = "資料寫入成功!";
            try
            {
                //FileInfo t = new FileInfo(txtfn);
                StreamWriter Tex = new StreamWriter(txtfn);
                Tex.Write(ctnt);
                Tex.Close();
            }
            catch (Exception ex)
            {
                Drt.Flg = false;
                Drt.Msg = ex.ToString();
            }
            return Drt;
        }

        //當字串中可能包含特別的ASCII碼時，做額外的轉換
        /// <summary>
        /// 當字串中可能包含特別的ASCII碼時，做額外的轉換
        /// </summary>
        /// <param name="raw"></param>
        /// <returns></returns>
        public static string XmlAttributeEncode(string raw)
        {
            string s = System.Web.HttpUtility.HtmlEncode(raw);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                //要處理的範圍
                if ((c >= 0 && c < 32) || (c > 127 && c <= 255))
                    sb.AppendFormat("&#x{0:x};", (byte)c);
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 把XML字串轉成DdsResult物件
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <returns></returns>
        public static DdsResult XmlToDr(string xmlstr)
        {
            DdsResult Dr = new DdsResult();

            try
            {
                System.IO.StringReader sr = new StringReader(xmlstr);
                Dr.Dts = new DataSet();
                Dr.Dts.ReadXml(sr);
                Dr.Count = Dr.Dts.Tables[0].Rows.Count;
                Dr.Flg = true;
                Dr.Msg = "寫入成功";
            }
            catch (Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg = ex.ToString();
            }

            return Dr;
        }
        /// <summary>
        /// 把XML字串轉成DdsResult物件
        /// </summary>
        /// <param name="xmlstr"></param>
        /// <param name="xrm"></param>
        /// <returns></returns>
        public static DdsResult XmlToDr(string xmlstr, XmlReadMode xrm)
        {
            DdsResult Dr = new DdsResult();

            try
            {
                System.IO.StringReader sr = new StringReader(xmlstr);
                Dr.Dts = new DataSet();
                Dr.Dts.ReadXml(sr, xrm);
                Dr.Count = Dr.Dts.Tables[0].Rows.Count;
                Dr.Flg = true;
                Dr.Msg = "寫入成功";
            }
            catch (Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg = ex.ToString();
            }

            return Dr;
        }

        #endregion S to Z 

        


        #region "[靜態方法 SetSsnValue] 在設定值在Session中"
        /// <summary>
        /// 在設定值在Session中
        /// </summary>
        /// <param name="name">Session名稱</param>        
        /// <param name="value">Session對應值</param>        /// 
        /// <returns></returns>
        public static void SetSsnValue(string name, string value)
        {
            HttpContext.Current.Session[name] = value;
        }

        #endregion

        #region "[靜態方法 GetSsnValue] 在取得特定Session對應值"

        /// <summary>
        /// 在取得特定Session對應值
        /// </summary>
        /// <param name="name">Session名稱</param>        /// 
        /// <returns>Session對應值[string]</returns>

        public static string GetSsnValue(string name)
        {
            if (HttpContext.Current.Session[name] != null)
            {
                return HttpContext.Current.Session[name].ToString();
            }
            else
            {
                return "";
            }
        }

        #endregion

        #region "[靜態方法 GetComboParamObjStr] 取得參數物件組合字串"
        /// <summary>
        /// 取得參數物件組合字串
        /// </summary>
        /// <param name="ParamObjList">ParamObj List 物件</param>  
        /// <param name="s">組合字串格式字串（通常為 "" 或 "@"）</param>  /// 
        /// <returns>string</returns>

        public static string GetComboParamObjStr(List<ParamObj> ParamObjList, string s)
        {
            string str = "";
            for (int i = 0; i < ParamObjList.Count; i++)
            {
                if (i == 0)
                {
                    str += s + Common.getPureLetter(ParamObjList[i].Name);
                }
                else
                {
                    str += " , " + s + Common.getPureLetter(ParamObjList[i].Name);
                }
            }
            str = " ( " + str + " ) ";
            return str;
        }
        #endregion

        #region "[靜態方法 GetCondParamObjStr] 取得條件參數物件組合字串"
        /// <summary>
        /// 取得條件參數物件And組合字串
        /// </summary>
        /// <param name="ParamObjList">ParamObj List 物件</param>  
        /// <param name="s">組合字串格式字串（通常為 "AND" 或 "OR"）</param>  /// 
        /// <returns>string</returns>

        public static string GetCondParamObjStr(List<ParamObj> ParamObjList, string s)
        {
            string str = "";
            for (int i = 0; i < ParamObjList.Count; i++)
            {
                if (i == 0)
                {

                }
                else
                {
                    str += " " + s + " ";
                }
                str += Common.getPureLetter(ParamObjList[i].Name) + " = " + "@" + Common.getPureLetter(ParamObjList[i].Name);
            }
            str = " ( " + str + " ) ";
            return str;
        }
        #endregion

        #region "[靜態方法 GetUpdateParamObjStr] 取得更新參數物件組合字串"
        /// <summary>
        /// 取得更新參數物件And組合字串
        /// </summary>
        /// <param name="ParamObjList">ParamObj List 物件</param>          
        /// <returns>string</returns>

        public static string GetUpdateParamObjStr(List<ParamObj> ParamObjList)
        {
            string str = "";
            for (int i = 0; i < ParamObjList.Count; i++)
            {
                if (i == 0)
                {

                }
                else
                {
                    str += " , ";
                }
                str += Common.getPureLetter(ParamObjList[i].Name) + " = " + "@" + Common.getPureLetter(ParamObjList[i].Name);
            }
            str = " " + str + " ";
            return str;
        }
        #endregion

        #region "[靜態方法 GetHashPassword] 取得Hash過的Password"
        /// <summary>
        /// 取得Hash過的Password
        /// </summary>
        /// <param name="pwd">密碼</param>  
        /// <returns>String</returns>
        public static string GetHashPassword(string pwd)
        {
            string unencrypted = "TEST" + pwd;
            MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
            byte[] data = hasher.ComputeHash(Encoding.UTF8.GetBytes(unencrypted));
            string ret = "";
            for (int i = 0; i < data.Length; i++)
            {
                ret += data[i].ToString("x2");
            }
            ret = ret.Length > 32 ? ret.Substring(0, 32) : ret;
            return ret;
        }
        #endregion
        /// <summary>
        /// 依據檔案類型輸出成適當的Content-Type字串
        /// </summary>
        /// <param name="fileName">檔名</param>
        /// <returns></returns>
        public static string GetContentTypeForFileName(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName);
            using (Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext))
            {
                if (registryKey == null)
                    return null;
                var value = registryKey.GetValue("Content Type");
                return (value == null) ? string.Empty : value.ToString();
            }
        }
        /// <summary>
        /// Portal UrlEncode
        /// </summary>
        /// <param name="str">字串</param>
        /// <returns>Encode 轉換字串</returns>
        public static string getPortalUrlEncode(string str)
        {
            return Microsoft.JScript.GlobalObject.escape(str);
        }
        /// <summary>
        /// Portal UrlDecode
        /// </summary>
        /// <param name="str">字串</param>
        /// <returns>Decode 轉換字串</returns>
        public static string getPortalUrlDecode(string str)
        {
            return Microsoft.JScript.GlobalObject.unescape(str);
        }
    }

}
