using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnvDTE80;

namespace ToolsX
{
    public partial class FormDMap : Form
    {
        public FormDMap()
        {
            InitializeComponent();
        }

        private void FormDMap_Load(object sender, EventArgs e)
        {
            this.LostFocus += new EventHandler(FormDMap_LostFocus);
        }

        private void FormDMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void FormDMap_LostFocus(object sender, EventArgs e)
        {
          //  this.Hide();
        }

        private void TipButtonClick(object sender, EventArgs e)
        {
            /*  跳转到tip位置或者添加tip */
            Button btn = (Button)sender;
            string strBtnName = btn.Name.ToString();
            string strSheetID = "FormDMap_" + strBtnName;    /* sheetid  = FormDMap_  + 按钮的 ID */   
            string strDesc = btn.Text.ToString();            /* 描述信息为按钮的文本 */
            bool bRet = SingleManager.getInstance().JumpToSheetID(strSheetID);
            if (false == bRet)
            {
                /* 尚未添加该TIP，先添加 */
                if (MessageBox.Show("DMAP节点尚不存在，现在添加？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    SingleManager.getInstance().AddDMapNode(strSheetID, strDesc);
                }
            }
        }
        public DTE2 _applicationObject;

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
