using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using ShadowViewer.Plugin.PluginManager.Responses;

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
    private static HttpRequestMessage CreateRequestMessage(HttpMethod method, string url,
        Dictionary<string, string>? headers = null)
    {
        var reqUrl = url;
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
    private async Task<T?> GetAsync<T>(string url)
    {
        try
        {
            using var request = CreateRequestMessage(HttpMethod.Get, url);
            using var response = await client.SendAsync(request);
            var resp = await response.Content.ReadAsStringAsync();
            Log.Debug("\n[GET]{Api}:\n\nreturn:{Resp}", url, resp);
            return JsonSerializer.Deserialize<T>(resp);
        }
        catch (Exception exception)
        {
            Log.Error("Get Exception: {Ex}", exception);
            throw;
        }
    }

    /// <summary>
    /// 获取插件列表
    /// </summary>
    public async Task<PluginItem[]?> GetPluginList(string storeUri, int page = 1)
    {
        return await GetAsync<PluginItem[]>(storeUri);
    }
}