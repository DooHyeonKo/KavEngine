using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KavEngine.NeuralNetwork.Model
{
    public class NeuralNetworkUtils
    {
        public enum MatrixType
        {
            None,
            Product,
            Substract,
            Sum,
            DotProduct,
            Sigmoid,
            SigmoidDerivative
        }

        public MatrixType GetMatrixType { get; set; }

        public static double[,,] GetConvolutionMatrix(double[,,] MatrixA, double[,,,] MatrixB)
        {
            int MatrixValue0 = MatrixA.GetLength(0);
            int MatrixValue1 = MatrixA.GetLength(1);
            int MatrixValue2 = MatrixA.GetLength(2);

            double[,,] Matrix = new double[MatrixValue0, MatrixValue1, MatrixValue2];

            for (int i = 0; i < MatrixB.GetLength(0); i++)
            {
                for (int j = 0; j < MatrixValue0; j++)
                {
                    for (int k = 0; k < MatrixValue1; k++)
                    {
                        for (int c = 0; c < MatrixValue2; c++)
                        {
                            Matrix[j, k, c] = Matrix[j, k, c] * MatrixB[i, j, k, c];
                        }
                    }
                }
            }

            return Matrix;
        }

        public static double[,,] GetActivationMatrix(double[,,] MatrixA)
        {
            int MatrixValue0 = MatrixA.GetLength(0);
            int MatrixValue1 = MatrixA.GetLength(1);
            int MatrixValue2 = MatrixA.GetLength(2);

            double[,,] Matrix = new double[MatrixValue0, MatrixValue1, MatrixValue2];

            for (int i = 0; i < MatrixValue0; i++)
            {
                for (int j = 0; j < MatrixValue1; j++)
                {
                    for (int k = 0; k < MatrixValue2; k++)
                    {
                        if (MatrixA[i, j, k] < 0)
                        {
                            Matrix[i, j, k] = 0;
                        }
                        else
                        {
                            Matrix[i, j, k] = MatrixA[i, j, k];
                        }
                    }
                }
            }

            return Matrix;
        }

        public static double[,,] GetMaxPoolingMatrix(double[,,] MatrixA, int PoolingSize)
        {
            double[,,] output = null;
            try
            {
                //Formula for MaxPooling
                // newwidth = Width/filtersize and newheight = Height/filtersize
                // width+newwidth + width and height+newheight +heigth

                var newHeight = ((MatrixA.GetLength(1) - PoolingSize) / 2) + 1;
                var newWidth = ((MatrixA.GetLength(2) - PoolingSize) / 2) + 1;
                //output= new double[input.GetLength(0), input.GetLength(1), input.GetLength(2)]; for (int j = 0; j < input.GetLength(0); j++)
                output = new double[MatrixA.GetLength(0), newHeight, newWidth];
                for (int j = 0; j < MatrixA.GetLength(0); j++)
                {
                    var cuurent_y = 0; var out_y = 0;
                    for (int k = cuurent_y + PoolingSize; k < MatrixA.GetLength(1); k++)
                    {
                        var cuurent_x = 0; var out_x = 0;
                        var rowValue = MatrixA[j, k, 0] * newWidth + MatrixA[j, k, 0];
                        for (int l = cuurent_x + PoolingSize; l < MatrixA.GetLength(2); l++)
                        {
                            var columnValue = MatrixA[j, k, l] * newHeight + MatrixA[j, k, l];
                            double maxValue = MaxValue(MatrixA, j, k, l, PoolingSize);
                            output[j, out_y, out_x] = MatrixA[j, k, l] > maxValue ? MatrixA[j, k, l] : maxValue; // using which is maximum value
                            cuurent_x = cuurent_x + 2;
                            out_x = out_x + 1;
                        }
                        cuurent_y = cuurent_y + 2;
                        out_y = out_y + 1;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return output;
        }

        public static double[] GetFlattern(double[,,] MatrixA)
        {

            int rgbChannel = MatrixA.GetLength(0);
            int rowPixel = MatrixA.GetLength(1);
            int columnPixel = MatrixA.GetLength(2);
            int length = rgbChannel * rowPixel * columnPixel;
            double[] output = new double[length];
            try
            {
                int count = 0;
                for (int i = 0; i < rgbChannel; i++)
                {
                    for (int j = 0; j < rowPixel; j++)
                    {
                        for (int k = 0; k < columnPixel; k++)
                        {
                            output[count] = MatrixA[i, j, k];
                            count = count + 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return output;
        }

        public static double GetFullyConnectedMatrix(double[] input, double[] weights)
        {
            double sum = 0;
            try
            {
                for (int i = 0; i < input.Length; i++)
                {
                    sum = sum + (input[i] * weights[i]);
                }
            }
            catch (Exception)
            {

            }

            return sum;
        }

        public static double[,,,] GetFilterMatrix(int filter, int nooffilters, int pixelsize)
        {
            double[,,,] doubleFilter = new double[filter, nooffilters, pixelsize, pixelsize];
            Random random = new Random();
            for (int i = 0; i < filter; i++)
            {
                for (int j = 0; j < nooffilters; j++)
                {
                    for (int k = 0; k < pixelsize; k++)
                    {
                        for (int l = 0; l < pixelsize; l++)
                        {
                            doubleFilter[i, j, k, l] = random.NextDouble();
                        }
                    }
                }
            }

            return doubleFilter;
        }

        public static double[] RandomWeights(int count)
        {
            double[] weights = new double[count];
            Random random = new Random();
            try
            {
                for (int i = 0; i < count; i++)
                {
                    weights[i] = random.NextDouble();
                }
            }
            catch (Exception ex)
            {

            }
            return weights;
        }

        public void GetMatrix(double[,] Matrix, double[,] MatrixA, double[,] MatrixB, double[,] ResultMatrix, MatrixType Type)
        {
            switch (Type)
            {
                case MatrixType.Product:
                    ResultMatrix = MatrixProduct(MatrixA, MatrixB);
                    break;
                case MatrixType.DotProduct:
                    ResultMatrix = MatrixDotProduct(MatrixA, MatrixB);
                    break;
                case MatrixType.Sigmoid:
                    ResultMatrix = MatrixSigmoid(Matrix);
                    break;
                case MatrixType.SigmoidDerivative:
                    ResultMatrix = MatrixSigmoidDerivative(Matrix);
                    break;
                case MatrixType.Substract:
                    ResultMatrix = SubstractMatrix(MatrixA, MatrixB);
                    break;
                case MatrixType.Sum:
                    ResultMatrix = MatrixSum(MatrixA, MatrixB);
                    break;
            }
        }

        public void GetMatrix(int[,] Matrix, int[,] MatrixA, int[,] MatrixB, int[,] ResultMatrix, MatrixType Type)
        {
            switch (Type)
            {
                case MatrixType.Product:
                    ResultMatrix = MatrixProduct(MatrixA, MatrixB);
                    break;
                case MatrixType.DotProduct:
                    ResultMatrix = MatrixDotProduct(MatrixA, MatrixB);
                    break;
                case MatrixType.Sigmoid:
                    ResultMatrix = MatrixSigmoid(Matrix);
                    break;
                case MatrixType.SigmoidDerivative:
                    ResultMatrix = MatrixSigmoidDerivative(Matrix);
                    break;
                case MatrixType.Substract:
                    ResultMatrix = SubstractMatrix(MatrixA, MatrixB);
                    break;
                case MatrixType.Sum:
                    ResultMatrix = MatrixSum(MatrixA, MatrixB);
                    break;
            }
        }
        public static int[,] MatrixProduct(int[,] MatrixA, int[,] MatrixB)
        {
            int RowA = MatrixA.GetLength(0);
            int ColB = MatrixA.GetLength(1);

            int[,] tmpMatrix = new int[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    tmpMatrix[i, j] = MatrixA[i, j] * MatrixB[i, j];
                }
            }

            return tmpMatrix;
        }
        public static double[,] MatrixProduct(double[,] MatrixA, double[,] MatrixB)
        {
            int RowA = MatrixA.GetLength(0);
            int ColB = MatrixA.GetLength(1);

            double[,] tmpMatrix = new double[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    tmpMatrix[i, j] = MatrixA[i, j] * MatrixB[i, j];
                }
            }

            return tmpMatrix;
        }
        public static int[,] SubstractMatrix(int[,] MatrixA, int[,] MatrixB)
        {
            int RowA = MatrixA.GetLength(0);
            int ColB = MatrixA.GetLength(1);

            int[,] tmpMatrix = new int[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    tmpMatrix[i, j] = MatrixA[i, j] - MatrixB[i, j];
                }
            }

            return tmpMatrix;
        }
        public static double[,] SubstractMatrix(double[,] MatrixA, double[,] MatrixB)
        {
            int RowA = MatrixA.GetLength(0);
            int ColB = MatrixA.GetLength(1);

            double[,] tmpMatrix = new double[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    tmpMatrix[i, j] = MatrixA[i, j] - MatrixB[i, j];
                }
            }

            return tmpMatrix;
        }
        public static int[,] MatrixSum(int[,] MatrixA, int[,] MatrixB)
        {
            int RowA = MatrixA.GetLength(0);
            int ColB = MatrixA.GetLength(1);

            int[,] tmpMatrix = new int[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    tmpMatrix[i, j] = MatrixA[i, j] + MatrixB[i, j];
                }
            }

            return tmpMatrix;
        }
        public static double[,] MatrixSum(double[,] MatrixA, double[,] MatrixB)
        {
            int RowA = MatrixA.GetLength(0);
            int ColB = MatrixA.GetLength(1);

            double[,] tmpMatrix = new double[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    tmpMatrix[i, j] = MatrixA[i, j] + MatrixB[i, j];
                }
            }

            return tmpMatrix;
        }
        public static int[,] MatrixDotProduct(int[,] MatrixA, int[,] MatrixB)
        {
            int RowA = MatrixA.GetLength(0);
            int ColA = MatrixA.GetLength(1);

            int RowB = MatrixA.GetLength(0);
            int ColB = MatrixA.GetLength(1);

            if (ColA != RowB)
                throw new Exception("Matrices dimensions don't fit.");

            int[,] tmpMatrix = new int[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    for (int k = 0; k < RowB; k++)
                    {
                        tmpMatrix[j, j] += MatrixA[i, k] * MatrixB[k, j];
                    }
                }
            }
            return tmpMatrix;
        }
        public static double[,] MatrixDotProduct(double[,] MatrixA, double[,] MatrixB)
        {
            int RowA = MatrixA.GetLength(0);
            int ColA = MatrixA.GetLength(1);

            int RowB = MatrixA.GetLength(0);
            int ColB = MatrixA.GetLength(1);

            if (ColA != RowB)
                throw new Exception("Matrices dimensions don't fit.");

            double[,] tmpMatrix = new double[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    for (int k = 0; k < RowB; k++)
                    {
                        tmpMatrix[j, j] += MatrixA[i, k] * MatrixB[k, j];
                    }
                }
            }
            return tmpMatrix;
        }
        public static double[,] MatrixSigmoid(double[,] Matrix)
        {
            int RowA = Matrix.GetLength(0);
            int ColB = Matrix.GetLength(1);

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    var value = Matrix[i, j];
                    Matrix[i, j] = 1 / (1 + Math.Exp(value * -1)); // Sigmoid
                }
            }

            return Matrix;
        }
        public static int[,] MatrixSigmoid(int[,] Matrix)
        {
            int RowA = Matrix.GetLength(0);
            int ColB = Matrix.GetLength(1);

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    var value = Matrix[i, j];
                    Matrix[i, j] = 1 / (1 + (int)Math.Exp(value * -1)); // Sigmoid
                }
            }

            return Matrix;
        }
        public static double[,] MatrixSigmoidDerivative(double[,] Matrix)
        {
            int RowA = Matrix.GetLength(0);
            int ColB = Matrix.GetLength(1);

            double[,] tmpMatrix = new double[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    var value = tmpMatrix[i, j];
                    tmpMatrix[i, j] = value * (1 - value);
                }
            }

            return tmpMatrix;
        }
        public static int[,] MatrixSigmoidDerivative(int[,] Matrix)
        {
            int RowA = Matrix.GetLength(0);
            int ColB = Matrix.GetLength(1);

            int[,] tmpMatrix = new int[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    var value = tmpMatrix[i, j];
                    tmpMatrix[i, j] = value * (1 - value);
                }
            }

            return tmpMatrix;
        }
        public static double[,] MatrixTranspose(double[,] Matrix)
        {
            int RowA = Matrix.GetLength(0);
            int ColB = Matrix.GetLength(1);

            double[,] tmpMatrix = new double[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    tmpMatrix[i, j] = Matrix[i, j];
                }
            }

            return tmpMatrix;
        }
        public static int[,] MatrixTranspose(int[,] Matrix)
        {
            int RowA = Matrix.GetLength(0);
            int ColB = Matrix.GetLength(1);

            int[,] tmpMatrix = new int[RowA, ColB];

            for (int i = 0; i < RowA; i++)
            {
                for (int j = 0; j < ColB; j++)
                {
                    tmpMatrix[i, j] = Matrix[i, j];
                }
            }

            return tmpMatrix;
        }

        public static double MaxValue(double[,,] input, int i, int j, int k, int filtersize)
        {
            double maxValue = 0;
            try
            {
                for (int a = 0; a < j + filtersize; a++)
                {
                    for (int b = 0; b < k + filtersize; b++)
                    {
                        maxValue = maxValue < input[i, a, b] ? input[i, a, b] : maxValue;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return maxValue;
        }
    }

    public class NeuralNetwork
    {
        private static double _Score = 0.0;

        public static void Train(double[,,] TrainSet, int Filter1, int Filter2, int Filter3)
        {
            dynamic Output = NeuralNetworkUtils.GetFilterMatrix(Filter1, Filter2, Filter3);
            Output = NeuralNetworkUtils.GetConvolutionMatrix(TrainSet, Output);
            //Activation Layer using ReLU
            Output = NeuralNetworkUtils.GetActivationMatrix(Output);
            //Max Pooling Layer with 2x2 filter and strides 2
            Output = NeuralNetworkUtils.GetMaxPoolingMatrix(Output, 2);
            //Flattern Layer
            Output = NeuralNetworkUtils.GetFlattern(Output);
            double[] weights = NeuralNetworkUtils.RandomWeights(Output.Length);
            //Fully Connected Layer
            _Score = NeuralNetworkUtils.GetFullyConnectedMatrix(Output, weights);
        }

        public static double Predict()
        {
            return _Score;
        }
    } 
}
