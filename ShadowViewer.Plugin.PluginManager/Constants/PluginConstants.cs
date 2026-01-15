using NuGet.Versioning;
using ShadowViewer.Sdk.Plugins;

namespace ShadowViewer.Plugin.PluginManager.Constants;


internal static class PluginConstants
{
    static PluginConstants()
    {
        SdkVersion = NuGetVersion.Parse(typeof(AShadowViewerPlugin).Assembly.GetName().Version!.ToString());
    }

    public static NuGetVersion SdkVersion { get; }
    public const string PluginId = "ShadowViewer.Plugin.PluginManager";
}
