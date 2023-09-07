using System;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
using ToolsX;
namespace ToolsX
{
    public partial class FormAdd : Form
    {
        public FormAdd()
        {
            InitializeComponent();
        }

        private void FormAdd_Load(object sender, EventArgs e)
        {
            this.LostFocus += new EventHandler(FormAdd_LostFocus);
        }

        private void FormAdd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void FormAdd_LostFocus(object sender, EventArgs e)
        {
          //  this.Hide();
        }

        private void BTN_ADD_Click(object sender, EventArgs e)
        {
            try
            {
                /* 先隐藏，再存储，这样用户体验更好 */
                this.Hide();

                /* 写入到数据库里 */
                string strDesc = textDesc.Text.Trim();
                string strSelection = textSelection.Text.Trim().Substring(0, 4096);
                string strSolution = textSolution.Text.Trim();
                string strFilePath = textPath.Text.Trim();

                /* 未打开解决方案的时候，这些参数可能没有 */
                if ("" == strDesc || "" == strSelection || "" == strSolution || "" == strFilePath)
                {
                    MessageBox.Show("参数为空！");
                    return;
                }

                Tip tip = new Tip();
                tip.sheetid = m_strSheetID;
                tip.solution = strSolution;
                tip.filepath = strFilePath;
                tip.description = strDesc;
                tip.user = "who?";
                tip.keystring = strSelection;

                SingleManager.getInstance().InsertATip(tip);
                m_strSheetID = "";
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }

        }

        private void BTN_CANCEL_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        public void initCtrl(int iType)
        {
            initParams();

            this.m_strSheetID = Guid.NewGuid().ToString();
            if (0 == iType)
            {
                this.Text = "新增一个代码注记点";
                textType.Text = "代码注记";
                this.textDesc.ReadOnly = false;
                this.textDesc.Text = "";
            }
            else if (1 == iType)
            {
                this.Text = "新增一个review问题";
                textType.Text = "代码审查";
                this.textDesc.ReadOnly = false;
                this.textDesc.Text = "Review问题:";
            }
            else if (2 == iType)
            {
                this.Text = "新增DMap节点 : " + textDesc.Text;
                textType.Text = "DMap节点";
                this.textDesc.ReadOnly = true;
            }
        }


        private void FormAdd_Activated(object sender, EventArgs e)
        {
            initParams();
        }

        public void initParams()
        {

            textSelection.Text = "";
            textPath.Text = "";
            try
            {
                /* 解决方案路径 */
                string strSolutionName = _applicationObject.Solution.FullName;
                if (strSolutionName == "")
                {
                    textSolution.Text = "当前未打开解决方案";
                }
                else
                {
                    /* 提取解决方案名称 */
                    int nIndex = strSolutionName.LastIndexOf("\\");
                    strSolutionName = strSolutionName.Substring(nIndex + 1, strSolutionName.Length - nIndex - 1);
                    textSolution.Text = strSolutionName;
                }

                /* 当前在编辑的文件 */
                if (null != _applicationObject.ActiveDocument)
                {
                    /* 当前在编辑的文件的路径 */
                    string strFilePath = _applicationObject.ActiveDocument.FullName;

                    /* 计算当前文件对解决方案的相对目录 */
                    string strRelativePath = SingleManager.getInstance().GetRelativePath(strFilePath);

                    /* 文件相对目录的展示 */
                    textPath.Text = strRelativePath;

                    /* 当前选中的文本 */
                    TextSelection selection = (TextSelection)_applicationObject.ActiveDocument.Selection;
                    textSelection.Text = selection.Text;
                }
                else
                {
                    /* 文件相对目录的展示 */
                    textPath.Text = "当前无在编辑中的文件";
                }
            }
            catch (Exception exp)
            {
               // MessageBox.Show(exp.ToString());
            }
        }

        public string m_strSheetID;
        public DTE2 _applicationObject;
    }
}
