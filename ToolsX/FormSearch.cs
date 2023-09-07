using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;

namespace ToolsX
{
    public partial class FormSearch : Form
    {
        public FormSearch()
        {
            InitializeComponent();
        }

        private void FormSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void initList()
        {
            try
            {
            //  SHEETID, SOLUTION, FILEPATH, DESCRIPTION, USER, STATUS, KEYSTRING
                DataSet ds = SingleManager.getInstance().queryTips(textKey.Text);
                int rowCount, columnCount, i, j;
                rowCount = ds.Tables[0].Rows.Count;
                columnCount = ds.Tables[0].Columns.Count;

                listView1.BeginUpdate();
                listView1.Items.Clear();
                string[] lvitem = new string[columnCount];
                for (i = 0; i < rowCount; i++)
                {
                    lvitem[0] = ds.Tables[0].Rows[i][3].ToString();
                    lvitem[1] = ds.Tables[0].Rows[i][6].ToString();
                    lvitem[2] = ds.Tables[0].Rows[i][2].ToString();
                    lvitem[3] = ds.Tables[0].Rows[i][0].ToString();
                    lvitem[4] = ds.Tables[0].Rows[i][1].ToString();

                    ListViewItem lvi = new ListViewItem(lvitem);
                    listView1.Items.Add(lvi);
                }
                listView1.EndUpdate();
            }
            catch (Exception exp)
            {
                // MessageBox.Show(exp.ToString());
            }
        }

        private void textKey_TextChanged(object sender, EventArgs e)
        {
            initList();
        }

        public DTE2 _applicationObject;

        private void FormSearch_Load(object sender, EventArgs e)
        {
            this.LostFocus += new EventHandler(FormSearch_LostFocus);
            this.Text = "注记点检索";
        }

        private void FormSearch_LostFocus(object sender, EventArgs e)
        {
          //  this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1_DoubleClick(sender, e);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                /* 跳转到指定的代码位置 */
                string strPath = listView1.FocusedItem.SubItems[2].Text;
                string strSheetID = listView1.FocusedItem.SubItems[3].Text;
                string strKeyString = listView1.FocusedItem.SubItems[1].Text;
                string strSolution = listView1.FocusedItem.SubItems[4].Text;

                if ("" == strPath || "" == strSheetID || "" == strKeyString || "" == strSolution)
                {
                    return;
                }

                Tip tip = new Tip();
                tip.sheetid = listView1.FocusedItem.SubItems[3].Text;
                tip.solution = listView1.FocusedItem.SubItems[4].Text;
                tip.filepath = listView1.FocusedItem.SubItems[2].Text;
                tip.keystring = listView1.FocusedItem.SubItems[1].Text;

                SingleManager.getInstance().openTip(tip);
                initList();
            }
            catch (Exception exp)
            { 
                // MessageBox.Show(exp.ToString());
            }
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Hide();
            if (e.KeyChar == '\r')
            {
                EventArgs ex = new EventArgs();
                listView1_DoubleClick(sender, ex);
            }
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                try
                {
                    string strSheetID = listView1.FocusedItem.SubItems[3].Text;
                    SingleManager.getInstance().DeleteATip(strSheetID);
                    initList();
                }
                catch (Exception exp)
                {
                    MessageBox.Show("未选中要删除的项目");
                }
            }
        }

        private void FormSearch_Activated(object sender, EventArgs e)
        {
            initList();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
