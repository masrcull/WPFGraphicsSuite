using System.Numerics;

namespace GraphicsCommon
{
    public class LinearAlgebra
    {
        public static Vector3 SubtractVectors(Vector3 v1, Vector3 v2)
        {
            return Vector3.Subtract(v1, v2);
        }

        public static Vector3 AddVectors(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Add(vector1, vector2);
        }

        public static double DegreeToRadians(double degree)
        {
            return degree * Math.PI / 180;
        }

        public static Vector3 Multiply(List<Vector3> matrix, Vector3 vector)
        {
            Vector3 result = new Vector3();

            result.X = matrix[0].X * vector.X + matrix[0].Y * vector.Y + matrix[0].Z * vector.Z;
            result.Y = matrix[1].X * vector.X + matrix[1].Y * vector.Y + matrix[1].Z * vector.Z;
            result.Z = matrix[2].X * vector.X + matrix[2].Y * vector.Y + matrix[2].Z * vector.Z;

            return result;
        }

        public static Vector3 RotateAroundX(Vector3 vertex, double angle)
        {
            // Rotation matrix for X axis
            List<Vector3> rotationMatrix = new List<Vector3>();
            rotationMatrix.Add(new Vector3(1, 0, 0 ));
            rotationMatrix.Add(new Vector3( 0, (float)Math.Cos(angle), (float)-Math.Sin(angle)));
            rotationMatrix.Add(new Vector3 ( 0, (float)Math.Sin(angle), (float)Math.Cos(angle)));

            return Multiply(rotationMatrix, vertex);
        }

        public static Vector3 RotateAroundY(Vector3 vertex, double angle)
        {
            // Rotation matrix for Y axis
            List<Vector3> rotationMatrix = new List<Vector3>();
            rotationMatrix.Add(new Vector3 ((float)Math.Cos(angle), 0, (float)Math.Sin(angle) ));
            rotationMatrix.Add(new Vector3( 0, 1, 0 ));
            rotationMatrix.Add(new Vector3( (float)-Math.Sin(angle), 0, (float)Math.Cos(angle) ));

            return Multiply(rotationMatrix, vertex);
        }

        public static Vector3 RotateAroundZ(Vector3 vertex, double angle)
        {
            // Rotation matrix for Z axis
            List<Vector3> rotationMatrix = new List<Vector3>();
            rotationMatrix.Add (new Vector3((float)Math.Cos(angle), (float)-Math.Sin(angle), 0 ));
            rotationMatrix.Add(new Vector3( (float)Math.Sin(angle), (float)Math.Cos(angle), 0 ));
            rotationMatrix.Add(new Vector3( 0, 0, 1 ));

            return Multiply(rotationMatrix, vertex);
        }

        public static Vector3 CrossProduct(Vector3 vec1, Vector3 vec2)
        {
            

            Vector3 result = new Vector3();
            result.X = vec1.Y * vec2.Z - vec1.Z * vec2.Y;
            result.Y = vec1.Z * vec2.X - vec1.X * vec2.Z;
            result.Z = vec1.X * vec2.Y - vec1.Y * vec2.X;

            return result;
        }

        public static float DotProduct(Vector3 v1, Vector3 v2)
        {
            return Vector3.Dot(v1, v2);
        }

        public static Vector3 CalculateCentroid(List<Vector3> vertices)
        {
            int n = vertices.Count;  // number of vertices

            if (n == 0)
                throw new ArgumentException("The vertices array must have at least one vertex.");

            float sumX = 0;
            float sumY = 0;
            float sumZ = 0;

            for (int i = 0; i < n; i++)
            {
                sumX += vertices[i].X;
                sumY += vertices[i].Y;
                sumZ += vertices[i].Z;
            }

            return new Vector3( sumX / n, sumY / n, sumZ / n );
        }

        public static Vector3 Normalize(Vector3 v)
        {
            float length = (float)Math.Sqrt(v.X * v.X + v.Y * v.Y + v.Z * v.Z);

            if (length == 0)
                return v; // prevent division by zero. Ideally, this should never be the case for normals.

            return new Vector3(v.X / length, v.Y / length, v.Z / length);
        }

        public static void TranslateVertices(List<Vector3> vertices, Vector3 translation)
        {
            int n = vertices.Count;  // number of vertices

            for (int i = 0; i < n; i++)
            {
                Vector3 translatedVector = new Vector3(
                    vertices[i].X + translation.X,
                    vertices[i].Y + translation.Y,
                    vertices[i].Z + translation.Z
                );

                vertices[i] = translatedVector;
            }
        }

        public static void ScaleVertices(List<Vector3> vertices, Vector3 scalingFactors)
        {
            int n = vertices.Count;  // number of vertices

            for (int i = 0; i < n; i++)
            {
                Vector3 scaledVector = new Vector3(
                    vertices[i].X * scalingFactors.X,
                    vertices[i].Y * scalingFactors.Y,
                    vertices[i].Z * scalingFactors.Z
                );

                vertices[i] = scaledVector;
            }
        }




    }
}