using CommunityToolkit.Mvvm.ComponentModel;
using Serilog;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Plugin.PluginManager.Configs;
using ShadowViewer.Plugin.PluginManager.Helpers;
using ShadowViewer.Plugin.PluginManager.Models;
using ShadowViewer.Sdk;
using ShadowViewer.Sdk.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

/// <summary>
/// 
/// </summary>
public partial class PluginStoreViewModel : ObservableObject
{
    /// <summary>
    /// 插件列表
    /// </summary>
    public ObservableCollection<PluginStoreModel> Models { get; } = [];

    /// <summary>
    /// PluginService
    /// </summary>
    [Autowired]
    public PluginLoader PluginService { get; }

    /// <summary>
    /// PluginManagerConfig
    /// </summary>
    [Autowired]
    public PluginManagerConfig PluginManagerConfig { get; }

    /// <summary>
    /// Logger
    /// </summary>
    [Autowired]
    public ILogger Logger { get; }

    /// <summary>
    /// NotifyService
    /// </summary>
    [Autowired]
    public INotifyService NotifyService { get; }

    /// <summary>
    /// Store Page
    /// </summary>
    [ObservableProperty]
    public partial int Page { get; set; } = 1;

    /// <summary>
    /// 
    /// </summary>
    public async Task Refresh()
    {
        if (PluginManagerConfig.StoreUri.IsNullOrEmpty()) return;
        Models.Clear();
        var response = await PluginStoreHelper.Instance.GetPluginList(PluginManagerConfig.StoreUri, Page,
            githubProxyUrl: PluginManagerConfig.GithubProxyUrl);
        if (response == null || response.Items.Count == 0) return;
        foreach (var model in response.Items)
        {
            Models.Add(model);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    public async Task ChangeVersion(string id)
    {
        var selectModel = Models.FirstOrDefault(m => m.Id == id);
        if (selectModel?.Version == null) return;
        var model = await PluginStoreHelper.Instance.GetPluginVersion(PluginManagerConfig.StoreUri,
            selectModel.Id, selectModel.Version, githubProxyUrl: PluginManagerConfig.GithubProxyUrl);
        if (model == null) return;
        var index = Models.IndexOf(selectModel);
        if (index < 0) return;
        Models[index].Authors = model.Authors;
        Models[index].Dependencies = model.Dependencies;
        Models[index].Description = model.Description;
        Models[index].DownloadUrl = model.DownloadUrl;
        Models[index].LastUpdated = model.LastUpdated;
        Models[index].SdkVersion = model.SdkVersion;
        Models[index].Tags = model.Tags;
        Models[index].BackgroundColor = model.BackgroundColor;
        Models[index].WebUri = Models[index].WebUri;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    public async void Install(string uri)
    {
        // await PluginService.ScanAsync(uri);
        // await PluginService.Load();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="uri"></param>
    public async void Upgrade(string id, string uri)
    {
        await PluginService.UpgradePlugin(id, new Uri(uri));
    }

    /// <summary>
    /// 导航
    /// </summary>
    public async Task NavigateTo(Uri uri)
    {
        var splitUri = uri.AbsolutePath.Split(['/',], StringSplitOptions.RemoveEmptyEntries);
        Log.Information("{Uri}", splitUri);
        if (splitUri.Length != 4) return;
        if (splitUri[1] == "install")
        {
            var model = await PluginStoreHelper.Instance.SearchPlugin(PluginManagerConfig.StoreUri, splitUri[2],
                splitUri[3]);
            if (model == null) return;
            await model.InstallCommand.ExecuteAsync(null);
        }
    }
}