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

namespace GraphicsCommon
{
    public class Model
    {
        public double[,] Vertices { get; set; }
        public int[,] edges { get; set; }
        //public List<int>[] faces2 { get; set; }
        public int[][] faces2 { get; set; }
        public int[,] faces { get; set; }
        public int nVertices { get; set; }
        public int nEdges { get; set; }
        public int nFaces { get; set; }
        public Canvas MainStage { get; set; }

        //public Model(string filePath, Canvas mainStage) : this(filePath, mainStage)
        //{
            
        //}

        //public Model(string filePath, Canvas mainStage) : this(filePath)
        //{
        //     MainStage = mainStage;
        //}

        public Model(string filePath, Canvas mainStage, double[] initialCoordinates = null)
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



            //for (int i = 0; i < modelData.faces.Length; i++)
            //{
            //    for (int j = 0; j < 4; j++)
            //    {
            //        this.faces[i, j] = modelData.faces[i][j];
            //    }
            //}

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

        public static Model CreateModel(string filePath, double[] initialCoordinates, Canvas mainStage)
        {
            var model = new Model("filePath", mainStage);
            model.Translate(initialCoordinates);

            return model;
        }

        //public static Model CreateModel(string filePath, double[] initialCoordinates, double[] scale)
        //{
        //    Model model = new Model("filePath");
        //    model.Translate(initialCoordinates);
        //    model.Scale(scale);

        //    return model;
        //}

        public Model(int nvertices, int nedges, int nfaces)
        {
            Vertices = new double[nvertices, 3];
            edges = new int[nedges, 2];
            faces = new int[nfaces, 4];
            nVertices = nvertices;
            nEdges = nedges;
            nFaces = nfaces;
        }

        public void RotateX(double radians)
        {
            var centroid = LinearAlgebra.CalculateCentroid(Vertices);
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

        public void RotateY(double radians)
        {
            var centroid = LinearAlgebra.CalculateCentroid(Vertices);
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

        public void RotateZ(double radians, bool moveToOrigin = true)
        {
            var centroid = LinearAlgebra.CalculateCentroid(Vertices);
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

        public void Translate(double[] coordinates)
        {
            LinearAlgebra.TranslateVertices(this.Vertices, coordinates);
        }

        public void DrawFaces(SolidColorBrush color, Canvas gp, double[] eye )
        {


            double xmin = -0.5;
            double xmax = 0.5;
            double ymin = -0.5;
            double ymax = 0.5;

            var foo = this.Vertices.Length;

            Point[] pictureVertices = new Point[this.Vertices.Length];
            double scale = 800;
            for (int i = 0; i < this.Vertices.Length / 3; i++)
            {
                double x = this.Vertices[i, 0];
                double y = this.Vertices[i, 1];
                double z = this.Vertices[i, 2];
                double xprime = x / z;
                double yprime = y / z;
                var foo2 = (1 - (xprime - xmin) / (xmax - xmin));
                pictureVertices[i].X = scale * (1 - (xprime - xmin) / (xmax - xmin));
                pictureVertices[i].Y = scale * (yprime - ymin) / (ymax - ymin); // x / z

            }

            for (int i = 0; i < this.nFaces; i++)
            {

                int[] faceVertices = new int[this.faces2[i].Length];

                for (int j = 0; j < this.faces2[i].Length; j++)
                {
                    faceVertices[j] = this.faces2[i][j];
                }



                if (IsFaceVisible(faceVertices, this.Vertices, eye))
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
                    ShapeHelper.DrawPolygon(points, color, gp);
                }
            }

            

        }

        public bool IsFaceVisible(int[] faceVerts, double[,] verts, double[] Eye)
        {

            double[] p0 = new double[] { verts[faceVerts[0], 0], verts[faceVerts[0], 1], verts[faceVerts[0], 2] };
            double[] p1 = new double[] { verts[faceVerts[1], 0], verts[faceVerts[1], 1], verts[faceVerts[1], 2] };
            double[] p2 = new double[] { verts[faceVerts[2], 0], verts[faceVerts[2], 1], verts[faceVerts[2], 2] };

            double[] v = new double[] { (p1[0] - p0[0]), (p1[1] - p0[1]), (p1[2] - p0[2]) };
            double[] w = new double[] { (p2[0] - p1[0]), (p2[1] - p1[1]), (p2[2] - p1[2]) };

            var crossProduct = LinearAlgebra.CrossProduct(v, w);

            return (LinearAlgebra.DotProduct(crossProduct, LinearAlgebra.SubtractVectors(p0, Eye)) < 0);
        }



    }



    public class ModelData
    {
        public double[][] vertices { get; set; }
        public int[][] edges { get; set; }
        public int[][] faces { get; set; }
        public int nVertices { get; set; }
        public int nEdges { get; set; }
        public int nFaces { get; set; }

    }
}
