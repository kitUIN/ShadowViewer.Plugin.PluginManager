using Serilog;
using ShadowPluginLoader.WinUI.Helpers;
using ShadowViewer.Plugin.PluginManager.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
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
    protected PluginStoreHelper() : base()
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
        return await GetAsync<PluginPageResponse>(storeUri, new Dictionary<string, string>()
        {
            { "page", page.ToString() },
            { "limit", limit.ToString() }
        });
    }
}