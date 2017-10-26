using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DdsLib
{

    public class ServerName
    {
        // 來源KpcConnStr
        // 目的GdlConnStr
    }
  
    public class ErrMsg
    {
        public const string Error = "錯誤";
        public const string ExceptionError = "發生例外錯誤：";
        public const string Login = "登入失敗";
        public const string UserPwdErr = "帳號密碼錯誤";
        public const string DataInsert = "資料新增失敗";
        public const string DataUpdate = "資料更新失敗";
        public const string DataDelete = "資料刪除失敗";
        public const string DataQuery = "資料查詢失敗";
        public const string EmailValid = "錯誤的Email格式";
        public const string IntValid = "錯誤整數格式";
        public const string UnIntValid = "錯誤正整數格式";
        public const string FloatValid = "錯誤浮點數格式";
        public const string DateTimeValid = "錯誤日期時間格式";
        public const string OpenDB = "開啟資料庫失敗";
        public const string CloseDB = "關閉資料庫失敗";

    }

    public class InfoMsg
    {
        public const string DataSetEmpty = "Data Set資料為NULL";
        public const string QueryEmpty = "查無資料";
        public const string EmptyInput = "沒有輸入值";
        public const string EmailExists = "此Email已存在";
        public const string EmailNotExists = "此Email不存在";

    }

    public class ScsMsg
    {
        public const string Success = "成功";
        public const string DataQuery = "資料查詢成功";
        public const string Login = "登入成功";
        public const string DataInsert = "資料新增成功";
        public const string DataUpdate = "資料更新成功";
        public const string DataDelete = "資料刪除成功";
        public const string EmailValid = "正確的Email格式";
        public const string IntFormatValid = "正確整數格式";
        public const string UnIntFormatValid = "正確整數格式";
        public const string FloatValid = "正確浮點數格式";
        public const string DateTimeValid = "正確日期時間格式";
        public const string OpenDB = "開啟資料庫成功";
        public const string CloseDB = "關閉資料庫成功";
    }

    public class Instruction
    {
        public const string Insert = "新增";
        public const string Update = "修改";
        public const string Query = "查詢";
        public const string Delete = "刪除";
    }

    public class DdsResult
    {

        #region "屬性"
        /// <summary>
        /// 處理狀況是否成功(成功|失敗/true|false)。
        /// </summary>
        public bool Flg;

        /// <summary>
        /// 處理訊息。
        /// </summary>
        public string Msg;

        /// <summary>
        /// 查詢結果DataSet。
        /// </summary>

        public DataSet Dts;

        /// <summary>
        /// 影響結果筆數。
        /// </summary>

        public int Count;
        #endregion

        #region "方法"

        public void SetStatus(string dbins)
        {
            if (dbins == Instruction.Query)
            {
                try
                {
                    if (this.Dts != null)
                    {
                        this.Count = this.Dts.Tables[0].Rows.Count;
                        if (this.Count > 0)
                        {
                            this.Flg = true;

                            this.Msg = ScsMsg.DataQuery;
                        }
                        else
                        {
                            this.Msg = InfoMsg.QueryEmpty ;
                        }

                    }
                    else
                    {
                        this.Msg = InfoMsg.DataSetEmpty;
                    }
                }
                catch (System.Exception ex)
                {
                    this.Msg = ErrMsg.ExceptionError + ex.Message;
                }
            }
            else if (dbins == Instruction.Insert)
            {
                if (Flg)
                {
                    Msg = ScsMsg.DataInsert;
                }
                else
                {
                    Msg = ErrMsg.DataInsert;
                }

            }
            else if (dbins == Instruction.Update)
            {
                if (Flg)
                {
                    Msg = ScsMsg.DataUpdate;
                }
                else
                {
                    Msg = ErrMsg.DataUpdate;
                }
            }
            else if (dbins == Instruction.Delete)
            {
                if (Flg)
                {
                    Msg = ScsMsg.DataDelete;
                }
                else
                {
                    Msg = ErrMsg.DataDelete;
                }
            }
            else
            {

            }

        }

        #endregion

        #region "建構子"
        public DdsResult()
        {
            Msg = "";
            Flg = false;
            Dts = null;
            Count = 0;
        }

        #endregion

        #region "解構子"
        ~DdsResult()
        {
            GC.Collect();
        }
        #endregion

    }

    #region [Param 相關物件]
    
    /// <summary>
    /// 參數物件List物件
    /// </summary>
    public class ParamListObj
    {
        /// <summary>
        /// 參數物件List
        /// </summary>        
        public List<ParamObj> poLst = new List<ParamObj>();

        /// <summary>
        /// 取得List數量
        /// </summary>
        /// <returns>int</returns>
        public int GetCount()
        {
            return poLst.Count;
        }


        /// <summary>
        /// 驗證結果Flag
        /// </summary>
        public bool Flg = false;


        /// <summary>
        /// 處理訊息
        /// </summary>
        public string Msg = "";

        #region "建構子"
        public ParamListObj()
        {
            Msg = "";
            Flg = false;
        }
        #endregion

        #region "解構子"
        ~ParamListObj()
        {
            GC.Collect();
        }
        #endregion
    }
    #endregion

    public class ParamObj
    {
        /// <summary>
        /// 參數名稱。
        /// </summary>
        public string Name;


        /// <summary>
        /// 對應數值。
        /// </summary>
        public Object Value;


        /// <summary>
        /// 驗證規則代碼。
        /// </summary>
        public string RuleCode;

        /// <summary>
        /// 是否可設定範圍。
        /// </summary>
        public bool IsRange;

        /// <summary>
        /// 是否驗證。
        /// </summary>
        public bool IsVerify = true;

        /// <summary>
        /// 是否可為空。
        /// </summary>
        public bool IsAllowEmpty = false;


        /// <summary>
        /// 範圍上限。
        /// </summary>
        public int Max;

        /// <summary>
        /// 範圍下限。
        /// </summary>
        public int Min;

        /// <summary>
        /// 符合規則。
        /// </summary>
        public bool Flg;

        /// <summary>
        /// SQL資料型別
        /// </summary>
        public SqlDbType SdType;

        /// <summary>
        /// 相關處理資訊。
        /// </summary>
        public string Msg;


        #region "建構子"
        void init()
        {
            Name = "";
            Value = "";
            IsRange = false; // 預設不定義範圍
            Min = -1; // 預設不處理
            Max = -1; // 預設不處理
            Flg = false;
            SdType = SqlDbType.Char;
            Msg = "";

        }
        public ParamObj()
        {
            init();
        }

        #endregion

        #region "建構子(name,value)"
        /// <summary>
        /// 建構子(Name,Value)
        /// </summary>
        /// <param name="Name">參數名稱</param>  
        /// <param name="Value">參數對應值</param>
        public ParamObj(string name, Object value)
        {
            init();
            Name = name;
            Value = value;
        }

        #endregion

        #region "解構子"
        ~ParamObj()
        {
            GC.Collect();
        }
        #endregion


    }

    public class DdsDataTable : DataTable
    {
        string defval = "";
        string classname = "";
        ParamListObj PloX = null;
        ParamListObj PloY = null;
        string TopLeftStr = "";

        public DdsDataTable()
        {
        }

        public DdsDataTable(ParamListObj PloX, ParamListObj PloY , string TopLeftStr ,string classname , string defval)        
        {
            this.defval = defval;
            this.classname = classname;
            this.PloX = PloX;
            this.PloY = PloY;
            this.TopLeftStr = TopLeftStr;
            this.Columns.Add(TopLeftStr, typeof(string));
            for (int i = 0; i < PloX.poLst.Count ; i++)
            {
                this.Columns.Add(PloX.poLst[i].Value.ToString().Trim(), typeof(string));
            }

            for (int i = 0; i < PloY.poLst.Count; i++)
            {
                this.Rows.Add(PloY.poLst[i].Value.ToString().Trim());
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='" + Common.getPureLetter(classname) + "'>");
            sb.Append("<thead><tr>");
            //sb.Append(string.Format("<th>{0}</th>", TopLeftStr));
            for (int i = 0; i <= PloX.poLst.Count; i++)
            {
                sb.Append(string.Format("<th>{0}</th>", string.IsNullOrEmpty(this.Columns[i].ColumnName) ? defval : this.Columns[i].ColumnName));
            }

            sb.Append("</tr></thead>");


            foreach (DataRow drw in this.Rows)
            {
                sb.Append("<tr>");
                for (int j = 0; j <= PloX.poLst.Count; j++)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(drw[j].ToString().Trim()))
                        {
                            sb.Append(string.Format("<td>{0}</td>", defval));
                        }
                        else
                        {
                            sb.Append(string.Format("<td>{0}</td>", drw[j]));
                        }
                    }
                    catch
                    {
                        sb.Append(string.Format("<td>{0}</td>", defval));
                    }
                }
                sb.Append("</tr>");

            }

            sb.Append("</table>");
            return sb.ToString();
        }
        public void Append(int colindex, int rowindex, string val)
        {
            try
            {
                this.Rows[rowindex-1][colindex] += val;
            }
            catch { }
        }
        public void Assign(int colindex, int rowindex, string val)
        {
            try
            {
                this.Rows[rowindex-1][colindex] = val;
            }
            catch { }
        }

    }

    

}
