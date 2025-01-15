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
}
