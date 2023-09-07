

using System;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Microsoft.VisualStudio.CommandBars;
using System.Windows.Forms;

namespace ToolsX
{
    /* 代码注记点 */
    public struct Tip
    {
        public string sheetid;
        public string user;
        public string solution;
        public string filepath;
        public string type;
        public string description;
        public string status;
        public string keystring;
    };

    /* 编程规范校验工具 */
    public struct Rule
    {
        public string desc;
        public string strRule;
    };
}














