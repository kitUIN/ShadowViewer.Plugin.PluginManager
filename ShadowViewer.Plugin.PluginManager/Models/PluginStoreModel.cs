using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Helpers;
using DryIoc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using NuGet.Versioning;
using Scriban;
using Serilog;
using ShadowPluginLoader.WinUI;
using ShadowPluginLoader.WinUI.Converters;
using ShadowPluginLoader.WinUI.Extensions;
using ShadowViewer.Plugin.PluginManager.Constants;
using ShadowViewer.Plugin.PluginManager.Enums;
using ShadowViewer.Plugin.PluginManager.I18n;
using ShadowViewer.Sdk;
using ShadowViewer.Sdk.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ShadowPluginLoader.WinUI.Enums;
using ShadowPluginLoader.WinUI.Models;
using Symbol = FluentIcons.Common.Symbol;

namespace ShadowViewer.Plugin.PluginManager.Models;

/// <summary>
/// 
/// </summary>
/// <seealso cref="CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
public class PluginStoreBaseModel : ObservableObject
{
    /// <summary>
    /// 获取依赖检查的图标
    /// </summary>
    public Symbol GetDependencyCheckSymbol(bool? status)
    {
        return status switch
        {
            true => Symbol.ShieldCheckmark,
            false => Symbol.ShieldProhibited,
            _ => Symbol.Shield
        };
    }

    /// <summary>
    /// 获取依赖检查的图标颜色
    /// </summary>
    public SolidColorBrush GetDependencyColorBrush(bool? status)
    {
        return status switch
        {
            true => new SolidColorBrush("#0f7b0f".ToColor()),
            false => new SolidColorBrush("#c42b1c".ToColor()),
            _ => new SolidColorBrush(Colors.Black)
        };
    }

    /// <summary>
    /// 获取依赖检查的图标
    /// </summary>
    public Symbol GetDependencySymbol(bool? status)
    {
        return status switch
        {
            true => Symbol.CheckmarkCircle,
            false => Symbol.DismissCircle,
            _ => Symbol.Circle
        };
    }
}

/// <summary>
/// 插件项，包含插件的详细信息。
/// </summary>
public partial class PluginStoreModel : PluginStoreBaseModel
{
    /// <summary>
    /// 插件唯一标识
    /// </summary>
    [JsonPropertyName("Id")]
    [ObservableProperty]
    public partial string Id { get; set; } = null!;

    /// <summary>
    /// 插件名称
    /// </summary>
    [JsonPropertyName("Name")]
    [ObservableProperty]
    public partial string Name { get; set; } = null!;

