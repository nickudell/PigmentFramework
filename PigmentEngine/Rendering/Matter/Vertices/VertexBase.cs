using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.Runtime.InteropServices;

namespace Pigment.Engine.Rendering.Matter.Vertices
{
    /// <summary>
    /// 
    /// </summary>
    public class VertexPos : IPositioned, IMoveable
    {

        /// <summary>
        /// A simple vertex type containing only position values
        /// </summary>
        public struct PositionVertex : IPositioned, IMoveable
        {
            /// <summary>
            /// The position
            /// </summary>
            private Vector3 position;
            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>
            /// The position.
            /// </value>
            public Vector3 Position
            {
                get
                {
                    return position;
                }
                set
                {
                    position = value;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="PositionVertex" /> struct.
            /// </summary>
            /// <param name="position">The position of the vertex.</param>
            public PositionVertex(Vector3 position)
            {
                this.position = position;
            }

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPos"/> class.
        /// </summary>
        public VertexPos()
        {

        }

        protected byte[] getBytes<T>(T str) where T:struct
        {
            int size = GetStride();
            byte[] arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(str, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        public virtual byte[] GetBytes()
        {
            return getBytes(new PositionVertex(position));
        }

        /// <summary>
        /// Gets the stride of the internal struct.
        /// </summary>
        /// <returns></returns>
        public virtual int GetStride()
        {
            return Marshal.SizeOf(typeof(PositionVertex));
        }

        /// <summary>
        /// The position
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPos"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        public VertexPos(Vector3 position)
        {
            this.Position = position;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VertexPosCol : VertexPos, IColoured
    {
        /// <summary>
        /// The colour
        /// </summary>
        private Color4 colour;

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>
        /// The colour.
        /// </value>
        public Color4 Colour
        {
            get { return colour; }
            set { colour = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosCol"/> class.
        /// </summary>
        public VertexPosCol()
        {

        }

        public override byte[] GetBytes()
        {
            return getBytes(new PositionVertex(Position));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosCol"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="colour">The colour.</param>
        public VertexPosCol(Vector3 position, Color4 colour)
            : base(position)
        {
            this.colour = colour;
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class VertexPosTex : VertexPos ,ITextured
    {
        /// <summary>
        /// Vertex data containing position and texture coordinates.
        /// </summary>
        public struct TexturedVertex : IPositioned, IMoveable, ITextured
        {
            /// <summary>
            /// The position
            /// </summary>
            private Vector3 position;
            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>
            /// The position.
            /// </value>
            public Vector3 Position
            {
                get
                {
                    return position;
                }
                set
                {
                    position = value;
                }
            }

            /// <summary>
            /// The texture coordinates
            /// </summary>
            private Vector2 texCoords;
            /// <summary>
            /// Gets or sets the texture coordinates.
            /// </summary>
            /// <value>
            /// The texture coordinates.
            /// </value>
            public Vector2 TexCoords
            {
                get
                {
                    return texCoords;
                }
                set
                {
                    texCoords = value;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TexturedVertex" /> struct.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="texCoords">The texture coordinates.</param>
            public TexturedVertex(Vector3 position, Vector2 texCoords)
            {
                this.position = position;
                this.texCoords = texCoords;
            }
        }

        /// <summary>
        /// Gets the stride of the internal struct.
        /// </summary>
        /// <returns></returns>
        public override int GetStride()
        {
            return Marshal.SizeOf(typeof(TexturedVertex));
        }

        public override byte[] GetBytes()
        {
            return getBytes(new TexturedVertex(Position,texCoords));
        }

        /// <summary>
        /// The tex coords
        /// </summary>
        private Vector2 texCoords;

        /// <summary>
        /// Gets or sets the tex coords.
        /// </summary>
        /// <value>
        /// The tex coords.
        /// </value>
        public Vector2 TexCoords
        {
            get { return texCoords; }
            set { texCoords = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosTex"/> class.
        /// </summary>
        public VertexPosTex()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosTex"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texCoords">The tex coords.</param>
        public VertexPosTex(Vector3 position, Vector2 texCoords) : base(position)
        {
            this.texCoords = texCoords;
        }

        /// <summary>
        /// Converts to vertex pos.
        /// </summary>
        /// <returns></returns>
        public VertexPos ConvertToVertexPos()
        {
            return new VertexPos(Position);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VertexPosTexCol : VertexPosTex, IColoured
    {

        /// <summary>
        /// Vertex data containing position, colour and texture coordinates.
        /// </summary>
        public struct ColouredTexturedVertex : IPositioned, IMoveable, IColoured, ITextured
        {
            /// <summary>
            /// The position
            /// </summary>
            private Vector3 position;
            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>
            /// The position.
            /// </value>
            public Vector3 Position
            {
                get
                {
                    return position;
                }
                set
                {
                    position = value;
                }
            }

            /// <summary>
            /// The colour
            /// </summary>
            private Color4 colour;
            /// <summary>
            /// Gets or sets the colour.
            /// </summary>
            /// <value>
            /// The colour.
            /// </value>
            public Color4 Colour
            {
                get
                {
                    return colour;
                }
                set
                {
                    colour = value;
                }
            }

            /// <summary>
            /// The texture coordinates
            /// </summary>
            private Vector2 texCoords;
            /// <summary>
            /// Gets or sets the texture coordinates.
            /// </summary>
            /// <value>
            /// The texture coordinates.
            /// </value>
            public Vector2 TexCoords
            {
                get
                {
                    return texCoords;
                }
                set
                {
                    texCoords = value;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ColouredTexturedVertex" /> struct.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="colour">The colour.</param>
            /// <param name="texCoords">The texture coordinates.</param>
            public ColouredTexturedVertex(Vector3 position, Color4 colour, Vector2 texCoords)
            {
                this.position = position;
                this.texCoords = texCoords;
                this.colour = colour;
            }

        }

        /// <summary>
        /// Gets the stride.
        /// </summary>
        /// <returns></returns>
        public override int GetStride()
        {
            return Marshal.SizeOf(typeof(ColouredTexturedVertex));
        }

        public override byte[] GetBytes()
        {
            return getBytes(new ColouredTexturedVertex(Position,colour,TexCoords));
        }

        /// <summary>
        /// The colour
        /// </summary>
        private Color4 colour;

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        /// <value>
        /// The colour.
        /// </value>
	    public Color4 Colour
	    {
		    get { return colour;}
		    set { colour = value;}
	    }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosTexCol"/> class.
        /// </summary>
        public VertexPosTexCol()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosTexCol"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texCoords">The tex coords.</param>
        /// <param name="colour">The colour.</param>
        public VertexPosTexCol(Vector3 position, Vector2 texCoords, Color4 colour)
            : base(position, texCoords)
        {
            this.Colour = colour;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class VertexPosTexNorm : VertexPosTex, INormal
    {

        /// <summary>
        /// Vertex data containing position, texture coordinates and normals.
        /// </summary>
        public struct TexturedNormalVertex : IPositioned, ITextured, INormal
        {
            /// <summary>
            /// The position
            /// </summary>
            private Vector3 position;
            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>
            /// The position.
            /// </value>
            public Vector3 Position
            {
                get
                {
                    return position;
                }
                set
                {
                    position = value;
                }
            }

            /// <summary>
            /// The texture coordinates
            /// </summary>
            private Vector2 texCoords;
            /// <summary>
            /// Gets or sets the texture coordinates.
            /// </summary>
            /// <value>
            /// The texture coordinates.
            /// </value>
            public Vector2 TexCoords
            {
                get
                {
                    return texCoords;
                }
                set
                {
                    texCoords = value;
                }
            }

            /// <summary>
            /// The normal
            /// </summary>
            private Vector3 normal;
            /// <summary>
            /// Gets or sets the normal.
            /// </summary>
            /// <value>
            /// The normal.
            /// </value>
            public Vector3 Normal
            {
                get
                {
                    return normal;
                }
                set
                {
                    normal = value;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TexturedNormalVertex" /> struct.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="texCoords">The texture coordinates.</param>
            /// <param name="normal">The normal.</param>
            public TexturedNormalVertex(Vector3 position, Vector2 texCoords, Vector3 normal)
            {
                this.position = position;
                this.texCoords = texCoords;
                this.normal = normal;
            }
        }

        /// <summary>
        /// Gets the stride.
        /// </summary>
        /// <returns></returns>
        public override int GetStride()
        {
            return Marshal.SizeOf(typeof(TexturedNormalVertex));
        }

        public override byte[] GetBytes()
        {
            return getBytes(new TexturedNormalVertex(Position,TexCoords,normal));
        }

        /// <summary>
        /// The normal
        /// </summary>
        private Vector3 normal;

        /// <summary>
        /// Gets or sets the normal.
        /// </summary>
        /// <value>
        /// The normal.
        /// </value>
        public Vector3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosTexNorm"/> class.
        /// </summary>
        public VertexPosTexNorm()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosTexNorm"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texCoords">The tex coords.</param>
        /// <param name="normal">The normal.</param>
        public VertexPosTexNorm(Vector3 position, Vector2 texCoords, Vector3 normal) : base(position,texCoords)
        {
            this.normal = normal;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VertexPosTexNormTanBinorm : VertexPosTexNorm, ITangent,IBinormal
    {

        /// <summary>
        /// Vertex data containing position, texture coordinates and normals.
        /// </summary>
        public struct TexturedNormalTangentBinormalVertex : IPositioned, ITextured, INormal, ITangent, IBinormal
        {
            /// <summary>
            /// The position
            /// </summary>
            private Vector3 position;
            /// <summary>
            /// Gets or sets the position.
            /// </summary>
            /// <value>
            /// The position.
            /// </value>
            public Vector3 Position
            {
                get
                {
                    return position;
                }
                set
                {
                    position = value;
                }
            }

            /// <summary>
            /// The texture coordinates
            /// </summary>
            private Vector2 texCoords;
            /// <summary>
            /// Gets or sets the texture coordinates.
            /// </summary>
            /// <value>
            /// The texture coordinates.
            /// </value>
            public Vector2 TexCoords
            {
                get
                {
                    return texCoords;
                }
                set
                {
                    texCoords = value;
                }
            }

            /// <summary>
            /// The normal
            /// </summary>
            private Vector3 normal;
            /// <summary>
            /// Gets or sets the normal.
            /// </summary>
            /// <value>
            /// The normal.
            /// </value>
            public Vector3 Normal
            {
                get
                {
                    return normal;
                }
                set
                {
                    normal = value;
                }
            }

            /// <summary>
            /// The tangent
            /// </summary>
            private Vector3 tangent;
            /// <summary>
            /// Gets or sets the tangent.
            /// </summary>
            /// <value>
            /// The tangent.
            /// </value>
            public Vector3 Tangent
            {
                get
                {
                    return tangent;
                }
                set
                {
                    tangent = value;
                }
            }

            /// <summary>
            /// The binormal
            /// </summary>
            private Vector3 binormal;
            /// <summary>
            /// Gets or sets the binormal.
            /// </summary>
            /// <value>
            /// The binormal.
            /// </value>
            public Vector3 Binormal
            {
                get
                {
                    return binormal;
                }
                set
                {
                    binormal = value;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="TexturedNormalVertex" /> struct.
            /// </summary>
            /// <param name="position">The position.</param>
            /// <param name="texCoords">The texture coordinates.</param>
            /// <param name="normal">The normal.</param>
            /// <param name="tangent">The tangent.</param>
            /// <param name="binormal">The binormal.</param>
            public TexturedNormalTangentBinormalVertex(Vector3 position, Vector2 texCoords, Vector3 normal, Vector3 tangent, Vector3 binormal)
            {
                this.position = position;
                this.texCoords = texCoords;
                this.normal = normal;
                this.tangent = tangent;
                this.binormal = binormal;
            }
        }

        /// <summary>
        /// Gets the stride.
        /// </summary>
        /// <returns></returns>
        public override int GetStride()
        {
            return Marshal.SizeOf(typeof(TexturedNormalTangentBinormalVertex));
        }

        public override byte[] GetBytes()
        {
            return getBytes(new TexturedNormalTangentBinormalVertex(Position,TexCoords,Normal,tangent,binormal));
        }

        /// <summary>
        /// The tangent
        /// </summary>
        private Vector3 tangent;

        /// <summary>
        /// Gets or sets the tangent.
        /// </summary>
        /// <value>
        /// The tangent.
        /// </value>
        public Vector3 Tangent
        {
            get { return tangent; }
            set { tangent = value; }
        }

        /// <summary>
        /// The binormal
        /// </summary>
        private Vector3 binormal;

        /// <summary>
        /// Gets or sets the binormal.
        /// </summary>
        /// <value>
        /// The binormal.
        /// </value>
        public Vector3 Binormal
        {
            get { return binormal; }
            set { binormal = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosTexNormTanBinorm"/> class.
        /// </summary>
        public VertexPosTexNormTanBinorm()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPosTexNormTanBinorm"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texCoords">The tex coords.</param>
        /// <param name="normal">The normal.</param>
        /// <param name="tangent">The tangent.</param>
        /// <param name="binormal">The binormal.</param>
        public VertexPosTexNormTanBinorm(Vector3 position, Vector2 texCoords, Vector3 normal, Vector3 tangent, Vector3 binormal) : base(position,texCoords,normal)
        {
            this.tangent = tangent;
            this.binormal = binormal;
        }
    }
}
