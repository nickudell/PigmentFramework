using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using Pigment.WPF;
using Pigment.Engine.Rendering.Matter;
using Pigment.Engine.Rendering.Matter.Vertices;
using Pigment.Engine.Rendering.Textures;

namespace Pigment.Engine.Rendering.UI
{
    public class Bitmap : RenderableIndexed<VertexPosTex>
    {
        /// <summary>
        /// The texture
        /// </summary>
        public Texture Texture { get; private set; }

        /// <summary>
        /// The screen width
        /// </summary>
        private int screenWidth;
        /// <summary>
        /// The screen height
        /// </summary>
        private int screenHeight;

        /// <summary>
        /// The bitmap width
        /// </summary>
        private int bitmapWidth;
        /// <summary>
        /// The bitmap height
        /// </summary>
        private int bitmapHeight;

        /// <summary>
        /// The previous pos X
        /// </summary>
        private int previousPosX;
        /// <summary>
        /// The previous pos Y
        /// </summary>
        private int previousPosY;


        /// <summary>
        /// Initializes a new instance of the <see cref="Bitmap" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="screenWidth">Width of the screen.</param>
        /// <param name="screenHeight">Height of the screen.</param>
        /// <param name="textureFileName">Name of the texture file.</param>
        /// <param name="bitmapWidth">Width of the bitmap.</param>
        /// <param name="bitmapHeight">Height of the bitmap.</param>
        public Bitmap(Device device, int screenWidth, int screenHeight, string textureFileName, int bitmapWidth, int bitmapHeight) : base(PrimitiveTopology.TriangleList)
        {
            this.vertexCount = 6;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
            this.bitmapHeight = bitmapHeight;
            this.bitmapWidth = bitmapWidth;
            this.Texture = new Texture(device, textureFileName);
            previousPosX = -1;
            previousPosY = -1;
            vertexBuffer = createVertexBuffer(device, createVertices(100, 100));

            ushort[] indices = new ushort[6] {0,1,2,3,4,5};

            vertexCount = 6;
            indexCount = 6;

            indexBuffer = createIndexBuffer(device, indices);
        }

        /// <summary>
        /// Creates the vertex buffer.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="vertices">The vertices.</param>
        /// <returns></returns>
        protected new SlimDX.Direct3D11.Buffer createVertexBuffer(Device device, List<VertexPosTex> vertices)
        {
            BufferDescription vertexBufferDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Dynamic,
                SizeInBytes = vertexStride * vertexCount,
                BindFlags = BindFlags.VertexBuffer,
                CpuAccessFlags = CpuAccessFlags.Write,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            DataStream vertexData = new DataStream(vertexBufferDesc.SizeInBytes, true, true);
            foreach (VertexPosTex vertex in vertices)
            {
                vertexData.Write(vertex.GetBytes(),0,vertex.GetStride());
            }
            vertexData.Position = 0;
            return new SlimDX.Direct3D11.Buffer(device, vertexData, vertexBufferDesc);
        }

        /// <summary>
        /// Updates the buffers.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="positionX">The position X.</param>
        /// <param name="positionY">The position Y.</param>
        public void UpdatePosition(DeviceContext context, int positionX, int positionY)
        {
            if (positionX == previousPosX && positionY == previousPosY)
            {
                return;
            }

            VertexPosTex[] vertices = createVertices(positionX, positionY).ToArray();

            DataBox box = context.MapSubresource(vertexBuffer, MapMode.WriteDiscard, MapFlags.None);
            box.Data.Position = 0;
            foreach (VertexPosTex vertex in vertices)
            {
                box.Data.Write(vertex.GetBytes(),0,vertex.GetStride());
            }
            box.Data.Position = 0;
            context.UnmapSubresource(vertexBuffer, 0);
        }

        /// <summary>
        /// Creates the vertices.
        /// </summary>
        /// <param name="positionX">The position X.</param>
        /// <param name="positionY">The position Y.</param>
        /// <returns></returns>
        private List<VertexPosTex> createVertices(int positionX, int positionY)
        {
            float left, right, top, bottom;
            List<VertexPosTex> vertices = new List<VertexPosTex>(6);

            left = (float)((screenWidth / 2) * -1) + (float)positionX;
            right = left + (float)bitmapWidth;
            top = (float)(screenHeight / 2) - (float)positionY;
            bottom = top - (float)bitmapHeight;

            vertices.Add(new VertexPosTex(new Vector3(left, top, 0f), new Vector2(0f, 0f)));

            vertices.Add(new VertexPosTex(new Vector3(right, bottom, 0f), new Vector2(1f, 1f)));

            vertices.Add(new VertexPosTex(new Vector3(left, bottom, 0f), new Vector2(0f, 1f)));

            vertices.Add(new VertexPosTex(new Vector3(left, top, 0f), new Vector2(0f, 0f)));

            vertices.Add(new VertexPosTex(new Vector3(right, top, 0f), new Vector2(1f, 0f)));

            vertices.Add(new VertexPosTex(new Vector3(right, bottom, 0f), new Vector2(1f, 1f)));
            return vertices;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        protected override void Dispose(bool managed)
        {
            if (managed)
            {
                if (Texture!= null)
                {
                    Texture.Dispose();
                    Texture = null;
                }
                if(indexBuffer != null)
                {
                    indexBuffer.Dispose();
                    indexBuffer = null;
                }
            }
        }
    }
}
