using ShadowPluginLoader.MetaAttributes;

namespace ShadowViewer.Plugin.PluginManager.Enums;
/// <summary>
/// Setting
/// </summary>
[ShadowPluginSettingClass(typeof(PluginManagerPlugin), "Setting")]
[ShadowSettingClass("ShadowViewer.Plugin.PluginManager", "PluginManagerSettings")]
public enum PluginManagerSetting
{
    /// <summary>
    /// Github代理域名
    /// </summary>
    [ShadowSetting(typeof(string), "https://mirror.ghproxy.com/", "Github代理域名")]
    GithubProxyDomain,

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
