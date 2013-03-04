using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using System.Diagnostics.Contracts;
namespace Pigment.Engine.Rendering
{
    public abstract class WVPTransformShader : ShaderBase
    {
        /// <summary>
        /// World, view, projection matrices constant buffer
        /// </summary>
        private struct MatrixCBuffer
        {
            /// <summary>
            /// The world
            /// </summary>
            public Matrix world;
            /// <summary>
            /// The view
            /// </summary>
            public Matrix view;
            /// <summary>
            /// The projection
            /// </summary>
            public Matrix projection;
        }

        /// <summary>
        /// The matrix constant buffer
        /// </summary>
        private SlimDX.Direct3D11.Buffer matrixConstantBuffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="WVPTransformShader" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="vertexShaderPath">The vertex shader path.</param>
        /// <param name="pixelShaderPath">The pixel shader path.</param>
        public WVPTransformShader(Device device, string vertexShaderPath, string pixelShaderPath, IInputLayoutProvider inputLayoutMaker)
            : base(device, vertexShaderPath, pixelShaderPath, inputLayoutMaker)
        {
            Contract.Ensures(matrixConstantBuffer != null, "matrixConstantBuffer must not be null after this method executes.");
            BufferDescription matrixBufferDesc = new BufferDescription(System.Runtime.InteropServices.Marshal.SizeOf(typeof(MatrixCBuffer)), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            matrixConstantBuffer = new SlimDX.Direct3D11.Buffer(device, matrixBufferDesc);

        }

        /// <summary>
        /// Sets the shader parameters.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="world">The world.</param>
        /// <param name="view">The view.</param>
        /// <param name="projection">The projection.</param>
        public virtual void SetWVPMatrices(DeviceContext context, Matrix world, Matrix view, Matrix projection)
        {
            Contract.Requires<ArgumentNullException>(context != null, "context");
            Contract.Requires<ArgumentNullException>(world != null, "world");
            Contract.Requires<ArgumentNullException>(view != null, "view");
            Contract.Requires<ArgumentNullException>(projection != null, "projection");
            using (DataStream data = new DataStream(System.Runtime.InteropServices.Marshal.SizeOf(typeof(MatrixCBuffer)), true, true))
            {
                data.Write(Matrix.Transpose(world));
                data.Write(Matrix.Transpose(view));
                data.Write(Matrix.Transpose(projection));
                data.Position = 0;
                context.UpdateSubresource(new DataBox(0, 0, data), matrixConstantBuffer, 0);
                context.VertexShader.SetConstantBuffer(matrixConstantBuffer, 0);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposable"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposable)
        {
            Contract.Ensures(matrixConstantBuffer == null, "matrixConstantBuffer must be null after this method executes.");
            if (disposable)
            {
                if (matrixConstantBuffer != null)
                {
                    matrixConstantBuffer.Dispose();
                    matrixConstantBuffer = null;
                }
            }
            base.Dispose(disposable);
        }

    }
}
