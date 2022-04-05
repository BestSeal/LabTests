namespace ProfilingApp;

public static class MathHelper
{
    public static bool IsDivisibleByDivisor(int divided, int divisor) => Math.DivRem(divided, divisor).Remainder == 0;

    public static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number == 2) return true;
        if (IsDivisibleByDivisor(number, 2)) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));

        var result = true;

        Parallel.For(3, boundary, (i, token) =>
        {
            if (IsDivisibleByDivisor(number, i))
            {
                result = false;
                token.Break();
            }
        });
    
        return result;    
    }
    
    public static int[] GetPrimeDividers(int number, int[] primes)
    {
        if (IsPrime(number)) return new[] { 1, number };

        var dividers = new List<int>();
        
        foreach (var prime in primes)
        {
            if(number == 1) break;

            var res = Math.DivRem(number, prime);
            if (res.Remainder == 0)
            {
                dividers.Add(prime);
                number = res.Quotient;
            }
        }

        return dividers.ToArray();
    }
}