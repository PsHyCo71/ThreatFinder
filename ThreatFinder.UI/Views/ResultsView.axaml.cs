using Avalonia.Controls;

namespace ThreatFinder.Views;

using Avalonia.Markup.Xaml;

/// <summary>
/// Represents the view used to display scan results.
/// </summary>
public partial class ResultsView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResultsView"/> class.
    /// </summary>
    public ResultsView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Loads the XAML definition associated with this view.
    /// </summary>
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}