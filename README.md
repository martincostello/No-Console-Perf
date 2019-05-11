# No Console Performance Benchmarks

Benchmarks for ASP.NET Core 3.0 with and without a console logger.

To run the benchmarks, clone the repo and run `dotnet run --configuration Release --framework netcoreapp3.0`.

``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview5-011568
  [Host]     : .NET Core 3.0.0-preview5-27626-15 (CoreCLR 4.6.27622.75, CoreFX 4.700.19.22408), 64bit RyuJIT
  DefaultJob : .NET Core 3.0.0-preview5-27626-15 (CoreCLR 4.6.27622.75, CoreFX 4.700.19.22408), 64bit RyuJIT


```
|           Method |     Mean |    Error |   StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------------- |---------:|---------:|---------:|------:|--------:|------:|------:|------:|----------:|
|      WithConsole | 210.1 us | 4.107 us | 5.758 us |  1.00 |    0.00 |     - |     - |     - |   2.09 KB |
|        NoConsole | 205.4 us | 2.676 us | 2.503 us |  0.99 |    0.03 |     - |     - |     - |   2.09 KB |
| WithSmartConsole | 201.4 us | 3.213 us | 3.006 us |  0.97 |    0.03 |     - |     - |     - |   2.09 KB |
