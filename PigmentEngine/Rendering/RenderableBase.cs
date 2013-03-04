﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using System.Runtime.InteropServices;
using System.Diagnostics.Contracts;
using Pigment.WPF;

namespace Pigment.Engine.Rendering
{
    public abstract class RenderableBase<V> : IDisposable, IRenderable
        where V : VertexPos
    {

        /// <summary>
        /// Gets the vertex stride.
        /// </summary>
        /// <value>
        /// The vertex stride.
        /// </value>
        protected int vertexStride;

        protected int vertexCount;

        /// <summary>
        /// Gets the vertex buffer.
        /// </summary>
        /// <value>
        /// The vertex buffer.
        /// </value>
        protected SlimDX.Direct3D11.Buffer vertexBuffer;

        /// <summary>
        /// Gets or sets the vertex topology.
        /// </summary>
        /// <value>
        /// The vertex topology.
        /// </value>
        protected PrimitiveTopology vertexTopology;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderableBase{V}" /> class and builds the first vertexbuffer
        /// </summary>
        /// <param name="device">The Direct3D11 device to use.</param>
        /// <param name="vertices">The vertices of this mesh.</param>
        /// <param name="vertexTopology">The vertex topology.</param>
        public RenderableBase(Device device, List<V> vertices, PrimitiveTopology vertexTopology) : this(vertexTopology)
        {
            Contract.Requires<ArgumentNullException>(device != null, "device");
            Contract.Requires<ArgumentNullException>(vertices != null, "vertices");
            Contract.Requires<ArgumentNullException>(vertexTopology != null, "vertexTopology");
            Contract.Requires<ArgumentException>(vertices.Count > 0, "vertices");
            this.vertexCount = vertices.Count;
            vertexBuffer = createVertexBuffer(device,vertices);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderableBase{V}"/> class.
        /// </summary>
        /// <param name="vertexTopology">The vertex topology.</param>
        public RenderableBase(PrimitiveTopology vertexTopology)
        {
            Contract.Requires<ArgumentNullException>(vertexTopology != null, "vertexTopology");
            this.vertexTopology = vertexTopology;
            vertexStride = Marshal.SizeOf(typeof(V));
        }

        /// <summary>
        /// Creates the vertex stream.
        /// </summary>
        /// <param name="vertices">The vertices.</param>
        /// <returns></returns>
        protected SlimDX.Direct3D11.Buffer createVertexBuffer(Device device, List<V> vertices)
        {
            Contract.Requires<ArgumentNullException>(device != null, "device");
            Contract.Requires<ArgumentNullException>(vertices != null, "vertices");
            Contract.Requires<ArgumentException>(vertices.Count > 0);
            Contract.Ensures(Contract.Result<SlimDX.Direct3D11.Buffer>() != null);
            SlimDX.Direct3D11.Buffer vertexBuffer;
            using (DataStream vertexStream = new DataStream(vertexStride * vertices.Count, true, true))
            {
                foreach (V vertex in vertices)
                {
                    vertexStream.Write(vertex.GetStruct());
                }
                vertexStream.Position = 0;
                vertexBuffer = new SlimDX.Direct3D11.Buffer(device, vertexStream, vertexStride * vertices.Count, ResourceUsage.Default, BindFlags.VertexBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            }
            return vertexBuffer;
        }

        /// <summary>
        /// Draws this object to the specified context
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void Draw(DeviceContext context)
        {

            context.InputAssembler.PrimitiveTopology = vertexTopology;
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, vertexStride, 0));
            context.Draw(vertexCount, 0);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool managed)
        {
            if (vertexBuffer != null)
            {
                vertexBuffer.Dispose();
                vertexBuffer = null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
