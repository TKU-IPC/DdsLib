using System;
using System.Text;
using System.Net.Mail;
using System.IO;
namespace DdsLib
{
    /// <summary>
    ///  發信相關API
    /// </summary>
    public class MailAPI
    {
        public MailMessage MailMsg = null;
        public string SmtpNme = "xxx.tku.edu.tw";
        public string FromNme = "XX組";
        public string FromMail = "epaper@xxx.tku.edu.tw";
        public string Subject = "XX組郵件元件";
        public string Content = @"<html><title>testtitle<title><body>testbody</body></html>";
        public string BdEncoding = "UTF-8";

        /// <summary>
        /// 初始MailMsg物件
        /// </summary>
        private void iniMailMsg()
        {
            MailMsg = new MailMessage();
            MailMsg.From = new MailAddress(FromMail, FromNme);

            //MailMsg.From.DisplayName = FromNme.Trim();
            //MailMsg.BodyFormat = MailFormat.Html;
            MailMsg.IsBodyHtml = true;
            //MailMsg.BodyEncoding = System.Text.Encoding.GetEncoding(BdEncoding);
            if (BdEncoding == "UTF-8")
            {
                MailMsg.BodyEncoding = Encoding.UTF8;
                MailMsg.SubjectEncoding = Encoding.UTF8;
            }
            //MailMsg.Headers["Content-Transfer-Encoding"] = "Quoted-Printable";
            MailMsg.Headers["Content-Type"] = "text/html";
            MailMsg.Subject = Subject;
            MailMsg.Body = Content;
        }

        /// <summary>
        /// 建構子
        /// </summary>
        public MailAPI()
        {
            iniMailMsg();
        }

        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="smtpnme">Smtp Server　名稱</param>
        /// <param name="fromnme">寄件者名稱</param>
        /// <param name="frommail">寄件者姓名</param>
        /// <param name="subject">標題</param>
        /// <param name="html">郵件內容</param>
        /// <param name="bdencoding">郵件編碼</param>
        public MailAPI(string smtpnme, string fromnme, string frommail, string subject, string html,string bdencoding)
        {
            SmtpNme = smtpnme;
            FromNme = fromnme;
            FromMail = frommail;
            Subject = subject;

            BdEncoding = bdencoding;
            Content = html;
            iniMailMsg();
        }

        /// <summary>
        /// 設定密件收信者
        /// </summary>
        /// <param name="Plo">ParamListObj物件</param>
        public void SetBCC(ParamListObj Plo)
        {
            if (Plo.poLst.Count > 0)
            {
                //string bcc = "";
                foreach (ParamObj po in Plo.poLst)
                {
                    //bcc += string.Format("{0}<{1}>,", po.Name.ToString().Trim(), po.Value.ToString().Trim().ToLower());
                    MailMsg.Bcc.Add(new MailAddress(po.Value.ToString().Trim().ToLower(), po.Name.ToString().Trim()));
                }
                
            }
        }

        /// <summary>
        /// 設定 Cc 收件者
        /// </summary>
        /// <param name="Plo">ParamListObj物件</param>
        public void SetCC(ParamListObj Plo)
        {
            if (Plo.poLst.Count > 0)
            {
                //string cc = "";
                foreach (ParamObj po in Plo.poLst)
                {
                    //cc += string.Format("{0}<{1}>,", po.Name.ToString().Trim(), po.Value.ToString().Trim().ToLower());
                    MailMsg.CC.Add(new MailAddress(po.Value.ToString().Trim().ToLower(), po.Name.ToString().Trim()));
                }
                //MailMsg.Cc = cc.Substring(0, cc.Length - 1);
            }
        }

        /// <summary>
        /// 設定 Bcc 收件者
        /// </summary>
        /// <param name="Plo">ParamListObj物件</param>
        public void SetTo(ParamListObj Plo)
        {
            if (Plo.poLst.Count > 0)
            {
                //string to = "";
                foreach (ParamObj po in Plo.poLst)
                {
                    //to += string.Format("{0}<{1}>,", po.Name.ToString().Trim(), po.Value.ToString().Trim().ToLower());
                    MailMsg.CC.Add(new MailAddress(po.Value.ToString().Trim().ToLower(), po.Name.ToString().Trim()));
                }
                //MailMsg.To = to.Substring(0, to.Length - 1);
            }
        }


        public void SetContent(string HtmlFileName, Encoding TextEncoding, ParamListObj Plo)
        {
            string line = "";
            try
            {                
                StreamReader sr = new StreamReader(HtmlFileName,TextEncoding);                
                line = sr.ReadToEnd();                
                sr.Close();                
            }
            catch 
            {                
            }
            finally
            {                
            }
            foreach (ParamObj po in Plo.poLst)
            {
                line = line.Replace(po.Name, po.Value.ToString());
            }
            MailMsg.Body = line;
        }

        public DdsResult Send()
        {
            DdsResult Dr = new DdsResult();

            try
            {
                SmtpClient SmtpMail = new SmtpClient();
                SmtpMail.Host = SmtpNme;
                SmtpMail.Port = 25;
                SmtpMail.Send(MailMsg);
                Dr.Flg = true;
                Dr.Msg = "傳送成功";
                //SmtpMail.
            }
            catch (Exception ex) 
            {
                Dr.Flg = false ;
                Dr.Msg = ex.ToString();                
            }
            return Dr;
        }

    }
}
