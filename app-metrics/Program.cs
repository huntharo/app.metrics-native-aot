using System.Threading.Tasks;
using app_metrics;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

MetricsRegistry.Metrics.Measure.Counter.Increment(MetricsRegistry.FailCount);

await Task.WhenAll(MetricsRegistry.Metrics.ReportRunner.RunAllAsync());
