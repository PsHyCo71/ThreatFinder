using System;
using System.Collections.Generic;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ThreatFinder.Core;
using ThreatFinder.Providers;
using ThreatFinder.UI;
using ThreatFinder.ViewModels;
using ThreatFinder.Views;

namespace ThreatFinder;

/// <summary>
/// Represents the main application class and is responsible for initializing
/// the Avalonia framework and configuring the application's dependencies.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Loads the application XAML resources and styles.
    /// </summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Configures the application's services, providers, and main window
    /// once the Avalonia framework has completed its initialization.
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        // Creates the HTTP client used by the threat intelligence providers.
        HttpClient httpClient = new HttpClient();

        // Creates the service responsible for storing and retrieving API keys.
        ApiKeyProvider keyProvider = new ApiKeyProvider();

        // Initializes the available threat intelligence providers.
        MBProvider mbProvider = new MBProvider(httpClient);
        URLhausProvider urlhausProvider = new URLhausProvider(httpClient);

        // Registers the providers available to the scan manager.
        List<IThreatIntelProvider> providers = [mbProvider, urlhausProvider];

        // Creates the scan manager responsible for coordinating scans.
        ScanManager scanManager = new ScanManager(providers, keyProvider);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Provides the file picker service with access to the application's main window.
            Func<TopLevel?> topLevel = () => desktop.MainWindow;
            FilePickerService filePicker = new FilePickerService(topLevel);

            // Creates the main window and assigns the root view model as its data context.
            desktop.MainWindow = new MainWindow
            {
                DataContext = new ShellViewModel(scanManager, filePicker, keyProvider)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}