namespace app_metrics;

using System.IO;
using System.Text;
using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Filters;
using App.Metrics.Formatters;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Reporting;

public static class MetricsRegistry
{
  public static readonly IMetricsRoot Metrics = new MetricsBuilder()
    .Report.ToConsole(options =>
    {
      options.FlushInterval = TimeSpan.FromSeconds(10);
    })
    .Build();

  static MetricsRegistry()
  {
    // This does not help
    // App.Metrics.Logging.LogProvider.IsDisabled = true;
  }

  public static readonly CounterOptions FailCount = new()
  {
    Name = "FailCount",
    MeasurementUnit = Unit.Requests,
  };
}