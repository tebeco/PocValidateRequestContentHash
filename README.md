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

## PoolBenchmarks
| Method | RequestBodyByteSize |             PoolKind |      Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------- |-------------------- |--------------------- |----------:|----------:|----------:|-------:|------:|------:|----------:|
|    Run |                   8 |      SharedArrayPool |  1.097 us | 0.0212 us | 0.0297 us |      - |     - |     - |         - |
|    Run |                   8 |            ArrayPool |  1.076 us | 0.0126 us | 0.0112 us |      - |     - |     - |         - |
|    Run |                   8 |  FixedLengthLockFree |  1.040 us | 0.0132 us | 0.0117 us | 0.0038 |     - |     - |      32 B |
|    Run |                   8 |  FixedLengthWithLock |  1.242 us | 0.0244 us | 0.0440 us |      - |     - |     - |         - |
|    Run |                   8 | PreAl(...)kFree [31] |  1.080 us | 0.0210 us | 0.0225 us | 0.0038 |     - |     - |      32 B |
|    Run |                   8 | PreAl(...)hLock [31] |  1.234 us | 0.0244 us | 0.0240 us |      - |     - |     - |         - |
|    Run |                2048 |      SharedArrayPool |  8.868 us | 0.1756 us | 0.1803 us |      - |     - |     - |         - |
|    Run |                2048 |            ArrayPool |  8.738 us | 0.1213 us | 0.1075 us |      - |     - |     - |         - |
|    Run |                2048 |  FixedLengthLockFree |  8.780 us | 0.1244 us | 0.1164 us |      - |     - |     - |      32 B |
|    Run |                2048 |  FixedLengthWithLock |  8.945 us | 0.0736 us | 0.0688 us |      - |     - |     - |         - |
|    Run |                2048 | PreAl(...)kFree [31] |  8.823 us | 0.0687 us | 0.0609 us |      - |     - |     - |      32 B |
|    Run |                2048 | PreAl(...)hLock [31] |  8.904 us | 0.1298 us | 0.1151 us |      - |     - |     - |         - |
|    Run |                4096 |      SharedArrayPool | 16.666 us | 0.3239 us | 0.3601 us |      - |     - |     - |         - |
|    Run |                4096 |            ArrayPool | 17.010 us | 0.3292 us | 0.3659 us |      - |     - |     - |         - |
|    Run |                4096 |  FixedLengthLockFree | 16.504 us | 0.1427 us | 0.1265 us |      - |     - |     - |      32 B |
|    Run |                4096 |  FixedLengthWithLock | 16.867 us | 0.3321 us | 0.5362 us |      - |     - |     - |         - |
|    Run |                4096 | PreAl(...)kFree [31] | 16.604 us | 0.3294 us | 0.3921 us |      - |     - |     - |      32 B |
|    Run |                4096 | PreAl(...)hLock [31] | 16.490 us | 0.1513 us | 0.1341 us |      - |     - |     - |         - |
|    Run |               16384 |      SharedArrayPool | 61.939 us | 1.0304 us | 0.8604 us |      - |     - |     - |         - |
|    Run |               16384 |            ArrayPool | 64.042 us | 1.2377 us | 1.5653 us |      - |     - |     - |         - |
|    Run |               16384 |  FixedLengthLockFree | 62.484 us | 1.2229 us | 1.6325 us |      - |     - |     - |      32 B |
|    Run |               16384 |  FixedLengthWithLock | 62.682 us | 1.0509 us | 0.9316 us |      - |     - |     - |         - |
|    Run |               16384 | PreAl(...)kFree [31] | 61.910 us | 0.6302 us | 0.5895 us |      - |     - |     - |      32 B |
|    Run |               16384 | PreAl(...)hLock [31] | 62.116 us | 0.4770 us | 0.4229 us |      - |     - |     - |         - |

