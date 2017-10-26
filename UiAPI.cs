using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace DdsLib
{
    /// <summary>
    /// 常用的Html UI 輸出
    /// </summary>
    public class UiAPI
    {
        /// <summary>
        /// 取得分頁列字串
        /// </summary>
        /// <param name="PgTotal">總比數</param>
        /// <param name="PgCount">總頁數</param>
        /// <param name="PgIndex">第幾頁</param>
        /// <param name="PgParm">Query字串</param>
        /// <param name="PgUrl">網址</param>
        /// <param name="ClassName">CSS Class Name</param>
        /// <example>
        /// <code>
        /// string pgurl = settings.StfSmyUrl;
        /// string pgpar = "?s=";
        /// if (ddlFdTyp.SelectedValue != "")       
        /// {            
        ///     pgpar += Common.getPureLetter(ddlFdTyp.SelectedValue);        
        /// }       
        /// if (ddlDeptCd.SelectedValue != "")       
        /// {            
        ///     pgpar += "&deptcd=" + Common.getPureLetter(ddlDeptCd.SelectedValue);        
        /// }
        /// if (ddlTeacher.SelectedValue != "")       
        /// {            
        ///     pgpar += "&tid=" + Common.getPureLetter(ddlTeacher.SelectedValue);        
        /// }
        /// if (ddlYrSemStr.SelectedValue != "")       
        /// {            
        ///     pgpar += "&str=" + Common.getPureLetter(ddlYrSemStr.SelectedValue);        
        /// }
        /// PagerParamObj ppo = new PagerParamObj("[第一頁]", "[上頁]", "[次頁]", "[最末頁]", "目前在第 {0} 頁 / 共有 {1:0,0} 筆查詢結果");
        /// pghtml = UiAPI.getNumPager(total.ToString(), pagecount.ToString(), pageindex.ToString(), pgpar + "&pg", pgurl, "pager_style1", ppo);
        /// </code>
        /// </example>
        /// <returns>字串</returns>
        public static string getNumPager(string PgTotal, string PgCount, string PgIndex, string PgParm, string PgUrl, string ClassName)
        {
            PagerParamObj ppo = new PagerParamObj();
            return getNumPager(PgTotal, PgCount, PgIndex, PgParm, PgUrl, ClassName, ppo);            
        }

        /// <summary>
        /// 取得分頁列字串(可自定)        
        /// </summary>
        /// <param name="PgTotal"></param>
        /// <param name="PgCount"></param>
        /// <param name="PgIndex"></param>
        /// <param name="PgParm"></param>
        /// <param name="PgUrl"></param>
        /// <param name="ClassName"></param>
        /// <param name="ppo"><example><code>PagerParamObj ppo = new PagerParamObj("[第一頁]", "[上頁]", "[次頁]", "[最末頁]", "目前在第 {0} 頁 / 共有 {1:0,0} 筆查詢結果");</code></example></param>
        /// <example>
        /// <code>
        /// string pgurl = settings.StfSmyUrl;
        /// string pgpar = "?s=";
        /// if (ddlFdTyp.SelectedValue != "")       
        /// {            
        ///     pgpar += Common.getPureLetter(ddlFdTyp.SelectedValue);        
        /// }       
        /// if (ddlDeptCd.SelectedValue != "")       
        /// {            
        ///     pgpar += "&deptcd=" + Common.getPureLetter(ddlDeptCd.SelectedValue);        
        /// }
        /// if (ddlTeacher.SelectedValue != "")       
        /// {            
        ///     pgpar += "&tid=" + Common.getPureLetter(ddlTeacher.SelectedValue);        
        /// }
        /// if (ddlYrSemStr.SelectedValue != "")       
        /// {            
        ///     pgpar += "&str=" + Common.getPureLetter(ddlYrSemStr.SelectedValue);        
        /// }
        /// PagerParamObj ppo = new PagerParamObj("[第一頁]", "[上頁]", "[次頁]", "[最末頁]", "目前在第 {0} 頁 / 共有 {1:0,0} 筆查詢結果");
        /// pghtml = UiAPI.getNumPager(total.ToString(), pagecount.ToString(), pageindex.ToString(), pgpar + "&pg", pgurl, "pager_style1", ppo);
        /// </code>
        /// </example>
        /// <returns>字串</returns>
        public static string getNumPager(string PgTotal, string PgCount, string PgIndex, string PgParm, string PgUrl, string ClassName , PagerParamObj ppo)
        {
            long total = 0;
            try
            {
                total = Math.Abs(Convert.ToInt32(PgTotal));
            }
            catch
            {
            }

            long count = 0;
            try
            {
                count = Math.Abs(Convert.ToInt32(PgCount));
            }
            catch
            {
            }

            long index = 0;
            try
            {
                index = Math.Abs(Convert.ToInt32(PgIndex));
            }
            catch
            {
            }

            long lastPgIndex = 0;
            try
            {
                lastPgIndex = total / count;
                if ((total % count) != 0)
                {
                    lastPgIndex++;
                }
            }
            catch
            {
            }

            if (lastPgIndex == 0)
            {
                lastPgIndex = 1;
            }


            string str = string.Empty;
            ClassName = Common.getPureLetter(ClassName);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Length = 0;
            if (!string.IsNullOrEmpty(ClassName))
            {
                ClassName = string.Format(" class='{0}' ", ClassName);
            }

            sb.Append(string.Format("<div {0} >", ClassName));

            if (index == 1)
            {
                //str = "<a>" + ppo.first + "</a>";
                str = ppo.first;
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, 1, ppo.first);
            }

            sb.Append(string.Format("<span>{0}</span>", str));

            long prevIndex = index - 1;
            if (prevIndex < 1)
            {
                prevIndex = 1;
            }
            if (prevIndex == 1)
            {
                //str = "<a>" + ppo.prev + "</a>";
                str = ppo.prev;
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, prevIndex, ppo.prev);
            }


            sb.Append(string.Format("<span>{0}</span>", str));

            long max = 10;
            long strIdx = 1;
            if (index >= max)
            {
                long pretenidx = (index - max);
                if (pretenidx <= 0)
                {
                    pretenidx = 1;
                }
                strIdx = index;
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, pretenidx, "...");
                sb.Append(string.Format("<span>{0}</span>", str));
            }
            for (long i = strIdx; i <= lastPgIndex; i++)
            {
                if (index == i)
                {
                    str = i.ToString();
                }
                else
                {
                    if ((i - strIdx + 1) >= max)
                    {
                        str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, i, "...");
                        sb.Append(string.Format("<span>{0}</span>", str));
                        break;
                    }
                    else
                    {
                        str = string.Format("<a href='{0}?{1}={2}'>{2}</a>", PgUrl, PgParm, i);

                    }
                }
                sb.Append(string.Format("<span>{0}</span>", str));

            }


            long nextIndex = index + 1;
            if (nextIndex > lastPgIndex)
            {
                prevIndex = lastPgIndex;
            }
            if (prevIndex == lastPgIndex)
            {
                //str = "<a>" + ppo.next + "</a>";
                str = ppo.next;
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, nextIndex, ppo.next);
            }
            sb.Append(string.Format("<span>{0}</span>", str));
            if (index == lastPgIndex)
            {
                //str = "<a>" + ppo.last + "</a>";
                str = ppo.last;
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, lastPgIndex, ppo.last);
            }
            sb.Append(string.Format("<span>{0}</span>", str));
            str = string.Format(ppo.info, index, total);
            sb.Append(string.Format("<span>{0}</span>", str));

            sb.Append(string.Format("</div>"));
            return sb.ToString();
        }


        /// <summary>
        /// 取得BootStrap Pagination分頁列字串(可自定)        
        /// </summary>
        /// <param name="PgTotal"></param>
        /// <param name="PgCount"></param>
        /// <param name="PgIndex"></param>
        /// <param name="PgParm"></param>
        /// <param name="PgUrl"></param>
        /// <param name="ClassName"></param>
        /// <param name="ppo"><example><code>PagerParamObj ppo = new PagerParamObj("[第一頁]", "[上頁]", "[次頁]", "[最末頁]", "目前在第 {0} 頁 / 共有 {1:0,0} 筆查詢結果");</code></example></param>
        /// <example>
        /// <code>
        /// string pgurl = settings.StfSmyUrl;
        /// string pgpar = "?s=";
        /// if (ddlFdTyp.SelectedValue != "")       
        /// {            
        ///     pgpar += Common.getPureLetter(ddlFdTyp.SelectedValue);        
        /// }       
        /// if (ddlDeptCd.SelectedValue != "")       
        /// {            
        ///     pgpar += "&deptcd=" + Common.getPureLetter(ddlDeptCd.SelectedValue);        
        /// }
        /// if (ddlTeacher.SelectedValue != "")       
        /// {            
        ///     pgpar += "&tid=" + Common.getPureLetter(ddlTeacher.SelectedValue);        
        /// }
        /// if (ddlYrSemStr.SelectedValue != "")       
        /// {            
        ///     pgpar += "&str=" + Common.getPureLetter(ddlYrSemStr.SelectedValue);        
        /// }
        /// PagerParamObj ppo = new PagerParamObj("[第一頁]", "[上頁]", "[次頁]", "[最末頁]", "目前在第 {0} 頁 / 共有 {1:0,0} 筆查詢結果");
        /// pghtml = UiAPI.getNumPager(total.ToString(), pagecount.ToString(), pageindex.ToString(), pgpar + "&pg", pgurl, "pager_style1", ppo);
        /// </code>
        /// </example>
        /// <returns>字串</returns>
        public static string getBsNumPager(string PgTotal, string PgCount, string PgIndex, string PgParm, string PgUrl, string ClassName, PagerParamObj ppo)
        {
            long total = 0;
            try
            {
                total = Math.Abs(Convert.ToInt32(PgTotal));
            }
            catch
            {
            }

            long count = 0;
            try
            {
                count = Math.Abs(Convert.ToInt32(PgCount));
            }
            catch
            {
            }

            long index = 0;
            try
            {
                index = Math.Abs(Convert.ToInt32(PgIndex));
            }
            catch
            {
            }

            long lastPgIndex = 0;
            try
            {
                lastPgIndex = total / count;
                if ((total % count) != 0)
                {
                    lastPgIndex++;
                }
            }
            catch
            {
            }

            if (lastPgIndex == 0)
            {
                lastPgIndex = 1;
            }


            string str = string.Empty;
            ClassName = Common.getPureLetter(ClassName);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Length = 0;
            if (!string.IsNullOrEmpty(ClassName))
            {
                ClassName = string.Format(" class='{0}' ", ClassName);
            }

            sb.Append(string.Format("<nav><ul {0} >", ClassName));

            if (index == 1)
            {
                //str = "<a>" + ppo.first + "</a>";
                str = ppo.first;
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, 1, ppo.first);
            }

            sb.Append(string.Format("<li>{0}</li>", str));

            long prevIndex = index - 1;
            if (prevIndex < 1)
            {
                prevIndex = 1;
            }
            if (prevIndex == 1)
            {
                //str = "<a>" + ppo.prev + "</a>";
                str = ppo.prev;
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, prevIndex, ppo.prev);
            }


            sb.Append(string.Format("<li>{0}</li>", str));

            long max = 10;
            long strIdx = 1;
            if (index >= max)
            {
                long pretenidx = (index - max);
                if (pretenidx <= 0)
                {
                    pretenidx = 1;
                }
                strIdx = index;
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, pretenidx, "...");
                sb.Append(string.Format("<li>{0}</li>", str));
            }
            for (long i = strIdx; i <= lastPgIndex; i++)
            {
                if (index == i)
                {
                    str = i.ToString();
                }
                else
                {
                    if ((i - strIdx + 1) >= max)
                    {
                        str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, i, "...");
                        sb.Append(string.Format("<li>{0}</li>", str));
                        break;
                    }
                    else
                    {
                        str = string.Format("<a href='{0}?{1}={2}'>{2}</a>", PgUrl, PgParm, i);

                    }
                }
                sb.Append(string.Format("<li>{0}</li>", str));

            }


            long nextIndex = index + 1;
            if (nextIndex > lastPgIndex)
            {
                prevIndex = lastPgIndex;
            }
            if (prevIndex == lastPgIndex)
            {
                //str = "<a>" + ppo.next + "</a>";
                str = ppo.next;
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, nextIndex, ppo.next);
            }
            sb.Append(string.Format("<li>{0}</li>", str));
            if (index == lastPgIndex)
            {
                //str = "<a>" + ppo.last + "</a>";
                str = ppo.last;
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, lastPgIndex, ppo.last);
            }
            sb.Append(string.Format("<li>{0}</li>", str));
            str = string.Format(ppo.info, index, total);
            sb.Append(string.Format("<li>{0}</li>", str));

            sb.Append(string.Format("</ul></nav>"));
            return sb.ToString();
        }

        /// <summary>
        /// 取得BootStrap Pagination分頁列字串(可自定)        
        /// </summary>
        /// <param name="PgTotal"></param>
        /// <param name="PgCount"></param>
        /// <param name="PgIndex"></param>
        /// <param name="PgParm"></param>
        /// <param name="PgUrl"></param>
        /// <param name="ClassName"></param>
        /// <param name="ppo"><example><code>PagerParamObj ppo = new PagerParamObj("[第一頁]", "[上頁]", "[次頁]", "[最末頁]", "目前在第 {0} 頁 / 共有 {1:0,0} 筆查詢結果");</code></example></param>
        /// <example>
        /// <code>
        /// string pgurl = settings.StfSmyUrl;
        /// string pgpar = "?s=";
        /// if (ddlFdTyp.SelectedValue != "")       
        /// {            
        ///     pgpar += Common.getPureLetter(ddlFdTyp.SelectedValue);        
        /// }       
        /// if (ddlDeptCd.SelectedValue != "")       
        /// {            
        ///     pgpar += "&deptcd=" + Common.getPureLetter(ddlDeptCd.SelectedValue);        
        /// }
        /// if (ddlTeacher.SelectedValue != "")       
        /// {            
        ///     pgpar += "&tid=" + Common.getPureLetter(ddlTeacher.SelectedValue);        
        /// }
        /// if (ddlYrSemStr.SelectedValue != "")       
        /// {            
        ///     pgpar += "&str=" + Common.getPureLetter(ddlYrSemStr.SelectedValue);        
        /// }
        /// PagerParamObj ppo = new PagerParamObj("[第一頁]", "[上頁]", "[次頁]", "[最末頁]", "目前在第 {0} 頁 / 共有 {1:0,0} 筆查詢結果");
        /// pghtml = UiAPI.getBs2016NumPager(total.ToString(), pagecount.ToString(), pageindex.ToString(), pgpar + "&pg", pgurl, "pager_style1", ppo);
        /// </code>
        /// </example>
        /// <returns>字串</returns>
        public static string getBs2016NumPager(string PgTotal, string PgCount, string PgIndex, string PgParm, string PgUrl, string ClassName, PagerParamObj ppo)
        {
            long total = 0;
            try
            {
                total = Math.Abs(Convert.ToInt32(PgTotal));
            }
            catch
            {
            }

            long count = 0;
            try
            {
                count = Math.Abs(Convert.ToInt32(PgCount));
            }
            catch
            {
            }

            long index = 0;
            try
            {
                index = Math.Abs(Convert.ToInt32(PgIndex));
            }
            catch
            {
            }

            long lastPgIndex = 0;
            try
            {
                lastPgIndex = total / count;
                if ((total % count) != 0)
                {
                    lastPgIndex++;
                }
            }
            catch
            {
            }

            if (lastPgIndex == 0)
            {
                lastPgIndex = 1;
            }


            string str = string.Empty;
            ClassName = Common.getPureLetter(ClassName);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Length = 0;
            if (!string.IsNullOrEmpty(ClassName))
            {
                ClassName = string.Format(" class='{0}' ", ClassName);
            }

            sb.Append(string.Format("<nav><ul {0} >", ClassName));

            if (index == 1)
            {
                //str = "<a>" + ppo.first + "</a>";
                str = ppo.first;
                sb.Append(string.Format("<li class='disabled'><span><span aria-hidden='true'>{0}</span></span></li>", str));
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, 1, ppo.first);
                sb.Append(string.Format("<li>{0}</li>", str));
            }



            long prevIndex = index - 1;
            if (prevIndex < 1)
            {
                prevIndex = 0;
            }
            if (prevIndex == 0)
            {
                //str = "<a>" + ppo.prev + "</a>";
                str = ppo.prev;
                sb.Append(string.Format("<li class='disabled'><span><span aria-hidden='true'>{0}</span></span></li>", str));
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, prevIndex, ppo.prev);
                sb.Append(string.Format("<li>{0}</li>", str));
            }

            long max = 10;
            long strIdx = 1;
            if (index >= max)
            {
                long pretenidx = (index - max);
                if (pretenidx <= 0)
                {
                    pretenidx = 1;
                }
                strIdx = index;
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, pretenidx, "...");
                sb.Append(string.Format("<li>{0}</li>", str));
            }
            for (long i = strIdx; i <= lastPgIndex; i++)
            {
                if (index == i)
                {
                    str = i.ToString();
                    sb.Append(string.Format("<li class='active'><span>{0} <span class='sr-only'>(current)</span></span></li>", str));
                }
                else
                {
                    if ((i - strIdx + 1) >= max)
                    {
                        str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, i, "...");
                        sb.Append(string.Format("<li>{0}</li>", str));
                        break;
                    }
                    else
                    {
                        str = string.Format("<a href='{0}?{1}={2}'>{2}</a>", PgUrl, PgParm, i);
                        sb.Append(string.Format("<li>{0}</li>", str));
                    }
                }


            }


            long nextIndex = index + 1;
            if (nextIndex > lastPgIndex)
            {
                prevIndex = lastPgIndex;
            }
            if (prevIndex == lastPgIndex)
            {
                //str = "<a>" + ppo.next + "</a>";
                str = ppo.next;
                sb.Append(string.Format("<li class='disabled'><span><span aria-hidden='true'>{0}</span></span></li>", str));
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, nextIndex, ppo.next);
                sb.Append(string.Format("<li>{0}</li>", str));
            }

            if (index == lastPgIndex)
            {
                //str = "<a>" + ppo.last + "</a>";
                str = ppo.last;
                sb.Append(string.Format("<li class='disabled'><span><span aria-hidden='true'>{0}</span></span></li>", str));
            }
            else
            {
                str = string.Format("<a href='{0}?{1}={2}'>{3}</a>", PgUrl, PgParm, lastPgIndex, ppo.last);
                sb.Append(string.Format("<li>{0}</li>", str));
            }

            str = string.Format(ppo.info, index, total);
            sb.Append(string.Format("<li class='ddspageinfo'><span>{0}</span></li>", str));

            sb.Append(string.Format("</ul></nav>"));
            return sb.ToString();
        }


        /// <summary>
        /// 把DataTable內容轉換成Html的Table字串
        /// </summary>
        /// <param name="dt">資料表</param>
        /// <param name="thead">thead的Html</param>
        /// <param name="tfoot">tfoot的Html</param>
        /// <param name="classname">Table的Css Class Name</param>
        /// <param name="colcls">各欄的Css Class Name</param>
        /// <param name="useoddeven">是否要加入基偶列的Css Class Name</param>
        /// <example>
        /// <code>
        /// DdsResult Dr = DB.DoStoredProc(sql, Plo.poLst, ref conn);       
        /// string[] colcls = { "c1", "c2", "", "c4" };       
        /// bool useaddeven = true;
        /// divXXX.InnerHtml = UiAPI.getDataViewHtmlTable(Dr.Dts.Tables[0], "head", "foot", "cls",colcls , useaddeven );
        /// </code>
        /// </example>
        /// <returns>字串</returns>
        public static string getDataTableHtmlTable(DataTable dt , string thead , string tfoot , string classname, string[] colcls , bool useoddeven)
        {
            StringBuilder sb = new StringBuilder();
            int colcount = 0;


            if (string.IsNullOrEmpty(classname))
            {
                sb.Append("<table>\n");
            }
            else
            {
                sb.Append(string.Format("<table class='{0}'>\n", classname));
            }
            if (dt != null)
            {
                colcount = dt.Columns.Count;
                try
                {
                    if (colcls == null)
                    {
                        colcls = new string[colcount];
                    }
                }
                catch
                {
                    colcls = new string[colcount];
                }

                if (!string.IsNullOrEmpty(thead))
                {
                    sb.Append(string.Format("<thead><tr><td colspan='{1}'>{0}</td></tr></thead>\n", thead, colcount));
                }
                sb.Append("<tbody>\n");
                
                sb.Append("<tr>\n");
                for (int i = 0; i < colcount; i++)
                {
                    if (colcls==null)
                    {
                        
                        sb.Append(string.Format("<th>{0}</th>", dt.Columns[i].ColumnName));
                    }
                    else
                    {
                        if (colcls[i] != null)
                        {
                            //sb.Append(string.Format("<th class='{1}'>{0}</th>", dt.Columns[i].ColumnName, colcls[i]));
                            sb.Append(string.Format("<th{1}>{0}</th>", dt.Columns[i].ColumnName, colcls[i] == "" ? "" : " class='" + colcls[i] + "' "));
                        }
                        else
                        {
                            sb.Append(string.Format("<th>{0}</th>", dt.Columns[i].ColumnName));
                        }
                    }
                }
                sb.Append("</tr>\n");

                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (useoddeven)
                    {
                        if (j % 2 == 0)
                        {
                            sb.Append("<tr class='odd'>\n");
                        }
                        else
                        {
                            sb.Append("<tr class='even'>\n");
                        }
                    }
                    else
                    {
                        sb.Append("<tr>\n");
                    }
                    for (int i = 0; i < colcount; i++)
                    {
                        if (colcls == null)
                        {                            
                            sb.Append(string.Format("<td>{0}</td>", dt.Rows[j][i].ToString()));
                        }
                        else
                        {
                            if (colcls[i] != null)
                            {
                                sb.Append(string.Format("<td{1}>{0}</td>", dt.Rows[j][i].ToString(), colcls[i] == "" ? "" : " class='" + colcls[i] + "' "));
                            }
                            else
                            {
                                sb.Append(string.Format("<td>{0}</td>", dt.Rows[j][i].ToString()));
                            }

                        }
                    }
                    sb.Append("</tr>\n");
                }

                sb.Append("</tbody>");
                if (!string.IsNullOrEmpty(thead))
                {
                    sb.Append(string.Format("<tfoot><tr><td colspan='{1}'>{0}</td></tr></tfoot>\n", tfoot, colcount));
                }
            }



            sb.Append("</table>\n");
            return sb.ToString();
        }
        /// <summary>
        /// 取得JwPlayer的嵌入程式碼
        /// </summary>
        /// <param name="file">串流檔名</param>
        /// <param name="streamer">串流協定與路徑</param>
        /// <param name="id">id</param>
        /// <param name="src">player路徑</param>
        /// <param name="w">寬度</param>
        /// <param name="h">高度</param>
        /// <param name="autoplay">是否自動播放</param>
        /// <example>
        /// <code>
        /// UiAPI.getJwPlayerEmbedCode("MOV002-1.flv", "rtmp://163.13.240.57/flash", "play1", "http://excellent.tku.edu.tw/js/jw/player.swf", 720, 480 ,true);
        /// </code>
        /// </example>
        /// <returns>字串</returns>
        public static string getJwPlayerEmbedCode(string file, string streamer, string id, string src, int w, int h , bool autoplay)
        {
            if (autoplay)
            {
                return string.Format(@"
            <embed
              flashvars='file={0}&streamer={1}&provider=rtmp&autostart=true'  
              allowfullscreen='true'
              allowscripaccess='always'
              id='{2}'
              name='{2}'
              src='{3}'
              width='{4}'
              height='{5}'
            />", file, streamer, id, src, w, h);
            }
            else
            {
                return string.Format(@"
            <embed
              flashvars='file={0}&streamer={1}&provider=rtmp'  
              allowfullscreen='true'
              allowscripaccess='always'
              id='{2}'
              name='{2}'
              src='{3}'
              width='{4}'
              height='{5}'
            />", file, streamer, id, src, w, h);
            }
        }

        /// <summary>
        /// 取得JwPlayer的嵌入程式碼
        /// </summary>
        /// <param name="file">串流檔名</param>
        /// <param name="streamer">串流協定與路徑</param>
        /// <param name="id">id</param>
        /// <param name="src">player路徑</param>
        /// <param name="w">寬度</param>
        /// <param name="h">高度</param>
        /// <example>
        /// <code>
        /// UiAPI.getJwPlayerEmbedCode("MOV002-1.flv", "rtmp://163.13.240.57/flash", "play1", "http://excellent.tku.edu.tw/js/jw/player.swf", 720, 480 );
        /// </code>
        /// </example>
        /// <returns>字串</returns>
        public static string getJwPlayerEmbedCode(string file, string streamer, string id, string src, int w, int h)
        {
            return getJwPlayerEmbedCode(file, streamer, id, src, w, h, true);
        }
    }
    /// <summary>
    /// Pager設定物件
    /// </summary>
    public class PagerParamObj
    {
        /// <summary>
        /// 屬性：首頁
        /// </summary>
        public string first = "<<";
        /// <summary>
        /// 屬性：前頁
        /// </summary>
        public string prev = "<";
        /// <summary>
        /// 屬性：次頁
        /// </summary>
        public string next = ">";
        /// <summary>
        /// 屬性：末頁
        /// </summary>
        public string last = ">>";
        /// <summary>
        /// 屬性：敘述。{0} 頁數 {1} 總筆數，不可省略。
        /// </summary>
        public string info = "目前在第 {0} 頁 共 {1} 筆";
        /// <summary>
        /// 預設建構子
        /// </summary>
        public PagerParamObj()
        {

        }
        /// <summary>
        /// 自訂建構子
        /// </summary>
        /// <param name="first">首頁</param>
        /// <param name="prev">前頁</param>
        /// <param name="next">次頁</param>
        /// <param name="last">末頁</param>
        /// <param name="info">敘述</param>
        /// <example>
        /// <code>
        /// PagerParamObj ppo = new PagerParamObj("[第一頁]", "[上頁]", "[次頁]", "[最末頁]", "目前在第 {0} 頁 / 共有 {1:0,0} 筆查詢結果");
        /// </code>
        /// </example>
        public PagerParamObj(string first, string prev, string next, string last, string info)
        {
            this.first =first;
            this.prev = prev;
            this.next = next;
            this.last = last;
            this.info = info;
        }
    }
}
