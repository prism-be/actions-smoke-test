// -----------------------------------------------------------------------
//  <copyright file="Program.cs" company="Prism">
//  Copyright (c) Prism. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prism.GithubActions.SmokeTest;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        // Configure Services Injection
        services.AddHttpClient<SmokeTester>();
        services.AddScoped<SmokeTester>();
    })
    .Build();

static TService Get<TService>(IHost host)
    where TService : notnull
{
    return host.Services.GetRequiredService<TService>();
}

var parser = Parser.Default.ParseArguments(() => new ActionInputs(), args);
parser.WithNotParsed(
    errors =>
    {
        // ReSharper disable once AccessToDisposedClosure
        Get<ILoggerFactory>(host)
            .CreateLogger(typeof(SmokeTester).FullName!)
            .LogError(string.Join(Environment.NewLine, errors.Select(error => error.ToString())));

        Environment.Exit(2);
    });

await parser.WithParsedAsync(options => StartSmokeTestsAsync(options, host));
await host.RunAsync();

static async Task StartSmokeTestsAsync(ActionInputs inputs, IHost host)
{
    var smokeTester = Get<SmokeTester>(host);
    await smokeTester.Run(inputs);

    Environment.Exit(0);
}