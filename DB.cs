using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;



namespace DdsLib
{
    public class DB
    {

        /// <summary>
        /// 取得連接
        /// </summary>
        /// <param name="ServerName">伺服器名稱</param>
        /// <returns>SqlConnection</returns>
        public static SqlConnection getConn(string ServerName)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[ServerName].ToString());
            return conn;
        }

        public static OdbcConnection getOdbcConn(string ServerName)
        {
            OdbcConnection conn = new OdbcConnection(ConfigurationManager.ConnectionStrings[ServerName].ToString());
            return conn;
        }

        
        /// <summary>
        /// 取得欄位字串
        /// </summary>
        /// <param name="obj">欄位物件</param>
        /// <returns>欄位</returns>
        public static string getDbFieldValue(Object obj)
        {
            if (obj != null)
            {
                return obj.ToString().Trim();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 取得欄位字串
        /// </summary>
        /// <param name="obj">欄位物件</param>
        /// <returns>欄位</returns>
        public static string getDbFieldValue(Object obj,string defval)
        {
            if (obj != null)
            {
                return obj.ToString().Trim();
            }
            else
            {
                return defval;
            }
        }
        /// <summary>
        /// 開啟資料庫
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static DdsResult openDB(ref System.Data.SqlClient.SqlConnection conn)
        {
            DdsResult ddsResult = new DdsResult();
            try
            {                
                conn.Open();
                ddsResult.Flg = true;
                ddsResult.Msg = DdsLib.ScsMsg.OpenDB;
            }
            catch(Exception ex)
            {
                ddsResult.Flg = false;
                ddsResult.Msg = DdsLib.ErrMsg.OpenDB + "：" + ex.ToString();
            }
            return ddsResult;
        }

        public static DdsResult openDB(ref OdbcConnection conn)
        {
            DdsResult ddsResult = new DdsResult();
            try
            {
                conn.Open();
                ddsResult.Flg = true;
                ddsResult.Msg = DdsLib.ScsMsg.OpenDB;
            }
            catch (Exception ex)
            {
                ddsResult.Flg = false;
                ddsResult.Msg = DdsLib.ErrMsg.OpenDB + "：" + ex.ToString();
            }
            return ddsResult;
        }
        


        /// <summary>
        /// 關閉MsSQL資料庫
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static DdsResult closeDB(ref System.Data.SqlClient.SqlConnection conn)
        {
            DdsResult ddsResult = new DdsResult();
            try
            {                
                conn.Close();
                //conn.Dispose();
                ddsResult.Flg = true;
                ddsResult.Msg = DdsLib.ScsMsg.CloseDB;
            }
            catch
            {
                ddsResult.Flg = false;
                ddsResult.Msg = DdsLib.ErrMsg.CloseDB;
            }
            return ddsResult;
        }
        public static DdsResult closeDB(ref OdbcConnection conn)
        {
            DdsResult ddsResult = new DdsResult();
            try
            {
                conn.Close();
                //conn.Dispose();
                ddsResult.Flg = true;
                ddsResult.Msg = DdsLib.ScsMsg.CloseDB;
            }
            catch
            {
                ddsResult.Flg = false;
                ddsResult.Msg = DdsLib.ErrMsg.CloseDB;
            }
            return ddsResult;
        }


        /// <summary>
        /// 開啟資料庫
        /// </summary>
        /// <param name="conn"></param>
        /// <returns></returns>
        public static DdsResult disposeDB(ref System.Data.SqlClient.SqlConnection conn)
        {
            DdsResult ddsResult = new DdsResult();
            try
            {
                conn.Dispose();
                ddsResult.Flg = true;
                ddsResult.Msg = "釋放Conn物件成功";
            }
            catch (Exception ex)
            {
                ddsResult.Flg = false;
                ddsResult.Msg = "釋放Conn物件失敗" + "：" + ex.ToString();
            }
            return ddsResult;
        }
        
        public static DdsResult DoInsert(string TableName, List<ParamObj> ParamObjList,ref System.Data.SqlClient.SqlConnection conn)
        {
            DdsResult Dr = new DdsResult();
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {                
                
                
                //cmd.Transaction = conn.BeginTransaction();
                //=== TestInsert 資料表存取異動 開始===
                string str = "";
                string sqlStr = "";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Length = 0;
                sb.Append("INSERT INTO [");
                sb.Append(Common.getPureLetter(TableName));
                sb.Append("] ");
                sb.Append(Common.GetComboParamObjStr(ParamObjList, ""));

                //sqlStr += str;
                sb.Append("VALUES ");

                sb.Append(Common.GetComboParamObjStr(ParamObjList, "@"));

                sb.Append(str);
                sqlStr = sb.ToString();
                cmd = new System.Data.SqlClient.SqlCommand(sqlStr,conn);                
                cmd.Parameters.Clear();                
                for (int i = 0; i < ParamObjList.Count; i++)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));                    
                }

                Dr.Count = cmd.ExecuteNonQuery();                
                Dr.Flg = true;
                Dr.SetStatus(Instruction.Insert);
                //Dr.Msg = sqlStr;
            }
            catch (System.Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg += ErrMsg.ExceptionError +":"+ ex.ToString();
                //cmd.Transaction.Rollback();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===
                
            }
            finally
            {
                //=== 釋放資料庫存物資源(釋放記憶體，切段連線等...) ===
                cmd.Dispose();
            }
            return Dr;
        }



        public static DdsResult DoInsert(string TableName, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn, ref SqlTransaction trans)
        {
            DdsResult Dr = new DdsResult();
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {


                //cmd.Transaction = conn.BeginTransaction();
                //=== TestInsert 資料表存取異動 開始===
                string str = "";
                string sqlStr = "";

                sqlStr = "INSERT INTO [" + Common.getPureLetter(TableName) + "] ";
                str = Common.GetComboParamObjStr(ParamObjList, "");

                sqlStr += str;
                sqlStr += "VALUES ";

                str = Common.GetComboParamObjStr(ParamObjList, "@");

                sqlStr += str;

                cmd = new System.Data.SqlClient.SqlCommand(sqlStr, conn, trans);
                cmd.Parameters.Clear();
                for (int i = 0; i < ParamObjList.Count; i++)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));
                }

                Dr.Count = cmd.ExecuteNonQuery();

                Dr.Flg = true;
                Dr.SetStatus(Instruction.Insert);
                //Dr.Msg = sqlStr;
            }
            catch (System.Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
                //cmd.Transaction.Rollback();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===

            }
            finally
            {
                //=== 釋放資料庫存物資源(釋放記憶體，切段連線等...) ===
                cmd.Dispose();
            }
            return Dr;
        }
        public static DdsResult DoMultiInsert(string TableName, List<ParamListObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn)
        {
            DdsResult Dr = new DdsResult();
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {


                //cmd.Transaction = conn.BeginTransaction();
                //=== TestInsert 資料表存取異動 開始===
                string str = "";
                string sqlStr = "";
                for (int j = 0; j < ParamObjList.Count; j++)
                {
                    sqlStr += "INSERT INTO [" + Common.getPureLetter(TableName) + "] ";
                    str = Common.GetComboParamObjStr(ParamObjList[j].poLst, "");

                    sqlStr += str;
                    sqlStr += "VALUES ";

                    str = Common.GetComboParamObjStr(ParamObjList[j].poLst, "@PM" + j.ToString());

                    sqlStr += str;
                }
                cmd = new System.Data.SqlClient.SqlCommand(sqlStr, conn);
                cmd.Parameters.Clear();
                for (int j = 0; j < ParamObjList.Count; j++)
                {
                    for (int i = 0; i < ParamObjList[j].poLst.Count; i++)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PM" + j.ToString() + Common.getPureLetter(ParamObjList[j].poLst[i].Name), ParamObjList[j].poLst[i].Value));
                    }
                }
                Dr.Count = cmd.ExecuteNonQuery();

                Dr.Flg = true;
                Dr.SetStatus(Instruction.Insert);
                //Dr.Msg = sqlStr;
            }
            catch (System.Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
                //cmd.Transaction.Rollback();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===

            }
            finally
            {
                //=== 釋放資料庫存物資源(釋放記憶體，切段連線等...) ===
                cmd.Dispose();
            }
            return Dr;
        }
        public static DdsResult DoMultiInsert(string TableName, List<ParamListObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn, ref SqlTransaction trans)
        {
            DdsResult Dr = new DdsResult();
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {


                //cmd.Transaction = conn.BeginTransaction();
                //=== TestInsert 資料表存取異動 開始===
                string str = "";
                string sqlStr = "";
                for (int j = 0; j < ParamObjList.Count; j++)
                {
                    sqlStr += "INSERT INTO [" + Common.getPureLetter(TableName) + "] ";
                    str = Common.GetComboParamObjStr(ParamObjList[j].poLst, "");

                    sqlStr += str;
                    sqlStr += "VALUES ";

                    str = Common.GetComboParamObjStr(ParamObjList[j].poLst, "@PM" + j.ToString());

                    sqlStr += str;
                }
                cmd = new System.Data.SqlClient.SqlCommand(sqlStr, conn, trans);
                cmd.Parameters.Clear();
                for (int j = 0; j < ParamObjList.Count; j++)
                {
                    for (int i = 0; i < ParamObjList[j].poLst.Count; i++)
                    {
                        cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PM" + j.ToString() + Common.getPureLetter(ParamObjList[j].poLst[i].Name), ParamObjList[j].poLst[i].Value));
                    }
                }
                Dr.Count = cmd.ExecuteNonQuery();

                Dr.Flg = true;
                Dr.SetStatus(Instruction.Insert);
                //Dr.Msg = sqlStr;
            }
            catch (System.Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
                //cmd.Transaction.Rollback();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===

            }
            finally
            {
                //=== 釋放資料庫存物資源(釋放記憶體，切段連線等...) ===
                cmd.Dispose();
            }
            return Dr;
        }

        public static DdsResult DoInsert(string TableName, List<ParamObj> ParamObjList, ref OdbcConnection conn)
        {
            DdsResult Dr = new DdsResult();
            OdbcCommand cmd = null;
            try
            {


                //cmd.Transaction = conn.BeginTransaction();
                //=== TestInsert 資料表存取異動 開始===
                string str = "";
                string sqlStr = "";

                sqlStr = "INSERT INTO " +Common.getPureLetter( TableName) + " ";
                str = Common.GetComboParamObjStr(ParamObjList, "");

                sqlStr += str;
                sqlStr += "VALUES ";

                str = Common.GetComboParamObjStr(ParamObjList, "@");

                sqlStr += str;

                cmd = new OdbcCommand(sqlStr, conn);
                cmd.Parameters.Clear();
                for (int i = 0; i < ParamObjList.Count; i++)
                {
                    cmd.Parameters.Add(new OdbcParameter("@" + Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));
                }

                Dr.Count = cmd.ExecuteNonQuery();
                Dr.Flg = true;
                Dr.SetStatus(Instruction.Insert);
                //Dr.Msg = sqlStr;
            }
            catch (System.Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
                //cmd.Transaction.Rollback();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===

            }
            finally
            {
                //=== 釋放資料庫存物資源(釋放記憶體，切段連線等...) ===
                cmd.Dispose();
            }
            return Dr;
        }

        public static int DoInsertGetLastId(string TableName, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn)
        {
            int lastIdetify = -1;            
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {


                //cmd.Transaction = conn.BeginTransaction();
                //=== TestInsert 資料表存取異動 開始===
                string str = "";
                string sqlStr = "";

                sqlStr = "INSERT INTO [" + Common.getPureLetter(TableName) + "] ";
                str = Common.GetComboParamObjStr(ParamObjList, "");

                sqlStr += str;
                sqlStr += "VALUES ";

                str = Common.GetComboParamObjStr(ParamObjList, "@");

                sqlStr += str;
                sqlStr += ";";

                sqlStr += "SELECT @Identity = SCOPE_IDENTITY();";

                cmd = new System.Data.SqlClient.SqlCommand(sqlStr, conn);
                cmd.Parameters.Clear();
                for (int i = 0; i < ParamObjList.Count; i++)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));
                }
                SqlParameter idParm = cmd.Parameters.Add("@Identity", SqlDbType.Int);
                idParm.Direction = ParameterDirection.Output;
                int affectedRows = cmd.ExecuteNonQuery();
                lastIdetify = (int)idParm.Value;
                //Dr.Msg = sqlStr;
            }
            catch
            {
                return lastIdetify;
                //return ex.ToString();
                //cmd.Transaction.Rollback();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===

            }
            return lastIdetify;
            //return lastIdetify.ToString();
        }
        public static int DoInsertGetLastId(string TableName, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn, ref SqlTransaction trans)
        {
            int lastIdetify = -1;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {


                //cmd.Transaction = conn.BeginTransaction();
                //=== TestInsert 資料表存取異動 開始===
                string str = "";
                string sqlStr = "";

                sqlStr = "INSERT INTO [" + Common.getPureLetter(TableName) + "] ";
                str = Common.GetComboParamObjStr(ParamObjList, "");

                sqlStr += str;
                sqlStr += "VALUES ";

                str = Common.GetComboParamObjStr(ParamObjList, "@");

                sqlStr += str;
                sqlStr += ";";

                sqlStr += "SELECT @Identity = SCOPE_IDENTITY();";

                cmd = new System.Data.SqlClient.SqlCommand(sqlStr, conn, trans);
                cmd.Parameters.Clear();
                for (int i = 0; i < ParamObjList.Count; i++)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" +Common.getPureLetter( ParamObjList[i].Name), ParamObjList[i].Value));
                }
                SqlParameter idParm = cmd.Parameters.Add("@Identity", SqlDbType.Int);
                idParm.Direction = ParameterDirection.Output;
                int affectedRows = cmd.ExecuteNonQuery();
                lastIdetify = (int)idParm.Value;
                //Dr.Msg = sqlStr;
            }
            catch
            {
                return lastIdetify;
                //return ex.ToString();
                //cmd.Transaction.Rollback();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===

            }
            return lastIdetify;
            //return lastIdetify.ToString();
        }
        public static long DoInsertGetLongLastId(string TableName, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn, ref SqlTransaction trans)
        {
            long lastIdetify = -1;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {


                //cmd.Transaction = conn.BeginTransaction();
                //=== TestInsert 資料表存取異動 開始===
                string str = "";
                string sqlStr = "";

                sqlStr = "INSERT INTO [" + Common.getPureLetter(TableName) + "] ";
                str = Common.GetComboParamObjStr(ParamObjList, "");

                sqlStr += str;
                sqlStr += "VALUES ";

                str = Common.GetComboParamObjStr(ParamObjList, "@");

                sqlStr += str;
                sqlStr += ";";

                sqlStr += "SELECT @Identity = SCOPE_IDENTITY();";

                cmd = new System.Data.SqlClient.SqlCommand(sqlStr, conn, trans);
                cmd.Parameters.Clear();
                for (int i = 0; i < ParamObjList.Count; i++)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));
                }
                SqlParameter idParm = cmd.Parameters.Add("@Identity", SqlDbType.Int);
                idParm.Direction = ParameterDirection.Output;
                int affectedRows = cmd.ExecuteNonQuery();
                lastIdetify = Convert.ToInt64(idParm.Value);
                //Dr.Msg = sqlStr;
            }
            catch //(Exception ex)
            {

                //HttpContext.Current.Response.Write(ex.ToString());
                return lastIdetify;
                //return ex.ToString();
                //cmd.Transaction.Rollback();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===

            }
            return lastIdetify;
            //return lastIdetify.ToString();
        }

        public static long DoInsertGetLongLastId(string TableName, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn)
        {
            long lastIdetify = -1;
            System.Data.SqlClient.SqlCommand cmd = null;
            try
            {


                //cmd.Transaction = conn.BeginTransaction();
                //=== TestInsert 資料表存取異動 開始===
                string str = "";
                string sqlStr = "";

                sqlStr = "INSERT INTO [" + Common.getPureLetter(TableName) + "] ";
                str = Common.GetComboParamObjStr(ParamObjList, "");

                sqlStr += str;
                sqlStr += "VALUES ";

                str = Common.GetComboParamObjStr(ParamObjList, "@");

                sqlStr += str;
                sqlStr += ";";

                sqlStr += "SELECT @Identity = SCOPE_IDENTITY();";

                cmd = new System.Data.SqlClient.SqlCommand(sqlStr, conn);
                cmd.Parameters.Clear();
                for (int i = 0; i < ParamObjList.Count; i++)
                {
                    cmd.Parameters.Add(new System.Data.SqlClient.SqlParameter("@" + Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));
                }
                SqlParameter idParm = cmd.Parameters.Add("@Identity", SqlDbType.BigInt);
                idParm.Direction = ParameterDirection.Output;
                long affectedRows = cmd.ExecuteNonQuery();
                lastIdetify = Convert.ToInt64(idParm.Value);
                //Dr.Msg = sqlStr;
            }
            catch //(Exception ex)
            {

                //HttpContext.Current.Response.Write(ex.ToString());
                return lastIdetify;
                //return ex.ToString();
                //cmd.Transaction.Rollback();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===

            }
            return lastIdetify;
            //return lastIdetify.ToString();
        }

        /// <summary>
        /// 執行條件式查詢
        /// </summary>        
        /// <returns>DdsResult[GDL物件]</returns>        
        public static DdsResult DoCondQuery(string TableName, List<ParamObj> ParamObjList, ref  System.Data.SqlClient.SqlConnection conn)
        {
            string SqlStr = "Select * from " + Common.getPureLetter(TableName);

            if (ParamObjList.Count > 0)
            {
                SqlStr += " Where " + Common.GetCondParamObjStr(ParamObjList, "AND");
            }

            DdsResult Gr = DoQuery(SqlStr, ParamObjList,ref conn);
            /*
                        if (ParamObjList != null)
                        {
                            //Gr.Msg = Par.Count.ToString();
                            for (int i = 0; i < ParamObjList.Count; i++)
                            {
                                Par.Add(DbUtility.SetSqlParameter(ParamObjList[i].Name, ParamObjList[i].Value));
                            }
                        }

                        string SelectCommand = string.Empty;
                        SelectCommand += SqlStr;

                        try
                        {
                            Gr.Dts = DbUtility.MsSqlQuery(SelectCommand, Par.ToArray(), ServerName);
                            Gr.SetStatus(DbInstruction.Query);
                        }
                        catch (System.Exception ex)
                        {
                            Gr.Msg += GdlErrMsg.ExceptionError + ex.ToString();
                        }
            */
            return Gr;
        }

        /// <summary>
        /// 執行條件式查詢
        /// </summary>        
        /// <returns>DdsResult[GDL物件]</returns>        
        public static DdsResult DoCondQuery(string TableName, List<ParamObj> ParamObjList, ref  System.Data.SqlClient.SqlConnection conn, ref SqlTransaction trans)
        {
            string SqlStr = "Select * from " + Common.getPureLetter(TableName);

            if (ParamObjList.Count > 0)
            {
                SqlStr += " Where " + Common.GetCondParamObjStr(ParamObjList, "AND");
            }

            DdsResult Gr = DoQuery(SqlStr, ParamObjList, ref conn, ref trans);
            /*
                        if (ParamObjList != null)
                        {
                            //Gr.Msg = Par.Count.ToString();
                            for (int i = 0; i < ParamObjList.Count; i++)
                            {
                                Par.Add(DbUtility.SetSqlParameter(ParamObjList[i].Name, ParamObjList[i].Value));
                            }
                        }

                        string SelectCommand = string.Empty;
                        SelectCommand += SqlStr;

                        try
                        {
                            Gr.Dts = DbUtility.MsSqlQuery(SelectCommand, Par.ToArray(), ServerName);
                            Gr.SetStatus(DbInstruction.Query);
                        }
                        catch (System.Exception ex)
                        {
                            Gr.Msg += GdlErrMsg.ExceptionError + ex.ToString();
                        }
            */
            return Gr;
        }

        public static DdsResult DoInsertDts(string TableName, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn)
        {
            TableName = Common.getPureLetter(TableName);
            DdsResult Gr = new DdsResult();
            try
            {
                //=== TestInsert 資料表存取異動 開始===
                Gr = DoCondQuery(TableName, ParamObjList, ref conn);
                if (Gr.Count <= 0)
                {
                    Gr = DB.DoInsert(TableName, ParamObjList,ref conn);
                    // 取得更新id
                    if (Gr.Count > 0)
                    {
                        Gr = DB.DoCondQuery(TableName, ParamObjList, ref conn);
                    }
                }

            }
            catch (System.Exception ex)
            {
                Gr.Flg = false;
                Gr.Msg += ErrMsg.ExceptionError + ex.ToString();
                //=== 發生任何資料存取例外即刻還原任何資料異動 ===
            }

            return Gr;            //return lastIdetify.ToString();
        }


        public static DdsResult DoQuery(string SqlStr, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn)
        {
            List<SqlParameter> Par = new List<SqlParameter>();
            DdsResult Dr = new DdsResult();
            SqlDataAdapter da = null;
            DataSet Result = new DataSet();

            try
            {                
                da = new SqlDataAdapter(SqlStr,conn);                
                //da.SelectCommand.CommandType = CommandType.Text;
               
                if (ParamObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value);
                    }
                }

                da.Fill(Result);
                Dr.Dts = Result;
                Dr.SetStatus(Instruction.Query);
            }
            catch (System.Exception ex)
            {
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }

            return Dr;
        }

        public static DdsResult DoQuery(string SqlStr, List<ParamObj> ParamObjList, ref OdbcConnection conn)
        {
            List<SqlParameter> Par = new List<SqlParameter>();
            DdsResult Dr = new DdsResult();
            OdbcDataAdapter da = null;
            DataSet Result = new DataSet();

            try
            {
                da = new OdbcDataAdapter(SqlStr, conn);
                //da.SelectCommand.CommandType = CommandType.Text;

                if (ParamObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value);
                    }
                }

                da.Fill(Result);
                Dr.Dts = Result;
                Dr.SetStatus(Instruction.Query);
            }
            catch (System.Exception ex)
            {
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }

            return Dr;
        }

        public static DdsResult DoPageQuery(string SqlStr, int pagesize , int pageindex , string OrdBySql , List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn)
        {
            int pagestr = 0;
            int pageend = 0;
            pagestr = (pageindex - 1) * pagesize+1;
            pageend = pageindex * pagesize;

            string sql = string.Format(@"
select * from 
(
SELECT  rank() over ({1}) as rn , *  from
	(
{0}
	) as t1

) as t2 where rn between    @pagestr and  @pageend;

select count(*) as pagetotal from ( {0} ) as tmpcnt;
            ", SqlStr, OrdBySql);

            List<SqlParameter> Par = new List<SqlParameter>();
            DdsResult Dr = new DdsResult();
            SqlDataAdapter da = null;
            DataSet Result = new DataSet();
            //System.Web.HttpContext.Current.Response.Write(sql);
            try
            {
                ParamObjList.Add(new ParamObj("pagestr", pagestr));
                ParamObjList.Add(new ParamObj("pageend", pageend));
                da = new SqlDataAdapter(sql, conn);
                //da.SelectCommand.CommandType = CommandType.Text;

                if (ParamObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value);
                    }
                }

                da.Fill(Result);
                Dr.Dts = Result;
                Dr.SetStatus(Instruction.Query);
            }
            catch (System.Exception ex)
            {
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }

            return Dr;
        }


        public static DdsResult DoQuery(string SqlStr, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn,ref SqlTransaction trans)
        {
            List<SqlParameter> Par = new List<SqlParameter>();
            DdsResult Dr = new DdsResult();
            SqlDataAdapter da = null;
            DataSet Result = new DataSet();

            try
            {
                da = new SqlDataAdapter(SqlStr, conn);
                //da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.Transaction = trans;
                if (ParamObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();                    
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value);
                    }
                }

                da.Fill(Result);
                Dr.Dts = Result;
                Dr.SetStatus(Instruction.Query);
            }
            catch (System.Exception ex)
            {
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }

            return Dr;
        }   
        
        

        public static DdsResult DoStoredProc(string SqlStr, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn,int timeoutseconds = 0)
        {
            List<SqlParameter> Par = new List<SqlParameter>();
            DdsResult Dr = new DdsResult();
            SqlDataAdapter da = null;
            DataSet Result = new DataSet();

            try
            {
                da = new SqlDataAdapter(SqlStr, conn);
                //da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (timeoutseconds > 0)
                {
                    da.SelectCommand.CommandTimeout = timeoutseconds;
                }
                if (ParamObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value);
                    }
                }

                da.Fill(Result);
                Dr.Dts = Result;
                Dr.SetStatus(Instruction.Query);
            }
            catch (System.Exception ex)
            {
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }

            return Dr;
        }

        public static DdsResult DoStoredProc(string SqlStr, List<ParamObj> ParamObjList, ref System.Data.SqlClient.SqlConnection conn,ref SqlTransaction trans )
        {
            List<SqlParameter> Par = new List<SqlParameter>();
            DdsResult Dr = new DdsResult();
            SqlDataAdapter da = null;
            DataSet Result = new DataSet();

            try
            {
                da = new SqlDataAdapter(SqlStr, conn);
                //da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Transaction = trans;
                if (ParamObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value);
                    }
                }

                da.Fill(Result);
                Dr.Dts = Result;
                Dr.SetStatus(Instruction.Query);
            }
            catch (System.Exception ex)
            {
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }

            return Dr;
        }

        public static DdsResult DoUpdateByTable(string TableName, List<ParamObj> ParamObjList, List<ParamObj> CondObjList, ref System.Data.SqlClient.SqlConnection conn)
        {
            List<SqlParameter> Par = new List<SqlParameter>();
            DdsResult Dr = new DdsResult();
            SqlDataAdapter da = null;
            DataSet Result = new DataSet();

            string SqlStr = "Update " + Common.getPureLetter(TableName) + " Set " + Common.GetUpdateParamObjStr(ParamObjList);

            if (CondObjList.Count > 0)
            {
                SqlStr += " Where " + Common.GetCondParamObjStr(CondObjList, "AND");
            }


            //List<SqlParameter> Par = new List<SqlParameter>();

            try
            {
                da = new SqlDataAdapter(SqlStr, conn);
                //da.SelectCommand.CommandType = CommandType.Text;

                if (ParamObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value);
                    }
                }

                if (CondObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();
                    for (int i = 0; i < CondObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(CondObjList[i].Name), CondObjList[i].Value);
                    }
                }


                da.Fill(Result);
                Dr.Dts = Result;
                Dr.SetStatus(Instruction.Query);
            }
            catch (System.Exception ex)
            {
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }

            return Dr;
        }

        public static DdsResult DoUpdateByTable(string TableName, List<ParamObj> ParamObjList, List<ParamObj> CondObjList, ref System.Data.SqlClient.SqlConnection conn,ref SqlTransaction trans)
        {
            List<SqlParameter> Par = new List<SqlParameter>();
            DdsResult Dr = new DdsResult();
            SqlDataAdapter da = null;
            DataSet Result = new DataSet();

            string SqlStr = "Update " + Common.getPureLetter(TableName) + " Set " + Common.GetUpdateParamObjStr(ParamObjList);

            if (CondObjList.Count > 0)
            {
                SqlStr += " Where " + Common.GetCondParamObjStr(CondObjList, "AND");
            }


            //List<SqlParameter> Par = new List<SqlParameter>();

            try
            {
                da = new SqlDataAdapter(SqlStr, conn);
                //da.SelectCommand.CommandType = CommandType.Text;
                da.SelectCommand.Transaction = trans;
                if (ParamObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value);
                    }
                }

                if (CondObjList != null)
                {
                    //Gr.Msg = Par.Count.ToString();
                    da.SelectCommand.Parameters.Clear();
                    for (int i = 0; i < CondObjList.Count; i++)
                    {
                        da.SelectCommand.Parameters.AddWithValue(Common.getPureLetter(CondObjList[i].Name), CondObjList[i].Value);
                    }
                }


                da.Fill(Result);
                Dr.Dts = Result;
                Dr.SetStatus(Instruction.Query);
            }
            catch (System.Exception ex)
            {
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }

            return Dr;
        } 

        public static DdsResult DoDelete(string SqlStr, List<ParamObj> ParamObjList, ref SqlConnection conn)
        {
            DdsResult Dr = new DdsResult();
            System.Data.SqlClient.SqlCommand cmd = null;


            //Gr.Msg = SqlStr;
            try
            {
                cmd = new System.Data.SqlClient.SqlCommand(SqlStr, conn);
                cmd.Parameters.Clear();
                if (ParamObjList != null)
                {
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        cmd.Parameters.Add(new SqlParameter(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));
                    }
                }
                
                //DataObj.ExecuteNonQuery();
                Dr.Count = cmd.ExecuteNonQuery();
                Dr.Flg = true;
                Dr.SetStatus(Instruction.Delete);
            }
            catch (System.Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }
            finally
            {
                //=== 釋放資料庫存物資源(釋放記憶體，切段連線等...) ===
                cmd.Dispose();
            }
            return Dr;
        }

        public static DdsResult DoUpdate(string SqlStr, List<ParamObj> ParamObjList, ref SqlConnection conn)
        {
            DdsResult Dr = new DdsResult();
            System.Data.SqlClient.SqlCommand cmd = null;


            //Gr.Msg = SqlStr;
            try
            {
                cmd = new System.Data.SqlClient.SqlCommand(SqlStr, conn);
                cmd.Parameters.Clear();
                if (ParamObjList != null)
                {
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        cmd.Parameters.Add(new SqlParameter(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));
                    }
                }

                //DataObj.ExecuteNonQuery();
                Dr.Count = cmd.ExecuteNonQuery();
                Dr.Flg = true;
                Dr.SetStatus(Instruction.Update);
            }
            catch (System.Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }
            finally
            {
                //=== 釋放資料庫存物資源(釋放記憶體，切段連線等...) ===
                cmd.Dispose();
            }
            return Dr;
        }
        public static DdsResult DoDelete(string SqlStr, List<ParamObj> ParamObjList, ref SqlConnection conn, ref SqlTransaction trans)
        {
            DdsResult Dr = new DdsResult();
            System.Data.SqlClient.SqlCommand cmd = null;


            //Gr.Msg = SqlStr;
            try
            {
                cmd = new System.Data.SqlClient.SqlCommand(SqlStr, conn, trans);
                cmd.Parameters.Clear();
                if (ParamObjList != null)
                {
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        cmd.Parameters.Add(new SqlParameter(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));
                    }
                }

                //DataObj.ExecuteNonQuery();
                Dr.Count = cmd.ExecuteNonQuery();
                Dr.Flg = true;
                Dr.SetStatus(Instruction.Delete);
            }
            catch (System.Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }
            finally
            {
                //=== 釋放資料庫存物資源(釋放記憶體，切段連線等...) ===
                cmd.Dispose();
            }
            return Dr;
        }

        public static DdsResult DoUpdate(string SqlStr, List<ParamObj> ParamObjList, ref SqlConnection conn,ref SqlTransaction trans)
        {
            DdsResult Dr = new DdsResult();
            System.Data.SqlClient.SqlCommand cmd = null;


            //Gr.Msg = SqlStr;
            try
            {
                cmd = new System.Data.SqlClient.SqlCommand(SqlStr, conn, trans);
                cmd.Parameters.Clear();
                if (ParamObjList != null)
                {
                    for (int i = 0; i < ParamObjList.Count; i++)
                    {
                        cmd.Parameters.Add(new SqlParameter(Common.getPureLetter(ParamObjList[i].Name), ParamObjList[i].Value));
                    }
                }

                //DataObj.ExecuteNonQuery();
                Dr.Count = cmd.ExecuteNonQuery();
                Dr.Flg = true;
                Dr.SetStatus(Instruction.Update);
            }
            catch (System.Exception ex)
            {
                Dr.Flg = false;
                Dr.Msg += ErrMsg.ExceptionError + ":" + ex.ToString();
            }
            finally
            {
                //=== 釋放資料庫存物資源(釋放記憶體，切段連線等...) ===
                cmd.Dispose();
            }
            return Dr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDatatableToXML(DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }
    }
}
