using System.Text.RegularExpressions;
namespace DdsLib
{
	public class Regexpr
	{
		public struct RegularExp
		{
			public const string Chinese = @"^[\u4E00-\u9FA5\uF900-\uFA2D]+$";
			public const string Color = "^#[a-fA-F0-9]{6}";
			public const string Date = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
			public const string DateTime = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
			public const string Email = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
			public const string Float = @"^(-?\d+)(\.\d+)?$";
			public const string ImageFormat = @"\.(?i:jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$";
			public const string Integer = @"^-?\d+$";
			public const string IP = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
			public const string Letter = "^[A-Za-z]+$";
			public const string LowerLetter = "^[a-z]+$";
			public const string MinusFloat = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
			public const string MinusInteger = "^-[0-9]*[1-9][0-9]*$";
			public const string Mobile = "^0{0,1}13[0-9]{9}$";
			public const string NumbericOrLetterOrChinese = @"^[A-Za-z0-9\u4E00-\u9FA5\uF900-\uFA2D]+$";
			public const string Numeric = "^[0-9]+$";
			public const string NumericOrLetter = "^[A-Za-z0-9]+$";
			public const string NumericOrLetterOrUnderline = @"^\w+$";
			public const string PlusFloat = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
			public const string PlusInteger = "^[0-9]*[1-9][0-9]*$";
			public const string Telephone = @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?";
			public const string UnMinusFloat = @"^\d+(\.\d+)?$";
			public const string UnMinusInteger = @"\d+$";
			public const string UnPlusFloat = @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$";
			public const string UnPlusInteger = @"^((-\d+)|(0+))$";
			public const string UpperLetter = "^[A-Z]+$";
			public const string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
		}
		#region Fields (9) 

		#region A to F (9) 

		/// <summary>
		/// 時間
		/// </summary>
		public static string fmtDat = "^([0-9]{4})[- /.]([1-9]|0[1-9]|1[012])[- /.]([1-9]|0[1-9]|[12][0-9]|3[01])$";
		public static string fmtTime = "^((0[0-9]|1[0-9]|2[0-3])[:][0-5][0-9][:][0-5][0-9])$";
		/// <summary>
		/// 數字 0-9 驗證
		/// </summary>
		public static string fmtDgt = "^[0-9]*$";
		/// <summary>
		/// 數字 英文大小寫 底線 驗證
		/// </summary>
		public static string fmtDgtLtrUle = "^[0-9a-zA-Z_]*$";
		/// <summary>
		/// Email
		/// </summary>
		public static string fmtEmail = @"^[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";
		/// <summary>
		/// IP 驗證
		/// </summary>
		public static string fmtIp = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
		/// <summary>
		/// Order By 
		/// </summary>
		public static string fmtOrdBy = "^([aA][sS][cC]|[dD][eE][sS][cC])?$";
		public static string fmtSignFloat2 = @"(-)?\d+(\.\d*)?";
		public static string fmtUrl = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
		/// <summary>
		/// 關鍵字
		/// </summary>
		public static string frmKwd = "^([a-zA-Z0-9\u0100-\uFFFF])*$";

		public static string fmtMsUrl = @"^(?:http|https|ftp)://[a-zA-Z0-9\.\-]+(?:\:\d{1,5})?(?:[A-Za-z0-9\.\;\:\@\&\=\+\$\,\?/]|%u[0-9A-Fa-f]{4}|%[0-9A-Fa-f]{2})*$";

		#endregion A to F 

		#endregion Fields 

		#region Methods (8) 

		#region G to L (8) 
		/// <summary>
		/// 判斷字串是否與指定正則運算式匹配
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <param name="regularExp">正則運算式</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsMatch(string input, string regularExp)
		{
			return Regex.IsMatch(input, regularExp);
		}

		/// <summary>
		/// 驗證非負整數（正整數 + 0）
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsUnMinusInt(string input)
		{
			return Regex.IsMatch(input, RegularExp.UnMinusInteger);
		}

		/// <summary>
		/// 驗證正整數
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsPlusInt(string input)
		{
			return Regex.IsMatch(input, RegularExp.PlusInteger);
		}

		/// <summary>
		/// 驗證非正整數（負整數 + 0） 
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsUnPlusInt(string input)
		{
			return Regex.IsMatch(input, RegularExp.UnPlusInteger);
		}

		/// <summary>
		/// 驗證負整數
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsMinusInt(string input)
		{
			return Regex.IsMatch(input, RegularExp.MinusInteger);
		}

		/// <summary>
		/// 驗證整數
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsInt(string input)
		{
			return Regex.IsMatch(input, RegularExp.Integer);
		}

		/// <summary>
		/// 驗證非負浮點數（正浮點數 + 0）
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsUnMinusFloat(string input)
		{
			return Regex.IsMatch(input, RegularExp.UnMinusFloat);
		}

		/// <summary>
		/// 驗證正浮點數
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsPlusFloat(string input)
		{
			return Regex.IsMatch(input, RegularExp.PlusFloat);
		}

