using ShadowViewer.Plugin.PluginManager.Models;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShadowViewer.Plugin.PluginManager.Responses;

/// <summary>
/// 插件分页响应，包含插件列表及分页信息。
/// </summary>
public class PluginPageResponse
{
    /// <summary>
    /// 插件总数
    /// </summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>
    /// 当前页码
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    /// 每页数量限制
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    [JsonPropertyName("pages")]
    public int Pages { get; set; }

    /// <summary>
    /// 当前页的插件项列表
    /// </summary>
    [JsonPropertyName("items")]
    public List<PluginStoreModel> Items { get; set; } = [];
}
