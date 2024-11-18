﻿using CommunityToolkit.Mvvm.DependencyInjection;
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
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.local.json", optional: true)
                  .AddJsonFile("appsettings.json", optional: false);

        IConfiguration config = configBuilder.Build();

        Ioc.Default.ConfigureServices(
            new ServiceCollection()
            .AddSingleton(config)
            .AddSingleton<IPersonsManager>(sp =>
            {
                var httpClient= sp.GetRequiredService<HttpClient>();
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var logger= loggerFactory.CreateLogger<FacesManager>();
                var config = FacesManagerConfiguration.Load(sp.GetRequiredService<IConfiguration>());
                return new FacesManager(httpClient,config.Endpoint, config.Key,logger);
            })
            .AddSingleton<IGroupsManager>(sp =>
            {
                var httpClient = sp.GetRequiredService<HttpClient>();
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<FacesManager>();
                var config = FacesManagerConfiguration.Load(sp.GetRequiredService<IConfiguration>());
                return new FacesManager(httpClient, config.Endpoint, config.Key, logger);
            })
            .AddTransient<IMessenger>(sp => WeakReferenceMessenger.Default)
            .AddTransient<MainWindowViewModel>()
            .AddHttpClient()
            .AddLogging()
            .BuildServiceProvider()
        );
    }
}
