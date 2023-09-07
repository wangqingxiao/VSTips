using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnvDTE80;

namespace ToolsX
{
    public partial class FormExport : Form
    {
        public FormExport()
        {
            InitializeComponent();
        }

        private void FormExport_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void FormExport_Load(object sender, EventArgs e)
        {
            this.LostFocus += new EventHandler(FormExport_LostFocus);
        }

        void FormExport_LostFocus(object sender, EventArgs e)
        {
            this.Hide();
        }

        public DTE2 _applicationObject;
    }
}
