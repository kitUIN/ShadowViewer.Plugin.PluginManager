using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Helpers;
using FluentIcons.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using ShadowPluginLoader.Attributes;
using ShadowViewer.Plugin.PluginManager.Configs;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

public partial class PluginManagerSettingsViewModel : ObservableObject
{
    /// <summary>
    /// 
    /// </summary>
    [Autowired]
    public PluginManagerConfig PluginManagerConfig { get; }

    /// <summary>
    /// <inheritdoc cref="PluginManagerConfig.PluginSecurityStatement"/>
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementColor))]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementVisible))]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementText))]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementIcon))]
    public partial bool PluginSecurityStatement { get; set; }


    /// <summary>
    /// 安全声明图标
    /// </summary>
    public Icon PluginSecurityStatementIcon =>
        PluginSecurityStatement ? Icon.ThumbLike : Icon.ShieldError;

    /// <summary>
    /// 安全声明颜色
    /// </summary>
    public SolidColorBrush PluginSecurityStatementColor =>
        PluginSecurityStatement ? new SolidColorBrush("#0f7b0f".ToColor()) : new SolidColorBrush("#C42B1C".ToColor());

    /// <summary>
    /// 安全声明按钮可见
    /// </summary>
    public Visibility PluginSecurityStatementVisible =>
        PluginSecurityStatement ? Visibility.Collapsed : Visibility.Visible;

    /// <summary>
    /// 安全声明按钮可见
    /// </summary>
    public string PluginSecurityStatementText =>
        PluginSecurityStatement ? I18n.I18N.Agree : I18n.I18N.Refuse;

    /// <summary>
    /// <inheritdoc cref="PluginManagerConfig.PluginSecurityStatement"/>
    /// </summary>
    [ObservableProperty] public partial bool PluginSecurityCheck { get; set; }


    partial void ConstructorInit()
    {
        PluginSecurityCheck = PluginManagerConfig.PluginSecurityStatement;
        PluginSecurityStatement = PluginManagerConfig.PluginSecurityStatement;
    }

    /// <summary>
    /// 同意安全声明
    /// </summary>
    [RelayCommand]
    private void SecurityConfirm()
    {
        PluginManagerConfig.PluginSecurityStatement = PluginSecurityCheck;
        if (PluginSecurityCheck) PluginSecurityStatement = PluginSecurityCheck;
    }
}