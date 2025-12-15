using CommunityToolkit.Mvvm.ComponentModel;
using Serilog;
using Serilog.Core;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Plugin.PluginManager.Configs;
using ShadowViewer.Plugin.PluginManager.Helpers;
using ShadowViewer.Plugin.PluginManager.Models;
using ShadowViewer.Sdk;
using ShadowViewer.Sdk.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
        Models.Clear();
        var response = await PluginStoreHelper.Instance.GetPluginList(PluginManagerConfig.StoreUri, Page);
        if (response == null || response.Items.Count == 0) return;
        foreach (var model in response.Items)
        {
            if (!PluginManagerConfig.GithubProxyUrl.IsNullOrEmpty())
            {
                if (model.Logo?.StartsWith("https://raw.githubusercontent.com") == true)
                {
                    model.Logo = PluginManagerConfig.GithubProxyUrl + model.Logo;
                }

                if (model.DownloadUrl?.StartsWith("https://github.com") == true)
                {
                    model.DownloadUrl = PluginManagerConfig.GithubProxyUrl + model.DownloadUrl;
                }
            }

            Models.Add(model);
        }
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