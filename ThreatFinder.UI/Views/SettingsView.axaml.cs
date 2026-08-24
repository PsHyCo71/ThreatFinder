using Avalonia.Controls;
using System.ComponentModel;
using ThreatFinder.ViewModels;
using Avalonia.Threading;

namespace ThreatFinder.Views;

/// <summary>
/// Represents the view used to manage provider API keys.
/// </summary>
public partial class SettingsView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsView"/> class
    /// and subscribes to changes in the associated view model.
    /// </summary>
    public SettingsView()
    {
        InitializeComponent();

        DataContextChanged += (sender, e) =>
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
        };
    }

    /// <summary>
    /// Handles property changes from the settings view model and moves
    /// keyboard focus to the API key field when editing is enabled.
    /// </summary>
    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(SettingsViewModel.IsEditingMalwareBazaarKey)
            && DataContext is SettingsViewModel vm
            && vm.IsEditingMalwareBazaarKey)
        {
            Dispatcher.UIThread.Post(() => MalwareBazaarTextBox.Focus());
        }
        else if (e.PropertyName == nameof(SettingsViewModel.IsEditingUrlhausKey)
                 && DataContext is SettingsViewModel vm2
                 && vm2.IsEditingUrlhausKey)
        {
            Dispatcher.UIThread.Post(() => URLhausTextBox.Focus());
        }
    }
}