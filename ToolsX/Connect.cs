using System;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Microsoft.VisualStudio.CommandBars;
using System.Windows.Forms;


namespace ToolsX
{
    /// <summary>用于实现外接程序的对象。</summary>
    /// <seealso class='IDTExtensibility2' />
    /// 
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        /// <summary>实现外接程序对象的构造函数。请将您的初始化代码置于此方法内。</summary>
        public Connect()
        {
        }

        /// <summary>实现 IDTExtensibility2 接口的 OnConnection 方法。接收正在加载外接程序的通知。</summary>
        /// <param term='application'>宿主应用程序的根对象。</param>
        /// <param term='connectMode'>描述外接程序的加载方式。</param>
        /// <param term='addInInst'>表示此外接程序的对象。</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;


            /* 全局单例初始化 */
            SingleManager mgr = SingleManager.getInstance();
            mgr._addInInstance = _addInInstance;
            mgr._applicationObject = _applicationObject;

            mgr.initSubs();

            if (connectMode == ext_ConnectMode.ext_cm_UISetup)
            {
                try
                {
                    _applicationObject.AddIns.Update();
                    object[] contextGUIDS = new object[] { };

                    mgr.initSubs();

                    /* 所有的命令 */
                    Commands2 commands = (Commands2)_applicationObject.Commands;

                    /* 生成新的子菜单对象,将会被插入到菜单条和工具条对象上 */
                    Command btnCommandFromTarget1 = commands.AddNamedCommand2(_addInInstance, "ToolsX_AddTip", "新增", "新增加一个代码标记点", true, 16, ref contextGUIDS, ((int)(vsCommandStatus.vsCommandStatusSupported)) + ((int)(vsCommandStatus.vsCommandStatusEnabled)), ((int)(vsCommandStyle.vsCommandStylePictAndText)), vsCommandControlType.vsCommandControlTypeButton);
                    Command btnCommandFromTarget1_1 = commands.AddNamedCommand2(_addInInstance, "ToolsX_AddTip_1", "新增代码注记", "新增加一个代码标记点", true, 16, ref contextGUIDS, ((int)(vsCommandStatus.vsCommandStatusSupported)) + ((int)(vsCommandStatus.vsCommandStatusEnabled)), ((int)(vsCommandStyle.vsCommandStylePictAndText)), vsCommandControlType.vsCommandControlTypeButton);

                    Command btnCommandFromTargetR = commands.AddNamedCommand2(_addInInstance, "ToolsX_AddRiviewTip", "新增Review问题", "新增加一个review问题点", true, 44, ref contextGUIDS, ((int)(vsCommandStatus.vsCommandStatusSupported)) + ((int)(vsCommandStatus.vsCommandStatusEnabled)), ((int)(vsCommandStyle.vsCommandStylePictAndText)), vsCommandControlType.vsCommandControlTypeButton);
                    Command btnCommandFromTarget2 = commands.AddNamedCommand2(_addInInstance, "ToolsX_Search", "检索", "检索代码标记点", true, 25, ref contextGUIDS, ((int)(vsCommandStatus.vsCommandStatusSupported)) + ((int)(vsCommandStatus.vsCommandStatusEnabled)), ((int)(vsCommandStyle.vsCommandStylePictAndText)), vsCommandControlType.vsCommandControlTypeButton);
                    Command btnCommandFromTarget3 = commands.AddNamedCommand2(_addInInstance, "ToolsX_Export", "报表", "review问题点统计统计和报表功能", true, 17, ref contextGUIDS, ((int)(vsCommandStatus.vsCommandStatusSupported)) + ((int)(vsCommandStatus.vsCommandStatusEnabled)), ((int)(vsCommandStyle.vsCommandStylePictAndText)), vsCommandControlType.vsCommandControlTypeButton);
                    Command btnCommandFromTarget4 = commands.AddNamedCommand2(_addInInstance, "ToolsX_Check", "检查", "检查选中文本是否存在编程规范问题", true, 109, ref contextGUIDS, ((int)(vsCommandStatus.vsCommandStatusSupported)) + ((int)(vsCommandStatus.vsCommandStatusEnabled)), ((int)(vsCommandStyle.vsCommandStylePictAndText)), vsCommandControlType.vsCommandControlTypeButton);
                    Command btnCommandFromTarget5 = commands.AddNamedCommand2(_addInInstance, "ToolsX_DMap", "DMap", "代码结构的宏观逻辑图", true, 43, ref contextGUIDS, ((int)(vsCommandStatus.vsCommandStatusSupported)) + ((int)(vsCommandStatus.vsCommandStatusEnabled)), ((int)(vsCommandStyle.vsCommandStylePictAndText)), vsCommandControlType.vsCommandControlTypeButton);

                    /* 所有的工具条 */
                    _CommandBars commandBars = (Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars;

                    // 菜单条对象和工具条对象都是CommandBar类型
                    // 创建主菜单项
                    CommandBar menuObj;
                    try
                    {
                        menuObj = (CommandBar)commandBars["XPTips"];
                    }
                    catch (System.ArgumentException exp)
                    {
                        menuObj = (CommandBar)_applicationObject.Commands.AddCommandBar("XPTips", vsCommandBarType.vsCommandBarTypeMenu,
                        ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["MenuBar"],
                        ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)["MenuBar"].Controls.Count);
                    }

                    CommandBar toolbarObj;
                    try
                    {
                        toolbarObj = (CommandBar)commandBars["XPTipsBar"];
                    }
                    catch (System.ArgumentException exp)
                    {
                        // 工具条
                        toolbarObj = (CommandBar)_applicationObject.Commands.AddCommandBar("XPTipsBar", vsCommandBarType.vsCommandBarTypeToolbar, null, -1);
                    }

                    // 添加主菜单
                    CommandBarButton buttonObj;

                    buttonObj = (CommandBarButton)btnCommandFromTarget1.AddControl(menuObj, menuObj.Controls.Count + 1);
                    buttonObj = (CommandBarButton)btnCommandFromTarget2.AddControl(menuObj, menuObj.Controls.Count + 1);
                    //    buttonObj = (CommandBarButton)btnCommandFromTarget3.AddControl(menuObj, menuObj.Controls.Count + 1);
                    buttonObj = (CommandBarButton)btnCommandFromTarget4.AddControl(menuObj, menuObj.Controls.Count + 1);
                    buttonObj = (CommandBarButton)btnCommandFromTarget5.AddControl(menuObj, menuObj.Controls.Count + 1);

                    buttonObj = (CommandBarButton)btnCommandFromTarget1.AddControl(toolbarObj, toolbarObj.Controls.Count + 1);
                    buttonObj = (CommandBarButton)btnCommandFromTarget2.AddControl(toolbarObj, toolbarObj.Controls.Count + 1);
                    //     buttonObj = (CommandBarButton)btnCommandFromTarget3.AddControl(toolbarObj, toolbarObj.Controls.Count + 1);
                    buttonObj = (CommandBarButton)btnCommandFromTarget4.AddControl(toolbarObj, toolbarObj.Controls.Count + 1);
                    buttonObj = (CommandBarButton)btnCommandFromTarget5.AddControl(toolbarObj, toolbarObj.Controls.Count + 1);

                    //检索代码编辑窗口右键弹出菜单的工具条
                    CommandBar projBar = commandBars["Code Window"];
                    btnCommandFromTargetR.AddControl(projBar, 1);
                    btnCommandFromTarget1_1.AddControl(projBar, 1);
                }
                catch (Exception exp)
                {
                    if (MessageBox.Show(exp.ToString(), "提示：请将此错误信息反馈给 W07398, 3Q!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {

                    }
                }
            }
        }

