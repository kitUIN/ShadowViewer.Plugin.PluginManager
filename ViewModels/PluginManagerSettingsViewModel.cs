using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Helpers;
using FluentIcons.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

public partial class PluginManagerSettingsViewModel : ObservableObject
{
    /// <summary>
    /// <inheritdoc cref="Enums.PluginManagerSettings.PluginSecurityStatement"/>
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementColor))]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementVisible))]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementText))]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementIcon))]
    private bool pluginSecurityStatement = PluginManagerPlugin.Setting.PluginSecurityStatement;

    /// <summary>
    /// <inheritdoc cref="Enums.PluginManagerSettings.PluginSecurityStatementVersion"/>
    /// </summary>
    [ObservableProperty]
    private string pluginSecurityStatementVersion = PluginManagerPlugin.Setting.PluginSecurityStatementVersion;

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
    /// <inheritdoc cref="Enums.PluginManagerSettings.PluginSecurityStatement"/>
    /// </summary>
    [ObservableProperty] private bool pluginSecurityCheck = PluginManagerPlugin.Setting.PluginSecurityStatement;

    /// <summary>
    /// 同意安全声明
    /// </summary>
    [RelayCommand]
    private void SecurityConfirm()
    {
        PluginManagerPlugin.Setting.PluginSecurityStatement = PluginSecurityCheck;
        if (PluginSecurityCheck) PluginSecurityStatement = PluginSecurityCheck;
    }

    /// <summary>
    /// <inheritdoc cref="Enums.PluginManagerSettings.GithubMirror"/>
    /// </summary>
    [ObservableProperty] private string githubMirror = PluginManagerPlugin.Setting.GithubMirror;

    /// <summary>
    /// <inheritdoc cref="Enums.PluginManagerSettings.StoreUri"/>
    /// </summary>
    [ObservableProperty] private string storeUri = PluginManagerPlugin.Setting.StoreUri;
}