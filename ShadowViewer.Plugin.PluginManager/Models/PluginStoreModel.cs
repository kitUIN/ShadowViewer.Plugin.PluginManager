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
using ShadowViewer.Sdk.Enums;
using ShadowViewer.Sdk.Services;
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


    /// <summary>
    /// Builds the content of the progress.
    /// </summary>
    private UIElement BuildProgressContent()
    {
        var panel = new StackPanel
        {
            Spacing = 4,
            Padding = new Thickness(0, 0, 0, 20),
            Height = 70,
            Width = 340,
        };

        // 主任务
        var taskGrid = new Grid { ColumnSpacing = 8 };
        taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
        taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        taskGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        taskGrid.Children.Add(new TextBlock { Name = "TaskProgressValue", Text = "00.00%" });
        taskGrid.Children.Add(new ProgressBar
        { Name = "TaskProgress", Minimum = 0, Maximum = 1, Width = 160, Height = 8 });
        taskGrid.Children.Add(new TextBlock
        { Name = "TaskProgressStatus", Text = nameof(InstallPipelineStep.Feeding) });

        Grid.SetColumn(taskGrid.Children[0] as FrameworkElement, 0);
        Grid.SetColumn(taskGrid.Children[1] as FrameworkElement, 1);
        Grid.SetColumn(taskGrid.Children[2] as FrameworkElement, 2);

        // 子任务
        var subGrid = new Grid { Name = "SubTaskGrid", ColumnSpacing = 8, Margin = new Thickness(20, 0, 0, 0) };
        subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(60) });
        subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        subGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

        subGrid.Children.Add(new TextBlock { Name = "SubTaskProgressValue", Text = "00.00%" });
        subGrid.Children.Add(new ProgressBar
        { Name = "SubTaskProgress", Minimum = 0, Maximum = 1, Width = 140, Height = 8 });
        subGrid.Children.Add(new TextBlock { Name = "SubTaskStatus", Text = "----" });

        Grid.SetColumn(subGrid.Children[0] as FrameworkElement, 0);
        Grid.SetColumn(subGrid.Children[1] as FrameworkElement, 1);
        Grid.SetColumn(subGrid.Children[2] as FrameworkElement, 2);

        panel.Children.Add(taskGrid);
        panel.Children.Add(subGrid);

        return panel;
    }
    /// <summary>
    /// Notifies the information bar.
    /// </summary>
    private async Task NotifyInfoBar(Func<IProgress<PipelineProgress>, Task> installAction)
    {
        var infoBar = new InfoBar
        {
            Title = await Template.Parse(I18N.InstallingTextTemplate)
                .RenderAsync(new { action = I18N.Install, name = Id, version = Version }),
            IsOpen = true,
            Severity = InfoBarSeverity.Informational,
            IsClosable = false,
            Content = BuildProgressContent()
        };
        infoBar.Loaded += async (_, _) => { await RunInstallPipelineAsync(infoBar, installAction); };
        var content = await Template.Parse(I18N.InstallTextTemplate)
            .RenderAsync(new { action = I18N.Install, name = Id, version = Version });
        await DialogHelper.ShowDialog(XamlHelper.CreateMessageDialog(I18N.Install, content,
            async void (sender, args) =>
            {
                try
                {
                    args.Cancel = true;
                    sender.PrimaryButtonText = I18N.Installing;
                    DiFactory.Services.Resolve<INotifyService>().NotifyTip(this, infoBar, 0D, TipPopupPosition.Right);
                    sender.Hide();
                }
                catch (Exception e)
                {
                    Log.Error(e, "Catch Error in InstallAsync");
                }
            }
        ));
    }
    /// <summary>
    /// Runs the install pipeline asynchronous.
    /// </summary>
    private async Task RunInstallPipelineAsync(InfoBar infoBar, Func<IProgress<PipelineProgress>, Task> installAction)
    {
        // 获取 InfoBar 内部控件
        var taskProgress = infoBar.FindName("TaskProgress") as ProgressBar;
        var taskProgressValue = infoBar.FindName("TaskProgressValue") as TextBlock;
        var taskProgressStatus = infoBar.FindName("TaskProgressStatus") as TextBlock;

        var subTaskGrid = infoBar.FindName("SubTaskGrid") as Grid;
        var subTaskProgress = infoBar.FindName("SubTaskProgress") as ProgressBar;
        var subTaskProgressValue = infoBar.FindName("SubTaskProgressValue") as TextBlock;
        var subTaskStatus = infoBar.FindName("SubTaskStatus") as TextBlock;

        var progress = new Progress<PipelineProgress>(p =>
        {
            taskProgress?.Value = p.TotalPercentage;
            taskProgressValue?.Text = p.TotalPercentage.ToString("00.00%");
            taskProgressStatus?.Text = p.Step.ToString();

            subTaskProgress?.Value = p.SubPercentage;
            subTaskProgressValue?.Text = p.SubPercentage.ToString("00.00%");
            subTaskStatus?.Text = p.SubStep.ToString();
        });

        try
        {
            // ⭐ 在 InfoBar Loaded 后执行安装
            await installAction.Invoke(progress);
            subTaskGrid?.Visibility = Visibility.Collapsed;
            // ⭐ 安装成功 → 切换 InfoBar 状态
            infoBar.Severity = InfoBarSeverity.Success;
            infoBar.Title = await Template.Parse(I18N.InstallSuccessTextTemplate)
                .RenderAsync(new { name = Id, version = Version });

            await Task.Delay(2000);

            infoBar.IsOpen = false;

            CheckVersion();
        }
        catch (Exception ex)
        {
            infoBar.Severity = InfoBarSeverity.Error;
            infoBar.Title = await Template.Parse(I18N.InstallErrorTextTemplate)
                .RenderAsync(new { name = Id, version = Version });
            ;
            infoBar.Message = ex.Message;
        }
    }


    [RelayCommand(CanExecute = nameof(InstallButtonEnabled))]
    private async Task Install()
    {
        var pluginManager = DiFactory.Services.Resolve<PluginLoader>();

        if (DownloadUrl == null) return;
        if (InstallStatus == PluginInstallStatus.Install)
        {
            await NotifyInfoBar(InstallPluginFronUrl);
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