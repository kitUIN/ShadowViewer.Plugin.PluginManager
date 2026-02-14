using Azure;
using Microsoft.IdentityModel.Tokens;
using ShadowPluginLoader.WinUI.Helpers;
using ShadowViewer.Plugin.PluginManager.Configs;
using ShadowViewer.Plugin.PluginManager.Models;
using ShadowViewer.Plugin.PluginManager.Requests;
using ShadowViewer.Plugin.PluginManager.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public async Task<PluginPageResponse?> GetPluginList(string storeUri, int page = 1, int limit = 20,
        string? githubProxyUrl = null)
    {
        var response = await GetAsync<PluginPageResponse>(storeUri + "/store/plugins", new Dictionary<string, string>()
        {
            { "page", page.ToString() },
            { "limit", limit.ToString() }
        });
        if (response == null || response.Items.Count == 0) return null;
        foreach (var model in response.Items.Where(_ => !githubProxyUrl.IsNullOrEmpty()))
        {
            CheckModel(model, githubProxyUrl!);
        }

        return response;
    }

    /// <summary>
    /// Gets the plugin version.
    /// </summary>
    /// <param name="storeUri">The store URI.</param>
    /// <param name="pluginId">The plugin identifier.</param>
    /// <param name="version">The version.</param>
    /// <param name="githubProxyUrl">The github proxy URL.</param>
    /// <returns></returns>
    public async Task<PluginStoreModel?> GetPluginVersion(string storeUri, string pluginId, string version,
        string? githubProxyUrl = null)
    {
        var response = await PostJsonAsync<PluginVersionReq, PluginStoreModel>(
            storeUri + "/store/plugins/version", new PluginVersionReq(pluginId, version));
        if (response == null) return null;
        CheckModel(response, githubProxyUrl!);
        return response;
    }

    /// <summary>
    /// Checks the model.
    /// </summary>
    /// <param name="model">The model.</param>
    /// <param name="githubProxyUrl">The github proxy URL.</param>
    private void CheckModel(PluginStoreModel model, string githubProxyUrl)
    {
        if (model.Logo?.StartsWith("https://raw.githubusercontent.com") == true)
        {
            model.Logo = githubProxyUrl + model.Logo;
        }

        if (model.DownloadUrl?.StartsWith("https://github.com") == true)
        {
            model.DownloadUrl = githubProxyUrl + model.DownloadUrl;
        }
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