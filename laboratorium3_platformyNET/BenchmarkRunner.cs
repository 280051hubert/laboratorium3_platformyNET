using System.Diagnostics;

namespace MatrixBenchmark;

public class BenchmarkRunner
{
    private readonly int _runsPerConfig;
    private readonly int[] _matrixSizes;
    private readonly int[] _threadCounts;

    public BenchmarkRunner(int[] matrixSizes, int[] threadCounts, int runsPerConfig = 5)
    {
        _matrixSizes = matrixSizes;
        _threadCounts = threadCounts;
        _runsPerConfig = runsPerConfig;
    }

    public void Run()
    {
        Console.WriteLine($"{"Size",-8} {"Threads",-9} {"Avg (ms)",10} {"Speedup",10}");
        Console.WriteLine(new string('-', 42));

        foreach (int size in _matrixSizes)
        {
            // Pre-generate matrices once per size (seeded → reproducible).
            var a = Matrix.Random(size, size, seed: 1);
            var b = Matrix.Random(size, size, seed: 2);

            // Baseline: sequential (1 thread).
            double baselineMs = MeasureAvg(a, b, threadCount: 1);

            foreach (int threads in _threadCounts)
            {
                double avgMs = (threads == 1) ? baselineMs : MeasureAvg(a, b, threads);
                double speedup = baselineMs / avgMs;

                Console.WriteLine($"{size + "x" + size,-8} {threads,-9} {avgMs,10:F1} {speedup,10:F2}x");
            }

            Console.WriteLine();
        }
    }

    private double MeasureAvg(Matrix a, Matrix b, int threadCount)
    {
        // Warm-up run (not counted).
        MatrixMultiplier.Multiply(a, b, threadCount);

        var sw = new Stopwatch();
        long totalMs = 0;

        for (int run = 0; run < _runsPerConfig; run++)
        {
            sw.Restart();
            MatrixMultiplier.Multiply(a, b, threadCount);
            sw.Stop();
            totalMs += sw.ElapsedMilliseconds;
        }

        return (double)totalMs / _runsPerConfig;
    }

    /// <summary>
    /// Quick correctness check: compare 1-thread vs N-thread result.
    /// </summary>
    public static bool VerifyCorrectness(int size, int threadCount)
    {
        var a = Matrix.Random(size, size, seed: 10);
        var b = Matrix.Random(size, size, seed: 20);

        var seq = MatrixMultiplier.Multiply(a, b, threadCount: 1);
        var par = MatrixMultiplier.Multiply(a, b, threadCount);

        return seq.ApproximatelyEquals(par);
    }
}
