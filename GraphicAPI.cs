using System;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using DdsLib.Exif;
namespace DdsLib
{
    /// <summary>
    /// 影像處理API
    /// </summary>
    public class GraphicAPI
    {
        /// <summary>
        /// HW[指定高寬縮放（可能變形）]
        /// </summary>
        public static string modeHW = "HW";
        /// <summary>
        /// W[指定寬度，高度按比例]
        /// </summary>
        public static string modeW = "W";
        /// <summary>
        /// MAXW[指定寬度，高度按比例，小於原圖若小於寬，則不變]
        /// </summary>
        public static string modeMAXW = "MAXW";
        /// <summary>
        /// MAXH[指定高度，寬度按比例，小於原圖若小於高，則不變]
        /// </summary>
        public static string modeMAXH = "MAXH";

        /// <summary>
        /// HW[指定高寬縮放（可能變形），小於原圖若小於寬或高，則不變]
        /// </summary>
        public static string modeMAXHW = "MAXHW";

        /// <summary>
        /// H[指定高度，寬度按比例]
        /// </summary>
        public static string modeH = "H";
        /// <summary>
        /// CUT[指定高寬裁減（不變形）]
        /// </summary>
        public static string modeCUT = "CUT";
        /// <summary>
        /// ORG[原圖]不論寬高設定多少
        /// </summary>
        public static string modeORG = "ORG";
        /// <summary>
        /// LONGER[取長邊]
        /// </summary>
        public static string modeLONGER = "LONGER";
        /// <summary>
        /// MAXLONGER[取長邊，，小於原圖若小於寬或高，則不變]
        /// </summary>
        public static string modeMAXLONGER = "MAXLONGER";
        /// <summary>
        /// 建立縮圖
        /// </summary>
        /// <param name="width">寬[int]</param>
        /// <param name="height">高[int]</param>
        /// <param name="originalImage">影像物件[Imgae]</param>
        /// <param name="mode">HW[指定高寬縮放（可能變形）]、W[指定寬度，高度按比例]、H[指定高度，寬度按比例]、CUT[指定高寬裁減（不變形）]、ORG[原圖]、LONGER[取長邊]</param>        
        /// <example>
        /// <code>
        /// getThumbnail(originalImage, width, height, GraphicAPI.modeORG);
        /// </code>
        /// </example>
        /// <returns>取得縮圖</returns>
        public static Image getThumbnail(Image originalImage, int width, int height, string mode)
        {
            Image bitmap = null;
            try
            {
                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;

                switch (mode)
                {
                    // 指定高寬縮放（可能變形）                 
                    case "HW":
                        break;
                    // 指定寬度，高度按比例 
                    case "W":
                        height = originalImage.Height * width / originalImage.Width;
                        break;
                    case "MAXW":
                        if (originalImage.Width < width)
                        {
                            width = originalImage.Width;
                        }
                        height = originalImage.Height * width / originalImage.Width;
                        break;
                    // 指定高度，寬度按比例 
                    case "H":
                        width = originalImage.Width * height / originalImage.Height;
                        break;
                    case "MAXH":
                        if (originalImage.Height < height)
                        {
                            height = originalImage.Height;
                        }
                        width = originalImage.Width * height / originalImage.Height;
                        break;
                    //指定高寬裁減（不變形） 
                    case "CUT":
                        if (((double)originalImage.Width) / originalImage.Height > ((double)width) / height)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * width / height;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / width;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    case "ORG":
                        height = oh;
                        width = ow;
                        break;
                    case "LONGER":
                        if (ow >= oh)
                        {
                            height = originalImage.Height * width / originalImage.Width;
                        }
                        else
                        {
                            width = originalImage.Width * height / originalImage.Height;
                        }
                        break;
                    case "MAXLONGER":
                        if (ow >= oh)
                        {
                            if (originalImage.Width < width)
                            {
                                width = originalImage.Width;
                            }
                            height = originalImage.Height * width / originalImage.Width;
                        }
                        else
                        {
                            if (originalImage.Height < height)
                            {
                                height = originalImage.Height;
                            }
                            width = originalImage.Width * height / originalImage.Height;
                        }
                        break;

                    default:

                        break;
                }

                bitmap = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
                Graphics g = Graphics.FromImage(bitmap);
                //g.InterpolationMode = InterpolationMode.High;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                //g.Clear(Color.White);
                g.DrawImage(originalImage, new Rectangle(0, 0, width, height), new Rectangle(x, y, ow, oh), GraphicsUnit.Pixel);
            }
            catch
            {
                return null;
            }
            return bitmap;
        }
        /// <summary>
        /// 高畫質縮圖Codec
        /// </summary>
        /// <param name="mimeType">影像格式例： image/jpeg</param>
        /// <returns>ImageCodecInfo 物件</returns>
        public static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public static bool toJpeg(Image bitmap, string thumbnailPath)
        {
            try
            {

                System.Drawing.Imaging.Encoder myEncoder;//这个是重点类, 
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;
                //请注意这里的myImageCodecInfo声名..可以修改为更通用的.看后面 
                ImageCodecInfo myImageCodecInfo = null;
                switch (Path.GetExtension(thumbnailPath).ToLower())
                {
                    case ".png":
                        myImageCodecInfo = GetEncoderInfo("image/png");
                        //bitmap.Save(thumbnailPath + DateTime.Now.ToString("HHmmss") +".png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".gif":
                        myImageCodecInfo = GetEncoderInfo("image/gif");
                        //bitmap.Save(thumbnailPath + ".gif", System.Drawing.Imaging.ImageFormat.Gif);

                        break;
                    default:
                        myImageCodecInfo = GetEncoderInfo("image/jpeg");



                        break;
                }
                myEncoder = System.Drawing.Imaging.Encoder.Quality;
                myEncoderParameters = new EncoderParameters(1);
                // 在这里设置图片的质量等级为95L. 
                myEncoderParameter = new EncoderParameter(myEncoder, 120L);
                myEncoderParameters.Param[0] = myEncoderParameter;//将构建出来的EncoderParameter类赋给EncoderParameters数组 

                if (bitmap != null)
                {
                    //MemoryStream oMS = new MemoryStream();
                    //bitmap.Save(oMS, myImageCodecInfo, myEncoderParameters);
                    //using (System.IO.FileStream oFS = System.IO.File.Open(thumbnailPath, System.IO.FileMode.OpenOrCreate))
                    //{
                    //    oFS.Write(oMS.ToArray(), 0, oMS.ToArray().Length);
                    //}

                    bitmap.Save(thumbnailPath , myImageCodecInfo, myEncoderParameters);

                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
            return true;

        }

        /// <summary>
        /// 建立縮圖
        /// </summary>
        /// <param name="sourceImagePath">原圖路徑[string]</param>
        /// <param name="thumbnailPath">縮圖路徑[string]</param>
        /// <param name="width">寬[int]</param>
        /// <param name="height">高[int]</param>
        /// <param name="mode">HW[指定高寬縮放（可能變形）]、W[指定寬度，高度按比例]、H[指定高度，寬度按比例]、CUT[指定高寬裁減（不變形）]、ORG[原圖]、LONGER[取長邊]</param>
        public static bool MakeThumbnail(string sourceImagePath, string thumbnailPath, int width, int height, string mode)
        {
            try
            {
                Image originalImage = Image.FromFile(sourceImagePath);
                Image bitmap = getThumbnail(originalImage, width, height, mode);
                int w = bitmap.Width;
                int h = bitmap.Height;
                int orien = getOrien(sourceImagePath);
                if (orien > 1)
                {
                    rotating(ref bitmap, ref w, ref h, orien);
                }
                originalImage.Dispose();
                return toJpeg(bitmap, thumbnailPath);
            }
            catch
            {
                return false;
            }
            //return true;
        }

        public static string MakeThumbnail_1(string sourceImagePath, string thumbnailPath, int width, int height, string mode)
        {
            try
            {
                Image originalImage = Image.FromFile(sourceImagePath);
                Image bitmap = getThumbnail(originalImage, width, height, mode);
                int w = bitmap.Width;
                int h = bitmap.Height;
                int orien = getOrien(sourceImagePath);
                if (orien > 1)
                {
                    rotating(ref bitmap, ref w, ref h, orien);
                }
                originalImage.Dispose();
                return toJpeg(bitmap, thumbnailPath) ? "成功" : "失敗";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            //return true;
        }

        public static int getOrien(string imgfile)
        {
            int ret = 1;
            ExifTagCollection _exif = new ExifTagCollection(imgfile);
            //            int i = 0;
            //            1 = The 0th row is at the visual top of the image, and the 0th column is the visual left-hand side. 
            //2 = The 0th row is at the visual top of the image, and the 0th column is the visual right-hand side. 
            //3 = The 0th row is at the visual bottom of the image, and the 0th column is the visual right-hand side. 
            //4 = The 0th row is at the visual bottom of the image, and the 0th column is the visual left-hand side. 
            //5 = The 0th row is the visual left-hand side of the image, and the 0th column is the visual top. 
            //6 = The 0th row is the visual right-hand side of the image, and the 0th column is the visual top. 
            //7 = The 0th row is the visual right-hand side of the image, and the 0th column is the visual bottom. 
            //8 = The 0th row is the visual left-hand side of the image, and the 0th column is the visual bottom. 
            foreach (ExifTag tag in _exif)
            {
                if (tag.FieldName == "Orientation")
                {
                    switch (tag.Value)
                    {
                        case "The 0th row is at the visual top of the image, and the 0th column is the visual right-hand side.":
                            ret = 2;
                            break;
                        case "The 0th row is at the visual bottom of the image, and the 0th column is the visual right-hand side.":
                            ret = 3;
                            break;
                        case "The 0th row is at the visual bottom of the image, and the 0th column is the visual left-hand side.":
                            ret = 4;
                            break;
                        case "The 0th row is the visual left-hand side of the image, and the 0th column is the visual top.":
                            ret = 5;
                            break;
                        case "The 0th row is the visual right-hand side of the image, and the 0th column is the visual top.":
                            ret = 6;
                            break;
                        case "The 0th row is the visual right-hand side of the image, and the 0th column is the visual bottom.":
                            ret = 7;
                            break;
                        case "The 0th row is the visual left-hand side of the image, and the 0th column is the visual bottom.":
                            ret = 8;
                            break;
                        default:
                            break;
                    }
                    break;
                }
            }
            return ret;
        }
        public static void rotating(ref Image img, ref int width, ref int height, int orien)
        {
            int ow = width;
            switch (orien)
            {
                case 2:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);//horizontal flip
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);//right-top
                    break;
                case 4:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);//vertical flip
                    break;
                case 5:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);//right-top
                    width = height;
                    height = ow;
                    break;
                case 7:
                    img.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);//left-bottom
                    width = height;
                    height = ow;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 建立縮圖
        /// </summary>
        /// <param name="sourceImagePath">原圖路徑[string]</param>
        /// <param name="thumbnailPath">縮圖路徑[string]</param>
        /// <param name="width">寬[int]</param>
        /// <param name="height">高[int]</param>
        /// <param name="mode">HW[指定高寬縮放（可能變形）]、W[指定寬度，高度按比例]、H[指定高度，寬度按比例]、CUT[指定高寬裁減（不變形）]、ORG[原圖]、LONGER[取長邊]</param>
        public static DdsResult getMakeThumbnailDr(string sourceImagePath, string thumbnailPath, int width, int height, string mode)
        {
            DdsResult Dr = new DdsResult();
            try
            {
                BufferedStream bs = new BufferedStream((Stream)new FileStream(sourceImagePath, FileMode.OpenOrCreate, FileAccess.ReadWrite));
                Image originalImage = Image.FromStream(bs);
                bs.Close();
                bs.Dispose();
                Image bitmap = getThumbnail(originalImage, width, height, mode);
                Dr.Flg = toJpeg(bitmap, thumbnailPath);
                if (Dr.Flg)
                {
                    Dr.Msg = "縮圖成功";
                }
                else
                {
                    Dr.Msg = "縮圖失敗";
                }
            }
            catch (Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg = "發生例外事件：" + ex.ToString();
                return Dr;
            }
            return Dr;
        }


