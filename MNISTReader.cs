using System;
using System.Collections.Generic;
using System.IO;

namespace NumbersNN
{
    public static class MNISTReader
    {
        private const string TrainImages = "F:\\repos\\NumbersNN\\mnist\\train-images.idx3-ubyte";
        private const string TrainLabels = "F:\\repos\\NumbersNN\\mnist\\train-labels.idx1-ubyte";
        private const string TestImages = "F:\\repos\\NumbersNN\\mnist\\t10k-images.idx3-ubyte";
        private const string TestLabels = "F:\\repos\\NumbersNN\\mnist\\t10k-labels.idx1-ubyte";

        public static IEnumerable<Image> ReadTrainData()
        {
            foreach (Image image in Read(TrainImages, TrainLabels))
            {
                yield return image;
            }
        }

        public static IEnumerable<Image> ReadTestData()
        {
            foreach (Image image in Read(TestImages, TestLabels))
            {
                yield return image;
            }

        }

        private static IEnumerable<Image> Read(string imagePath, string labelPath)
        {
            using BinaryReader labels = new BinaryReader(new FileStream(labelPath, FileMode.Open));
            using BinaryReader images = new BinaryReader(new FileStream(imagePath, FileMode.Open));

            int magicNumber = images.ReadBigInt32();
            int numImages = images.ReadBigInt32();
            int numRows = images.ReadBigInt32();
            int numCols = images.ReadBigInt32();

            int magicNumberLabel = labels.ReadBigInt32();
            int numLabels = labels.ReadBigInt32();

            for (int i = 0; i < numImages; i++)
            {
                byte[] bytes = images.ReadBytes(numRows * numCols);
                byte[,] arr = new byte[numCols, numRows];

                arr.ForEach((j, k) => arr[j, k] = bytes[j * numCols + k]);
                
                yield return new Image()
                {
                    Pixels = arr,
                    Label = labels.ReadByte()
                };
            }
        }

        public static int ReadBigInt32 (this BinaryReader br)
        {
            byte[] bytes = br.ReadBytes(sizeof(Int32));
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToInt32(bytes, 0);
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

    }

    public class Image
    {
        public byte[,] Pixels { get; set; }
        public byte Label { get; set; }
    }
}
