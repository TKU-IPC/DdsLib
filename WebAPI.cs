using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.Text;

namespace DdsLib
{
    /// <summary>
    /// Web相關API
    /// </summary>
    public class WebAPI
    {

        /// <summary>
        /// 預設編碼
        /// </summary>
        public static Encoding defaultCod = UTF8Encoding.UTF8;

        public static string[] mobiles = new[] {
                "midp", "j2me", "avant", "docomo", "novarra", "palmos", "palmsource",
                "240x320", "opwv", "chtml","pda", "windows ce", "mmp/",
                "blackberry", "mib/", "symbian", "wireless", "nokia", "hand", "mobi",
                "phone", "cdm", "up.b", "audio", "sie-", "sec-", "samsung", "htc",
                "mot-", "mitsu", "sagem", "sony", "alcatel", "lg", "eric", "vx",
                "NEC", "philips", "mmm", "xx", "panasonic", "sharp", "wap", "sch",
                "rover", "pocket", "benq", "java", "pt", "pg", "vox", "amoi",
                "bird", "compal", "kg", "voda","sany", "kdd", "dbt", "sendo",
                "sgh", "gradi", "jb", "dddi", "moto", "iphone", "android",
                "iPod", "incognito", "webmate", "dream", "cupcake", "webos",
                "s8000", "bada", "googlebot-mobile", "ipad" 
        };

        public static string[] android = new[] { "android" };
        public static string[] ios = new[] { "ipod", "ipad", "iphone" };

