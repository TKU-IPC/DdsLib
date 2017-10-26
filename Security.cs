using System;
using System.Text;
using System.IO;
using System.Security.Cryptography; 

namespace DdsLib
{
    /// <!--Security類別--> 
    /**//// <summary> 
    /// Security類別 - design By Phoenix 2008 - 
    /// </summary> 
    public class Security 
    { 
        private string _Key; 
        private string _IV; 
 
        /**//// <summary> 
        /// 加密金鑰(8個英文字) 
        /// </summary> 
        public string Key 
        { 
            set 
            { 
                _Key = value.Length == 8 ? value : "1234567K"; // 可自訂
            } 
        } 
        /**//// <summary> 
        /// 初始化向量(8個英文字) 
        /// </summary> 
        public string IV 
        { 
            set 
            { 
                _IV = value.Length == 8 ? value : "7654321I";                 
            } 
        } 
 
        /**//// <summary> 
        /// 初始化 Clib.Security 類別的新執行個體 
        /// </summary> 
        public Security() 
        {
            _Key = "1234567K";
            _IV = "7654321I"; 
        } 
 
        /**//// <summary> 
        /// 初始化 Clib.Security 類別的新執行個體 
        /// </summary> 
        /// <param name="newKey">加密金鑰</param> 
        /// <param name="newIV">初始化向量</param> 
        public Security(string newKey,string newIV) 
        {
            this._Key = string.Format("{0,-8}", newKey);
            this._IV = string.Format("{0,-8}", newIV);
        } 
 
        /// <!--加密字串--> 
        /**//// <summary> 
        /// 加密字串 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="value">加密的字串</param> 
        /// <returns>加密過後的字串</returns> 
        public string Encrypt(string value) 
        { 
            return Encrypt(value, _Key,_IV); 
        } 
 
        /// <!--解密字串--> 
        /**//// <summary> 
        /// 解密字串 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="value">解密的字串</param> 
        /// <returns>解密過後的字串</returns> 
        public string Decrypt(string value) 
        { 
            return Decrypt(value, _Key,_IV); 
        }
 
        /// <!--DEC 加密法 --> 
        /**//// <summary> 
        /// DEC 加密法 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="pToEncrypt">加密的字串</param> 
        /// <param name="sKey">加密金鑰</param> 
        /// <param name="sIV">初始化向量</param> 
        /// <returns></returns> 
        private string Encrypt(string pToEncrypt, string sKey,string sIV) 
        { 
            StringBuilder ret = new StringBuilder(); 
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) 
            { 
                //將字元轉換為Byte 
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt); 
                //設定加密金鑰(轉為Byte) 
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); 
                //設定初始化向量(轉為Byte) 
                des.IV = ASCIIEncoding.ASCII.GetBytes(sIV); 
 
