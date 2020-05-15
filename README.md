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
|       Method | RequestBodyByteSize | WithValidation |         Mean |        Error |       StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |-------------------- |--------------- |-------------:|-------------:|-------------:|------:|------:|------:|----------:|
| RunValidData |                   8 |          False |     59.34 ns |     0.921 ns |     0.862 ns |     - |     - |     - |         - |
| RunValidData |                   8 |           True |  1,031.71 ns |    20.128 ns |    26.172 ns |     - |     - |     - |         - |
| RunValidData |                2048 |          False |     56.89 ns |     0.361 ns |     0.338 ns |     - |     - |     - |         - |
| RunValidData |                2048 |           True |  8,402.14 ns |    57.716 ns |    51.164 ns |     - |     - |     - |         - |
| RunValidData |                3072 |          False |     58.43 ns |     1.030 ns |     0.913 ns |     - |     - |     - |         - |
| RunValidData |                3072 |           True | 12,086.80 ns |   102.959 ns |    96.308 ns |     - |     - |     - |         - |
| RunValidData |                4032 |          False |     58.39 ns |     0.486 ns |     0.454 ns |     - |     - |     - |         - |
| RunValidData |                4032 |           True | 15,428.71 ns |   121.145 ns |   113.320 ns |     - |     - |     - |         - |
| RunValidData |                4096 |          False |     57.10 ns |     0.497 ns |     0.465 ns |     - |     - |     - |         - |
| RunValidData |                4096 |           True | 16,207.74 ns |   246.764 ns |   230.824 ns |     - |     - |     - |         - |
| RunValidData |               16384 |          False |     66.86 ns |     0.474 ns |     0.420 ns |     - |     - |     - |         - |
| RunValidData |               16384 |           True | 61,074.58 ns | 1,178.839 ns | 1,210.581 ns |     - |     - |     - |         - |

## InvalidHeaderBenchmarks
|   Method | RequestBodyByteSize | WithHeader | FastCorruptedHeader |     Mean |   Error |  StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------- |-------------------- |----------- |-------------------- |---------:|--------:|--------:|------:|------:|------:|----------:|
| Validate |                   8 |      False |               False | 102.4 ns | 0.94 ns | 0.88 ns |     - |     - |     - |         - |
| Validate |                   8 |      False |                True | 111.2 ns | 1.25 ns | 0.97 ns |     - |     - |     - |         - |
| Validate |                   8 |       True |               False | 132.0 ns | 1.89 ns | 1.67 ns |     - |     - |     - |         - |
| Validate |                   8 |       True |                True | 132.3 ns | 2.60 ns | 3.47 ns |     - |     - |     - |         - |
| Validate |                4096 |      False |               False | 116.0 ns | 0.91 ns | 0.85 ns |     - |     - |     - |         - |
| Validate |                4096 |      False |                True | 109.4 ns | 1.44 ns | 1.27 ns |     - |     - |     - |         - |
| Validate |                4096 |       True |               False | 129.3 ns | 2.22 ns | 2.08 ns |     - |     - |     - |         - |
| Validate |                4096 |       True |                True | 135.0 ns | 1.28 ns | 1.20 ns |     - |     - |     - |         - |
| Validate |               16384 |      False |               False | 111.9 ns | 1.30 ns | 1.22 ns |     - |     - |     - |         - |
| Validate |               16384 |      False |                True | 107.0 ns | 1.05 ns | 0.98 ns |     - |     - |     - |         - |
| Validate |               16384 |       True |               False | 131.2 ns | 1.67 ns | 1.56 ns |     - |     - |     - |         - |
| Validate |               16384 |       True |                True | 135.7 ns | 1.77 ns | 1.38 ns |     - |     - |     - |         - |



## InvalidHashBenchmarks
|   Method | RequestBodyByteSize |        Mean |     Error |    StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------- |-------------------- |------------:|----------:|----------:|------:|------:|------:|----------:|
| Validate |                   8 |    643.1 ns |   8.12 ns |   6.34 ns |     - |     - |     - |         - |
| Validate |                2048 |  8,470.4 ns | 167.80 ns | 323.29 ns |     - |     - |     - |         - |
| Validate |                3072 | 12,088.3 ns | 192.84 ns | 161.03 ns |     - |     - |     - |         - |
| Validate |                4032 | 15,946.1 ns | 245.66 ns | 217.77 ns |     - |     - |     - |         - |
| Validate |                4096 | 16,039.0 ns | 319.21 ns | 524.47 ns |     - |     - |     - |         - |
| Validate |               16384 | 60,341.5 ns | 535.02 ns | 500.46 ns |     - |     - |     - |         - |

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