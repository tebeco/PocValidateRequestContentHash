# TLDR
As the benchmarks indicate 0 allocation with naive code ... well ... I can't trust the code or the benchmark yet

# Naive Benchmark

Small payload

```
// * Summary *

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19628
Intel Core i9-9980HK CPU 2.40GHz, 1 CPU, 16 logical and 8 physical cores
.NET Core SDK=5.0.100-preview.3.20216.6
  [Host]     : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT
  DefaultJob : .NET Core 5.0.0 (CoreCLR 5.0.20.21406, CoreFX 5.0.20.21406), X64 RyuJIT
```

|   Method | RequestBodyByteSize | WithValidation | WithHeader | ValidHash |         Mean |        Error |     StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------- |-------------------- |--------------- |----------- |---------- |-------------:|-------------:|-----------:|-------:|------:|------:|----------:|
| Validate |                   8 |          False |      False |     False |     59.38 ns |     0.784 ns |   0.733 ns |      - |     - |     - |         - |
| Validate |                   8 |          False |      False |      True |     55.74 ns |     0.700 ns |   0.655 ns |      - |     - |     - |         - |
| Validate |                   8 |          False |       True |     False |     57.80 ns |     0.457 ns |   0.428 ns |      - |     - |     - |         - |
| Validate |                   8 |          False |       True |      True |     66.70 ns |     0.824 ns |   0.771 ns |      - |     - |     - |         - |
| Validate |                   8 |           True |      False |     False |    110.55 ns |     1.332 ns |   1.181 ns |      - |     - |     - |         - |
| Validate |                   8 |           True |      False |      True |    114.16 ns |     2.039 ns |   1.908 ns |      - |     - |     - |         - |
| Validate |                   8 |           True |       True |     False |    657.80 ns |    13.188 ns |  22.034 ns |      - |     - |     - |         - |
| Validate |                   8 |           True |       True |      True |  1,078.31 ns |    17.390 ns |  16.267 ns |      - |     - |     - |         - |
| Validate |                2048 |          False |      False |     False |     60.96 ns |     0.578 ns |   0.513 ns |      - |     - |     - |         - |
| Validate |                2048 |          False |      False |      True |     60.17 ns |     0.669 ns |   0.559 ns |      - |     - |     - |         - |
| Validate |                2048 |          False |       True |     False |     67.61 ns |     1.369 ns |   2.572 ns |      - |     - |     - |         - |
| Validate |                2048 |          False |       True |      True |     71.30 ns |     1.456 ns |   2.392 ns |      - |     - |     - |         - |
| Validate |                2048 |           True |      False |     False |    116.11 ns |     2.211 ns |   2.069 ns |      - |     - |     - |         - |
| Validate |                2048 |           True |      False |      True |    113.31 ns |     1.351 ns |   1.264 ns |      - |     - |     - |         - |
| Validate |                2048 |           True |       True |     False |  8,306.21 ns |   162.659 ns | 180.795 ns |      - |     - |     - |         - |
| Validate |                2048 |           True |       True |      True |  8,739.56 ns |   174.706 ns | 171.585 ns |      - |     - |     - |         - |
| Validate |                3072 |          False |      False |     False |     58.92 ns |     1.184 ns |   1.735 ns |      - |     - |     - |         - |
| Validate |                3072 |          False |      False |      True |     58.52 ns |     1.195 ns |   2.215 ns |      - |     - |     - |         - |
| Validate |                3072 |          False |       True |     False |     55.90 ns |     0.824 ns |   0.731 ns |      - |     - |     - |         - |
| Validate |                3072 |          False |       True |      True |     59.15 ns |     0.895 ns |   0.793 ns |      - |     - |     - |         - |
| Validate |                3072 |           True |      False |     False |    119.53 ns |     2.357 ns |   3.065 ns |      - |     - |     - |         - |
| Validate |                3072 |           True |      False |      True |    108.50 ns |     2.193 ns |   3.215 ns |      - |     - |     - |         - |
| Validate |                3072 |           True |       True |     False | 11,981.76 ns |   169.160 ns | 141.256 ns |      - |     - |     - |         - |
| Validate |                3072 |           True |       True |      True | 12,940.01 ns |   258.321 ns | 503.835 ns |      - |     - |     - |         - |
| Validate |                4032 |          False |      False |     False |     54.81 ns |     0.484 ns |   0.429 ns |      - |     - |     - |         - |
| Validate |                4032 |          False |      False |      True |     56.43 ns |     1.154 ns |   1.541 ns |      - |     - |     - |         - |
| Validate |                4032 |          False |       True |     False |     65.46 ns |     0.668 ns |   0.625 ns |      - |     - |     - |         - |
| Validate |                4032 |          False |       True |      True |     57.73 ns |     0.334 ns |   0.313 ns |      - |     - |     - |         - |
| Validate |                4032 |           True |      False |     False |    105.76 ns |     0.974 ns |   0.863 ns |      - |     - |     - |         - |
| Validate |                4032 |           True |      False |      True |    109.75 ns |     0.651 ns |   0.543 ns |      - |     - |     - |         - |
| Validate |                4032 |           True |       True |     False | 15,589.36 ns |   127.454 ns | 119.221 ns | 0.4578 |     - |     - |    4056 B |
| Validate |                4032 |           True |       True |      True | 15,926.41 ns |    76.417 ns |  67.742 ns | 0.4578 |     - |     - |    4056 B |
| Validate |                4096 |          False |      False |     False |     56.59 ns |     0.452 ns |   0.401 ns |      - |     - |     - |         - |
| Validate |                4096 |          False |      False |      True |     54.37 ns |     0.268 ns |   0.251 ns |      - |     - |     - |         - |
| Validate |                4096 |          False |       True |     False |     61.35 ns |     0.437 ns |   0.409 ns |      - |     - |     - |         - |
| Validate |                4096 |          False |       True |      True |     65.17 ns |     0.509 ns |   0.476 ns |      - |     - |     - |         - |
| Validate |                4096 |           True |      False |     False |    109.86 ns |     0.790 ns |   0.700 ns |      - |     - |     - |         - |
| Validate |                4096 |           True |      False |      True |    103.24 ns |     1.225 ns |   1.023 ns |      - |     - |     - |         - |
| Validate |                4096 |           True |       True |     False | 15,831.58 ns |   106.168 ns |  88.655 ns | 0.4883 |     - |     - |    4120 B |
| Validate |                4096 |           True |       True |      True | 16,177.98 ns |    92.365 ns |  86.398 ns | 0.4883 |     - |     - |    4120 B |
| Validate |               16384 |          False |      False |     False |     57.73 ns |     0.366 ns |   0.342 ns |      - |     - |     - |         - |
| Validate |               16384 |          False |      False |      True |     58.78 ns |     0.238 ns |   0.222 ns |      - |     - |     - |         - |
| Validate |               16384 |          False |       True |     False |     60.62 ns |     0.554 ns |   0.519 ns |      - |     - |     - |         - |
| Validate |               16384 |          False |       True |      True |     57.40 ns |     0.348 ns |   0.308 ns |      - |     - |     - |         - |
| Validate |               16384 |           True |      False |     False |    114.91 ns |     1.977 ns |   1.942 ns |      - |     - |     - |         - |
| Validate |               16384 |           True |      False |      True |    111.25 ns |     1.403 ns |   1.172 ns |      - |     - |     - |         - |
| Validate |               16384 |           True |       True |     False | 63,160.17 ns | 1,002.846 ns | 938.063 ns | 1.9531 |     - |     - |   16408 B |
| Validate |               16384 |           True |       True |      True | 63,476.01 ns | 1,197.462 ns | 999.935 ns | 1.9531 |     - |     - |   16408 B |