        /// <summary> 

        /// 设置GIF大小 
        /// </summary> 
        /// <param name="path">图片路径</param> 
        /// <param name="width">宽</param> 
        /// <param name="height">高</param> 
        public static void setGifSize(string path, int width, int height)
        {
            Image gif = new Bitmap(width, height);
            Image frame = new Bitmap(width, height);
            Image res = Image.FromFile(path);
            Graphics g = Graphics.FromImage(gif);
            Rectangle rg = new Rectangle(0, 0, width, height);
            Graphics gFrame = Graphics.FromImage(frame);

            foreach (Guid gd in res.FrameDimensionsList)
            {
                FrameDimension fd = new FrameDimension(gd);

                //因为是缩小GIF文件所以这里要设置为Time，如果是TIFF这里要设置为PAGE，因为GIF以时间分割，TIFF为页分割 
                FrameDimension f = FrameDimension.Time;
                int count = res.GetFrameCount(fd);
                ImageCodecInfo codecInfo = GetEncoder(ImageFormat.Gif);
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;
                EncoderParameters eps = null;

                for (int i = 0; i < count; i++)
                {
                    res.SelectActiveFrame(f, i);
                    if (0 == i)
                    {

                        g.DrawImage(res, rg);

                        eps = new EncoderParameters(1);

                        //第一帧需要设置为MultiFrame 

                        eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);
                        bindProperty(res, gif);
                        gif.Save(@"C:\tmp\test\aaa.gif", codecInfo, eps);
                    }
                    else
                    {

                        gFrame.DrawImage(res, rg);

                        eps = new EncoderParameters(1);

                        //如果是GIF这里设置为FrameDimensionTime，如果为TIFF则设置为FrameDimensionPage 

                        eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionTime);

                        bindProperty(res, frame);
                        gif.SaveAdd(frame, eps);
                    }
                }

