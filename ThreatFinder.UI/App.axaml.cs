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
            Func<TopLevel?> topLevel = () => desktop.MainWindow;
            FilePickerService filePicker = new FilePickerService(topLevel);
            desktop.MainWindow = new MainWindow
            {
                DataContext = new ShellViewModel(scanManager, filePicker)
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}