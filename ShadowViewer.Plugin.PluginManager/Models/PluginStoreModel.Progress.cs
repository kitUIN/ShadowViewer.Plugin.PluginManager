using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Scriban;
using ShadowPluginLoader.WinUI.Enums;
using ShadowPluginLoader.WinUI.Models;
using ShadowViewer.Sdk.Helpers;
using System;
using System.Threading.Tasks;
using Serilog;
using ShadowViewer.Plugin.PluginManager.I18n;
using ShadowViewer.Sdk.Enums;

namespace ShadowViewer.Plugin.PluginManager.Models;
/// <summary>
/// 
/// </summary>
/// <seealso cref="ShadowViewer.Plugin.PluginManager.Models.PluginStoreBaseModel" />
public partial class PluginStoreModel
{
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
            IsClosable = true,
            Content = BuildProgressContent()
        };
        infoBar.Loaded += async (_, _) => { await RunInstallPipelineAsync(infoBar, installAction); };
        var content = await Template.Parse(I18N.InstallTextTemplate)
            .RenderAsync(new { action = I18N.Install, name = Id, version = Version });
        await NotifyService.ShowDialog(this, XamlHelper.CreateMessageDialog(I18N.Install, content,
            async void (sender, args) =>
            {
                try
                {
                    args.Cancel = true;
                    sender.PrimaryButtonText = I18N.Installing;
                    NotifyService.NotifyTip(this, infoBar, 0D, TipPopupPosition.Right);
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
}