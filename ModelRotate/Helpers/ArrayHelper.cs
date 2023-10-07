using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace ModelRender.Helpers
{
    public class ArrayHelper
    {

        public static List<Vector3> Double2DToVector3List(double[][] data)
        {
            return data.Where(point => point.Length >= 3)
                       .Select(point => new Vector3((float)point[0], (float)point[1], (float)point[2]))
                       .ToList();
        }

        public static double[][] ToJaggedArray(double[,] twoDArray)
        {
            int rows = twoDArray.GetLength(0);
            int cols = twoDArray.GetLength(1);

            double[][] jaggedArray = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new double[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = twoDArray[i, j];
                }
            }

            return jaggedArray;
        }

        public static double[,] To2DArray(double[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;

            double[,] twoDArray = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    twoDArray[i, j] = jaggedArray[i][j];
                }
            }

            return twoDArray;
        }

        public static int[][] ToJaggedArray(int[,] twoDArray)
        {
            int rows = twoDArray.GetLength(0);
            int cols = twoDArray.GetLength(1);

            int[][] jaggedArray = new int[rows][];

            for (int i = 0; i < rows; i++)
            {
                jaggedArray[i] = new int[cols];
                for (int j = 0; j < cols; j++)
                {
                    jaggedArray[i][j] = twoDArray[i, j];
                }
            }

            return jaggedArray;
        }

        public static int[,] To2DArray(int[][] jaggedArray)
        {
            int rows = jaggedArray.Length;
            int cols = jaggedArray[0].Length;

            var twoDArray = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    twoDArray[i, j] = jaggedArray[i][j];
                }
            }

            return twoDArray;
        }

        public static double[][] PointsToDoubleArray(Point[] points)
        {
            double[][] result = new double[points.Length][];

            for (int i = 0; i < points.Length; i++)
            {
                result[i] = new double[] { points[i].X, points[i].Y };
            }

            return result;
        }

        public static Point[] DoubleArrayToPoints(double[][] array)
        {
            Point[] points = new Point[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                // Assuming that each inner array has at least 2 elements for X and Y
                points[i] = new Point(array[i][0], array[i][1]);
            }

            return points;
        }

        public static double[][] Vec3ToDouble2DArray(List<Vector3> vectors)
        {
            return vectors.Select(v => new double[] { v.X, v.Y, v.Z }).ToArray();
        }

        public static float[][] Vec3ToFloat2DArray(List<Vector3> vectors)
        {
            return vectors.Select(v => new float[] { v.X, v.Y, v.Z }).ToArray();
        }









    }
}
