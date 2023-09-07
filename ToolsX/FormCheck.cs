using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;
namespace ToolsX
{
    public partial class FormCheck : Form
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int GetPrivateProfileString(string section, string key, string defval, StringBuilder retval, int size, string filepath);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern int WritePrivateProfileString(string section, string key, string val, string filepath);

        public FormCheck()
        {
            InitializeComponent();
        }

        public Dictionary<int, Rule> RuleMap = new Dictionary<int, Rule>();

        public void initRule()
        {
            RuleMap.Clear();

            /* 遍历读取编程规范校验规则 */
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string strAddinPath = System.IO.Path.GetDirectoryName(path);
            strAddinPath += "\\config.ini";
            strAddinPath = System.IO.Path.GetFullPath(strAddinPath);


            StringBuilder bufferTittle = new StringBuilder(1024);
            StringBuilder bufferDesc   = new StringBuilder(1024);


            for (int i = 0; i < 200; i++)
            {
                string RuleTittle = "RULE" + i.ToString();

                /* 取正则表达式字符串 */
                GetPrivateProfileString("CodeRule", RuleTittle, "END", bufferTittle, 1024, strAddinPath);

                /* 取校验规则说明 */
                GetPrivateProfileString("CodeRuleDesc", RuleTittle, "END", bufferDesc, 1024, strAddinPath);

                /*  到结尾就退出 */
                if ("END" == bufferTittle.ToString())
                {
                    break;
                }

                Rule rule = new Rule();
                rule.strRule = bufferTittle.ToString();
                rule.desc = bufferDesc.ToString();
                RuleMap.Add(i, rule);

            }
        }

        public void initCtrl()
        {
            /* 逐条验证编程规范 */
            try
            {
                /* 当前选中的文本 */
                TextSelection selection = (TextSelection)_applicationObject.ActiveDocument.Selection;
                string stringSelected = selection.Text;

                /* 用单空格替代多空格 */
                stringSelected = stringSelected.Replace("  ", " ");
                stringSelected = stringSelected.Replace("   ", " ");
                stringSelected = stringSelected.Replace("    ", " ");
                stringSelected = stringSelected.Replace("     ", " ");
                stringSelected = stringSelected.Replace("      ", " ");
                stringSelected = stringSelected.Replace("       ", " ");
                stringSelected = stringSelected.Replace("        ", " ");
                stringSelected = stringSelected.Replace("         ", " ");

                listView1.BeginUpdate();
                listView1.Items.Clear();

                /* 遍历执行规则校验 */

                var val = RuleMap.Keys.ToList();

                int i = 0;
                foreach (var key in val)
                {
                    i++;
                    Rule rule = RuleMap[key];
                    Console.WriteLine(rule.desc);

                    if (Regex.IsMatch(stringSelected, rule.strRule))
                    {
                        string[] lvitem = new string[2];
                        lvitem[0] = i.ToString();
                        lvitem[1] = rule.desc.ToString();
                        ListViewItem lvi = new ListViewItem(lvitem);
                        listView1.Items.Add(lvi);
                    }
                }
                listView1.EndUpdate();
            }
            catch (Exception exp)
            {
                //MessageBox.Show(exp.ToString());
            }
        }

        private void FormCheck_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void FormCheck_Load(object sender, EventArgs e)
        {
            this.LostFocus += new EventHandler(FormCheck_LostFocus);
            initRule();
        }

        private void FormCheck_LostFocus(object sender, EventArgs e)
        {
            this.Hide();
        }

        public DTE2 _applicationObject;

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
