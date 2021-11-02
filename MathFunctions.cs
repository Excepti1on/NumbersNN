using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumbersNN
{
    public static class MathFunctions
    {
        public static double Sigmoid(double x)
        {
            return 1 / (Math.Exp(-x) + 1d);
        }

        public static double SigmoidDx(double x)
        {
            return (Math.Exp(-x)) / (Math.Pow(Math.Exp(-x) + 1d, 2d));
        }

        

        public static void Softmax(double[] array)
        {
            double sum;
            double max = array.Max();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Math.Exp(array[i] - max);
            }

            sum = array.Sum();

            for (int i = 0; i < array.Length; i++)
            {
                array[i] /= sum;
            }
            
        }

        public static void SoftmaxDx(double[] array)
        {
            double[] helper = array;
            double max = array.Max();

            for (int i = 0; i < array.Length; i++)
            {
                helper[i] = Math.Exp(array[i] - max);
            }
            double sum = helper.Sum();
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = helper[i] / sum * (1 - helper[i] / sum);
            }
        }
    }
}