		/// <summary>
		/// 驗證非正浮點數（負浮點數 + 0）
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsUnPlusFloat(string input)
		{
			return Regex.IsMatch(input, RegularExp.UnPlusFloat);
		}

		/// <summary>
		/// 驗證負浮點數
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsMinusFloat(string input)
		{
			return Regex.IsMatch(input, RegularExp.MinusFloat);
		}

		/// <summary>
		/// 驗證浮點數
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsFloat(string input)
		{
			return Regex.IsMatch(input, RegularExp.Float);
		}

		/// <summary>
		/// 驗證由26個英文字母組成的字串
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsLetter(string input)
		{
			return Regex.IsMatch(input, RegularExp.Letter);
		}

		/// <summary>
		/// 驗證由中文組成的字串
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsChinese(string input)
		{
			return Regex.IsMatch(input, RegularExp.Chinese);
		}

		/// <summary>
		/// 驗證由26個英文字母的大寫組成的字串
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsUpperLetter(string input)
		{
			return Regex.IsMatch(input, RegularExp.UpperLetter);
		}

		/// <summary>
		/// 驗證由26個英文字母的小寫組成的字串
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsLowerLetter(string input)
		{
			return Regex.IsMatch(input, RegularExp.LowerLetter);
		}

		/// <summary>
		/// 驗證由數位和26個英文字母組成的字串
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsNumericOrLetter(string input)
		{
			return Regex.IsMatch(input, RegularExp.NumericOrLetter);
		}

		/// <summary>
		/// 驗證由數位組成的字串
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsNumeric(string input)
		{
			return Regex.IsMatch(input, RegularExp.Numeric);
		}
		/// <summary>
		/// 驗證由數位和26個英文字母或中文組成的字串
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsNumericOrLetterOrChinese(string input)
		{
			return Regex.IsMatch(input, RegularExp.NumbericOrLetterOrChinese);
		}

		/// <summary>
		/// 驗證由數位、26個英文字母或者下劃線組成的字串
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsNumericOrLetterOrUnderline(string input)
		{
			return Regex.IsMatch(input, RegularExp.NumericOrLetterOrUnderline);
		}

		/// <summary>
		/// 驗證email地址
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsEmail(string input)
		{
			return Regex.IsMatch(input, RegularExp.Email);
		}

		/// <summary>
		/// 驗證URL
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsUrl(string input)
		{
			return Regex.IsMatch(input, RegularExp.Url);
		}

		/// <summary>
		/// 驗證電話號碼
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsTelephone(string input)
		{
			return Regex.IsMatch(input, RegularExp.Telephone);
		}

		/// <summary>
		/// 驗證手機號碼
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsMobile(string input)
		{
			return Regex.IsMatch(input, RegularExp.Mobile);
		}

		/// <summary>
		/// 通過檔副檔名驗證圖像格式
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsImageFormat(string input)
		{
			return Regex.IsMatch(input, RegularExp.ImageFormat);
		}

		/// <summary>
		/// 驗證IP
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsIP(string input)
		{
			return Regex.IsMatch(input, RegularExp.IP);
		}

		/// <summary>
		/// 驗證日期（YYYY-MM-DD）
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsDate(string input)
		{
			return Regex.IsMatch(input.Replace("/", "-"), RegularExp.Date);
		}

		/// <summary>
		/// 驗證日期和時間（YYYY-MM-DD HH:MM:SS）
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsDateTime(string input)
		{
			return Regex.IsMatch(input.Replace("/", "-"), RegularExp.DateTime);
		}

		/// <summary>
		/// 驗證顏色（#ff0000）
		/// </summary>
		/// <param name="input">要驗證的字串</param>
		/// <returns>驗證通過返回true</returns>
		public static bool IsColor(string input)
		{
			return Regex.IsMatch(input, RegularExp.Color);
		}
		public static string getRegexTrim(string s, string pt, int len)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");            
			string rs = Regex.Replace(s, pt, "");
			try
			{
				rs = rs.Substring(0, len);
			}
			catch
			{
			}
			return rs;
		}

