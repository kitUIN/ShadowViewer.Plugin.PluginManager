using Serilog;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using FluentIcon.WinUI;

namespace ShadowViewer.Plugin.PluginManager.Responses;
/// <summary>
/// 
/// </summary>
public class AssetsItem
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("content_type")]
    public string ContentType { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("size")]
    public long Size { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("browser_download_url")]
    public string BrowserDownloadUrl { get; set; }

    /// <summary>
    /// 是否是Zip
    /// </summary>
    [JsonIgnore()]
    public FluentFilledIconSymbol IsZip => ContentType == "application/x-zip-compressed"? FluentFilledIconSymbol.FolderZip16Filled : FluentFilledIconSymbol.Document10016Filled;
}

/// <summary>
/// 
/// </summary> 
public class PluginItem
{
    /// <summary>
    /// 
    /// </summary>
    public string DllName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Authors { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string WebUri { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Logo { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string Version { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<string> Dependencies { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<AssetsItem> Assets { get; set; }
}