        /// <summary>
        /// 判斷是否為Android瀏覽器
        /// </summary>
        /// <param name="UserAnget">UserAnget</param>
        /// <returns>Android瀏覽器（是）/非Android瀏覽器（否）</returns>
        public static bool isAndroid(string UserAnget)
        {
            if (string.IsNullOrEmpty(UserAnget))
                return false;

            foreach (var item in android)
            {
                if (UserAnget.ToLower().IndexOf(item) != -1)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判斷是否為特定瀏覽器
        /// </summary>
        /// <param name="UserAnget">UserAnget</param>
        /// <returns>特定瀏覽器（是）/非特定瀏覽器（否）</returns>
        public static bool isSomeBrowsers(string UserAnget , string[] browers)
        {
            try
            {
                if (browers == null)
                    return false;
                if (browers.Length == 0)
                    return false;
                if (string.IsNullOrEmpty(UserAnget))
                    return false;

                foreach (var item in browers)
                {
                    if (UserAnget.ToLower().IndexOf(item) != -1)
                        return true;
                }
            }
            catch {
                return false;
            }
            return false;
        }

        /// <summary>
        /// 判斷是否為iOS瀏覽器
        /// </summary>
        /// <param name="UserAnget">UserAnget</param>
        /// <returns>iOS瀏覽器（是）/非iOS瀏覽器（否）</returns>
        public static bool isiOS(string UserAnget)
        {
            if (string.IsNullOrEmpty(UserAnget))
                return false;

            foreach (var item in ios)
            {
                if (UserAnget.ToLower().IndexOf(item) != -1)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 判斷是否為行動版瀏覽器
        /// </summary>
        /// <param name="UserAnget">UserAnget</param>
        /// <returns>行動版瀏覽器（是）/非行動版瀏覽器（否）</returns>
        public static bool isMobile(string UserAnget)
        {
            if (string.IsNullOrEmpty(UserAnget))
                return false;

            foreach (var item in mobiles)
            {
                if (UserAnget.ToLower().IndexOf(item) != -1)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 取得網址之原始碼
        /// </summary>
        /// <param name="url">網址</param>
        /// <returns>原始碼</returns>
        public static string getUrlContent(string url)
        {
            return getUrlContent(url, defaultCod);
        }
        /// <summary>
        /// 取得網址之原始碼
        /// </summary>
        /// <param name="url">網址</param>
        /// <param name="cod">原始碼編碼</param>
        /// <returns>原始碼</returns>
        public static string getUrlContent(string url,Encoding cod)
        {
            try
            {
                WebClient Wc = new WebClient();

                Wc.UseDefaultCredentials = true;
                Wc.Encoding = cod;
                string str = Wc.DownloadString(url);
                return str;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

     

    }
    /// <summary>
    /// 網頁縮圖
    /// </summary>
    /// <example>
    /// <code>
    /// WebSiteThumbnail wt = new WebSiteThumbnail(url ,BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight);
    /// Bitmap bmp = wt.GenerateWebSiteThumbnailImage();
    /// </code>
    /// </example>
    public class WebSiteThumbnail
    {
        /// <summary>
        /// Bitmap物件
        /// </summary>
        Bitmap m_Bitmap;
        /// <summary>
        /// 輸入網址
        /// </summary>
        string m_Url;
        /// <summary>
        /// 瀏覽寬度
        /// </summary>
        int m_BrowserWidth;
        /// <summary>
        /// 瀏覽高度
        /// </summary>
        int m_BrowserHeight;
        /// <summary>
        /// 截圖寬度
        /// </summary>
        int m_ThumbnailWidth;
        /// <summary>
        /// 截圖高度
        /// </summary>
        int m_ThumbnailHeight;
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="Url">網址</param>
        /// <param name="BrowserWidth">瀏覽寬度</param>
        /// <param name="BrowserHeight">瀏覽高度</param>
        /// <param name="ThumbnailWidth">截圖寬度</param>
        /// <param name="ThumbnailHeight">截圖高度</param>
        public WebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            m_Url = Url;
            m_BrowserHeight = BrowserHeight;
            m_BrowserWidth = BrowserWidth;
            m_ThumbnailWidth = ThumbnailWidth;
            m_ThumbnailHeight = ThumbnailHeight;
        }
        /// <summary>
        /// 取得瀏覽網址縮圖
        /// </summary>
        /// <param name="Url">網址</param>
        /// <param name="BrowserWidth">瀏覽寬度</param>
        /// <param name="BrowserHeight">瀏覽高度</param>
        /// <param name="ThumbnailWidth">截圖寬度</param>
        /// <param name="ThumbnailHeight">截圖高度</param>
        /// <returns>Bitmap物件</returns>
        public static Bitmap GetWebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            WebSiteThumbnail thumbnailGenerator = new WebSiteThumbnail(Url, BrowserWidth, BrowserHeight, ThumbnailWidth, ThumbnailHeight);
            return thumbnailGenerator.GenerateWebSiteThumbnailImage();
        }
        /// <summary>
        /// 取得縮圖
        /// </summary>
        /// <returns>Bitmap物件</returns>
        public Bitmap GenerateWebSiteThumbnailImage()
        {
            Thread m_thread = new Thread(new ThreadStart(_GenerateWebSiteThumbnailImage));
            m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.Start();
            m_thread.Join();
            return m_Bitmap;
        }
        /// <summary>
        /// 縮圖處理
        /// </summary>
        private void _GenerateWebSiteThumbnailImage()
        {
            WebBrowser m_WebBrowser = new WebBrowser();
            m_WebBrowser.ScrollBarsEnabled = false;
            m_WebBrowser.Navigate(m_Url);
            m_WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
            while (m_WebBrowser.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
            m_WebBrowser.Dispose();
        }
        /// <summary>
        /// 瀏覽畫面讀取
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser m_WebBrowser = (WebBrowser)sender;
            m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, m_WebBrowser.Document.Body.ScrollRectangle.Height);
            //m_WebBrowser.ClientSize = new Size(m_WebBrowser.Document.Body.ScrollRectangle.Width, m_WebBrowser.Document.Body.ScrollRectangle.Height);
            //webBrowser.Document.Body.ScrollRectangle.Height
            m_WebBrowser.ScrollBarsEnabled = false;
            //m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
            m_Bitmap = new Bitmap(this.m_BrowserWidth, m_WebBrowser.Document.Body.ScrollRectangle.Height);
            m_WebBrowser.BringToFront();
            m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
            //m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);
            m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(this.m_BrowserWidth, m_WebBrowser.Document.Body.ScrollRectangle.Height, null, IntPtr.Zero);
        }
    }
}
