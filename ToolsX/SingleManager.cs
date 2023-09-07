using System;
using EnvDTE80;
using EnvDTE;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text;
using System.Reflection;


namespace ToolsX
{
    public sealed class SingleManager
    {
        public DTE2 _applicationObject;         /* 宿主 */
        public AddIn _addInInstance;            /* 插件对象 */
        public database db;                     /* 数据库服务 */
        public utils util;                      /* 工具集 */
        public FormAdd m_formAdd;               /* 增加界面 */
        public FormCheck m_formCheck;           /* 编程规范检查 */
        public FormDMap m_formDMap;             /* Dream Map 图 */
        public FormSearch m_formSearch;         /* 代码注记检索界面 */
        public FormExport m_formExport;         /* 代码审查报表界面 */

        /* 单例对象 */
        private static SingleManager instance = new SingleManager();
        private SingleManager()
        {
            Init();
        }

        /* 单例 */
        public static SingleManager getInstance()
        {
            return instance;
        }
        public void Init()
        {
            /* 绑定需要监听的开发环境事件 */
            // _applicationObject.Events.SelectionEvents.OnChange += new _dispSelectionEvents_OnChangeEventHandler(SelectionEvents_OnChange);
            try
            {
                db = new database();
                util = new utils();
                m_formAdd = new FormAdd();
                m_formCheck = new FormCheck();
                m_formDMap = new FormDMap();
                m_formSearch = new FormSearch();
                m_formExport = new FormExport();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        /* 宿主信息传递 */
        public void initSubs()
        {
            try
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                string strAddinPath = System.IO.Path.GetDirectoryName(path);
                strAddinPath += "\\config.ini";
                strAddinPath = System.IO.Path.GetFullPath(strAddinPath);
                StringBuilder buffer = new StringBuilder(1024);
                GetPrivateProfileString("CONFIG", "database", "port=5432;database=imos;host=206.206.128.228;password=admin_123;userid=postgres;", buffer, 1024, strAddinPath);
                db.ConStr = buffer.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }

            /* 参数传递 */
            util._applicationObject = _applicationObject;
            m_formAdd._applicationObject = _applicationObject;
            m_formCheck._applicationObject = _applicationObject;
            m_formDMap._applicationObject = _applicationObject;
            m_formSearch._applicationObject = _applicationObject;
            m_formExport._applicationObject = _applicationObject;

            try
            {
                m_formCheck.initRule();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        /* 按钮命令的执行 */
        public void showFormAdd(int iType)
        {
            m_formAdd.initCtrl(iType);
            m_formAdd.Show();
            m_formAdd.Activate();
        }
        public void showFormSearch()
        {
            m_formSearch.initList();
            m_formSearch.Show();
            m_formSearch.Activate();
        }
        public void showFormExport()
        {
            m_formExport.Show();
            m_formExport.Activate();
        }
        public void checkCode()
        {
            m_formCheck.initCtrl();
            m_formCheck.Show();
            m_formCheck.Activate();
        }
        public void showDMap()
        {
            m_formDMap.Show();
            m_formDMap.Activate();
        }

        /* tips 的增删改查 */
        public void InsertATip(Tip tip)
        {
            /* 准备插入语句 */
            string strSql = " INSERT INTO public.tbl_toolx_tips(\"SHEETID\", \"SOLUTION\", \"FILEPATH\", \"DESCRIPTION\", \"USER\", \"KEYSTRING\") ";
            strSql += "VALUES ( \'"
                + tip.sheetid + "\', \'"
                + tip.solution + "\', \'"
                + tip.filepath + "\', \'"
                + tip.description + "\',\' "
                + tip.user + "\', \'"
                + tip.keystring + "\');";

            /* 写入数据库 */
            db.ExecuteNonQuery(strSql);
        }

        public void DeleteATip(string strSheetID)
        {
            string sql = "DELETE FROM public.tbl_toolx_tips	WHERE  LOWER(\"SHEETID\") = LOWER(\'" + strSheetID + "\');";
            db.ExecuteNonQuery(sql);
        }

        public void UpdateATip(string strSheetID)
        {

        }

        public DataSet queryTips(string strKeystring)
        {
            /* 提取解决方案名称进行验证 */
            string strSolution = util.GetCurSolution();

            /* 组装模糊查询语句 */
            string strSql = "SELECT \"SHEETID\", \"SOLUTION\", \"FILEPATH\", \"DESCRIPTION\", \"USER\", \"STATUS\", \"KEYSTRING\" FROM PUBLIC.TBL_TOOLX_TIPS ";
            strSql += " WHERE LOWER(\"SOLUTION\") = LOWER(\'"
                + strSolution + "\') AND ( LOWER(\"DESCRIPTION\") LIKE LOWER(\'%"
                + strKeystring + "%\') OR  LOWER(\"KEYSTRING\") LIKE LOWER(\'%"
                + strKeystring + "%\'))";

            /* 执行查询 */
            return db.ExecuteQuery(strSql);
        }

        public void AddDMapNode(string strSheetID, string strDescription)
        {
            /* 弹出新增界面 */
            m_formAdd.textDesc.Text = strDescription;
            m_formAdd.initCtrl(2);
            m_formAdd.textDesc.Text = "DMap: " + strDescription;
            m_formAdd.m_strSheetID = strSheetID;
            m_formAdd.Show();
            m_formAdd.Activate();
        }

        public string GetRelativePath(string strFilePath)
        {
            return util.GetRelativePath(strFilePath);
        }

        public string GetRealPath(string strRelativePath)
        {
            return util.GetRealPath(strRelativePath);
        }

        public void openTip(Tip tip)
        {
            if (tip.solution != util.GetCurSolution())
            {
                MessageBox.Show("所选节点不是当前解决方案的节点!");
                return;
            }

            /* 组装文件的绝对目录 */
            string strRealPath = SingleManager.getInstance().GetRealPath(tip.filepath);
            try
            {
                /* 打开文件 */
                EnvDTE.Document obj = _applicationObject.Documents.Open(strRealPath, "auto", false);
                if (null == obj)
                {
                    return;
                }
            }
            catch (Exception exp)
            {
                string strDesc = "文件不存在:" + strRealPath;
                if (MessageBox.Show(strDesc, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                }
            }

            try
            {
                /* 拿到当前打开的文件 TextDocument */
                TextDocument textDoc = (TextDocument)_applicationObject.ActiveDocument.Object("TextDocument");
                EditPoint objEditPoint = textDoc.StartPoint.CreateEditPoint();

                /* 跳转到文件开头 */
                objEditPoint.StartOfDocument();

                /* 新建一个编辑点 */
                EditPoint editPoint2 = textDoc.EndPoint.CreateEditPoint();

                /* 查找 keystring */
                TextSelection ts = (TextSelection)_applicationObject.ActiveDocument.Selection;
                TextRanges trs = ts.TextRanges;
                bool bFind = objEditPoint.FindPattern(tip.keystring, 0, ref editPoint2, ref trs);
                if (bFind)
                {
                    /* 找到了 keystring 则跳转到对应位置 */
                    objEditPoint.TryToShow(vsPaneShowHow.vsPaneShowCentered, 0);
                }
                else
                {
                    /* 没找到的时候，尝试找一下和前后32的字符相同的位置 */
                    if (tip.keystring.Length > 32)
                    {
                        /* 跳转到文件开头 */
                        objEditPoint.StartOfDocument();
                        string strKeyStringFront = tip.keystring.Substring(0, 32);
                        bool bFindFront = objEditPoint.FindPattern(strKeyStringFront, 0, ref editPoint2, ref trs);
                        if (bFindFront)
                        {
                            /* 找到了 subkeystring 则跳转到对应位置 */
                            objEditPoint.TryToShow(vsPaneShowHow.vsPaneShowCentered, 0);
                        }
                        else
                        {
                            /* 跳转到文件开头 */
                            objEditPoint.StartOfDocument();
                            int len = tip.keystring.Length;
                            string strKeyStringTail = tip.keystring.Substring(len - 32, 32);
                            bool bFindTail = objEditPoint.FindPattern(strKeyStringTail, 0, ref editPoint2, ref trs);
                            if (bFindTail)
                            {
                                /* 找到了 subkeystring 则跳转到对应位置 */
                                objEditPoint.TryToShow(vsPaneShowHow.vsPaneShowCentered, 0);
                            }
                            else
                            {
                                if (MessageBox.Show("代码注记点在本机代码中未找到,确认在其他代码分支也不存在的话建议删除，删除 ？ ", "警告：", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                                {
                                    SingleManager.getInstance().DeleteATip(tip.sheetid);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                if (MessageBox.Show(exp.ToString(), "提示：请将此错误信息反馈给 W07398, 3Q!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                }
            }
        }

        public bool JumpToSheetID(string strSheetID)
        {
            try
            {
                /* 根据sheetid 查找 tip */
                string sql = "SELECT \"SHEETID\", \"SOLUTION\", \"FILEPATH\", \"DESCRIPTION\", \"USER\", \"KEYSTRING\" FROM PUBLIC.TBL_TOOLX_TIPS WHERE ";
                sql += " LOWER( \"SHEETID\" ) =  LOWER(\'" + strSheetID + "\')";

                DataSet ds = db.ExecuteQuery(sql);


                int rowCount, columnCount, i, j;
                rowCount = ds.Tables[0].Rows.Count;
                columnCount = ds.Tables[0].Columns.Count;

                /* 数据库里查不到就是没有添加过 */
                if (rowCount < 1)
                {
                    return false;
                }

                Tip tip = new Tip();
                tip.sheetid = ds.Tables[0].Rows[0][0].ToString();
                tip.solution = ds.Tables[0].Rows[0][1].ToString();
                tip.filepath = ds.Tables[0].Rows[0][2].ToString();
                tip.description = ds.Tables[0].Rows[0][3].ToString();
                tip.user = ds.Tables[0].Rows[0][4].ToString();
                tip.keystring = ds.Tables[0].Rows[0][5].ToString();

                openTip(tip);
                return true;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
                return true;
            }
        }



        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string defval, StringBuilder retval, int size, string filepath);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern int WritePrivateProfileString(string section, string key, string val, string filepath);
    }
}
