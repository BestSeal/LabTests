using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using ProfilingApp;

int number = 84;
var processorCount = Environment.ProcessorCount;
var threads = new List<Thread>(processorCount);

ThreadPool.SetMaxThreads(processorCount, processorCount);
ThreadPool.SetMinThreads(processorCount, processorCount);

var checkBloc = number / processorCount;
if (processorCount > number)
{
    checkBloc = number;
    processorCount = 1;
}

Parallel.For(1, number, (i, token) =>
{
    if (MathHelper.IsPrime(i))
    {
        Holder.primes.Enqueue(i);
    }
});

for (int i = 0; i < processorCount; i++)
{
    threads.Add(new Thread(Holder.GetPrimeBlock));
    threads[i].Start(new Holder.Block(i * checkBloc, (i + 1) * checkBloc, checkBloc));
}

while (Holder.results.TryDequeue(out var dividers))
{
    Console.WriteLine($"Prime divisors for number {dividers.Item2}:");
    foreach (var divider1 in dividers.Item1)
    {
        Console.Write($"{divider1} ");
    }

    Console.WriteLine("\n");
}

public static class Holder
{
    public static readonly ConcurrentQueue<(List<int>, int)> results = new();

    public static readonly ConcurrentQueue<int> primes = new();

    public static void GetPrimeDividers(int number)
    {
        if (MathHelper.IsPrime(number)) return;

        var dividers = (new List<int>(), number);

        while (true)
        {
            if (number == 1 || number == 0) break;
            if (MathHelper.IsPrime(number))
            {
                dividers.Item1.Add(number);
                break;
            }
            
            foreach (var prime in primes)
            {
                var res = Math.DivRem(number, prime);
                if (res.Remainder == 0)
                {
                    dividers.Item1.Add(prime);
                    number = res.Quotient;
                    break;
                }
            }
        }
        
        results.Enqueue(dividers);
    }

    public static void GetPrimeBlock(object? x)
    {
        var block = (Block)x!;
        
        for (int i = block.Start; i <= block.End; i++)
        {
            GetPrimeDividers(i);
        }
    }
    
    public class Block
    {
        public int Start;
        public int End;
        public int BlockSize;

        public Block(int start, int end, int blockSize)
        {
            Start = start;
            End = end;
            BlockSize = blockSize;
        }
    }
}