# Bombardier
`> bombardier-windows-amd64.exe --connections 200 --requests 1000000 https://localhost:5001/ValidateContent/validated --body="payload" --method="POST" --header="X-Content-Hash: 239F59ED55E737C77147CF55AD0C1B030B6D7EE748A7426952F9B852D5A935E5" --format=json -p r`

|                        PoolKind | Latency (mean) | Latency (max) |   RPS (mean) |          RPS (max) |          50 |          75 |          90 |          95 |          99 |
|---------------------------------|----------------|---------------|--------------|--------------------|-------------|-------------|-------------|-------------|-------------|
|                 SharedArrayPool |    1918.752118 |        440000 | 103084.9488  | 148420.0902805554  | 112629.6929 | 120669.5398 | 125366.4416 | 128236.599  | 131588.5166 |
|              DedicatedArrayPool |    1990.481452 |        478002 |  99446.22105 | 131272.7272727273  | 108139.5719 | 115408.297  | 120159.8541 | 122706.1353 | 126346.2096 |
|             FixedLengthLockFree |    1889.781931 |        442999 | 103736.1931  | 160190.6952709573  | 113384.6154 | 122916.826  | 128468.9506 | 130457.2462 | 135336.0401 |
|             FixedLengthWithLock |    2034.225963 |        479001 |  97719.67712 | 152477.12843073538 | 105803.7031 | 114613.6213 | 121038.742  | 123704.427  | 128235.0587 |
| PreAllocatedFixedLengthLockFree |    1757.201798 |        376000 |  112742.9014 | 162650.40162650403 | 118730.2857 | 123806.1903 | 127630.2355 | 129313.4795 | 134552.5655 |
| PreAllocatedFixedLengthWithLock |    2041.358069 |        449996 |  96928.43497 | 161008.0504025201  | 106898.931  | 114407.918  | 118571.598  | 122281.5142 | 128829.8936 |


## Raw data 
#### SharedArrayPool
`{"spec":{"numberOfConnections":200,"testType":"number-of-requests","numberOfRequests":1000000,"method":"POST","url":"https://localhost:5001/ValidateContent/validated","headers":[{"key":"X-Content-Hash","value":"239F59ED55E737C77147CF55AD0C1B030B6D7EE748A7426952F9B852D5A935E5"}],"body":"payload","stream":false,"timeoutSeconds":2,"client":"fasthttp"},"result":{"bytesRead":123042065,"bytesWritten":274064400,"timeTakenSeconds":9.6189994,"req1xx":0,"req2xx":802015,"req3xx":0,"req4xx":197985,"req5xx":0,"others":0,"latency":{"mean":1918.752118,"stddev":3533.940674615616,"max":440000},"rps":{"mean":103084.9487998893,"stddev":23500.38114029669,"max":148420.0902805554,"percentiles":{"50":112629.692904,"75":120669.539751,"90":  ,"95":128236.599025,"99":131588.516572}}}}`

#### DedicatedArrayPool
`{"spec":{"numberOfConnections":200,"testType":"number-of-requests","numberOfRequests":1000000,"method":"POST","url":"https://localhost:5001/ValidateContent/validated","headers":[{"key":"X-Content-Hash","value":"239F59ED55E737C77147CF55AD0C1B030B6D7EE748A7426952F9B852D5A935E5"}],"body":"payload","stream":false,"timeoutSeconds":2,"client":"fasthttp"},"result":{"bytesRead":123307754,"bytesWritten":274064400,"timeTakenSeconds":9.9750019,"req1xx":0,"req2xx":772494,"req3xx":0,"req4xx":227506,"req5xx":0,"others":0,"latency":{"mean":1990.481452,"stddev":3870.2134739121316,"max":478002},"rps":{"mean":99446.22104827361,"stddev":21134.63218418287,"max":131272.7272727273,"percentiles":{"50":108139.571898,"75":115408.296978,"90": ,"95":122706.135307,"99":126346.209614}}}}`

