# TLDR
As the benchmarks indicate 0 allocation with naive code ... well ... I can't trust the code or the benchmark yet

# Naive Benchmark

Small payload

```
// * Summary *

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19624
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=5.0.100-preview.3.20216.6
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT
  DefaultJob : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT
```

|                      Method |     Mean |    Error |   StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |---------:|---------:|---------:|------:|------:|------:|----------:|
| Not_Validated_Small_Payload | 69.50 ns | 1.404 ns | 2.670 ns |     - |     - |     - |         - |
|  Not_Validated_16KB_Payload | 64.65 ns | 1.315 ns | 2.160 ns |     - |     - |     - |         - |
|     Validated_Small_Payload | 70.15 ns | 1.386 ns | 1.228 ns |     - |     - |     - |         - |
|      Validated_16KB_Payload | 64.98 ns | 1.317 ns | 2.273 ns |     - |     - |     - |         - |
