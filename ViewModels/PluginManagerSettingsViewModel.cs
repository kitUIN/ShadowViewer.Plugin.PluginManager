using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Helpers;
using FluentIcons.Common;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using ShadowViewer.Plugin.PluginManager.Settings;

namespace ShadowViewer.Plugin.PluginManager.ViewModels;

public partial class PluginManagerSettingsViewModel : ObservableObject
{
    /// <summary>
    /// <inheritdoc cref="PluginManagerSettings.PluginSecurityStatement"/>
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementColor))]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementVisible))]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementText))]
    [NotifyPropertyChangedFor(nameof(PluginSecurityStatementIcon))]
    private bool pluginSecurityStatement = PluginManagerPlugin.Settings.PluginSecurityStatement;

    /// <summary>
    /// <inheritdoc cref="PluginManagerSettings.PluginSecurityStatementVersion"/>
    /// </summary>
    [ObservableProperty]
    private string pluginSecurityStatementVersion = PluginManagerPlugin.Settings.PluginSecurityStatementVersion;

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
    /// <inheritdoc cref="PluginManagerSettings.PluginSecurityStatement"/>
    /// </summary>
    [ObservableProperty] private bool pluginSecurityCheck = PluginManagerPlugin.Settings.PluginSecurityStatement;

    /// <summary>
    /// 同意安全声明
    /// </summary>
    [RelayCommand]
    private void SecurityConfirm()
    {
        PluginManagerPlugin.Settings.PluginSecurityStatement = PluginSecurityCheck;
        if (PluginSecurityCheck) PluginSecurityStatement = PluginSecurityCheck;
    }

    /// <summary>
    /// <inheritdoc cref="PluginManagerSettings.GithubMirror"/>
    /// </summary>
    [ObservableProperty] private string githubMirror = PluginManagerPlugin.Settings.GithubMirror;

    /// <summary>
    /// <inheritdoc cref="PluginManagerSettings.StoreUri"/>
    /// </summary>
    [ObservableProperty] private string storeUri = PluginManagerPlugin.Settings.StoreUri;
}