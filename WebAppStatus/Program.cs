using Serilog;
using WebAppStatus;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File(@"C:\Temp\Serilog\LogInfo.txt")
    .CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();



try
{
    await host.RunAsync();
    return;
}
catch (Exception ex)
{
    Log.Fatal(ex, "There is a problem starting the servie");
}
finally
{
    Log.CloseAndFlush();
}
