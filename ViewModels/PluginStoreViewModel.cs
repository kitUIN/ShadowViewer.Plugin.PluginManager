using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShadowViewer.Plugin.PluginManager.Helpers;
using ShadowViewer.Plugin.PluginManager.Models;
using ShadowViewer.Core;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

/// <summary>
/// 
/// </summary>
public class PluginStoreViewModel : ObservableObject
{
    /// <summary>
    /// 插件列表
    /// </summary>
    public ObservableCollection<PluginStoreModel> Models { get; } = [];

    /// <summary>
    /// PluginService
    /// </summary>
    public PluginLoader PluginService { get; }

    /// <summary>
    /// 
    /// </summary>
    public PluginStoreViewModel(PluginLoader pluginLoader)
    {
        PluginService = pluginLoader;
        Init();
    }

    /// <summary>
    /// 
    /// </summary>
    public async void Init()
    {
        Models.Clear();
        var pluginList = await PluginStoreHelper.Instance.GetPluginList();
        if (pluginList == null) return;
        foreach (var meta in pluginList)
        {
            var model = new PluginStoreModel(meta);
            Models.Add(model);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uri"></param>
    public async void Install(string uri)
    {
        await PluginService.ImportAsync(uri);
        await PluginService.Load();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="uri"></param>
    public async void Upgrade(string id, string uri)
    {
        await PluginService.UpgradePlugin(id, uri);
    }
}