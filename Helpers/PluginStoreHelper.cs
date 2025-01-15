using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using ShadowViewer.Plugin.PluginManager.Models;
using ShadowViewer.Plugin.PluginManager.Responses;
using ShadowViewer.Plugins;

namespace ShadowViewer.Plugin.PluginManager.Helpers;

/// <summary>
/// PluginStoreHelper
/// </summary>
public class PluginStoreHelper
{
    /// <summary>
    /// 
    /// </summary>
    public static PluginStoreHelper Instance { get; } = new();
    /// <summary>
    /// 
    /// </summary>
    private readonly HttpClient client = new(new HttpClientHandler());
    /// <summary>
    /// 创建HttpRequestMessage
    /// </summary>
    /// <param name="method"></param>
    /// <param name="url"></param>
    /// <param name="headers"></param>
    /// <returns></returns>
    private HttpRequestMessage CreateRequestMessage(HttpMethod method, string url, Dictionary<string, string>? headers = null)
    {
        var reqUrl = url;
        if (!string.IsNullOrEmpty(PluginManagerPlugin.Setting.GithubProxyDomain))
        {
            reqUrl = PluginManagerPlugin.Setting.GithubProxyDomain + reqUrl;
        }
        var httpRequestMessage = new HttpRequestMessage(method, reqUrl);
        if (headers == null) return httpRequestMessage;
        foreach (var header in headers)
        {
            httpRequestMessage.Headers.Add(header.Key, header.Value);
        }
        return httpRequestMessage;
    }

    /// <summary>
    /// Get Async
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <param name="api">Pica API</param>
    /// <returns></returns>
    private async Task<T> GetAsync<T>(string url) 
    {
        try
        {
            using var request = CreateRequestMessage(HttpMethod.Get, url);
            using var response = await client.SendAsync(request);
            var resp = await response.Content.ReadAsStringAsync();
            Log.Debug("\n[GET]{Api}:\nproxy:{Proxy}\nreturn:{Resp}", url,
                PluginManagerPlugin.Setting.GithubProxyDomain, resp);
            var res = JsonSerializer.Deserialize<T>(resp);

            return res;
        }
        catch (Exception exception)
        {
            Log.Error("Get Exception: {Ex}", exception);
            throw;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public async Task<PluginItem[]> GetPluginList(int page=1)
    {
        return await GetAsync<PluginItem[]>($"https://raw.githubusercontent.com/kitUIN/ShadowViewer.PluginStore/refs/heads/main/plugin.json");
    }
}

