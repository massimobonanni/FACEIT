using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using FACEIT.Client.ViewModels;
using FACEIT.Core.Interfaces;
using FACEIT.Client.Configurations;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using FACEIT.FaceService.Implementations;

namespace FACEIT.Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var configBuilder = new ConfigurationBuilder()
            .AddUserSecrets<App>()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.local.json", optional: true)
            .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = configBuilder.Build();

        Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .AddSingleton(config)
            .UsePersonsManager()
            .UseGroupsManager()
            .UseFaceRecognizer()
            .AddTransient<IMessenger>(sp => WeakReferenceMessenger.Default)
            .AddTransient<MainViewModel>()
            .AddTransient<GroupsManagementViewModel>()
            .AddTransient<PersonsManagementViewModel>()
            .AddHttpClient()
            .AddLogging()
            .BuildServiceProvider()
        );
    }
}
