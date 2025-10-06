using ShadowObservableConfig.Attributes;

namespace ShadowViewer.Plugin.PluginManager.Configs;

[ObservableConfig(FileName = "plugin_manage_config")]
public partial class PluginManagerConfig
{
    /// <summary>
    /// 商店列表网址
    /// </summary>
    [ObservableConfigProperty(Description = "商店列表网址")]
    private string storeUri =
        "https://raw.githubusercontent.com/kitUIN/ShadowViewer.PluginStore/refs/heads/main/plugin.json";

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