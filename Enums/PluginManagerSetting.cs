using ShadowPluginLoader.Attributes;

namespace ShadowViewer.Plugin.PluginManager.Enums;
/// <summary>
/// Setting
/// </summary>
[ShadowSettingClass(Container = "ShadowViewer.Plugin.PluginManager",ClassName = "PluginManagerSettings")]
public enum PluginManagerSetting
{
    /// <summary>
    /// Github代理域名
    /// </summary>
    [ShadowSetting(typeof(string), "https://mirror.ghproxy.com/", "Github代理域名")]
    GithubMirror,
    
    /// <summary>
    /// 商店列表网址
    /// </summary>
    [ShadowSetting(typeof(string), "https://raw.githubusercontent.com/kitUIN/ShadowViewer.PluginStore/refs/heads/main/plugin.json", "商店列表网址")]
    StoreUri,

    /// <summary>
    /// 插件安全声明
    /// </summary>
    [ShadowSetting(typeof(bool), "false", "插件安全声明")]
    PluginSecurityStatement,
    
    /// <summary>
    /// 插件安全声明版本号
    /// </summary>
    [ShadowSetting(typeof(string), "0.1.0", "插件安全声明版本号")]
    PluginSecurityStatementVersion,
}
