using CommunityToolkit.Mvvm.ComponentModel;
using ThreatFinder.Core;
using ThreatFinder.ViewModels;

namespace ThreatFinder.ViewModels;
public partial class ResultsViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial ScanResult? Result { get; set; } = null;
}