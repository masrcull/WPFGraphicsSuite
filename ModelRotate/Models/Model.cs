using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using ModelRender.Helpers;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using ModelRender.Models;
using System.Numerics;

namespace GraphicsCommon
{
    public class ExportModel
    {
        public float[][] vertices { get; set; }
        public int[][] edges { get; set; }
        public int[][] faces { get; set; }
        public int nVertices { get; set; }
        public int nEdges { get; set; }
        public int nFaces { get; set; }
        public int[] color { get; set; }
    }


    public class Model
    {
        public List<Vector3> Vertices { get; set; }
        private int[][] _vertexFaceAdjacency;
        public int[][] VertexFaceAdjacency
        {
            get
            {
                if(_vertexFaceAdjacency == null)
                {
                    CalculateAdjacentFaceMatrix();
                }
                return _vertexFaceAdjacency;
            }
        }
        public int[,] edges { get; set; }
        //public List<int>[] faces2 { get; set; }
        public int[][] faces2 { get; set; }
        public int[,] faces { get; set; }
        public int nVertices { get; set; }
        public int nEdges { get; set; }
        public int nFaces { get; set; }
        public int[] color { get; set; }
        public Canvas MainStage { get; set; }
        public Vector3 Centroid {
            get 
            {
                return LinearAlgebra.CalculateCentroid(this.Vertices);
            }
        }

        public Model()
        {

        }

        public Model(string filePath, float X, float Y, float Z) : this(filePath, new Vector3( X, Y, Z)) { }

        public Model(string filePath, Vector3? initialCoordinates = null)
        {
            if (initialCoordinates == null)
            {
                initialCoordinates = new Vector3(0, 0, 0 );
            }


            var jsonString = File.ReadAllText(filePath);
            var modelData = JsonSerializer.Deserialize<ModelData>(jsonString);

            this.nVertices = modelData.nVertices;
            this.nEdges = modelData.nEdges;
            this.nFaces = modelData.nFaces;
            Vertices = new List<Vector3>();
            //Vertices = new double[this.nVertices, 3];
            edges = new int[this.nEdges, 2];
            faces = new int[this.nFaces, 4];

            faces2 = new int[nFaces][];

            this.color = modelData.color;

            //faces2 = new List<int>[this.nFaces];
            

            for (int i = 0; i < modelData.nVertices; i++)
            {

                this.Vertices = ArrayHelper.Double2DToVector3List(modelData.vertices);

                //for (int j = 0; j < 3; j++)
                //{
                //    this.Vertices[i, j] = modelData.vertices[i][j];
                //}
            }

            for (int i = 0; i < modelData.edges.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    this.edges[i, j] = modelData.edges[i][j];
                }
            }

            for (int i = 0; i < modelData.faces.Length; i++)
            {
                //this.faces2[i] = new List<int>();
                this.faces2[i] = new int[modelData.faces[i].Length];
                for (int j = 0; j < modelData.faces[i].Length; j++)
                {
                    this.faces2[i][j] = modelData.faces[i][j];
                }
            }

            this.Translate(initialCoordinates.Value);


        }

        public Model(ModelData modelData, float? X = null, float? Y = null, float? Z = null )
        {
            Vector3 initialCoordinates;
            if (X == null || Y == null || Z == null)
            {
                initialCoordinates = new Vector3( 0, 0, 0 );
            }
            else
                initialCoordinates = new Vector3( X.Value, Y.Value, Z.Value);


            this.nVertices = modelData.nVertices;
            this.nEdges = modelData.nEdges;
            this.nFaces = modelData.nFaces;
            Vertices = new List<Vector3>();
            edges = new int[this.nEdges, 2];
            faces = new int[this.nFaces, 4];

            faces2 = new int[nFaces][];

            this.color = modelData.color;

            //faces2 = new List<int>[this.nFaces];


            for (int i = 0; i < modelData.nVertices; i++)
            {
                this.Vertices = ArrayHelper.Double2DToVector3List(modelData.vertices);
                //for (int j = 0; j < 3; j++)
                //{
                //    this.Vertices[i, j] = modelData.vertices[i][j];
                //}
            }

            for (int i = 0; i < modelData.edges.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    this.edges[i, j] = modelData.edges[i][j];
                }
            }

