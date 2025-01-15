using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.PluginManager.Enums;
/// <summary>
/// 插件安装状态说明
/// </summary>
public enum PluginInstallStatus
{
    /// <summary>
    /// 无操作
    /// </summary>
    None,
    /// <summary>
    /// 已安装
    /// </summary>
    Installed,
    /// <summary>
    /// 可升级
    /// </summary>
    Upgrade,

}