```
// * Warnings *
MultimodalDistribution
  Benchmarks.Validate: Default -> It seems that the distribution can have several modes (mValue = 3.13)

// * Hints *
Outliers
  Benchmarks.Validate: Default -> 1 outlier  was  removed (155.39 ns)
  Benchmarks.Validate: Default -> 2 outliers were removed (733.99 ns, 782.82 ns)
  Benchmarks.Validate: Default -> 2 outliers were removed (1.13 us, 1.14 us)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (65.84 ns)
  Benchmarks.Validate: Default -> 2 outliers were removed (63.57 ns, 64.14 ns)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (98.33 ns)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (83.22 ns)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (9.25 us)
  Benchmarks.Validate: Default -> 2 outliers were removed (9.43 us, 9.58 us)
  Benchmarks.Validate: Default -> 2 outliers were removed (69.98 ns, 82.86 ns)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (61.36 ns)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (64.50 ns)
  Benchmarks.Validate: Default -> 2 outliers were removed (12.43 us, 12.63 us)
  Benchmarks.Validate: Default -> 2 outliers were removed (14.37 us, 15.11 us)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (57.85 ns)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (156.09 ns)
  Benchmarks.Validate: Default -> 2 outliers were removed (113.39 ns, 114.13 ns)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (16.16 us)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (59.80 ns)
  Benchmarks.Validate: Default -> 1 outlier  was  removed, 3 outliers were detected (110.14 ns, 110.42 ns, 113.10 ns)
  Benchmarks.Validate: Default -> 2 outliers were removed (108.99 ns, 113.95 ns)
  Benchmarks.Validate: Default -> 2 outliers were removed (16.18 us, 16.60 us)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (60.48 ns)
  Benchmarks.Validate: Default -> 1 outlier  was  removed (121.91 ns)
  Benchmarks.Validate: Default -> 2 outliers were removed (118.39 ns, 119.88 ns)
  Benchmarks.Validate: Default -> 2 outliers were removed (69.93 us, 74.05 us)

// * Legends *
  RequestBodyByteSize : Value of the 'RequestBodyByteSize' parameter
  WithValidation      : Value of the 'WithValidation' parameter
  WithHeader          : Value of the 'WithHeader' parameter
  ValidHash           : Value of the 'ValidHash' parameter
  Mean                : Arithmetic mean of all measurements
  Error               : Half of 99.9% confidence interval
  StdDev              : Standard deviation of all measurements
  Gen 0               : GC Generation 0 collects per 1000 operations
  Gen 1               : GC Generation 1 collects per 1000 operations
  Gen 2               : GC Generation 2 collects per 1000 operations
  Allocated           : Allocated memory per single operation (managed only, inclusive, 1KB = 1024B)
  1 ns                : 1 Nanosecond (0.000000001 sec)
  ```