            for (int i = 0; i < modelData.faces.Length; i++)
            {
                //this.faces2[i] = new List<int>();
                this.faces2[i] = new int[modelData.faces[i].Length];
                for (int j = 0; j < modelData.faces[i].Length; j++)
                {
                    this.faces2[i][j] = modelData.faces[i][j];
                }
            }

            this.Translate(initialCoordinates);
        }

        public static ModelData GetModelData(string filePath, double X, double Y, double Z)
        {
            var initialCoordinates = new double[] { X, Y, Z };
            if (initialCoordinates == null)
            {
                initialCoordinates = new double[] { 0, 0, 0 };
            }

            var jsonString = File.ReadAllText(filePath);
            var modelData = JsonSerializer.Deserialize<ModelData>(jsonString);

            return modelData;
        }

        

        private static ExportModel ToExportModel(Model model)
        {
            var exportModel = new ExportModel
            {
                nEdges = model.nEdges,
                nFaces = model.nFaces,
                nVertices = model.nVertices,
                edges = ArrayHelper.ToJaggedArray(model.edges),
                faces = model.faces2,
                vertices = ArrayHelper.Vec3ToFloat2DArray(model.Vertices)
            };
            return exportModel;
        }

        public void ExportModel(string filePath)
        {
            var jagged = ArrayHelper.Vec3ToDouble2DArray(this.Vertices);

            var exportModel = ToExportModel(this);
            var stringy = JsonSerializer.Serialize(exportModel/*, new JsonSerializerOptions { WriteIndented = true }*/);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, stringy);
        }

        public static void ExportModel(string filePath, ExportModel exportModel)
        {
            var stringy = JsonSerializer.Serialize(exportModel/*, new JsonSerializerOptions { WriteIndented = true }*/);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, stringy);
        }

        //public void RotateX(double radians, Vector3? centroid = null)
        //{
        //    if(centroid == null)
        //        centroid = LinearAlgebra.CalculateCentroid(Vertices);
        //    LinearAlgebra.TranslateVertices(Vertices, new Vector3( -centroid.Value.X, -centroid.Value.Y, -centroid.Value.Y ));
        //    for (int i = 0; i < nVertices; i++)
        //    {
        //        var vertex = LinearAlgebra.RotateAroundX(new Vector3(Vertices[i].X, Vertices[i].Y, Vertices[i].Z ), radians);

        //        Vertices[i].X = vertex[0];
        //        Vertices[i].Y = vertex[1];
        //        Vertices[i].Z = vertex[2];
        //    }
        //    LinearAlgebra.TranslateVertices(Vertices, new double[] { centroid[0], centroid[1], centroid[2] });
        //}

        public void RotateX(double radians, Vector3? centroid = null)
        {
            if (centroid == null)
                centroid = LinearAlgebra.CalculateCentroid(Vertices);

            LinearAlgebra.TranslateVertices(Vertices, new Vector3(-centroid.Value.X, -centroid.Value.Y, -centroid.Value.Z));

            for (int i = 0; i < Vertices.Count; i++)
            {
                var vertex = LinearAlgebra.RotateAroundX(new Vector3(Vertices[i].X, Vertices[i].Y, Vertices[i].Z), radians);

                Vector3 modifiedVertex = Vertices[i];
                modifiedVertex.X = vertex.X;
                modifiedVertex.Y = vertex.Y;
                modifiedVertex.Z = vertex.Z;
                Vertices[i] = modifiedVertex;
            }

            LinearAlgebra.TranslateVertices(Vertices, new Vector3(centroid.Value.X, centroid.Value.Y, centroid.Value.Z));
        }

        public void RotateY(double radians, Vector3? centroid = null)
        {
            if (centroid == null)
                centroid = LinearAlgebra.CalculateCentroid(Vertices);
            LinearAlgebra.TranslateVertices(Vertices, new Vector3(-centroid.Value.X, -centroid.Value.Y, -centroid.Value.Z));
            for (int i = 0; i < nVertices; i++)
            {
                Vector3 vertex = LinearAlgebra.RotateAroundY(Vertices[i], radians);
                Vertices[i] = vertex;  // Assign the entire vector back into the list
            }
            LinearAlgebra.TranslateVertices(Vertices, centroid.Value);
        }

        public void RotateZ(double radians, Vector3? centroid = null, bool moveToOrigin = true)
        {
            if (centroid == null)
                centroid = LinearAlgebra.CalculateCentroid(Vertices);
            if (moveToOrigin)
                LinearAlgebra.TranslateVertices(Vertices, new Vector3(-centroid.Value.X, -centroid.Value.Y, -centroid.Value.Z));
            for (int i = 0; i < nVertices; i++)
            {
                Vector3 vertex = LinearAlgebra.RotateAroundZ(Vertices[i], radians);
                Vertices[i] = vertex;  // Assign the entire vector back into the list
            }
            if (moveToOrigin)
                LinearAlgebra.TranslateVertices(Vertices, centroid.Value);
        }

        public void Scale(float X, float Y, float Z)
        {
            Scale(new Vector3( X, Y, Z ));
        }

        public void Scale(float scalar) => Scale(scalar, scalar, scalar);

        public void Scale(Vector3 scalars)
        { 
            // move model to origin
            var centroid = LinearAlgebra.CalculateCentroid(this.Vertices);
            LinearAlgebra.TranslateVertices(this.Vertices, new Vector3 ( -centroid.X, -centroid.Y, -centroid.Z ));

            // model scaled
            LinearAlgebra.ScaleVertices(this.Vertices, new Vector3(scalars.X, scalars.Y, scalars.Z ));

            // return to original position
            LinearAlgebra.TranslateVertices(this.Vertices, new Vector3(centroid.X, centroid.Y, centroid.Z ));
        }

        public void SetColor(int R, int G, int B)
        {
            color = new int[] { R, G, B };
        }

        public void Translate(Vector3 coordinates)
        {
            LinearAlgebra.TranslateVertices(this.Vertices, coordinates);
        }

        public void Translate(float x, float y, float z)
        {
            Translate(new Vector3(x, y, z ));
        }

        public void DrawFaces(GraphicContextControl contextControl)
        {


            double xmin = -0.5;
            double xmax = 0.5;
            double ymin = -0.5;
            double ymax = 0.5;

            var foo = this.Vertices.Count;

            Point[] pictureVertices = new Point[this.Vertices.Count];
            //double scale = 800;
            for (int i = 0; i < this.Vertices.Count; i++)
            {
                double x = this.Vertices[i].X;
                double y = this.Vertices[i].Y;
                double z = this.Vertices[i].Z;
                double xprime = x / z;
                double yprime = y / z;

                pictureVertices[i].X = contextControl.Width * ((xprime - xmin) / (xmax - xmin));
                pictureVertices[i].Y = contextControl.Height * (1 - (yprime - ymin) / (ymax - ymin)); 

            }

            for (int i = 0; i < this.nFaces; i++)
            {

                int[] faceVertices = new int[this.faces2[i].Length];

                for (int j = 0; j < this.faces2[i].Length; j++)
                {
                    faceVertices[j] = this.faces2[i][j];
                }



                if (IsFaceVisible(faceVertices, this.Vertices, contextControl.Eye))
                {
                    var points = new Point[faceVertices.Length];
                    for (int j = 0; j < faceVertices.Length - 1; j++)
                    {
                        points[j] = pictureVertices[this.faces2[i][j]];
                        //gp.Children.Add(new Line { X1 = pictureVertices[model.Faces[i, j]].X, Y1 = pictureVertices[model.Faces[i, j]].Y, X2 = pictureVertices[model.Faces[i, j + 1]].X, Y2 = pictureVertices[model.Faces[i, j + 1]].Y, Stroke = new SolidColorBrush(Colors.Black) });
                        if (j == faceVertices.Length - 2)
                        {

                            //gp.Children.Add(new Line { X1 = pictureVertices[model.Faces[i, 0]].X, Y1 = pictureVertices[model.Faces[i, 0]].Y, X2 = pictureVertices[model.Faces[i, faceVertices.Length - 1]].X, Y2 = pictureVertices[model.Faces[i, faceVertices.Length - 1]].X, Stroke = new SolidColorBrush(Colors.Black) });
                            points[j + 1] = pictureVertices[this.faces2[i][ faceVertices.Length - 1]];
                        }

                    }

                    if (contextControl.DirectionalLightEnabled)
                    {
                        var faceNormal = CalculateNormal(Vertices, faceVertices);
                        var intensity = LinearAlgebra.DotProduct(faceNormal, contextControl.DirectionLight);
                        var adjustedColor = ColorHelper.CalculateIntensity(color[0], color[1], color[2], Math.Abs(intensity));
                        ShapeHelper.DrawPolygon(points.ToList(), new SolidColorBrush(Color.FromRgb(adjustedColor[0], adjustedColor[1], adjustedColor[2])), contextControl.MainStage);
                    }
                    else
                    {
                        ShapeHelper.DrawPolygon(points.ToList(), new SolidColorBrush(Color.FromRgb((byte)this.color[0], (byte)this.color[1], (byte)this.color[2])), contextControl.MainStage);
                    }
                   


                    
                }
            }

            

        }

        public Vector3 CalculateNormal(List<Vector3> verts, int[] faceVerts)
        {
            // Retrieve the vertices of the face
            var p0 = new Vector3( verts[faceVerts[0]].X, verts[faceVerts[0]].Y, verts[faceVerts[0]].Z );
            var p1 = new Vector3(verts[faceVerts[1]].X, verts[faceVerts[1]].Y, verts[faceVerts[1]].Z );
            var p2 = new Vector3(verts[faceVerts[2]].X, verts[faceVerts[2]].Y, verts[faceVerts[2]].Z);

            // Compute two vectors on the plane
            var v = new Vector3((p1.X - p0.X), (p1.Y - p0.Y), (p1.Z - p0.Z) );
            var w = new Vector3( (p2.X - p1.X), (p2.Y - p1.Y), (p2.Z - p1.Z) );

            // Return the cross product of v and w
            return LinearAlgebra.CrossProduct(v, w);
        }

        public bool IsFaceVisible(int[] faceVerts, List<Vector3> verts, Vector3 Eye)
        {
            Vector3 normal = CalculateNormal(verts, faceVerts);

            Vector3 p0 = new Vector3(verts[faceVerts[0]].X, verts[faceVerts[0]].Y, verts[faceVerts[0]].Z );

            // Check the orientation of the face with respect to the viewpoint using the dot product
            return (LinearAlgebra.DotProduct(normal, LinearAlgebra.SubtractVectors(p0, Eye)) < 0);
        }

        private void CalculateAdjacentFaceMatrix()
        {
            int[,] adjacentFaces = new int[nVertices,nFaces];

            for (int i = 0; i < faces2.Length; i++)
            {
                for(int j = 0; j < faces2[i].Length; j++)
                {
                    adjacentFaces[i,j] = faces2[i][j];
                }
                
            }

            _vertexFaceAdjacency = ArrayHelper.ToJaggedArray(adjacentFaces);
        }

        public Vector3 CalculateVertexNormal(int vertex)
        {
            float XSum = 0;
            float YSum = 0;
            float ZSum = 0;
            int adjacentFaceCount = VertexFaceAdjacency[vertex].Length;

            for (int i = 0; i < adjacentFaceCount; i++)
            {
                var tempNormal = CalculateNormal(Vertices, faces2[VertexFaceAdjacency[vertex][i]]);
                XSum += tempNormal.X;
                YSum += tempNormal.Y;
                ZSum += tempNormal.Z;
            }

            return new Vector3(XSum / adjacentFaceCount, YSum / adjacentFaceCount, ZSum / adjacentFaceCount );
        }








        //public bool IsFaceVisible(int[] faceVerts, double[,] verts, double[] Eye)
        //{

        //    double[] p0 = new double[] { verts[faceVerts[0], 0], verts[faceVerts[0], 1], verts[faceVerts[0], 2] };
        //    double[] p1 = new double[] { verts[faceVerts[1], 0], verts[faceVerts[1], 1], verts[faceVerts[1], 2] };
        //    double[] p2 = new double[] { verts[faceVerts[2], 0], verts[faceVerts[2], 1], verts[faceVerts[2], 2] };

        //    double[] v = new double[] { (p1[0] - p0[0]), (p1[1] - p0[1]), (p1[2] - p0[2]) };
        //    double[] w = new double[] { (p2[0] - p1[0]), (p2[1] - p1[1]), (p2[2] - p1[2]) };

        //    var crossProduct = LinearAlgebra.CrossProduct(v, w);

        //    return (LinearAlgebra.DotProduct(crossProduct, LinearAlgebra.SubtractVectors(p0, Eye)) < 0);
        //}



    }



    public class ModelData
    {
        public double[][] vertices { get; set; }
        public int[][] edges { get; set; }
        public int[][] faces { get; set; }
        public int nVertices { get; set; }
        public int nEdges { get; set; }
        public int nFaces { get; set; }
        public int[] color { get; set; }

    }
}
