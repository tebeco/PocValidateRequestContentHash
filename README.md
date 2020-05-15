# Naive Benchmark

```
// * Summary *

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19628
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=5.0.100-preview.3.20216.6
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT
  DefaultJob : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT
  
  
// * Legends *
  RequestBodyByteSize : Value of the 'RequestBodyByteSize' parameter
  Mean                : Arithmetic mean of all measurements
  Error               : Half of 99.9% confidence interval
  StdDev              : Standard deviation of all measurements
  Gen 0               : GC Generation 0 collects per 1000 operations
  Gen 1               : GC Generation 1 collects per 1000 operations
  Gen 2               : GC Generation 2 collects per 1000 operations
  Allocated           : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 ns                : 1 Nanosecond (0.000000001 sec)
```

## RunValidData

|       Method | RequestBodyByteSize | WithValidation |         Mean |      Error |     StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |-------------------- |--------------- |-------------:|-----------:|-----------:|-------:|------:|------:|----------:|
| RunValidData |                   8 |          False |     64.44 ns |   1.244 ns |   1.661 ns |      - |     - |     - |         - |
| RunValidData |                   8 |           True |  1,057.58 ns |  21.064 ns |  24.258 ns |      - |     - |     - |         - |
| RunValidData |                2048 |          False |     59.98 ns |   1.135 ns |   1.664 ns |      - |     - |     - |         - |
| RunValidData |                2048 |           True |  8,663.90 ns | 142.821 ns | 133.594 ns |      - |     - |     - |         - |
| RunValidData |                3072 |          False |     59.28 ns |   1.213 ns |   1.245 ns |      - |     - |     - |         - |
| RunValidData |                3072 |           True | 12,283.47 ns | 168.063 ns | 140.340 ns |      - |     - |     - |         - |
| RunValidData |                4032 |          False |     58.15 ns |   0.585 ns |   0.518 ns |      - |     - |     - |         - |
| RunValidData |                4032 |           True | 15,940.04 ns | 109.443 ns | 102.373 ns | 0.4578 |     - |     - |    4056 B |
| RunValidData |                4096 |          False |     59.43 ns |   0.271 ns |   0.227 ns |      - |     - |     - |         - |
| RunValidData |                4096 |           True | 16,220.22 ns | 117.458 ns | 109.870 ns | 0.4883 |     - |     - |    4120 B |
| RunValidData |               16384 |          False |     57.08 ns |   0.593 ns |   0.463 ns |      - |     - |     - |         - |
| RunValidData |               16384 |           True | 61,810.19 ns | 757.170 ns | 671.211 ns | 1.9531 |     - |     - |   16408 B |

## InvalidHeaderBenchmarks
|   Method | RequestBodyByteSize | WithHeader | FastCorruptedHeader |     Mean |   Error |  StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------- |-------------------- |----------- |-------------------- |---------:|--------:|--------:|------:|------:|------:|----------:|
| Validate |                   8 |      False |               False | 112.5 ns | 2.23 ns | 3.54 ns |     - |     - |     - |         - |
| Validate |                   8 |      False |                True | 119.3 ns | 2.35 ns | 3.21 ns |     - |     - |     - |         - |
| Validate |                   8 |       True |               False | 133.1 ns | 2.69 ns | 5.50 ns |     - |     - |     - |         - |
| Validate |                   8 |       True |                True | 133.6 ns | 1.31 ns | 1.09 ns |     - |     - |     - |         - |
| Validate |                4096 |      False |               False | 108.1 ns | 0.99 ns | 0.93 ns |     - |     - |     - |         - |
| Validate |                4096 |      False |                True | 115.4 ns | 2.32 ns | 4.58 ns |     - |     - |     - |         - |
| Validate |                4096 |       True |               False | 129.5 ns | 2.12 ns | 1.98 ns |     - |     - |     - |         - |
| Validate |                4096 |       True |                True | 148.1 ns | 2.94 ns | 5.38 ns |     - |     - |     - |         - |
| Validate |               16384 |      False |               False | 102.1 ns | 1.43 ns | 1.34 ns |     - |     - |     - |         - |
| Validate |               16384 |      False |                True | 106.5 ns | 1.15 ns | 1.02 ns |     - |     - |     - |         - |
| Validate |               16384 |       True |               False | 127.3 ns | 2.49 ns | 2.67 ns |     - |     - |     - |         - |
| Validate |               16384 |       True |                True | 134.2 ns | 2.72 ns | 5.48 ns |     - |     - |     - |         - |



## InvalidHashBenchmarks
|   Method | RequestBodyByteSize |        Mean |       Error |      StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------- |-------------------- |------------:|------------:|------------:|-------:|------:|------:|----------:|
| Validate |                   8 |    628.7 ns |     4.47 ns |     4.18 ns |      - |     - |     - |         - |
| Validate |                2048 |  8,098.1 ns |   148.29 ns |   131.46 ns |      - |     - |     - |         - |
| Validate |                3072 | 11,702.0 ns |   106.74 ns |    94.62 ns |      - |     - |     - |         - |
| Validate |                4032 | 16,264.5 ns |   321.79 ns |   596.45 ns | 0.4578 |     - |     - |    4056 B |
| Validate |                4096 | 16,059.6 ns |   298.69 ns |   428.37 ns | 0.4883 |     - |     - |    4120 B |
| Validate |               16384 | 63,847.6 ns | 1,263.45 ns | 1,812.01 ns | 1.9531 |     - |     - |   16408 B |

```
// * Warnings *
MultimodalDistribution
  InvalidHashBenchmarks.Validate: Default -> It seems that the distribution can have several modes (mValue = 2.87)

// * Hints *
Outliers
  InvalidHashBenchmarks.Validate: Default -> 1 outlier  was  removed (9.72 us)
  InvalidHashBenchmarks.Validate: Default -> 1 outlier  was  removed (12.25 us)
  InvalidHashBenchmarks.Validate: Default -> 5 outliers were removed (18.89 us..19.89 us)
  InvalidHashBenchmarks.Validate: Default -> 3 outliers were removed (17.65 us..18.37 us)
  InvalidHashBenchmarks.Validate: Default -> 1 outlier  was  removed (76.10 us)
```