using System.Windows;
using CalculatorExample.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace CalculatorExample;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    public static IServiceProvider ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e) {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services) {
        services.AddTransient(typeof(MainWindow));
    }
}