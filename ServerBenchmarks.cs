// Copyright (c) Martin Costello, 2019. All rights reserved.
// Licensed under the MIT license. See the LICENSE file in the project root for full license information.

namespace NoConsolePerf
{
    using System;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using BenchmarkDotNet.Attributes;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    [MemoryDiagnoser]
    public class ServerBenchmarks : IDisposable
    {
        private readonly HttpClient _client;
        private bool _disposed;
        private IHost _host;

        public ServerBenchmarks()
        {
            _client = new HttpClient() { BaseAddress = new Uri("http://localhost:5000") };
        }

        ~ServerBenchmarks()
        {
            Dispose(false);
        }

        [GlobalSetup]
        public async Task WarmupAsync()
        {
            _host = CreateHost(useConsole: true);

            await _host.StartAsync();
            await _host.StopAsync();

            _host.Dispose();
            _host = null;
        }

        [GlobalSetup(Target = nameof(NoConsole))]
        public async Task StartServerNoConsoleAsync()
        {
            _host = CreateHost(useConsole: false);
            await _host.StartAsync();
        }

        [GlobalSetup(Target = nameof(WithConsole))]
        public async Task StartServerWithConsoleAsync()
        {
            _host = CreateHost(useConsole: true);
            await _host.StartAsync();
        }

        [GlobalSetup(Target = nameof(WithSmartConsole))]
        public async Task StartServerWithSmartConsoleAsync()
        {
            _host = CreateHost(useConsole: true, useSmartConsole: true);
            await _host.StartAsync();
        }

        [GlobalCleanup(Targets = new[] { nameof(NoConsole), nameof(WithConsole), nameof(WithSmartConsole) })]
        public async Task StopServerAsync()
        {
            if (_host != null)
            {
                await _host.StopAsync();
                _host.Dispose();
                _host = null;
            }
        }

        [Benchmark(Baseline = true)]
        public async Task<byte[]> WithConsole()
        {
            return await _client.GetByteArrayAsync("/");
        }

        [Benchmark]
        public async Task<byte[]> NoConsole()
        {
            return await _client.GetByteArrayAsync("/");
        }

        [Benchmark]
        public async Task<byte[]> WithSmartConsole()
        {
            return await _client.GetByteArrayAsync("/");
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _client?.Dispose();
                _host?.Dispose();
            }

            _disposed = true;
        }

        private IHost CreateHost(bool useConsole, bool useSmartConsole = false)
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureLogging((builder) => builder.ClearProviders().SetMinimumLevel(LogLevel.Error))
                .ConfigureServices((services) => services.AddSingleton<IHostLifetime, BenchmarkHostLifetime>())
                .ConfigureWebHostDefaults((builder) => builder.UseStartup<Startup>().UseUrls("http://localhost:5000"));

            if (useConsole && useSmartConsole)
            {
                useConsole = !Console.IsErrorRedirected || !Console.IsOutputRedirected;
            }

            if (useConsole)
            {
                builder.ConfigureLogging((builder) => builder.AddConsole());
            }

            return builder.Build();
        }

        private sealed class BenchmarkHostLifetime : IHostLifetime
        {
            public Task StopAsync(CancellationToken cancellationToken)
                => Task.CompletedTask;

            public Task WaitForStartAsync(CancellationToken cancellationToken)
                => Task.CompletedTask;
        }
    }
}
