using System.Threading.Tasks;

namespace ThreatFinder.Core;

/// <summary>
/// Defines the contract for navigating between different views of the application.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Navigates to the settings view, optionally displaying an error message.
    /// </summary>
    /// <param name="errorMessage">Optional error message to display in the settings view.</param>
    Task NavigateToSettings(string? errorMessage = null);

    /// <summary>
    /// Navigates to the scan view.
    /// </summary>
    void NavigateToScan();

    /// <summary>
    /// Navigates to the results view and provides the scan results.
    /// </summary>
    /// <typeparam name="TResults">Type of the results to display.</typeparam>
    /// <param name="results">Results to pass to the results view.</param>
    void NavigateToResults<TResults>(TResults results);
}