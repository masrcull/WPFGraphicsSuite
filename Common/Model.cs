using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GraphicsCommon
{
    public class Model
    {
        public double[,] Vertices { get; set; }
        public int[,] Edges { get; set; }
        public int[,] Faces { get; set; }
        public int nVertices { get; set; }
        public int nEdges { get; set; }
        public int nFaces { get; set; }
        public Model(int nvertices, int nedges, int nfaces)
        {
            Vertices = new double[nvertices, 3];
            Edges = new int[nedges, 2];
            Faces = new int[nfaces, 4];
            nVertices = nvertices;
            nEdges = nedges;
            nFaces = nfaces;
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

        // You can add other properties like Textures, Normals, Metadata, etc. as needed.

        public static Model ReadCoordinatesFromJson(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            var modelData = JsonSerializer.Deserialize<ModelData>(jsonString);

            var model = new Model(modelData.nVertices,
                                        modelData.nEdges,
                                        modelData.nFaces);




            for (int i = 0; i < modelData.nVertices; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    model.Vertices[i, j] = modelData.vertices[i][j];
                }
            }



            for (int i = 0; i < modelData.edges.Length; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    model.Edges[i, j] = modelData.edges[i][j];
                }
            }



            for (int i = 0; i < modelData.faces.Length; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    model.Faces[i, j] = modelData.faces[i][j];
                }
            }

            model.nVertices = modelData.nVertices;
            model.nEdges = modelData.nEdges;
            model.nFaces = modelData.nFaces;



            return model;
        }

    }
}
