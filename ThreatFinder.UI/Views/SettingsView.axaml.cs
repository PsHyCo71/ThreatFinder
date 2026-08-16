using Avalonia.Controls;
using System.ComponentModel;
using ThreatFinder.ViewModels;
using Avalonia.Threading;

namespace ThreatFinder.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        
        DataContextChanged += (sender , e) =>
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
        };
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName  == nameof(SettingsViewModel.IsEditingMalwareBazaarKey)
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