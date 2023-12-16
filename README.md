# Overview

This demonstrats an issue with App.Metrics preventing usage of Native AOT due to, at least initiatlly, a logging class.

## Steps to reproduce

1. Clone this repo
1. Run `dotnet restore`
1. Run `dotnet build`
1. Run `dotnet publish non-app-metrics/non-app-metrics.csproj -c Release --self-contained true --runtime linux-arm64 -o out`
1. Run `./out/non-app-metrics`
1. Observe that there is no error
1. Run `dotnet publish app-metrics/app-metrics.csproj -c Release --self-contained true --runtime linux-arm64 -o out`
1. Observe the trim warnings during the build
1. Run `./out/app-metrics`
1. Observe the `TypeInitializationException` error
1. Error comes from `App.Metrics.Reporting.Console.ConsoleMetricsReporter..ctor`
1. The error is from this location: https://github.com/AppMetrics/AppMetrics/blob/31e5888d97d6ea799d96598c35411da95b95869b/src/Reporting/src/App.Metrics.Reporting.Console/ConsoleMetricsReporter.cs#L20
1. The line is `App.Metrics.Logging.LogProvider.GetLogger(String)`

## Build Warnings

```log
dotnet publish app-metrics/app-metrics.csproj -c Release --self-contained true --runtime linux-arm64 -o out
MSBuild version 17.8.3+195e7f5a3 for .NET
  Determining projects to restore...
  All projects are up-to-date for restore.
  app-metrics -> /workspaces/app.metrics-native-aot/app-metrics/bin/Release/net8.0/linux-arm64/app-metrics.dll
  Generating native code
/home/vscode/.nuget/packages/app.metrics.core/4.3.0/lib/netstandard2.0/App.Metrics.Core.dll : warning IL2104: Assembly 'App.Metrics.Core' produced trim warnings. For more information see https://aka.ms/dotnet-illink/libraries [/workspaces/app.metrics-native-aot/app-metrics/app-metrics.csproj]
/home/vscode/.nuget/packages/app.metrics.core/4.3.0/lib/netstandard2.0/App.Metrics.Core.dll : warning IL3053: Assembly 'App.Metrics.Core' produced AOT analysis warnings. [/workspaces/app.metrics-native-aot/app-metrics/app-metrics.csproj]
/home/vscode/.nuget/packages/runtime.linux-arm64.microsoft.dotnet.ilcompiler/8.0.0/framework/Microsoft.CSharp.dll : warning IL3053: Assembly 'Microsoft.CSharp' produced AOT analysis warnings. [/workspaces/app.metrics-native-aot/app-metrics/app-metrics.csproj]
/home/vscode/.nuget/packages/runtime.linux-arm64.microsoft.dotnet.ilcompiler/8.0.0/framework/System.Linq.Expressions.dll : warning IL3053: Assembly 'System.Linq.Expressions' produced AOT analysis warnings. [/workspaces/app.metrics-native-aot/app-metrics/app-metrics.csproj]
  app-metrics -> /workspaces/app.metrics-native-aot/out/
```

## Runtime Error

```log
./out/app-metrics
Hello, World!
Unhandled Exception: System.TypeInitializationException: A type initializer threw an exception. To determine which type, inspect the InnerException's StackTrace property.
 ---> System.TypeInitializationException: A type initializer threw an exception. To determine which type, inspect the InnerException's StackTrace property.
 ---> System.NullReferenceException: Object reference not set to an instance of an object.
   at Microsoft.CSharp.RuntimeBinder.Semantics.ExpressionTreeRewriter.VisitBoundLambda(ExprBoundLambda) + 0x158
   at Microsoft.CSharp.RuntimeBinder.RuntimeBinder.CreateExpressionTreeFromResult(Expression[], Scope, Expr) + 0x84
   at Microsoft.CSharp.RuntimeBinder.RuntimeBinder.Bind(ICSharpBinder, Expression[], DynamicMetaObject[], DynamicMetaObject&) + 0x64
   at Microsoft.CSharp.RuntimeBinder.BinderHelper.Bind(ICSharpBinder, RuntimeBinder, DynamicMetaObject[], IEnumerable`1, DynamicMetaObject) + 0x4b0
   at System.Dynamic.DynamicMetaObjectBinder.Bind(Object[], ReadOnlyCollection`1, LabelTarget) + 0x124
   at System.Runtime.CompilerServices.CallSiteBinder.BindCore[T](CallSite`1, Object[]) + 0xa4
   at app-metrics!<BaseAddress>+0x2f1e30
   at System.Reflection.DynamicInvokeInfo.InvokeWithFewArguments(IntPtr, Byte&, Byte&, Object[], BinderBundle, Boolean) + 0x78
--- End of stack trace from previous location ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() + 0x24
   at System.Linq.Expressions.Interpreter.ExceptionHelpers.UnwrapAndRethrow(TargetInvocationException) + 0x18
   at System.Linq.Expressions.Interpreter.MethodInfoCallInstruction.Run(InterpretedFrame) + 0x1bc
   at System.Linq.Expressions.Interpreter.Interpreter.Run(InterpretedFrame) + 0x48
   at System.Linq.Expressions.Interpreter.LightLambda.Run(Object[] arguments) + 0x8c
   at app-metrics!<BaseAddress>+0x2baea8
   at App.Metrics.Logging.LogProvider.GetLogger(String) + 0x28
   at App.Metrics.Reporting.Console.ConsoleMetricsReporter..cctor() + 0x3c
   at System.Runtime.CompilerServices.ClassConstructorRunner.EnsureClassConstructorRun(StaticClassConstructionContext*) + 0xbc
   --- End of inner exception stack trace ---
   at System.Runtime.CompilerServices.ClassConstructorRunner.EnsureClassConstructorRun(StaticClassConstructionContext*) + 0x15c
   at System.Runtime.CompilerServices.ClassConstructorRunner.CheckStaticClassConstructionReturnGCStaticBase(StaticClassConstructionContext*, Object) + 0x14
   at App.Metrics.Reporting.Console.ConsoleMetricsReporter..ctor(MetricsReportingConsoleOptions) + 0x138
   at App.Metrics.MetricsConsoleReporterBuilder.ToConsole(IMetricsReportingBuilder, Action`1) + 0x58
   at app_metrics.MetricsRegistry..cctor() + 0x64
   at System.Runtime.CompilerServices.ClassConstructorRunner.EnsureClassConstructorRun(StaticClassConstructionContext*) + 0xbc
   --- End of inner exception stack trace ---
   at System.Runtime.CompilerServices.ClassConstructorRunner.EnsureClassConstructorRun(StaticClassConstructionContext*) + 0x15c
   at System.Runtime.CompilerServices.ClassConstructorRunner.CheckStaticClassConstructionReturnGCStaticBase(StaticClassConstructionContext*, Object) + 0x14
   at Program.<<Main>$>d__0.MoveNext() + 0xf0
--- End of stack trace from previous location ---
   at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw() + 0x24
   at System.Runtime.CompilerServices.TaskAwaiter.ThrowForNonSuccess(Task) + 0x100
   at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task, ConfigureAwaitOptions) + 0x68
   at Program.<Main>(String[] args) + 0x30
   at app-metrics!<BaseAddress>+0x2bf858
Aborted
```