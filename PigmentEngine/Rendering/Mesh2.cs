using System.Runtime.InteropServices;
using SlimDX;
using SlimDX.Direct3D11;
using System.Collections.Generic;
using System.IO;
using System;
using Pigment.WPF;
using System.Globalization;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Rendering
{
    /// <summary>
    /// A mesh object for handling .obj file loading, vertex / index buffer storage and rendering and for pairing meshes with materials (to do)
    /// </summary>
    /// <typeparam name="V">The type of Vertex the mesh uses</typeparam>
    public class Mesh<V> : RenderableBase<V>
        where V : VertexPos
    {
        public Texture[] Textures { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh{V}" /> class and builds the first vertexbuffer
        /// </summary>
        /// <param name="device">The Direct3D11 device to use.</param>
        /// <param name="vertices">The vertices of this mesh.</param>
        /// <param name="vertexTopology">The vertex topology.</param>
        /// <param name="textureFileNames">The texture file names. textureFileNames[0] is diffuse, and textureFileNames[1] is normal mapping</param>
        public Mesh(Device device, List<V> vertices, PrimitiveTopology vertexTopology, string[] textureFileNames) : base(device,vertices,vertexTopology)
        {
            Contract.Requires<ArgumentNullException>(device != null, "device");
            Contract.Requires<ArgumentNullException>(vertices != null, "vertices");
            Contract.Requires<ArgumentException>(textureFileNames.Length > 0,"textureFileNames");

            Textures = new Texture[textureFileNames.Length];
            for (int i = 0; i < textureFileNames.Length; i++)
            {
                //Create the Texture from the image file
                Textures[i] = new Rendering.Texture(device, textureFileNames[i]);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool managed)
        {
            Contract.Ensures(!managed || Textures == null, "If managed is true, Textures must be set to null by this function.");
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
        /// Face struct for .obj format
        /// </summary>
        private struct Face
        {
            /// <summary>
            /// The vertex indices
            /// </summary>
            public short[] VertexIndices;
            /// <summary>
            /// The texture coordinate indices
            /// </summary>
            public short[] TexCoordIndices;
            /// <summary>
            /// The normal indices
            /// </summary>
            public short[] NormalIndices;
        }

        /// <summary>
        /// Loads the a .obj file and returns a list of vertex positions, a list of normals, a list of texture coordinates and a list of faces for reconstituting into vertex buffers.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="vertexPositions">The list of vertex positions.</param>
        /// <param name="normals">The list of normals.</param>
        /// <param name="texCoords">The list of texture coordinates.</param>
        /// <param name="faces">The list of faces.</param>
        private static void LoadObj(string fileName, out List<Vector3> vertexPositions, out List<Vector3> normals, out List<Vector2> texCoords, out List<Face> faces)
        {
            Contract.Requires<FileNotFoundException>(File.Exists(fileName),"fileName");

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
                    if (fields[0].ToLower(CultureInfo.InvariantCulture)[0] == 'v') //vertex
                    {
                        if (fields[0].Length == 1) //vertex position
                        {
                            vertexPositions.Add(new Vector3(Convert.ToSingle(fields[1], CultureInfo.InvariantCulture), Convert.ToSingle(fields[2], CultureInfo.InvariantCulture), Convert.ToSingle(fields[3], CultureInfo.InvariantCulture) * -1f));
                        }
                        else
                        {
                            if (fields[0].ToLower(CultureInfo.InvariantCulture)[1] == 't') //texture coordinates
                            {
                                texCoords.Add(new Vector2(Convert.ToSingle(fields[1], CultureInfo.InvariantCulture), 1f - Convert.ToSingle(fields[2], CultureInfo.InvariantCulture)));
                            }
                            else
                            {
                                if (fields[0].ToLower(CultureInfo.InvariantCulture)[1] == 'n') //normals
                                {
                                    normals.Add(new Vector3(Convert.ToSingle(fields[1], CultureInfo.InvariantCulture), Convert.ToSingle(fields[2], CultureInfo.InvariantCulture), Convert.ToSingle(fields[3], CultureInfo.InvariantCulture) * -1f));
                                }
                            }
                        }
                    }
                    else if (fields[0].ToLower(CultureInfo.InvariantCulture) == "f") //face
                    {
                        //Read the face data backwards to convert it to a left hand system
                        Face face = new Face();

                        face.VertexIndices = new short[3];
                        face.TexCoordIndices = new short[3];
                        face.NormalIndices = new short[3];

                        string[] subfields = fields[1].Split('/');
                        face.VertexIndices[2] = Convert.ToInt16(subfields[0], CultureInfo.InvariantCulture);
                        face.TexCoordIndices[2] = Convert.ToInt16(subfields[1], CultureInfo.InvariantCulture);
                        face.NormalIndices[2] = Convert.ToInt16(subfields[2], CultureInfo.InvariantCulture);

                        subfields = fields[2].Split('/');
                        face.VertexIndices[1] = Convert.ToInt16(subfields[0], CultureInfo.InvariantCulture);
                        face.TexCoordIndices[1] = Convert.ToInt16(subfields[1], CultureInfo.InvariantCulture);
                        face.NormalIndices[1] = Convert.ToInt16(subfields[2], CultureInfo.InvariantCulture);

                        subfields = fields[3].Split('/');
                        face.VertexIndices[0] = Convert.ToInt16(subfields[0], CultureInfo.InvariantCulture);
                        face.TexCoordIndices[0] = Convert.ToInt16(subfields[1], CultureInfo.InvariantCulture);
                        face.NormalIndices[0] = Convert.ToInt16(subfields[2], CultureInfo.InvariantCulture);
                        faces.Add(face);
                    }
                }
            }
            sr.Close();
        }

        /// <summary>
        /// Creates a mesh from a .obj file
        /// </summary>
        /// <param name="device">The D3D device for creating the vertex buffers.</param>
        /// <param name="fileName">The path to the .obj file.</param>
        /// <returns></returns>
        public static Mesh<V> FromObj(Device device, string fileName)
        {
            Contract.Requires<ArgumentNullException>(device != null, "device");
            Contract.Requires<ArgumentException>(File.Exists(fileName),"fileName");
            Contract.Ensures(Contract.Result<Mesh<V>>() != null);

            List<V> vertices = new List<V>();
            List<Vector3> vertexPositions;
            List<Vector3> normals;
            List<Vector2> texCoords;
            List<Face> faces;

            LoadObj(fileName, out vertexPositions, out normals, out texCoords, out faces);

            foreach (Face face in faces)
            {
                VertexPosTexNorm[] faceVertices = BuildFace(vertexPositions, normals, texCoords, face);
                //Check if we need binormals and tangents
                if (typeof(VertexPosTexNormTanBinorm).IsAssignableFrom(typeof(V)))
                {
                    //Calculate binormal and tangent
                    Vector3 vector1, vector2;
                    Vector2 texCoord1, texCoord2;

                    vector1 = new Vector3(faceVertices[1].Position.X - faceVertices[0].Position.X, faceVertices[1].Position.Y - faceVertices[0].Position.Y, faceVertices[1].Position.Z - faceVertices[0].Position.Z);
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
                    //Convert VertexPosTexNorm to VertexPosTexNormTanBinorm and add to result
                    foreach (VertexPosTexNorm faceVertex in faceVertices)
                    {
                        VertexPosTexNormTanBinorm vertex2 = new VertexPosTexNormTanBinorm(faceVertex.Position, faceVertex.TexCoords, faceVertex.Normal, tangent, binormal);
                        vertices.Add(vertex2 as V);
                    }
                }
                else
                {
                    foreach (VertexPosTexNorm faceVertex in faceVertices)
                    {
                        vertices.Add(faceVertex as V);
                    }
                }
            }
            return new Mesh<V>(device, vertices, PrimitiveTopology.TriangleList, new string[]{System.IO.Directory.GetCurrentDirectory() + "/Textures/metal.jpg",
                                                                                                                                System.IO.Directory.GetCurrentDirectory() + "/Textures/metal_n.jpg"});
        }

        /// <summary>
        /// Builds three vertex objects that make up a .obj face
        /// </summary>
        /// <param name="vertexPositions">The list of vertex positions.</param>
        /// <param name="normals">The list of normals.</param>
        /// <param name="texCoords">The list of texture coordinates.</param>
        /// <param name="face">The face to build.</param>
        /// <returns>An array containing three VertexPosTexNorm objects, correctly filled with the corresponding vertex positions, normals and texture coordinates for the face supplied.</returns>
        private static VertexPosTexNorm[] BuildFace(List<Vector3> vertexPositions, List<Vector3> normals, List<Vector2> texCoords, Face face)
        {
            Contract.Requires<ArgumentNullException>(vertexPositions != null, "vertexPosition");
            Contract.Requires<ArgumentNullException>(normals != null, "normals");
            Contract.Requires<ArgumentNullException>(texCoords != null, "texCoords");
            Contract.Ensures(Contract.Result<VertexPosTexNorm[]>().Length  ==3, "This method must return exactly 3 vertices.");

            VertexPosTexNorm[] vertices = new VertexPosTexNorm[3];
            int vertexIndex = face.VertexIndices[0] - 1;
            int texIndex = face.TexCoordIndices[0] - 1;
            int normIndex = face.NormalIndices[0] - 1;
            vertices[0] = new VertexPosTexNorm(vertexPositions[vertexIndex], texCoords[texIndex], normals[normIndex]);
            vertexIndex = face.VertexIndices[1] - 1;
            texIndex = face.TexCoordIndices[1] - 1;
            normIndex = face.NormalIndices[1] - 1;
            vertices[1] = new VertexPosTexNorm(vertexPositions[vertexIndex], texCoords[texIndex], normals[normIndex]);
            vertexIndex = face.VertexIndices[2] - 1;
            texIndex = face.TexCoordIndices[2] - 1;
            normIndex = face.NormalIndices[2] - 1;
            vertices[2] = new VertexPosTexNorm(vertexPositions[vertexIndex], texCoords[texIndex], normals[normIndex]);
            return vertices;
        }
    }
}