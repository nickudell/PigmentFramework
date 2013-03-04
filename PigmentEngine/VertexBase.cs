using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
namespace Pigment.WPF
{
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

        public PositionVertex GetStruct()
        {
            return new PositionVertex(position);
        }
        
        private Vector3 position;

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public VertexPos(Vector3 position)
        {
            this.Position = position;
        }
    }

    public class VertexPosCol : VertexPos, IColoured
    {
        private Color4 colour;

        public Color4 Colour
        {
            get { return colour; }
            set { colour = value; }
        }

        public VertexPosCol(Vector3 position, Color4 colour)
            : base(position)
        {
            this.colour = colour;
        }
        
    }

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

        private Vector2 texCoords;

        public Vector2 TexCoords
        {
            get { return texCoords; }
            set { texCoords = value; }
        }
        
        public VertexPosTex(Vector3 position, Vector2 texCoords) : base(position)
        {
            this.texCoords = texCoords;
        }

        public VertexPos ConvertToVertexPos()
        {
            return new VertexPos(Position);
        }

        public new TexturedVertex GetStruct()
        {
            return new TexturedVertex(Position, texCoords);
        }
    }

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
            /// <param name="texCoords">The texture coordinates.</param>
            public ColouredTexturedVertex(Vector3 position, Color4 colour, Vector2 texCoords)
            {
                this.position = position;
                this.texCoords = texCoords;
                this.colour = colour;
            }

        }

        private Color4 colour;

	    public Color4 Colour
	    {
		    get { return colour;}
		    set { colour = value;}
	    }
	
        public VertexPosTexCol(Vector3 position, Vector2 texCoords, Color4 colour)
            : base(position, texCoords)
        {
            this.Colour = colour;
        }

        public new ColouredTexturedVertex GetStruct()
        {
            return new ColouredTexturedVertex(Position, Colour, TexCoords);
        }
    }

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

        private Vector3 normal;

        public Vector3 Normal
        {
            get { return normal; }
            set { normal = value; }
        }

        public VertexPosTexNorm(Vector3 position, Vector2 texCoords, Vector3 normal) : base(position,texCoords)
        {
            this.normal = normal;
        }

        public new TexturedNormalVertex GetStruct()
        {
            return new TexturedNormalVertex(Position, TexCoords, normal);
        }
    }

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

            private Vector3 tangent;
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

            private Vector3 binormal;
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
            public TexturedNormalTangentBinormalVertex(Vector3 position, Vector2 texCoords, Vector3 normal, Vector3 tangent, Vector3 binormal)
            {
                this.position = position;
                this.texCoords = texCoords;
                this.normal = normal;
                this.tangent = tangent;
                this.binormal = binormal;
            }
        }


        private Vector3 tangent;

        public Vector3 Tangent
        {
            get { return tangent; }
            set { tangent = value; }
        }

        private Vector3 binormal;

        public Vector3 Binormal
        {
            get { return binormal; }
            set { binormal = value; }
        }

        public VertexPosTexNormTanBinorm(Vector3 position, Vector2 texCoords, Vector3 normal, Vector3 tangent, Vector3 binormal) : base(position,texCoords,normal)
        {
            this.tangent = tangent;
            this.binormal = binormal;
        }

        public new TexturedNormalTangentBinormalVertex GetStruct()
        {
            return new TexturedNormalTangentBinormalVertex(Position, TexCoords, Normal, Tangent, Binormal);
        }
    }
}
