namespace Zz;

using System;

public static class RandomHelpers
{
    private static readonly Random random = new Random(Random.Shared.Next(1000, 9999));

    public static Random RandomShared => random;

    public static int Random4() => RandomShared.Next(1000, 10_000);

    public static int Random6() => RandomShared.Next(100_000, 1_000_000);

    public static int NextRandom(int min, int max) => RandomShared.Next(min, max);

    public static int NextRandom(int max) => RandomShared.Next(max);

    public static int NextRandom() => RandomShared.Next();
}
