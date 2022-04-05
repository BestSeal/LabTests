using System.Collections.Concurrent;
using ProfilingApp;

//var num = Console.ReadLine();

int number = 84;

var primes = new ConcurrentQueue<int>();
var results = new ConcurrentQueue<(ConcurrentQueue<int>, int)>();

var processorCount = Environment.ProcessorCount;

ThreadPool.SetMaxThreads(processorCount, processorCount);
ThreadPool.SetMinThreads(processorCount, processorCount);

Parallel.For(1, number, (i, token) =>
{
    if (MathHelper.IsPrime(i))
    {
        primes.Enqueue(i);
    }
});

primes.ToList().Sort();

Parallel.For(1, number, () => (new ConcurrentQueue<int>(), 0), (i, token, tmp) =>
    {
        tmp.Item2 = i;
        tmp.Item1 = new ConcurrentQueue<int>();
        
        if (MathHelper.IsPrime(i)) return (tmp);
        
        foreach (var prime in primes)
        {
            if (tmp.Item2 == 1) break;
            
            if (tmp.Item2 % prime == 0)
            {
                tmp.Item2 = i / prime;
                tmp.Item1.Enqueue(prime);
            }
        }

        return tmp;
    },
    x => { results.Enqueue(x); });

while (results.TryDequeue(out var dividers))
{
    Console.WriteLine($"Prime divisors for number {dividers.Item2}:");
    while (dividers.Item1.TryDequeue(out var divider))
    {
        Console.Write($"{divider} ");
    }

    Console.WriteLine("\n");
}