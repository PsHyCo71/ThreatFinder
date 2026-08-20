using Avalonia.Controls;

namespace ThreatFinder.Views;

using Avalonia.Markup.Xaml;

public partial class ResultsView : UserControl
{
    public ResultsView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}