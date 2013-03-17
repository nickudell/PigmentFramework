using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using Pigment.WPF;
using Pigment.Engine.Rendering.Matter.Vertices;

namespace Pigment.Engine.Rendering.Matter
{
    public abstract class RenderableIndexed<V> : RenderableBase<V> where V : VertexPos
    {
        /// <summary>
        /// The index buffer
        /// </summary>
        protected SlimDX.Direct3D11.Buffer indexBuffer;

        protected int indexCount;

        public RenderableIndexed(PrimitiveTopology topology) : base(topology)
        {

        }

        public RenderableIndexed(Device device, List<V> vertices, PrimitiveTopology topology, List<ushort> indices) : base(device,vertices,topology)
        {
            this.indexCount = indices.Count;
            indexBuffer = createIndexBuffer(device, indices.ToArray());
        }

        public override void Draw(SlimDX.Direct3D11.DeviceContext context)
        {
            context.InputAssembler.PrimitiveTopology = vertexTopology;
            context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, vertexStride, 0));
            context.InputAssembler.SetIndexBuffer(indexBuffer, SlimDX.DXGI.Format.R16_UInt, 0);
            context.DrawIndexed(indexCount, 0, 0);
        }

        protected SlimDX.Direct3D11.Buffer createIndexBuffer(Device device, ushort[] indices)
        {
            indexCount = indices.Count();
            BufferDescription indexBufferDesc = new BufferDescription()
            {
                Usage = ResourceUsage.Default,
                SizeInBytes = sizeof(ushort) * indexCount,
                BindFlags = BindFlags.IndexBuffer,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None,
                StructureByteStride = 0
            };

            DataStream indexData = new DataStream(indexBufferDesc.SizeInBytes, true, true);
            foreach (short index in indices)
            {
                indexData.Write(index);
            }
            indexData.Position = 0;
            return new SlimDX.Direct3D11.Buffer(device, indexData, indexBufferDesc);
        }
    }
}