        /// <summary>实现 IDTExtensibility2 接口的 OnDisconnection 方法。接收正在卸载外接程序的通知。</summary>
        /// <param term='disconnectMode'>描述外接程序的卸载方式。</param>
        /// <param term='custom'>特定于宿主应用程序的参数数组。</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {

        }

        /// <summary>实现 IDTExtensibility2 接口的 OnAddInsUpdate 方法。当外接程序集合已发生更改时接收通知。</summary>
        /// <param term='custom'>特定于宿主应用程序的参数数组。</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>实现 IDTExtensibility2 接口的 OnStartupComplete 方法。接收宿主应用程序已完成加载的通知。</summary>
        /// <param term='custom'>特定于宿主应用程序的参数数组。</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
            /* 全局单例初始化 */
        }

        /// <summary>实现 IDTExtensibility2 接口的 OnBeginShutdown 方法。接收正在卸载宿主应用程序的通知。</summary>
        /// <param term='custom'>特定于宿主应用程序的参数数组。</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
            return;
            try
            {
                Command commandObj;
                commandObj = _applicationObject.Commands.Item("ToolsX.Connect.ToolsX_AddTip", -1);
                if (commandObj != null) { commandObj.Delete(); }

                commandObj = _applicationObject.Commands.Item("ToolsX.Connect.ToolsX_AddRiviewTip", -1);
                if (commandObj != null) { commandObj.Delete(); }

                commandObj = _applicationObject.Commands.Item("ToolsX.Connect.ToolsX_Search", -1);
                if (commandObj != null) { commandObj.Delete(); }

                commandObj = _applicationObject.Commands.Item("ToolsX.Connect.ToolsX_Export", -1);
                if (commandObj != null) { commandObj.Delete(); }

                commandObj = _applicationObject.Commands.Item("ToolsX.Connect.ToolsX_Check", -1);
                if (commandObj != null) { commandObj.Delete(); }

                commandObj = _applicationObject.Commands.Item("ToolsX.Connect.ToolsX_DMap", -1);
                if (commandObj != null) { commandObj.Delete(); }

                /* 所有的工具条 */
                _CommandBars commandBars = (Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars;

                /* 删除菜单栏 */
                CommandBar menuObj = (CommandBar)commandBars["XPTips"];
                if (menuObj != null)
                {
                    _applicationObject.Commands.RemoveCommandBar(menuObj);
                }

                /* 删除工具条 */
                CommandBar toolbarObj = (CommandBar)commandBars["XPTipsBar"];
                if (toolbarObj != null)
                {
                    _applicationObject.Commands.RemoveCommandBar(toolbarObj);
                }
            }

            catch (Exception exp)
            {
                if (MessageBox.Show(exp.ToString(), "提示：请将此错误信息反馈给 W07398, 3Q!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                }
            }
        }

        /// <summary>实现 IDTCommandTarget 接口的 QueryStatus 方法。此方法在更新该命令的可用性时调用</summary>
        /// <param term='commandName'>要确定其状态的命令的名称。</param>
        /// <param term='neededText'>该命令所需的文本。</param>
        /// <param term='status'>该命令在用户界面中的状态。</param>
        /// <param term='commandText'>neededText 参数所要求的文本。</param>
        /// <seealso class='Exec' />
        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
            {
                string str = commandName.Substring(0, 6);
                if (str == "ToolsX")
                {
                    status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported | vsCommandStatus.vsCommandStatusEnabled;
                    return;
                }
            }
        }

        /// <summary>实现 IDTCommandTarget 接口的 Exec 方法。此方法在调用该命令时调用。</summary>
        /// <param term='commandName'>要执行的命令的名称。</param>
        /// <param term='executeOption'>描述该命令应如何运行。</param>
        /// <param term='varIn'>从调用方传递到命令处理程序的参数。</param>
        /// <param term='varOut'>从命令处理程序传递到调用方的参数。</param>
        /// <param term='handled'>通知调用方此命令是否已被处理。</param>
        /// <seealso class='Exec' />
        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;
            if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
            {
                try
                {
                    if (commandName == "ToolsX.Connect.ToolsX_AddTip" || commandName == "ToolsX.Connect.ToolsX_AddTip_1")
                    {
                        handled = true;
                        SingleManager.getInstance().showFormAdd(0);
                        return;
                    }
                    else if (commandName == "ToolsX.Connect.ToolsX_AddRiviewTip")
                    {
                        handled = true;
                        SingleManager.getInstance().showFormAdd(1);
                        return;
                    }
                    else if (commandName == "ToolsX.Connect.ToolsX_Search")
                    {
                        handled = true;
                        SingleManager.getInstance().showFormSearch();
                        return;
                    }
                    else if (commandName == "ToolsX.Connect.ToolsX_Export")
                    {
                        handled = true;
                        SingleManager.getInstance().showFormExport();
                        return;
                    }
                    else if (commandName == "ToolsX.Connect.ToolsX_Check")
                    {
                        handled = true;
                        SingleManager.getInstance().checkCode();
                        return;
                    }
                    else if (commandName == "ToolsX.Connect.ToolsX_DMap")
                    {
                        handled = true;
                        SingleManager.getInstance().showDMap();
                        return;
                    }
                }
                catch (Exception exp)
                {
                    if (MessageBox.Show(exp.ToString(), "提示：请将此错误信息反馈给 W07398, 3Q!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {

                    }
                }
            }
        }
        private DTE2 _applicationObject;
        private AddIn _addInInstance;
    }
}