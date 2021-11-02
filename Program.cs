using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Diagnostics;

namespace NumbersNN
{
    class Program
    {
        static double loss, accuracy;
        public static void Main()
        {
            var timer = new Stopwatch();
            timer.Start();
            int batchSize = 100;
            int epochs = 15000;
            IEnumerable<Image> trainDataset = MNISTReader.ReadTrainData();
            IEnumerable<Image> testDataset = MNISTReader.ReadTestData();
            Neuralnet Network = new();

            for (int i = 0; i < epochs; i++)
            {
                IEnumerable<Image> batch = trainDataset.Skip((i*batchSize) % 600 ).Take(batchSize);
                loss = Network.TrainingStep(batch, 0.09);
                accuracy = Network.CalculateAccuracy(testDataset);
                Console.WriteLine($"Epoch: {i} \t Average Loss: {loss / batchSize} \t Accuracy: {accuracy}");
            }
            timer.Stop();
            Console.WriteLine(timer.Elapsed);

            

        }

     
    }
}