#### FixedLengthLockFree
`{"spec":{"numberOfConnections":200,"testType":"number-of-requests","numberOfRequests":1000000,"method":"POST","url":"https://localhost:5001/ValidateContent/validated","headers":[{"key":"X-Content-Hash","value":"239F59ED55E737C77147CF55AD0C1B030B6D7EE748A7426952F9B852D5A935E5"}],"body":"payload","stream":false,"timeoutSeconds":2,"client":"fasthttp"},"result":{"bytesRead":123320345,"bytesWritten":274064400,"timeTakenSeconds":9.4699726,"req1xx":0,"req2xx":771095,"req3xx":0,"req4xx":228905,"req5xx":0,"others":0,"latency":{"mean":1889.781931,"stddev":3953.133230848118,"max":442999},"rps":{"mean":103736.1931014924,"stddev":25525.759717086345,"max":160190.6952709573,"percentiles":{"50":113384.615385,"75":122916.825987,"90": ,"95":130457.246190,"99":135336.040054}}}}`

#### FixedLengthWithLock
`{"spec":{"numberOfConnections":200,"testType":"number-of-requests","numberOfRequests":1000000,"method":"POST","url":"https://localhost:5001/ValidateContent/validated","headers":[{"key":"X-Content-Hash","value":"239F59ED55E737C77147CF55AD0C1B030B6D7EE748A7426952F9B852D5A935E5"}],"body":"payload","stream":false,"timeoutSeconds":2,"client":"fasthttp"},"result":{"bytesRead":123429389,"bytesWritten":274064400,"timeTakenSeconds":10.1984924,"req1xx":0,"req2xx":758979,"req3xx":0,"req4xx":241021,"req5xx":0,"others":0,"latency":{"mean":2034.225963,"stddev":4078.919101362623,"max":479001},"rps":{"mean":97719.67712259587,"stddev":21607.3100792371,"max":152477.12843073538,"percentiles":{"50":105803.703130,"75":114613.621329,"90": ,"95":123704.427007,"99":128235.058698}}}}`

#### PreAllocatedFixedLengthLockFree
`{"spec":{"numberOfConnections":200,"testType":"number-of-requests","numberOfRequests":1000000,"method":"POST","url":"https://localhost:5001/ValidateContent/validated","headers":[{"key":"X-Content-Hash","value":"239F59ED55E737C77147CF55AD0C1B030B6D7EE748A7426952F9B852D5A935E5"}],"body":"payload","stream":false,"timeoutSeconds":2,"client":"fasthttp"},"result":{"bytesRead":123271484,"bytesWritten":274064400,"timeTakenSeconds":8.8089664,"req1xx":0,"req2xx":776524,"req3xx":0,"req4xx":223476,"req5xx":0,"others":0,"latency":{"mean":1757.201798,"stddev":3223.18752777695,"max":376000},"rps":{"mean":112742.9014492126,"stddev":22250.575928200287,"max":162650.40162650403,"percentiles":{"50":118730.285664,"75":123806.190310,"90": ,"95":129313.479510,"99":134552.565499}}}}`

#### PreAllocatedFixedLengthWithLock
`{"spec":{"numberOfConnections":200,"testType":"number-of-requests","numberOfRequests":1000000,"method":"POST","url":"https://localhost:5001/ValidateContent/validated","headers":[{"key":"X-Content-Hash","value":"239F59ED55E737C77147CF55AD0C1B030B6D7EE748A7426952F9B852D5A935E5"}],"body":"payload","stream":false,"timeoutSeconds":2,"client":"fasthttp"},"result":{"bytesRead":123290672,"bytesWritten":274064400,"timeTakenSeconds":10.2273042,"req1xx":0,"req2xx":774392,"req3xx":0,"req4xx":225608,"req5xx":0,"others":0,"latency":{"mean":2041.358069,"stddev":3465.7908176160076,"max":449996},"rps":{"mean":96928.43496620869,"stddev":22061.505993581395,"max":161008.0504025201,"percentiles":{"50":106898.931011,"75":114407.917955,"90": ,"95":122281.514157,"99":128829.893627}}}}`

