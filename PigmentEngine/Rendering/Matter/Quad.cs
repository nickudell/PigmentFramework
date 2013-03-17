using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.Windows;
using Pigment.WPF;
using Pigment.Engine.Rendering.Matter.Vertices;

namespace Pigment.Engine.Rendering.Matter
{
    /// <summary>
    /// Quad class that you render in groups for additional speed
    /// </summary>
    public class Quad
    {
        /// <summary>
        /// The vertices that make up this quad
        /// </summary>
        protected VertexPosTexCol[] vertices;
        /// <summary>
        /// Gets the vertices.
        /// </summary>
        /// <value>
        /// The vertices.
        /// </value>
        public VertexPosTexCol[] Vertices
        {
            get
            {
                return vertices;
            }
        }


        /// <summary>
        /// The indices used to render this quad
        /// </summary>
        protected int[] indices;
        /// <summary>
        /// Gets the indices used to render this quad.
        /// </summary>
        /// <value>
        /// The indices used to render this quad
        /// </value>
        public int[] Indices
        {
            get
            {
                return indices;
            }
        }
        /// <summary>
        /// The colour
        /// </summary>
        public Color4 Colour { get; set; }

        /// <summary>
        /// Gets or sets the texture path.
        /// </summary>
        /// <value>
        /// The texture path.
        /// </value>
        public string TexturePath { get; set; }

        /// <summary>
        /// The width and height of the screen in pixels
        /// </summary>
        private Tuple<int,int> screenDimensions;
        /// <summary>
        /// Gets or sets the screen width and height in pixels
        /// </summary>
        /// <value>
        /// The screen dimensions in pixels.
        /// </value>
        public Tuple<int, int> ScreenDimensions
        {
            get { return screenDimensions; }
            set
            {
                screenDimensions = value;
                vertices = buildVertices();
            }
        }

        /// <summary>
        /// The position of the top-left vertex of this quad
        /// </summary>
        private Tuple<int, int> position;
        /// <summary>
        /// Gets or sets the position of the top-left vertex.
        /// </summary>
        /// <value>
        /// The position of the top-left vertex.
        /// </value>
        public Tuple<int, int> Position
        {
            get { return position; }
            set
            {
                position = value;
                //update the vertices with this new position information
                vertices = buildVertices();
            }
        }

        /// <summary>
        /// The width
        /// </summary>
        private int width;
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int Width
        {
            get { return width; }
            set
            {
                vertices[1].Position = new Vector3(vertices[0].Position.X + width,vertices[1].Position.Y,0);
                vertices[2].Position = new Vector3(vertices[1].Position.X, vertices[2].Position.Y, 0);
            }
        }
        /// <summary>
        /// The height
        /// </summary>
        private int height;
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                vertices[2].Position = new Vector3(vertices[2].Position.X, vertices[1].Position.Y+height, 0);
                vertices[3].Position = new Vector3(vertices[3].Position.X, vertices[2].Position.Y, 0);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Quad" /> class.
        /// </summary>
        /// <param name="x">The x coordinate of the upper-left vertex.</param>
        /// <param name="y">The y coordinate of the upper-left vertex.</param>
        /// <param name="screenWidth">Width of the screen.</param>
        /// <param name="screenHeight">Height of the screen.</param>
        /// <param name="textureFileName">Path to the texture file.</param>
        /// <param name="width">The width of the quad.</param>
        /// <param name="height">The height of the quad.</param>
        /// <param name="colour">The colour to blend this quad's texture with.</param>
        public Quad(int x, int y, int screenWidth, int screenHeight, string textureFileName, int width, int height, Color4 colour)
        {
            this.position = new Tuple<int, int>(x, y);
            this.screenDimensions = new Tuple<int, int>(screenWidth, screenHeight);
            this.width = width;
            this.height = height;
            this.Colour = colour;
            TexturePath = textureFileName;
            vertices = buildVertices();
            indices = buildIndices();
        }

        /// <summary>
        /// Gets the bounding box of the quad
        /// </summary>
        /// <returns>The bounding box of the quad</returns>
        protected Rect getBounds()
        {
            return new Rect()
            {
                X = ((screenDimensions.X / 2) * -1) + position.X,
                Width = width,
                Y = (screenDimensions.Y / 2) - position.Y,
                Height = -height
            };
        }

        /// <summary>
        /// Builds the vertices used to render this quad
        /// </summary>
        /// <returns>The vertices that make this quad</returns>
        protected VertexPosTexCol[] buildVertices()
        {
            VertexPosTexCol[] vertices = new VertexPosTexCol[6];

            Rect bounds = getBounds();

            vertices[0] = new VertexPosTexCol(new Vector3((float)bounds.Left, (float)bounds.Top, 0f), new Vector2(0f, 0f), Colour);

            vertices[1] = new VertexPosTexCol(new Vector3((float)bounds.Right, (float)bounds.Bottom, 0f), new Vector2(1f, 1f), Colour);

            vertices[2] = new VertexPosTexCol(new Vector3((float)bounds.Left, (float)bounds.Bottom, 0f), new Vector2(0f, 1f), Colour);

            vertices[3] = new VertexPosTexCol(new Vector3((float)bounds.Left, (float)bounds.Top, 0f), new Vector2(0f, 0f), Colour);

            vertices[4] = new VertexPosTexCol(new Vector3((float)bounds.Right, (float)bounds.Top, 0f), new Vector2(1f, 0f), Colour);

            vertices[5] = new VertexPosTexCol(new Vector3((float)bounds.Right, (float)bounds.Bottom, 0f), new Vector2(1f, 1f), Colour);

            return vertices;
        }

        /// <summary>
        /// Builds the indices used to render this quad.
        /// </summary>
        /// <returns>The indices used to render this quad</returns>
        private int[] buildIndices()
        {
            return new int[] {0,1,2,3,4,5};
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Quad objQuad = obj as Quad;
            if (objQuad != null)
            {
                if (objQuad.Position.Equals(position) && objQuad.Height == height && objQuad.Width == width)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
