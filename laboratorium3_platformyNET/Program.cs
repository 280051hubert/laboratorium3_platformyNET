using MatrixBenchmark;


int[] matrixSizes = [100, 200, 400, 600];
int maxCores = Environment.ProcessorCount;
int[] threadCounts = [1, 2, 4, maxCores, maxCores * 2];
int runsPerConfig = 5;

Console.WriteLine("3.1");
Console.WriteLine($"CPU cores {maxCores}");
Console.WriteLine("Weryfikacja");
bool allOk = true;
foreach (int t in threadCounts)
{
    bool ok = BenchmarkRunner.VerifyCorrectness(100, t);
    Console.WriteLine($"  {t,2} thread(s): {(ok ? "OK" : "MISMATCH!")}");
    if (!ok) allOk = false;
}

if (!allOk)
{
    Console.WriteLine("correctness check fail");
    return;
}
Console.WriteLine();
{
    int demoSize = 4;
    var dA = Matrix.Random(demoSize, demoSize, seed: Random.Shared.Next());
    var dB = Matrix.Random(demoSize, demoSize, seed: Random.Shared.Next());
    var dC = MatrixMultiplier.Multiply(dA, dB, threadCount: 2);

    dA.Print("A:");
    dB.Print("B:");
    dC.Print("A × B:");
}

Console.WriteLine("Uruchamianie benchmarków\n");
var runner = new BenchmarkRunner(matrixSizes, threadCounts, runsPerConfig);
runner.Run();