Sprawozdanie - Laboratorium 3 - Platformy .NET

Struktura projektu

Program.cs - punkt wejscia aplikacji. Definiuje rozmiary macierzy (100, 200, 400, 600), liczby watkow (1, 2, 4, liczba rdzeni CPU, podwojona liczba rdzeni) oraz liczbe powtorzen benchmarku (5). Na poczatku uruchamia weryfikacje poprawnosci, potem wyswietla przykladowe mnozenie macierzy 4x4, a na koncu uruchamia benchmark.

Matrix.cs - klasa reprezentujaca macierz. Przechowuje dane w dwuwymiarowej tablicy double. Udostepnia metode Random do generowania macierzy z losowymi wartosciami (z zadanym seedem dla powtarzalnosci), metode Print do wyswietlania macierzy w konsoli oraz metode ApproximatelyEquals do porownywania dwoch macierzy z zadana tolerancja.

MatrixBenchmark.cs - klasa MatrixMultiplier z metoda statyczna Multiply. Realizuje mnozenie macierzy algorytmem klasycznym o zlozonosci O(n^3). Zewnetrzna petla (po wierszach) jest zrownoleglona za pomoca Parallel.For z parametrem MaxDegreeOfParallelism ustawionym na zadana liczbe watkow.

BenchmarkRunner.cs - klasa odpowiedzialna za przeprowadzanie pomiarow. Dla kazdego rozmiaru macierzy i kazdej liczby watkow wykonuje serie powtorzen (domyslnie 5), mierzy sredni czas i oblicza przyspieszenie (speedup) wzgledem wersji jednowatkowej. Przed pomiarami wykonywany jest jeden rozgrzewkowy przebieg (warm-up), ktory nie jest liczony. Klasa zawiera tez metode VerifyCorrectness, ktora porownuje wynik mnozenia sekwencyjnego z rownoleglym.