		public static DdsResult getRegexTrim(ref string s, string pt, int len)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
			DdsResult Dr = new DdsResult();
			//            string pt = @"[^\w]";
			bool b = Regex.IsMatch(s, pt);
			if (b)
			{
				Dr.Msg = "符合規則";
			}
			else
			{
				Dr.Msg = "不符合規則";
			}
			if (s.Length > len)
			{
				b = false;
				Dr.Msg += "-超過長度";
			}
			s = Regex.Replace(s, pt, "");
			try
			{
				s = s.Substring(0, len);
			}
			catch
			{
			}
			Dr.Flg = b;
			return Dr;
			//return rs;
		}

		/// <summary>
		/// 檢查輸入字串是否為數字、英文大小寫、底線的組合
		/// </summary>
		/// <param name="str">輸入字串</param>
		/// <returns>檢查結果[bool]</returns>
		public static bool is_Dg_Lt_Ul_Fmt(string str)
		{
			string rule = fmtDgtLtrUle;
			return Regex.IsMatch(str, rule);
		}

		/// <summary>
		/// 檢查輸入字串是否為數字
		/// </summary>
		/// <param name="str">輸入字串</param>
		/// <returns>檢查結果[bool]</returns>
		public static bool isDgFmt(string str)
		{
			string rule = fmtDgt;
			return Regex.IsMatch(str, rule);
		}

		/// <summary>
		/// 檢查輸入字串是否為日期
		/// </summary>
		/// <param name="str">輸入字串</param>
		/// <returns>檢查結果[bool]</returns>
		public static bool isDtFmt(string str)
		{
			string rule = fmtDat;
			return Regex.IsMatch(str, rule);
		}

		/// <summary>
		/// 檢查輸入字串是否為IP格式
		/// </summary>
		/// <param name="str">輸入字串</param>
		/// <returns>檢查結果[bool]</returns>
		public static bool isFmt(string str, string fmt)
		{            
			return Regex.IsMatch(str, fmt);
		}

		/// <summary>
		/// 檢查輸入字串是否為IP格式
		/// </summary>
		/// <param name="str">輸入字串</param>
		/// <returns>檢查結果[bool]</returns>
		public static bool isFmt(string str, string fmt , bool IsIgnCase)
		{         

			Regex rx = null;
			if (IsIgnCase)
			{
				rx = new Regex(fmt, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			}
			else
			{
				rx = new Regex(fmt);
			}
			return rx.IsMatch(str);
		}

		/// <summary>
		/// 檢查輸入字串是否為IP格式
		/// </summary>
		/// <param name="str">輸入字串</param>
		/// <returns>檢查結果[bool]</returns>
		public static bool isIpFmt(string str)
		{
			string rule = fmtIp;
			return Regex.IsMatch(str, rule);
		}

		#endregion G to L 

		#endregion Methods 

		public static string getPureLetter(string s)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
			string pt = @"[^\w]";
			string rs = Regex.Replace(s, pt, "");
			return rs;
		}

		public static bool getPureLetter(ref string s)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
			string pt = @"[^\w]";
			bool b = Regex.IsMatch(s, pt);
			s = Regex.Replace(s, pt, "");
			return b;
			//return rs;
		}

		public static string getPureAndSpaceLetter(string s)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
			string pt = @"[^\w|\s]";
			string rs = Regex.Replace(s, pt, "");
			return rs;
		}

		public static bool getPureAndSpaceLetter(ref string s)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
			string pt = @"[^\w|\s]";
			bool b = Regex.IsMatch(s, pt);
			s = Regex.Replace(s, pt, "");
			return b;
			//return rs;
		}

		public static string getPureEnglishLetter(string s)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
			string pt = @"[^\w|\s|,|-|?[\x27]]";
			//string pt = @"[^\w|\s|,]";
			string rs = Regex.Replace(s, pt, "");
			return rs;
		}

		public static bool getPureEnglishLetter(ref string s)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
			//string pt = @"[^\w|\s|,|-|?[\x27]]";
			string pt = @"[^\w|\s|,]";
			bool b = Regex.IsMatch(s, pt);
			s = Regex.Replace(s, pt, "");
			return b;
			//return rs;
		}

		public static string getUrlLetter(string s)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
			string pt = @"[^\w|\s|\.|-|/|:|=|&|?]";
			string rs = Regex.Replace(s, pt, "");
			return rs;
		}

		public static bool getUrlLetter(ref string s)
		{
			//string rs = Regex.Replace( s, @"/<br\s*?/??>/i", "\n");
			string pt = @"[^\w|\s|\.|-|/|:|=|&|?]";
			bool b = Regex.IsMatch(s, pt);
			s = Regex.Replace(s, pt, "");
			return b;
			//return rs;
		}

		#region "[靜態方法 ValidEmail] 檢查Email格式是否符合 (符合|不符合/true|false)"
		/// <summary>
		/// 檢查Email格式
		/// </summary>
		/// <param name="Email">使用者Email</param>  
		/// <returns>DdsResult</returns>

		public static DdsResult GrValidEmail(string Email)
		{
			DdsResult Gr = new DdsResult();
			Regex regex = new Regex(fmtEmail);
			Gr.Flg = regex.IsMatch(Email);
			if (Gr.Flg)
			{
				Gr.Msg = ScsMsg.EmailValid;
			}
			else
			{
				Gr.Msg = ErrMsg.EmailValid;
			}
			return Gr;

		}
		/// <summary>
		/// 檢查Email格式是否符合 (符合|不符合/true|false)
		/// </summary>
		/// <param name="Email">使用者Email</param>
		/// <returns>bool</returns>
		public static bool IsValidEmail(string Email)
		{
			DdsResult Gr = GrValidEmail(Email);
			return Gr.Flg;
		}
		#endregion
	}
}
