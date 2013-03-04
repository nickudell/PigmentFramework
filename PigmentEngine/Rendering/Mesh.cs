using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D11;
using System.Collections.Generic;
using System.IO;
using System;
using Pigment.WPF;

namespace Pigment.Engine.Rendering
{
    /// <summary>
    /// A mesh
    /// </summary>
    /// <typeparam name="V">The type of Vertex the mesh uses</typeparam>
    /*public class Mesh<V> : RenderableBase<V>
        where V : struct, IPositioned
    {
        public Texture[] Textures { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh{V}" /> class and builds the first vertexbuffer
        /// </summary>
        /// <param name="device">The Direct3D11 device to use.</param>
        /// <param name="vertices">The vertices of this mesh.</param>
        /// <param name="vertexTopology">The vertex topology.</param>
        public Mesh(Device device, List<V> vertices, PrimitiveTopology vertexTopology, string[] textureFileNames) : base(device,vertices,vertexTopology)
        {
            Textures = new Texture[textureFileNames.Length];
            for (int i = 0; i < textureFileNames.Length; i++)
            {
                Textures[i] = new Rendering.Texture(device, textureFileNames[i]);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool managed)
        {
            if (managed)
            {
                if (Textures != null)
                {
                    foreach (Texture texture in Textures)
                    {
                        texture.Dispose();
                    }
                    Textures = null;
                }
            }
            base.Dispose(managed);
        }

        /// <summary>
        /// Creates a triangle mesh
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns></returns>
        public static Mesh<PositionVertex> Triangle(Device device)
        {
            List<PositionVertex> vertices = new List<PositionVertex>();
            vertices.AddRange(new PositionVertex[]{new PositionVertex(0f, 0.5f, 0.5f), new PositionVertex(0.5f, -0.5f, 0.5f), new PositionVertex(-0.5f, -0.5f, 0.5f) });
            return new Mesh<PositionVertex>(device, vertices, PrimitiveTopology.TriangleList,new string[] {"texName.png"});
        }

        /// <summary>
        /// Face struct for .obj format
        /// </summary>
        private struct Face
        {
            /// <summary>
            /// The vertex indices
            /// </summary>
            public short[] VertexIndices;
            /// <summary>
            /// The tex coord indices
            /// </summary>
            public short[] TexCoordIndices;
            /// <summary>
            /// The normal indices
            /// </summary>
            public short[] NormalIndices;
        }

        /// <summary>
        /// Creates a mesh from a .obj file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static Mesh<TexturedNormalVertex> FromOBJ(Device device, string fileName)
        {
            List<TexturedNormalVertex> vertices= new List<TexturedNormalVertex>();
            List<Vector3> vertexPositions;
            List<Vector3> normals;
            List<Vector2> texCoords;
            List<Face> faces;

            LoadObj(fileName, out vertexPositions, out normals, out texCoords, out faces);

            foreach (Face face in faces)
            {
                TexturedNormalVertex[] faceVertices = BuildFace(vertexPositions, normals, texCoords, face);
                vertices.AddRange(faceVertices);
            }

            return new Mesh<TexturedNormalVertex>(device, vertices, PrimitiveTopology.TriangleList, new string[]{System.IO.Directory.GetCurrentDirectory() + "/Textures/StoneTiles.jpg"});
        }

        /// <summary>
        /// Creates a mesh from a .obj file
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static Mesh<PositionVertex> FromOBJJustPosition(Device device, string fileName)
        {
            List<PositionVertex> vertices = new List<PositionVertex>();
            List<Vector3> vertexPositions;
            List<Vector3> normals;
            List<Vector2> texCoords;
            List<Face> faces;

            LoadObj(fileName, out vertexPositions, out normals, out texCoords, out faces);

            foreach (Face face in faces)
            {
                TexturedNormalVertex[] faceVertices = BuildFace(vertexPositions, normals, texCoords, face);
                foreach (TexturedNormalVertex tnVertex in faceVertices)
                {
                    vertices.Add(new PositionVertex(tnVertex.Position));
                }
            }

            return new Mesh<PositionVertex>(device, vertices, PrimitiveTopology.TriangleList, new string[] { });
        }

        private static void LoadObj(string fileName, out List<Vector3> vertexPositions, out List<Vector3> normals, out List<Vector2> texCoords, out List<Face> faces)
        {
            StreamReader sr = new StreamReader(File.OpenRead(fileName));
            
            vertexPositions = new List<Vector3>();
            List<short> indices = new List<short>();
            normals = new List<Vector3>();
            texCoords = new List<Vector2>();
            faces = new List<Face>();
            while (!sr.EndOfStream)
            {
                //get the line
                string[] fields = sr.ReadLine().Split(' ');
                if (fields[0].Length > 0)
                {
                    if (fields[0].ToLower()[0] == 'v') //vertex
                    {
                        if (fields[0].Length == 1) //vertex position
                        {
                            vertexPositions.Add(new Vector3(Convert.ToSingle(fields[1]), Convert.ToSingle(fields[2]), Convert.ToSingle(fields[3]) * -1f));
                        }
                        else
                        {
                            if (fields[0].ToLower()[1] == 't') //texture coordinates
                            {
                                texCoords.Add(new Vector2(Convert.ToSingle(fields[1]), 1f - Convert.ToSingle(fields[2])));
                            }
                            else
                            {
                                if (fields[0].ToLower()[1] == 'n') //normals
                                {
                                    normals.Add(new Vector3(Convert.ToSingle(fields[1]), Convert.ToSingle(fields[2]), Convert.ToSingle(fields[3]) * -1f));
                                }
                            }
                        }
                    }
                    else if (fields[0].ToLower() == "f") //face
                    {
                        //Read the face data backwards to convert it to a left hand system
                        Face face = new Face();

                        face.VertexIndices = new short[3];
                        face.TexCoordIndices = new short[3];
                        face.NormalIndices = new short[3];

                        string[] subfields = fields[1].Split('/');
                        face.VertexIndices[2] = Convert.ToInt16(subfields[0]);
                        face.TexCoordIndices[2] = Convert.ToInt16(subfields[1]);
                        face.NormalIndices[2] = Convert.ToInt16(subfields[2]);

                        subfields = fields[2].Split('/');
                        face.VertexIndices[1] = Convert.ToInt16(subfields[0]);
                        face.TexCoordIndices[1] = Convert.ToInt16(subfields[1]);
                        face.NormalIndices[1] = Convert.ToInt16(subfields[2]);

                        subfields = fields[3].Split('/');
                        face.VertexIndices[0] = Convert.ToInt16(subfields[0]);
                        face.TexCoordIndices[0] = Convert.ToInt16(subfields[1]);
                        face.NormalIndices[0] = Convert.ToInt16(subfields[2]);
                        faces.Add(face);


                    }
                }
            }
            sr.Close();
        }

        public static Mesh<TexturedNormalTangentBinormalVertex> FromObjWithBinormals(Device device, string fileName)
        {
            List<TexturedNormalTangentBinormalVertex> vertices = new List<TexturedNormalTangentBinormalVertex>();
            List<Vector3> vertexPositions;
            List<Vector3> normals;
            List<Vector2> texCoords;
            List<Face> faces;
            LoadObj(fileName, out vertexPositions, out normals, out texCoords, out faces);

            foreach (Face face in faces)
            {
                TexturedNormalVertex[] faceVertices = BuildFace(vertexPositions, normals, texCoords, face);

                //Calculate binormal and tangent
                Vector3 vector1, vector2;
                Vector2 texCoord1, texCoord2;

                vector1 = new Vector3(faceVertices[1].Position.X - faceVertices[0].Position.X, faceVertices[1].Position.Y - faceVertices[0].Position.Y, faceVertices[1].Position.Z- faceVertices[0].Position.Z);
                vector2 = new Vector3(faceVertices[2].Position.X - faceVertices[0].Position.X, faceVertices[2].Position.Y - faceVertices[0].Position.Y, faceVertices[2].Position.Z - faceVertices[0].Position.Z);

                texCoord1 = new Vector2(faceVertices[1].TexCoords.X - faceVertices[0].TexCoords.X, faceVertices[1].TexCoords.Y - faceVertices[0].TexCoords.Y);
                texCoord2 = new Vector2(faceVertices[2].TexCoords.X - faceVertices[0].TexCoords.X, faceVertices[2].TexCoords.Y - faceVertices[0].TexCoords.Y);

                float den = 1.0f / (texCoord1.X * texCoord2.Y - texCoord2.X * texCoord1.Y);

                Vector3 tangent = new Vector3((texCoord2.Y * vector1.X - texCoord1.Y * vector2.X) * den, (texCoord2.Y * vector1.Y - texCoord1.Y * vector2.Y) * den, (texCoord2.Y * vector1.Z - texCoord1.Y * vector2.Z) * den);
                Vector3 binormal = new Vector3((texCoord1.X * vector2.X - texCoord2.X * vector1.X) * den, (texCoord1.X * vector2.Y - texCoord2.X * vector1.Y) * den, (texCoord1.X * vector2.Z - texCoord2.X * vector2.Z) * den);

                binormal.Normalize();
                //Recalculate normal
                Vector3 normal = Vector3.Cross(tangent, binormal);
                normal.Normalize();

                foreach (TexturedNormalVertex faceVertex in faceVertices)
                {
                    vertices.Add(new TexturedNormalTangentBinormalVertex(faceVertex.Position, faceVertex.TexCoords, normal, tangent, binormal));
                }
            }
            return new Mesh<TexturedNormalTangentBinormalVertex>(device, vertices, PrimitiveTopology.TriangleList, new string[]{System.IO.Directory.GetCurrentDirectory() + "/Textures/metal.jpg",
                                                                                                                                System.IO.Directory.GetCurrentDirectory() + "/Textures/metal_n.jpg"});
        }

        private static TexturedNormalVertex[] BuildFace(List<Vector3> vertexPositions, List<Vector3> normals, List<Vector2> texCoords, Face face)
        {
            TexturedNormalVertex[] vertex = new TexturedNormalVertex[3];
            int vertexIndex = face.VertexIndices[0] - 1;
            int texIndex = face.TexCoordIndices[0] - 1;
            int normIndex = face.NormalIndices[0] - 1;
            vertex[0] = new TexturedNormalVertex(vertexPositions[vertexIndex], texCoords[texIndex], normals[normIndex]);
            vertexIndex = face.VertexIndices[1] - 1;
            texIndex = face.TexCoordIndices[1] - 1;
            normIndex = face.NormalIndices[1] - 1;
            vertex[1] = new TexturedNormalVertex(vertexPositions[vertexIndex], texCoords[texIndex], normals[normIndex]);
            vertexIndex = face.VertexIndices[2] - 1;
            texIndex = face.TexCoordIndices[2] - 1;
            normIndex = face.NormalIndices[2] - 1;
            vertex[2] = new TexturedNormalVertex(vertexPositions[vertexIndex], texCoords[texIndex], normals[normIndex]);
            return vertex;
        }
    }*/
}