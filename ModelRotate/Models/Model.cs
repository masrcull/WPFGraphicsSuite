﻿using System;
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

namespace GraphicsCommon
{
    public class ExportModel
    {
        public double[][] vertices { get; set; }
        public int[][] edges { get; set; }
        public int[][] faces { get; set; }
        public int nVertices { get; set; }
        public int nEdges { get; set; }
        public int nFaces { get; set; }
        public int[] color { get; set; }
    }


    public class Model
    {
        public double[,] Vertices { get; set; }
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
        public double[] Centroid {
            get 
            {
                return LinearAlgebra.CalculateCentroid(this.Vertices);
            }
        }

        public Model()
        {

        }

        public Model(string filePath, double X, double Y, double Z) : this(filePath, new double[] { X, Y, Z}) { }

        public Model(string filePath, double[] initialCoordinates = null)
        {
            if (initialCoordinates == null)
            {
                initialCoordinates = new double[] { 0, 0, 0 };
            }


            var jsonString = File.ReadAllText(filePath);
            var modelData = JsonSerializer.Deserialize<ModelData>(jsonString);

            this.nVertices = modelData.nVertices;
            this.nEdges = modelData.nEdges;
            this.nFaces = modelData.nFaces;
            Vertices = new double[this.nVertices, 3];
            edges = new int[this.nEdges, 2];
            faces = new int[this.nFaces, 4];

            faces2 = new int[nFaces][];

            this.color = modelData.color;

            //faces2 = new List<int>[this.nFaces];
            

            for (int i = 0; i < modelData.nVertices; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this.Vertices[i, j] = modelData.vertices[i][j];
                }
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

        public Model(ModelData modelData, double? X = null, double? Y = null, double? Z = null )
        {
            double[] initialCoordinates;
            if (X == null || Y == null || Z == null)
            {
                initialCoordinates = new double[] { 0, 0, 0 };
            }
            else
                initialCoordinates = new double[] { (double)X, (double)Y, (double)Z };


            this.nVertices = modelData.nVertices;
            this.nEdges = modelData.nEdges;
            this.nFaces = modelData.nFaces;
            Vertices = new double[this.nVertices, 3];
            edges = new int[this.nEdges, 2];
            faces = new int[this.nFaces, 4];

            faces2 = new int[nFaces][];

            this.color = modelData.color;

            //faces2 = new List<int>[this.nFaces];


            for (int i = 0; i < modelData.nVertices; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    this.Vertices[i, j] = modelData.vertices[i][j];
                }
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

        public static Model CreateModel(string filePath, double[] initialCoordinates, Canvas mainStage)
        {
            var model = new Model("filePath");
            model.Translate(initialCoordinates);

            return model;
        }

        public Model(int nvertices, int nedges, int nfaces)
        {
            Vertices = new double[nvertices, 3];
            edges = new int[nedges, 2];
            faces = new int[nfaces, 4];
            nVertices = nvertices;
            nEdges = nedges;
            nFaces = nfaces;
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
                vertices = ArrayHelper.ToJaggedArray(model.Vertices)
            };
            return exportModel;
        }

        public void ExportModel(string filePath)
        {
            var jagged = ArrayHelper.ToJaggedArray(this.Vertices);

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

        public void RotateX(double radians, double[] centroid = null)
        {
            if(centroid == null)
                centroid = LinearAlgebra.CalculateCentroid(Vertices);
            LinearAlgebra.TranslateVertices(Vertices, new double[] { -centroid[0], -centroid[1], -centroid[2] });
            for (int i = 0; i < nVertices; i++)
            {
                var foo = LinearAlgebra.RotateAroundX(new double[] { Vertices[i, 0], Vertices[i, 1], Vertices[i, 2] }, radians);

                Vertices[i, 0] = foo[0];
                Vertices[i, 1] = foo[1];
                Vertices[i, 2] = foo[2];
            }
            LinearAlgebra.TranslateVertices(Vertices, new double[] { centroid[0], centroid[1], centroid[2] });
        }

        public void RotateY(double radians, double[] centroid = null)
        {
            if (centroid == null)
                centroid = LinearAlgebra.CalculateCentroid(Vertices);
            LinearAlgebra.TranslateVertices(Vertices, new double[] { -centroid[0], -centroid[1], -centroid[2] });
            for (int i = 0; i < nVertices; i++)
            {
                var foo = LinearAlgebra.RotateAroundY(new double[] { Vertices[i, 0], Vertices[i, 1], Vertices[i, 2] }, radians);

                Vertices[i, 0] = foo[0];
                Vertices[i, 1] = foo[1];
                Vertices[i, 2] = foo[2];
            }
            LinearAlgebra.TranslateVertices(Vertices, new double[] { centroid[0], centroid[1], centroid[2] });
        }

        public void RotateZ(double radians, double[] centroid = null, bool moveToOrigin = true)
        {
            if (centroid == null)
                centroid = LinearAlgebra.CalculateCentroid(Vertices);
            if (moveToOrigin) 
                LinearAlgebra.TranslateVertices(Vertices, new double[] { -centroid[0], -centroid[1], -centroid[2] });
            for (int i = 0; i < nVertices; i++)
            {
                var foo = LinearAlgebra.RotateAroundZ(new double[] { Vertices[i, 0], Vertices[i, 1], Vertices[i, 2] }, radians);

                Vertices[i, 0] = foo[0];
                Vertices[i, 1] = foo[1];
                Vertices[i, 2] = foo[2];
            }
            if (moveToOrigin) 
                LinearAlgebra.TranslateVertices(Vertices, new double[] { centroid[0], centroid[1], centroid[2] });
        }

        public void Scale(double X, double Y, double Z)
        {
            Scale(new double[] { X, Y, Z });
        }

        public void Scale(double scalar) => Scale(scalar, scalar, scalar);

        public void Scale(double[] scalars)
        { 
            // move model to origin
            var centroid = LinearAlgebra.CalculateCentroid(this.Vertices);
            LinearAlgebra.TranslateVertices(this.Vertices, new double[] { -centroid[0], -centroid[1], -centroid[2] });

            // model scaled
            LinearAlgebra.ScaleVertices(this.Vertices, new double[] { scalars[0], scalars[1], scalars[2] });

            // return to original position
            LinearAlgebra.TranslateVertices(this.Vertices, new double[] { centroid[0], centroid[1], centroid[2] });
        }

        public void SetColor(int R, int G, int B)
        {
            color = new int[] { R, G, B };
        }

        public void Translate(double[] coordinates)
        {
            LinearAlgebra.TranslateVertices(this.Vertices, coordinates);
        }

        public void Translate(double x, double y, double z)
        {
            Translate(new double[] { x, y, z });
        }

        public void DrawFaces(GraphicContextControl contextControl)
        {


            double xmin = -0.5;
            double xmax = 0.5;
            double ymin = -0.5;
            double ymax = 0.5;

            var foo = this.Vertices.Length;

            Point[] pictureVertices = new Point[this.Vertices.Length];
            //double scale = 800;
            for (int i = 0; i < this.Vertices.Length / 3; i++)
            {
                double x = this.Vertices[i, 0];
                double y = this.Vertices[i, 1];
                double z = this.Vertices[i, 2];
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

                    var faceNormal = CalculateNormal(Vertices, faceVertices);
                    var intensity = LinearAlgebra.DotProduct(faceNormal, new double[] {0,-1,0});



                    var adjustedColor = ColorHelper.CalculateIntensity(color[0], color[1], color[2], Math.Abs(intensity));

                    ShapeHelper.DrawPolygon(ArrayHelper.PointsToDoubleArray(points), new SolidColorBrush(Color.FromRgb(adjustedColor[0], adjustedColor[1], adjustedColor[2])), contextControl.MainStage);
                }
            }

            

        }

        public double[] CalculateNormal(double[,] verts, int[] faceVerts)
        {
            // Retrieve the vertices of the face
            double[] p0 = { verts[faceVerts[0], 0], verts[faceVerts[0], 1], verts[faceVerts[0], 2] };
            double[] p1 = { verts[faceVerts[1], 0], verts[faceVerts[1], 1], verts[faceVerts[1], 2] };
            double[] p2 = { verts[faceVerts[2], 0], verts[faceVerts[2], 1], verts[faceVerts[2], 2] };

            // Compute two vectors on the plane
            double[] v = { (p1[0] - p0[0]), (p1[1] - p0[1]), (p1[2] - p0[2]) };
            double[] w = { (p2[0] - p1[0]), (p2[1] - p1[1]), (p2[2] - p1[2]) };

            // Return the cross product of v and w
            return LinearAlgebra.CrossProduct(v, w);
        }

        public bool IsFaceVisible(int[] faceVerts, double[,] verts, double[] Eye)
        {
            double[] normal = CalculateNormal(verts, faceVerts);

            double[] p0 = { verts[faceVerts[0], 0], verts[faceVerts[0], 1], verts[faceVerts[0], 2] };

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

        public double[] CalculateVertexNormal(int vertex)
        {
            double XSum = 0;
            double YSum = 0;
            double ZSum = 0;
            int adjacentFaceCount = VertexFaceAdjacency[vertex].Length;

            for (int i = 0; i < adjacentFaceCount; i++)
            {
                var tempNormal = CalculateNormal(Vertices, faces2[VertexFaceAdjacency[vertex][i]]);
                XSum += tempNormal[0];
                YSum += tempNormal[1];
                ZSum += tempNormal[2];
            }

            return new double[] { XSum / adjacentFaceCount, YSum / adjacentFaceCount, ZSum / adjacentFaceCount };
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