    /// <summary>
    /// 当前版本号
    /// </summary>
    [JsonPropertyName("Version")]
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(InstallButtonText))]
    public partial string Version { get; set; } = null!;

    /// <summary>
    /// 所有可用版本号
    /// </summary>
    [JsonPropertyName("Versions")]
    public ObservableCollection<string> Versions { get; set; } = [];

    /// <summary>
    /// 背景颜色（如用于UI展示）
    /// </summary>
    [JsonPropertyName("BackgroundColor")]
    [ObservableProperty]
    public partial string? BackgroundColor { get; set; }

    /// <summary>
    /// 插件标签
    /// </summary>
    [JsonPropertyName("Tags")]
    public ObservableCollection<string> Tags { get; set; } = [];

    /// <summary>
    /// 插件描述
    /// </summary>
    [JsonPropertyName("Description")]
    [ObservableProperty]
    public partial string? Description { get; set; }

    /// <summary>
    /// 作者信息
    /// </summary>
    [JsonPropertyName("Authors")]
    [ObservableProperty]
    public partial string? Authors { get; set; }

    /// <summary>
    /// 插件主页链接
    /// </summary>
    [JsonPropertyName("WebUri")]
    [ObservableProperty]
    public partial string? WebUri { get; set; }

    /// <summary>
    /// 插件Logo图片链接
    /// </summary>
    [JsonPropertyName("Logo")]
    [ObservableProperty]
    public partial string? Logo { get; set; }

    /// <summary>
    /// 依赖的SDK版本
    /// </summary>
    [JsonPropertyName("SdkVersion")]
    [ObservableProperty]
    [JsonConverter(typeof(VersionRangeJsonConverter))]
    public partial VersionRange SdkVersion { get; set; } = null!;

    /// <summary>
    /// 插件依赖项列表
    /// </summary>
    [JsonPropertyName("Dependencies")]
    public ObservableCollection<PluginItemDependency> Dependencies { get; set; } = [];

    /// <summary>
    /// 插件下载链接
    /// </summary>
    [JsonPropertyName("DownloadUrl")]
    [ObservableProperty]
    public partial string? DownloadUrl { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    [JsonPropertyName("LastUpdated")]
    [ObservableProperty]
    public partial DateTime LastUpdated { get; set; }


    /// <summary>
    /// 安装状态说明
    /// </summary> 
    [ObservableProperty]
    [JsonIgnore]
    [NotifyPropertyChangedFor(nameof(InstallButtonText))]
    [NotifyPropertyChangedFor(nameof(InstallButtonEnabled))]
    [NotifyCanExecuteChangedFor(nameof(InstallCommand))]
    public partial PluginInstallStatus InstallStatus { get; set; }

    /// <summary>
    /// 安装按钮文本
    /// </summary> 
    [JsonIgnore]
    public string InstallButtonText => InstallStatus switch
    {
        PluginInstallStatus.Upgrade => I18N.UpgradeAction,
        PluginInstallStatus.Downgrade => I18N.DowngradeAction,
        PluginInstallStatus.Installed => I18N.Installed,
        _ => I18N.Install
    } + " " + Version;

    /// <summary>
    /// 安装按钮是否启用
    /// </summary> 
    [JsonIgnore]
    public bool InstallButtonEnabled => InstallStatus != PluginInstallStatus.Installed;

    /// <summary>
    /// 已经安装的插件版本
    /// </summary>
    [ObservableProperty]
    [JsonIgnore]
    public partial NuGetVersion? InstalledVersion { get; set; }

    /// <summary>
    /// Sdk版本检查状态
    /// </summary>
    [JsonIgnore]
    public bool? SdkVersionStatus => SdkVersion.Satisfies(PluginConstants.SdkVersion);

    /// <summary>
    /// 依赖检查状态
    /// </summary> 
    [JsonIgnore]
    [ObservableProperty]
    public partial bool? DependencyCheckStatus { get; set; }

    partial void OnIdChanged(string oldValue, string newValue)
    {
        if (oldValue == newValue) return;
        InstalledVersion = DiFactory.Services.Resolve<PluginLoader>().GetPlugin(Id)?.MetaData.Version;
    }

    partial void OnVersionChanged(string oldValue, string newValue)
    {
        if (oldValue == newValue) return;
        CheckVersion();
    }

    /// <summary>
    /// Checks the version.
    /// </summary>
    public void CheckVersion()
    {
        InstalledVersion ??= DiFactory.Services.Resolve<PluginLoader>().GetPlugin(Id)?.MetaData.Version;
        if (InstalledVersion == null)
        {
            InstallStatus = PluginInstallStatus.Install;
            return;
        }

        var pluginStoreVersion = new NuGetVersion(Version);
        if (pluginStoreVersion > InstalledVersion)
        {
            InstallStatus = PluginInstallStatus.Upgrade;
        }
        else if (pluginStoreVersion == InstalledVersion)
        {
            InstallStatus = PluginInstallStatus.Installed;
        }
        else
        {
            InstallStatus = PluginInstallStatus.Downgrade;
        }
    }

    [RelayCommand]
    private void CheckDependencies()
    {
        var pluginManager = DiFactory.Services.Resolve<PluginLoader>();
        Dictionary<string, NuGetVersion> installedPlugins = new();
        foreach (var plugin in pluginManager.GetPlugins())
        {
            installedPlugins[plugin.MetaData.Id] = plugin.MetaData.Version;
        }

        var flag = true;
        foreach (var dependency in Dependencies)
        {
            if (installedPlugins.TryGetValue(dependency.Id, out var version))
            {
                dependency.Installed = version;
                dependency.Status = dependency.Need.Satisfies(version);
            }
            else
            {
                flag = false;
            }
        }

        DependencyCheckStatus = flag;
    }

    private async Task ShowInstallContentDialog(Func<IProgress<PipelineProgress>, Task> installAction)
    {
        var content = await Template.Parse(I18N.InstallTextTemplate)
            .RenderAsync(new { action = I18N.Install, name = Id, version = Version });
        var dialog = XamlHelper.CreateMessageDialog(I18N.Install, null, null);

        #region TaskProgressGrid

        var taskProgressGrid = new Grid()
        {
            Width = 320,
            ColumnSpacing = 8,
            Visibility = Visibility.Collapsed,
            Padding = new Thickness(0, 4, 0, 4)
        };
        taskProgressGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
        taskProgressGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
        taskProgressGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

        var taskProgressValue = new TextBlock() { Text = "00.00%", VerticalAlignment = VerticalAlignment.Center };
        var taskProgress = new ProgressBar()
        {
            Minimum = 0D,
            Maximum = 1D,
            Width = 200,
            VerticalAlignment = VerticalAlignment.Center,
            Height = 8
        };
        var taskProgressStatus = new TextBlock() { Text = nameof(InstallPipelineStep.Feeding), VerticalAlignment = VerticalAlignment.Center };

        Grid.SetColumn(taskProgressValue, 0);
        Grid.SetColumn(taskProgress, 1);
        Grid.SetColumn(taskProgressStatus, 2);

        taskProgressGrid.Children.Add(taskProgressValue);
        taskProgressGrid.Children.Add(taskProgress);
        taskProgressGrid.Children.Add(taskProgressStatus);

        #endregion

        #region SubTaskProgressGrid

        var subTaskGrid = new Grid()
        {
            Width = 320,
            Visibility = Visibility.Collapsed,
            Padding = new Thickness(20, 4, 0, 4),
            ColumnSpacing = 8
        };
        subTaskGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
        subTaskGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
        subTaskGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

        var subTaskProgressValue = new TextBlock()
        {
            Text = "00.00%",
            VerticalAlignment = VerticalAlignment.Center
        };

        var subTaskProgress = new ProgressBar()
        {
            Minimum = 0D,
            Maximum = 1D,
            Width = 180,
            VerticalAlignment = VerticalAlignment.Center,
            Height = 8
        };

        var subTaskStatus = new TextBlock()
        {
            Text = "--",
            VerticalAlignment = VerticalAlignment.Center
        };

        Grid.SetColumn(subTaskProgressValue, 0);
        Grid.SetColumn(subTaskProgress, 1);
        Grid.SetColumn(subTaskStatus, 2);

        subTaskGrid.Children.Add(subTaskProgressValue);
        subTaskGrid.Children.Add(subTaskProgress);
        subTaskGrid.Children.Add(subTaskStatus);

        #endregion

        dialog.Content = new StackPanel()
        {
            Orientation = Orientation.Vertical,
            Spacing = 4,
            Children =
            {
                new TextBlock()
                {
                    Text = content
                },
                taskProgressGrid,
                subTaskGrid
            }
        };
        var progress = new Progress<PipelineProgress>(pipelineProgress =>
        {
            taskProgress.Value = pipelineProgress.TotalPercentage;
            taskProgressValue.Text = pipelineProgress.TotalPercentage.ToString("00.00%");
            taskProgressStatus.Text = pipelineProgress.Step.ToString();

            subTaskProgress.Value = pipelineProgress.SubPercentage;
            subTaskProgressValue.Text = pipelineProgress.SubPercentage.ToString("00.00%");
            subTaskStatus.Text = pipelineProgress.SubStep.ToString();
            if (Math.Abs(pipelineProgress.TotalPercentage - 1D) < 0.001D)
            {
                subTaskGrid.Visibility = Visibility.Collapsed;
            }
        });

        dialog.PrimaryButtonText = Sdk.I18n.I18N.Confirm;
        dialog.IsPrimaryButtonEnabled = true;
        dialog.PrimaryButtonClick +=
            async void (sender, args) =>
            {
                try
                {
                    args.Cancel = true;
                    sender.PrimaryButtonText = I18N.Installing;
                    taskProgressGrid.Visibility = Visibility.Visible;
                    subTaskGrid.Visibility = Visibility.Visible;
                    await installAction.Invoke(progress);
                    sender.Hide();
                    CheckVersion();
                }
                catch (Exception e)
                {
                    Log.Error(e, "Catch Error in InstallAsync");
                }
            };
        await DialogHelper.ShowDialog(dialog);
    }

    [RelayCommand(CanExecute = nameof(InstallButtonEnabled))]
    private async Task Install()
    {
        var pluginManager = DiFactory.Services.Resolve<PluginLoader>();

        if (DownloadUrl == null) return;
        if (InstallStatus == PluginInstallStatus.Install)
        {
            await ShowInstallContentDialog(InstallPluginFronUrl);
        }
        else if (InstallStatus is PluginInstallStatus.Upgrade or PluginInstallStatus.Downgrade)
        {
            var action = InstallStatus == PluginInstallStatus.Upgrade ? I18N.Upgrade : I18N.Downgrade;
            await DialogHelper.ShowDialog(XamlHelper.CreateMessageDialog(action,
                await Template.Parse(I18N.UpgradeTextTemplate)
                    .RenderAsync(new
                    {
                        action = action,
                        name = Id,
                        newVersion = Version,
                        oldVersion = InstalledVersion?.ToString()
                    }),
                async void (sender, args) =>
                {
                    try
                    {
                        args.Cancel = true;
                        sender.PrimaryButtonText = I18N.Installing;
                        await InstallPluginFronUrl();
                        sender.Hide();
                        CheckVersion();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e, "Catch Error in InstallAsync");
                    }
                }
            ));
        }

        return;

        async Task InstallPluginFronUrl(IProgress<PipelineProgress>? progress = null)
        {
            var pipeline = pluginManager.CreatePipeline();
            await pipeline.Feed(new Uri(DownloadUrl!)).ProcessAsync(progress);
        }
    }
}

/// <summary>
/// 插件依赖项，描述依赖的插件及需求。
/// </summary>
public partial class PluginItemDependency : PluginStoreBaseModel
{
    /// <summary>
    /// 依赖插件的唯一标识
    /// </summary>
    [JsonPropertyName("Id")]
    [ObservableProperty]
    public partial string Id { get; set; } = null!;

    /// <summary>
    /// 依赖需求描述
    /// </summary>
    [JsonPropertyName("Need")]
    [ObservableProperty]
    [JsonConverter(typeof(VersionRangeJsonConverter))]
    public partial VersionRange Need { get; set; } = null!;

    /// <summary>
    /// 已经安装依赖版本
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial NuGetVersion? Installed { get; set; }


    /// <summary>
    /// 依赖是否符合条件
    /// </summary>
    [JsonIgnore]
    [ObservableProperty]
    public partial bool Status { get; set; }
}