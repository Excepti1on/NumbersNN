using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumbersNN
{
    public class Neuralnet
    {
        private const int imageSize = 784;
        private const int layer1Size = 128;
        private const int labelCount = 10;

        private double[,] weightsl1 = new double[layer1Size, imageSize];
        private double[,] weightGradientl1 = new double[layer1Size, imageSize];
        private double[] biasl1 = new double[layer1Size];
        private double[] biasGradientl1 = new double[layer1Size];

        private double[,] weightsl2 = new double[labelCount, layer1Size];
        private double[,] weightGradientl2 = new double[labelCount, layer1Size];
        private double[] biasl2 = new double[labelCount];
        private double[] biasGradientl2 = new double[labelCount];

        private double[] output = new double[labelCount];

        public Neuralnet()
        {
            RandomizeWeights(weightsl1);
            RandomizeBias(biasl1);
            RandomizeWeights(weightsl2);
            RandomizeBias(biasl2);
        }

        public static double NormalizePixel(byte x)
        {
            return x / 255.0d;
        }


        //Initialize the Weights and Bias with random Values
        private void RandomizeWeights(double[,] _weights)
        {
            Random _random = new Random();
            _weights.ForEach((j, k) => _weights[j, k] = _random.NextDouble());
        }
        private void RandomizeBias(double[] _bias)
        {
            Random _random = new Random();
            _bias.ForEach(j => _bias[j] = _random.NextDouble());
        }

        //Get the output array for an Image Input
        public void Forwards(Image image)
        {
            double[] layer1 = new double[layer1Size];
            for (int i = 0; i < layer1Size; i++)
            {
                layer1[i] = biasl1[i];
                for (int j = 0; j < imageSize; j++)
                {
                    layer1[i] += MathFunctions.Sigmoid(weightsl1[i, j] * NormalizePixel(image.Pixels[j]));
                }               
                
            }
            for (int i = 0; i < labelCount; i++)
            {
                output[i] = biasl2[i];
                for (int j = 0; j < layer1Size; j++)
                {
                    output[i] += MathFunctions.Sigmoid(weightsl2[i, j] * layer1[j]);
                }
            }
            MathFunctions.Softmax(output);
        }


        public double GradientDescent(Image image, double[] prediction)
        {
            double bGrad, wGrad;

            Forwards(image);
            for (int i = 0; i < labelCount; i++)
            {
                bGrad = (i == image.Label) ? prediction[i] - 1 : prediction[i];

                for (int j = 0; j < imageSize; j++)
                {
                    wGrad = bGrad * NormalizePixel(image.Pixels[j]);

                    weightGradientl1[i, j] += wGrad;
                }
                biasGradientl1[i] += bGrad;
            }
            double pred = Array.IndexOf(prediction, prediction.Max());
            //Console.WriteLine($"Prediction: {pred}\t Label: {image.Label}");
            return 0.0d - Math.Log(prediction[image.Label]);
        }

        public double GradientDescent2(Image image)
        {
            double bGrad1, wGrad1, bGrad2, wGrad2;
            Forwards(image);
            
            

        }

        public double TrainingStep(IEnumerable<Image> dataset, double learningRate)
        {
            double totalLoss = 0.0d;

            foreach (Image _image in dataset)
            {
                totalLoss += GradientDescent(_image, output);
            }

            for (int i = 0; i < labelCount; i++)
            {
                biasl1[i] -= learningRate * biasGradientl1[i];

                for (int j = 0; j < imageSize; j++)
                {
                    weightsl1[i, j] -= learningRate * weightGradientl1[i, j];
                }
            }
            return totalLoss;
        }

        public double CalculateAccuracy(IEnumerable<Image> images)
        {
            int correct = 0;
            int position;
            foreach (Image _image in images)
            {
                Forwards(_image);
                position = Array.IndexOf(output, output.Max());
                correct += (position == _image.Label) ? 1 : 0;
            }
            return correct / ((double)images.Count());

        }
    }
}
