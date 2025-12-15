using ShadowPluginLoader.WinUI.Helpers;
using ShadowViewer.Plugin.PluginManager.Models;
using ShadowViewer.Plugin.PluginManager.Responses;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace ShadowViewer.Plugin.PluginManager.Helpers;

/// <summary>
/// PluginStoreHelper
/// </summary>
public class PluginStoreHelper : BaseHttpHelper
{
    /// <summary>
    /// PluginStoreHelper
    /// </summary>
    protected PluginStoreHelper()
    {
    }

    /// <summary>
    /// Lazy, thread-safe singleton instance
    /// </summary>
    private static readonly Lazy<PluginStoreHelper> InnerInstance =
        new(() => new PluginStoreHelper(), LazyThreadSafetyMode.ExecutionAndPublication);

    /// <summary>
    /// 获取单例实例（线程安全、惰性初始化）
    /// </summary>
    public new static PluginStoreHelper Instance => InnerInstance.Value;

    /// <summary>
    /// 获取插件列表
    /// </summary>
    public async Task<PluginPageResponse?> GetPluginList(string storeUri, int page = 1, int limit = 20)
    {
        return await GetAsync<PluginPageResponse>(storeUri + "/store/plugins", new Dictionary<string, string>()
        {
            { "page", page.ToString() },
            { "limit", limit.ToString() }
        });
    }

    /// <summary>
    /// Searches the plugin.
    /// </summary>
    /// <param name="storeUri">The store URI.</param>
    /// <param name="pluginId">The plugin identifier.</param>
    /// <param name="version">The version.</param>
    /// <returns></returns>
    public async Task<PluginStoreModel?> SearchPlugin(string storeUri, string pluginId, string version)
    {
        return await PostJsonAsync<SearchPluginRequest, PluginStoreModel>(
            storeUri + "/store/plugins/version",
            new SearchPluginRequest(pluginId, version));
    }
}

/// <summary>
/// 
/// </summary>
public class SearchPluginRequest
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("plugin_id")]
    public string PluginId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("version")]
    public string Version { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pluginId"></param>
    /// <param name="version"></param>
    public SearchPluginRequest(string pluginId, string version)
    {
        PluginId = pluginId;
        Version = version;
    }
}