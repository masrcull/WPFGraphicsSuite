using System.Numerics;

namespace GraphicsCommon
{
    public class LinearAlgebra
    {
        public static double[] SubtractVectors(double[] vector1, double[] vector2)
        {
            double[] result = new double[vector1.Length];
            if (vector1 != null && vector2 != null)
            {
                if (vector1.Length != vector2.Length)
                {
                    throw new ArgumentException("Vectors must have the same dimension.");
                }

                result = new double[vector1.Length];

                for (int i = 0; i < vector1.Length; i++)
                {
                    result[i] = vector1[i] - vector2[i];
                }
            }

            return result;
        }

        public static double[] AddVectors(double[] vector1, double[] vector2)
        {
            if (vector1.Length != vector2.Length)
            {
                throw new ArgumentException("Vectors must have the same length.");
            }

            return vector1.Zip(vector2, (v1, v2) => v1 + v2).ToArray();
        }

        public static double DegreeToRadians(double degree)
        {
            return degree * Math.PI / 180;
        }

        public static double[] Multiply(double[][] matrix, double[] vector)
        {
            if (matrix.Length != 3 || vector.Length != 3)
            {
                throw new ArgumentException("Invalid dimensions.");
            }

            double[] result = new double[3];

            for (int i = 0; i < 3; i++)
            {
                result[i] = matrix[i][0] * vector[0] + matrix[i][1] * vector[1] + matrix[i][2] * vector[2];
            }

            return result;
        }

        public static double[] RotateAroundX(double[] vertex, double angle)
        {
            if (vertex.Length != 3)
            {
                throw new ArgumentException("Invalid vertex dimensions.");
            }

            // Rotation matrix for X axis
            double[][] rotationMatrix = new double[3][];
            rotationMatrix[0] = new double[] { 1, 0, 0 };
            rotationMatrix[1] = new double[] { 0, Math.Cos(angle), -Math.Sin(angle) };
            rotationMatrix[2] = new double[] { 0, Math.Sin(angle), Math.Cos(angle) };

            return Multiply(rotationMatrix, vertex);
        }

        public static double[] RotateAroundY(double[] vertex, double angle)
        {
            if (vertex.Length != 3)
            {
                throw new ArgumentException("Invalid vertex dimensions.");
            }

            // Rotation matrix for Y axis
            double[][] rotationMatrix = new double[3][];
            rotationMatrix[0] = new double[] { Math.Cos(angle), 0, Math.Sin(angle) };
            rotationMatrix[1] = new double[] { 0, 1, 0 };
            rotationMatrix[2] = new double[] { -Math.Sin(angle), 0, Math.Cos(angle) };

            return Multiply(rotationMatrix, vertex);
        }

        public static double[] RotateAroundZ(double[] vertex, double angle)
        {
            if (vertex.Length != 3)
            {
                throw new ArgumentException("Invalid vertex dimensions.");
            }

            // Rotation matrix for Z axis
            double[][] rotationMatrix = new double[3][];
            rotationMatrix[0] = new double[] { Math.Cos(angle), -Math.Sin(angle), 0 };
            rotationMatrix[1] = new double[] { Math.Sin(angle), Math.Cos(angle), 0 };
            rotationMatrix[2] = new double[] { 0, 0, 1 };

            return Multiply(rotationMatrix, vertex);
        }

        public static double[] CrossProduct(double[] vec1, double[] vec2)
        {
            if (vec1.Length != 3 || vec2.Length != 3)
            {
                throw new ArgumentException("Both vectors must have 3 components.");
            }

            double[] result = new double[3];
            result[0] = vec1[1] * vec2[2] - vec1[2] * vec2[1];
            result[1] = vec1[2] * vec2[0] - vec1[0] * vec2[2];
            result[2] = vec1[0] * vec2[1] - vec1[1] * vec2[0];

            return result;
        }

        public static double DotProduct(double[] vector1, double[] vector2)
        {
            if (vector1.Length != vector2.Length)
            {
                throw new ArgumentException("Vectors must have the same length.");
            }

            return vector1.Zip(vector2, (v1, v2) => v1 * v2).Sum();
        }

        public static double[] CalculateCentroid(double[,] vertices)
        {
            int n = vertices.GetLength(0);  // number of vertices

            if (n == 0)
                throw new ArgumentException("The vertices array must have at least one vertex.");

            double sumX = 0;
            double sumY = 0;
            double sumZ = 0;

            for (int i = 0; i < n; i++)
            {
                sumX += vertices[i, 0];
                sumY += vertices[i, 1];
                sumZ += vertices[i, 2];
            }

            return new double[] { sumX / n, sumY / n, sumZ / n };
        }

        public static void TranslateVertices(double[,] vertices, double[] translation)
        {
            if (translation.Length != 3)
                throw new ArgumentException("The translation array must have 3 elements.");

            int n = vertices.GetLength(0);  // number of vertices

            for (int i = 0; i < n; i++)
            {
                vertices[i, 0] += translation[0];
                vertices[i, 1] += translation[1];
                vertices[i, 2] += translation[2];
            }
        }

        public static void ScaleVertices(double[,] vertices, double[] scalingFactors)
        {
            if (scalingFactors.Length != 3)
                throw new ArgumentException("The scalingFactors array must have 3 elements.");

            int n = vertices.GetLength(0);  // number of vertices

            for (int i = 0; i < n; i++)
            {


                vertices[i, 0] *= scalingFactors[0];
                vertices[i, 1] *= scalingFactors[1];
                vertices[i, 2] *= scalingFactors[2];
            }
        }

        


    }
}