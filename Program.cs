// Copyright (c) Martin Costello, 2019. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace NoConsolePerf
{
    internal static class Program
    {
        internal static async Task Main()
        {
            if (Debugger.IsAttached)
            {
                using (var benchmark = new ServerBenchmarks())
                {
                    await benchmark.WarmupAsync();

                    await benchmark.StartServerNoConsoleAsync();
                    await benchmark.NoConsole();
                    await benchmark.StopServerNoConsoleAsync();

                    await benchmark.StartServerWithConsoleAsync();
                    await benchmark.WithConsole();
                    await benchmark.StopServerWithConsoleAsync();
                }
            }
            else
            {
                BenchmarkRunner.Run<ServerBenchmarks>();
            }
        }
    }
}
