using System;
using System.Collections.Generic;
using System.Linq;

namespace NumbersNN
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.Shuffle(new Random());
        }
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (rng == null) throw new ArgumentNullException(nameof(rng));

            return source.ShuffleIterator(rng);
        }

        public static IEnumerable<T> ShuffleIterator<T>(this IEnumerable<T> source, Random rng)
        {
            List<T> buffer = source.ToList();
            int n = buffer.Count;

            for (int i = 0; i < n; i++)
            {
                int j = rng.Next(i, n);
                yield return buffer[j];
                buffer[j] = buffer[i];
            }
        }

        public static void ForEach<T>(this T[,] source, Action<int, int> action)
        {
            for (int r = 0; r < source.GetLength(0); r++)
            {
                for (int c = 0; c < source.GetLength(1); c++)
                {
                    action(r, c);
                }
            }
        }

        public static void ForEach<T>(this T[] source, Action<int> action)
        {
            for (int i = 0; i < source.Length; i++)
            {
                action(i);
            }
        }
    }
}
