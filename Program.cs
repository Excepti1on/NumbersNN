using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Linq;

namespace NumbersNN
{
    class Program
    {
        private static IEnumerable<Image> TestData;
        private static IEnumerable<Image> TrainData;
        private static IEnumerable<Image> ValData;

        public static void Main()
        {
            foreach (Image image in MNISTReader.ReadTrainData())
            {
                Console.WriteLine(image.Label);
            }
        }
    }

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
    }
}