                eps = new EncoderParameters(1);
                eps.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
                gif.SaveAdd(eps);
            }
        }

        /// <summary> 
        /// 将源图片文件里每一帧的属性设置到新的图片对象里 
        /// </summary> 
        /// <param name="a">源图片帧</param> 
        /// <param name="b">新的图片帧</param> 
        public static void bindProperty(Image a, Image b)
        {

            //这个东西就是每一帧所拥有的属性，可以用GetPropertyItem方法取得这里用为完全复制原有属性所以直接赋值了 

            //顺便说一下这个属性里包含每帧间隔的秒数和透明背景调色板等设置，这里具体那个值对应那个属性大家自己在msdn搜索GetPropertyItem方法说明就有了 

            for (int i = 0; i < a.PropertyItems.Length; i++)
            {
                b.SetPropertyItem(a.PropertyItems[i]);
            }
        }
        /// <summary>
        /// RotateImage
        /// </summary>
        /// <param name="sm"></param>
        /// <returns></returns>
        public static Stream RotateImage(Stream sm)
        {
            Image img = Image.FromStream(sm);
            //var exif = img.PropertyItems;
            if (Array.IndexOf(img.PropertyIdList, 274) > -1)
            {
                var orien = (int)img.GetPropertyItem(274).Value[0];
                switch (orien)
                {
                    case 2:
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);//horizontal flip  
                        break;
                    case 3:
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);//right-top  
                        break;
                    case 4:
                        img.RotateFlip(RotateFlipType.RotateNoneFlipY);//vertical flip  
                        break;
                    case 5:
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);//right-top  
                        break;
                    case 7:
                        img.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);//left-bottom  
                        break;
                    default:
                        break;
                }
            }
            try
            {
                img.RemovePropertyItem(274);
            }
            catch { }
            var stream = new System.IO.MemoryStream();
            img.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            return stream;
            //byte orien = 0;
            //var item = exif.Where(m => m.Id == 274).ToArray();
            //if (item.Length > 0)
            //    orien = item[0].Value[0];


        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 建立縮圖
        /// </summary>
        /// <param name="sourceImageStream">原圖Stream</param>
        /// <param name="thumbnailPath">縮圖路徑[string]</param>
        /// <param name="width">寬[int]</param>
        /// <param name="height">高[int]</param>
        /// <param name="mode">HW[指定高寬縮放（可能變形）]、W[指定寬度，高度按比例]、H[指定高度，寬度按比例]、CUT[指定高寬裁減（不變形）]、ORG[原圖]、LONGER[取長邊]</param>
        public static bool MakeThumbnail(Stream sourceImageStream, string thumbnailPath, int width, int height, string mode)
        {

            try
            {
                Image originalImage = Image.FromStream(sourceImageStream);
                Image bitmap = getThumbnail(originalImage, width, height, mode);
                return toJpeg(bitmap, thumbnailPath);
            }
            catch
            {
                return false;
            }
        }

        ///// <summary>
        ///// 高畫質縮圖Codec
        ///// </summary>
        ///// <param name="mimeType">影像格式例： image/jpeg</param>
        ///// <returns>ImageCodecInfo 物件</returns>
        //private static ImageCodecInfo GetEncoderInfo(String mimeType)
        //{
        //    int j;
        //    ImageCodecInfo[] encoders;
        //    encoders = ImageCodecInfo.GetImageEncoders();
        //    for (j = 0; j < encoders.Length; ++j)
        //    {
        //        if (encoders[j].MimeType == mimeType)
        //            return encoders[j];
        //    }
        //    return null;
        //}
        public static byte[] ImageToBuffer(Image image)
        {
            byte[] buffer = null;
            using (Bitmap oBitmap = new Bitmap(image))
            {
                using (MemoryStream MS = new MemoryStream())
                {
                    oBitmap.Save(MS, System.Drawing.Imaging.ImageFormat.Jpeg);
                    MS.Position = 0;
                    buffer = new byte[MS.Length];
                    MS.Read(buffer, 0, Convert.ToInt32(MS.Length));
                    MS.Flush();
                }
            }

            return buffer;
        }
        public static Image BufferToImage(byte[] Buffer)
        {
            byte[] data = null;
            Image oImage = null;
            MemoryStream oMemoryStream = null;
            Bitmap oBitmap = null;
            //建立副本
            data = (byte[])Buffer.Clone();
            try
            {
                oMemoryStream = new MemoryStream(data);
                //設定資料流位置
                oMemoryStream.Position = 0;
                oImage = System.Drawing.Image.FromStream(oMemoryStream);
                //建立副本
                oBitmap = new Bitmap(oImage);
            }
            catch
            {
                throw;
            }
            finally
            {
                oMemoryStream.Close();
                oMemoryStream.Dispose();
                oMemoryStream = null;
            }
            //return oImage;
            return oBitmap;
        }
        public static Stream RotateImage(Stream sm, string thumbnailPath)
        {
            Image img = Image.FromStream(sm);
            //var exif = img.PropertyItems;
            if (Array.IndexOf(img.PropertyIdList, 274) > -1)
            {
                var orien = -1;
                try
                {
                    orien = (int)img.GetPropertyItem(274).Value[0];
                }
                catch { }
                switch (orien)
                {
                    case 2:
                        img.RotateFlip(RotateFlipType.RotateNoneFlipX);//horizontal flip  
                        break;
                    case 3:
                        img.RotateFlip(RotateFlipType.Rotate180FlipNone);//right-top  
                        break;
                    case 4:
                        img.RotateFlip(RotateFlipType.RotateNoneFlipY);//vertical flip  
                        break;
                    case 5:
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        img.RotateFlip(RotateFlipType.Rotate90FlipNone);//right-top  
                        break;
                    case 7:
                        img.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        img.RotateFlip(RotateFlipType.Rotate270FlipNone);//left-bottom  
                        break;
                    default:
                        break;
                }
            }
            try
            {
                img.RemovePropertyItem(274);
            }
            catch { }

            byte[] _SourceImage;
            _SourceImage = ImageToBuffer(img);
            Image image = BufferToImage(_SourceImage);

            System.Drawing.Imaging.Encoder myEncoder;//这个是重点类, 
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            //请注意这里的myImageCodecInfo声名..可以修改为更通用的.看后面 
            ImageCodecInfo myImageCodecInfo = null;
            switch (Path.GetExtension(thumbnailPath).ToLower())
            {
                case ".png":
                    myImageCodecInfo = GetEncoderInfo("image/png");
                    //bitmap.Save(thumbnailPath + DateTime.Now.ToString("HHmmss") +".png", System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case ".gif":
                    myImageCodecInfo = GetEncoderInfo("image/gif");
                    //bitmap.Save(thumbnailPath + ".gif", System.Drawing.Imaging.ImageFormat.Gif);

                    break;
                default:
                    myImageCodecInfo = GetEncoderInfo("image/jpeg");



                    break;
            }
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameters = new EncoderParameters(1);
            // 在这里设置图片的质量等级为95L. 
            myEncoderParameter = new EncoderParameter(myEncoder, 120L);
            myEncoderParameters.Param[0] = myEncoderParameter;//将构建出来的EncoderParameter类赋给EncoderParameters数组 
            var stream = new System.IO.MemoryStream();
            //img.Save(stream, ImageFormat.Jpeg);
            image.Save(stream, myImageCodecInfo, myEncoderParameters);
            stream.Position = 0;
            //var stream = new System.IO.MemoryStream();
            //img.Save(stream, ImageFormat.Jpeg);
            //stream.Position = 0;
            return stream;
            //byte orien = 0;
            //var item = exif.Where(m => m.Id == 274).ToArray();
            //if (item.Length > 0)
            //    orien = item[0].Value[0];


        }
    }

    public class DdsExifObj
    {
        public string EquipmentMake = "";
        public string CameraModel = "";
        public string ModifyDate = "";
        public string ExposureTime = "";
        public string ISOSpeed = "";
        public string ImageDescription = "";
        public string FocalLength = "";
        public string FNumber = "";
        public string LightSource = "";
        public DdsExifObj(string filePath)
        {
            Image img = Image.FromFile(filePath);
            PropertyItem[] pt = img.PropertyItems;
            for (int i = 0; i < pt.Length; i++)
            {

                PropertyItem p = pt[i];

                switch (pt[i].Id)
                {  // 设备制造商 20. 

                    case 0x010F:

                        EquipmentMake = System.Text.ASCIIEncoding.ASCII.GetString(pt[i].Value);

                        break;

                    case 0x0110: // 设备型号 25. 

                        CameraModel = GetValueOfType2(p.Value);

                        break;

                    case 0x0132: // 拍照时间 30.

                        ModifyDate = GetValueOfType2(p.Value);

                        break;

                    case 0x829A: // .曝光时间 rational

                        ExposureTime = GetValueOfType5(p.Value) + " sec";

                        break;

                    case 0x8827: // ISO 40.  short

                        ISOSpeed = GetValueOfType3(p.Value);

                        break;

                    case 0x010E: // 图像说明info.description

                        ImageDescription = GetValueOfType2(p.Value);

                        break;

                    case 0x920a: //相片的焦距

                        FocalLength = GetValueOfType5A(p.Value) + " mm";

                        break;

                    case 0x829D: //相片的光圈值

                        FNumber = GetValueOfType5A(p.Value);

                        break;

                    default:

                        break;

                }
            }
        }
        public static string GetValueOfType2(byte[] b)// 对type=2 的value值进行读取
        {
            return System.Text.Encoding.ASCII.GetString(b);
        }
        public static string GetValueOfType3(byte[] b) //对type=3 的value值进行读取
        {
            if (b.Length != 2) return "unknow";
            return Convert.ToUInt16(b[1] << 8 | b[0]).ToString();
        }
        public static string GetValueOfType5(byte[] b) //对type=5 的value值进行读取
        {
            if (b.Length != 8) return "unknow";
            UInt32 fm, fz;
            fm = 0;
            fz = 0;
            fz = Convert.ToUInt32(b[7] << 24 | b[6] << 16 | b[5] << 8 | b[4]);
            fm = Convert.ToUInt32(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);
            return fm.ToString() + "/" + fz.ToString() + " sec";
        }
        public static string GetValueOfType5A(byte[] b)//获取光圈的值
        {
            if (b.Length != 8) return "unknow";
            UInt32 fm, fz;
            fm = 0;
            fz = 0;
            fz = Convert.ToUInt32(b[7] << 24 | b[6] << 16 | b[5] << 8 | b[4]);
            fm = Convert.ToUInt32(b[3] << 24 | b[2] << 16 | b[1] << 8 | b[0]);
            double temp = (double)fm / fz;
            return (temp).ToString();
        }

        
    }
}