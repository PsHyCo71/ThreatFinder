using System.Collections.Generic;
using System.Net.Http;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ThreatFinder.Core;
using ThreatFinder.Providers;
using ThreatFinder.ViewModels;
using ThreatFinder.Views;

namespace ThreatFinder;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        HttpClient httpClient = new HttpClient();
        ApiKeyProvider keyProvider = new ApiKeyProvider();
        MBProvider mbProvider = new MBProvider(httpClient);
        URLhausProvider urlhausProvider = new URLhausProvider(httpClient);
        List<IThreatIntelProvider> providers = [mbProvider, urlhausProvider];
        ScanManager scanManager = new ScanManager(providers, keyProvider);


        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel(scanManager),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}