                using (MemoryStream ms = new MemoryStream()) 
                { 
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write)) 
                    { 
                        cs.Write(inputByteArray, 0, inputByteArray.Length); 
                        cs.FlushFinalBlock(); 
                    } 
                    //輸出資料 
                    foreach (byte b in ms.ToArray()) 
                        ret.AppendFormat("{0:X2}", b); 
                } 
            } 
            //回傳 
            return ret.ToString(); 
        }

        


        /// <!--DEC 解密法--> 
        /**//// <summary> 
        /// DEC 解密法 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="pToDecrypt">解密的字串</param> 
        /// <param name="sKey">加密金鑰</param> 
        /// <param name="sIV">初始化向量</param> 
        /// <returns></returns> 
        private string Decrypt(string pToDecrypt, string sKey, string sIV) 
        { 
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider()) 
            { 
 
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2]; 
                //反轉 
                for (int x = 0; x < pToDecrypt.Length / 2; x++) 
                { 
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16)); 
                    inputByteArray[x] = (byte)i; 
                } 
                //設定加密金鑰(轉為Byte) 
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey); 
                //設定初始化向量(轉為Byte) 
                des.IV = ASCIIEncoding.ASCII.GetBytes(sIV); 
                using (MemoryStream ms = new MemoryStream()) 
                { 
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write)) 
                    { 
                        //例外處理 
                        try 
                        { 
                            cs.Write(inputByteArray, 0, inputByteArray.Length); 
                            cs.FlushFinalBlock(); 
                            //輸出資料 
                            return System.Text.Encoding.Default.GetString(ms.ToArray()); 
                        } 
                        catch (CryptographicException) 
                        { 
                            //若金鑰或向量錯誤，傳回N/A 
                            return "N/A"; 
                        } 
                    } 
                } 
            } 
        }

         

        /// <!--驗證加密字串--> 
        /**//// <summary> 
        /// 驗證加密字串 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="EnString">加密後的字串</param> 
        /// <param name="FoString">加密前的字串</param> 
        /// <returns>是/否</returns> 
        public bool ValidateString(string EnString, string FoString) 
        { 
            //呼叫Decrypt解密 
            //判斷是否相符 
            //回傳結果 
            return Decrypt(EnString, _Key,_IV) == FoString.ToString() ? true : false; 
        } 

    }

    public class Security_DES_UTF8
    {
        private string _Key;
        private string _IV;

        /**/
        /// <summary> 
        /// 加密金鑰(8個英文字) 
        /// </summary> 
        public string Key
        {
            set
            {
                //_Key = value.Length == 8 ? value : "PhoenixK";
                _Key = value.Length == 8 ? value : "1234567K";

            }
        }
        /**/
        /// <summary> 
        /// 初始化向量(8個英文字) 
        /// </summary> 
        public string IV
        {
            set
            {
                //_IV = value.Length == 8 ? value : "PhoenixI"; 
                _IV = value.Length == 8 ? value : "7654321I";
            }
        }

        /**/
        /// <summary> 
        /// 初始化 Clib.Security 類別的新執行個體 
        /// </summary> 
        public Security_DES_UTF8()
        {
            _Key = "1234567K";
            _IV = "7654321I";
        }

        /**/
        /// <summary> 
        /// 初始化 Clib.Security 類別的新執行個體 
        /// </summary> 
        /// <param name="newKey">加密金鑰</param> 
        /// <param name="newIV">初始化向量</param> 
        public Security_DES_UTF8(string newKey, string newIV)
        {
            this._Key = string.Format("{0,-8}", newKey);
            this._IV = string.Format("{0,-8}", newIV);
        }

        /// <!--加密字串--> 
        /**/
        /// <summary> 
        /// 加密字串 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="value">加密的字串</param> 
        /// <returns>加密過後的字串</returns> 
        public string Encrypt(string value)
        {
            return Encrypt(value, _Key, _IV);
        }

        /// <!--解密字串--> 
        /**/
        /// <summary> 
        /// 解密字串 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="value">解密的字串</param> 
        /// <returns>解密過後的字串</returns> 
        public string Decrypt(string value)
        {
            return Decrypt(value, _Key, _IV);
        }

        /// <!--DEC 加密法 --> 
        /**/
        /// <summary> 
        /// DEC 加密法 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="pToEncrypt">加密的字串</param> 
        /// <param name="sKey">加密金鑰</param> 
        /// <param name="sIV">初始化向量</param> 
        /// <returns></returns> 
        private string Encrypt(string pToEncrypt, string sKey, string sIV)
        {
            StringBuilder ret = new StringBuilder();
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                //將字元轉換為Byte 
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                //設定加密金鑰(轉為Byte) 
                des.Key = UTF8Encoding.UTF8.GetBytes(sKey);
                //設定初始化向量(轉為Byte) 
                des.IV = UTF8Encoding.UTF8.GetBytes(sIV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                    }
                    //輸出資料 
                    foreach (byte b in ms.ToArray())
                        ret.AppendFormat("{0:X2}", b);
                }
            }
            //回傳 
            return ret.ToString();
        }




        /// <!--DEC 解密法--> 
        /**/
        /// <summary> 
        /// DEC 解密法 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="pToDecrypt">解密的字串</param> 
        /// <param name="sKey">加密金鑰</param> 
        /// <param name="sIV">初始化向量</param> 
        /// <returns></returns> 
        private string Decrypt(string pToDecrypt, string sKey, string sIV)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {

                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                //反轉 
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                //設定加密金鑰(轉為Byte) 
                des.Key = UTF8Encoding.UTF8.GetBytes(sKey);
                //設定初始化向量(轉為Byte) 
                des.IV = UTF8Encoding.UTF8.GetBytes(sIV);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        //例外處理 
                        try
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();
                            //輸出資料 
                            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
                        }
                        catch (CryptographicException)
                        {
                            //若金鑰或向量錯誤，傳回N/A 
                            return "N/A";
                        }
                    }
                }
            }
        }



        /// <!--驗證加密字串--> 
        /**/
        /// <summary> 
        /// 驗證加密字串 - design By Phoenix 2008 - 
        /// </summary> 
        /// <param name="EnString">加密後的字串</param> 
        /// <param name="FoString">加密前的字串</param> 
        /// <returns>是/否</returns> 
        public bool ValidateString(string EnString, string FoString)
        {
            //呼叫Decrypt解密 
            //判斷是否相符 
            //回傳結果 
            return Decrypt(EnString, _Key, _IV) == FoString.ToString() ? true : false;
        }

    }

    public class Security_AES
    {
        private static string superKey = string.Format("{0,-32}", "123456key");
        private static string vectoryString = string.Format("{0,-16}", "123456iv");
        private static RijndaelManaged rijndael = new RijndaelManaged();
        private static byte[] key;
        private static byte[] iv;

        private static void InitialKeyAndIV(string k, string v)
        {
            key = new byte[32];
            k = string.Format("{0,-32}", superKey);
            v = string.Format("{0,-16}", vectoryString);
            Array.Copy(Encoding.UTF8.GetBytes(k), key, 32);
            iv = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(v), key, 16);
        }

        private static void InitialKeyAndIV()
        {
            key = new byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(superKey), key, 32);
            iv = new byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(vectoryString), key, 16);
        }
        public static string EncryptInforamtion(string dataString)
        {
            UTF32Encoding utf32Encoding = new UTF32Encoding();

            if (key == null || iv == null)
            {
                InitialKeyAndIV();
            }

            Byte[] returnVal = AESEncrypt(utf32Encoding.GetBytes(dataString), rijndael.CreateEncryptor(key, iv));
            return Convert.ToBase64String(returnVal);
        }

        public static string DecryptInformation(string dataString)
        {
            UTF32Encoding utf32Encoding = new UTF32Encoding();

            if (key == null || iv == null)
            {
                InitialKeyAndIV();
            }

            Byte[] returnVal = AESDencrypt(Convert.FromBase64String(dataString), rijndael.CreateDecryptor(key, iv));

            //因為加解密會對byte[]做填充，所以解完密後要去掉。
            return utf32Encoding.GetString(returnVal).Replace("\0", "");
        }

        public static string EncryptInforamtion(string dataString, string k, string v)
        {
            UTF32Encoding utf32Encoding = new UTF32Encoding();

            if (key == null || iv == null)
            {
                InitialKeyAndIV(k, v);
            }

            Byte[] returnVal = AESEncrypt(utf32Encoding.GetBytes(dataString), rijndael.CreateEncryptor(key, iv));
            return Convert.ToBase64String(returnVal);
        }

        public static string DecryptInformation(string dataString, string k, string v)
        {
            UTF32Encoding utf32Encoding = new UTF32Encoding();

            if (key == null || iv == null)
            {
                InitialKeyAndIV(k, v);
            }

            Byte[] returnVal = AESDencrypt(Convert.FromBase64String(dataString), rijndael.CreateDecryptor(key, iv));

            //因為加解密會對byte[]做填充，所以解完密後要去掉。
            return utf32Encoding.GetString(returnVal).Replace("\0", "");
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encryptor"></param>
        /// <returns></returns>
        private static byte[] AESEncrypt(byte[] input, ICryptoTransform encryptor)
        {
            //Encrypt the data.
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

            //Write all data to the crypto stream and flush it.
            csEncrypt.Write(input, 0, input.Length);
            csEncrypt.FlushFinalBlock();

            //Get encrypted array of bytes.
            return msEncrypt.ToArray();
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="input"></param>
        /// <param name="decryptor"></param>
        /// <returns></returns>
        private static byte[] AESDencrypt(byte[] input, ICryptoTransform decryptor)
        {
            //Now decrypt the previously encrypted message using the decryptor
            // obtained in the above step.
            MemoryStream msDecrypt = new MemoryStream(input);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

            byte[] fromEncrypt = new byte[input.Length];

            //Read the data out of the crypto stream.
            csDecrypt.Read(fromEncrypt, 0, fromEncrypt.Length);


            return fromEncrypt;
        }
    }

}
