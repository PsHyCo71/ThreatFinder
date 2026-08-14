using CommunityToolkit.Mvvm.ComponentModel;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

public partial class ShellViewModel : ViewModelBase, INavigationService
{
    [ObservableProperty]
    public partial ViewModelBase CurrentViewModel { get; set; }
    public ShellViewModel(ScanManager scanManager, IFilePickerService filePickerService)
    {
        ScanViewModel scanViewModel = new ScanViewModel(scanManager, filePickerService, this);
        CurrentViewModel = scanViewModel;
    }

    public void NavigateToSettings()
    {
        CurrentViewModel = new SettingsViewModel();
    }
}