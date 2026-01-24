using ShadowObservableConfig.Attributes;

namespace ShadowViewer.Plugin.PluginManager.Configs;

[ObservableConfig(FileName = "plugin_manage_config")]
public partial class PluginManagerConfig
{
    /// <summary>
    /// 商店列表api
    /// </summary>
    [ObservableConfigProperty(Description = "商店列表api")]
    private string storeUri = "";

    /// <summary>
    /// github代理下载加速
    /// </summary>
    [ObservableConfigProperty(Description = "github代理下载加速")]
    private string? githubProxyUrl = "https://hk.gh-proxy.org/";

    /// <summary>
    /// 插件安全声明
    /// </summary>
    [ObservableConfigProperty(Description = "插件安全声明")]
    private bool pluginSecurityStatement;

    /// <summary>
    /// 插件安全声明版本号
    /// </summary> 
    [ObservableConfigProperty(Description = "插件安全声明版本号")]
    private string pluginSecurityStatementVersion = "0.1.0";
}