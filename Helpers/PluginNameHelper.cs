namespace ShadowViewer.Plugin.PluginManager.Helpers;

/// <summary>
/// 
/// </summary>
public static class PluginNameHelper
{
    /// <summary>
    /// GetPluginId in manager page
    /// </summary>
    /// <param name="rawName"></param>
    /// <returns></returns>
    public static string GetPluginId(string rawName)
    {
        return rawName.Replace("ShadowViewer.Plugins.", "");
    }
}