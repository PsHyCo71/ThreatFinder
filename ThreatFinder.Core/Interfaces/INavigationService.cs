namespace ThreatFinder.Core;
public interface INavigationService
{
    void NavigateToSettings(string? errorMessage = null);
    void NavigateToScan();
}