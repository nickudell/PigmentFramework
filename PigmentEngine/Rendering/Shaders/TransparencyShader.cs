using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Rendering.Shaders
{
    public class TransparencyShader : TextureShader
    {

        /// <summary>
        /// InputLayout maker class for Light Shaders
        /// </summary>
        private class TransparencyInputMaker : IInputLayoutProvider
        {
            /// <summary>
            /// Builds an input layout based on what this shader requires, and the shader signature passed to it
            /// </summary>
            /// <param name="device">The D3D Device to create the layout with</param>
            /// <param name="inputSignature">The shader input signature to verify the input layout against</param>
            /// <returns>
            /// An input layout for this shader
            /// </returns>
            public InputLayout MakeInputLayout(Device device, SlimDX.D3DCompiler.ShaderSignature inputSignature)
            {
                return new InputLayout(device, inputSignature, new[] 
                { 
                    new InputElement("POSITION", 0, SlimDX.DXGI.Format.R32G32B32_Float, 0), 
                    new InputElement("TEXCOORD",0,SlimDX.DXGI.Format.R32G32_Float,0)
                });
            }
        }

        private struct TransparencyCBuffer
        {
            float opacity;
        }

        private SlimDX.Direct3D11.Buffer transparencyConstantBuffer;

        public TransparencyShader(Device device) : this(device, "shaders/transparency.fx","shaders/transparency.fx")
        {

        }

        public TransparencyShader(Device device, string vertexShaderPath, string pixelShaderPath) : this(device,vertexShaderPath,pixelShaderPath,new TransparencyInputMaker())
        {
            
        }

        protected TransparencyShader(Device device, string vertexShaderPath, string pixelShaderPath, IInputLayoutProvider inputLayoutMaker)
            : base(device, vertexShaderPath, pixelShaderPath, inputLayoutMaker)
        {
            Contract.Ensures(transparencyConstantBuffer != null, "lightConstantBuffer must be instantiated by this function.");
            BufferDescription transparencyBufferDesc = new BufferDescription(System.Runtime.InteropServices.Marshal.SizeOf(typeof(TransparencyCBuffer)), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            transparencyConstantBuffer = new SlimDX.Direct3D11.Buffer(device, transparencyBufferDesc);
        }

        /// <summary>
        /// Sets the shader parameters.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="world">The world.</param>
        /// <param name="view">The view.</param>
        /// <param name="projection">The projection.</param>
        public virtual void SetTransparencyParameters(DeviceContext context, float opacity)
        {
            /*Contract.Requires<ArgumentNullException>(context != null, "Parameter context must not be null.");
            Contract.Requires<ArgumentNullException>(ambientColour != null, "Parameter ambientColour must not be null.");
            Contract.Requires<ArgumentNullException>(diffuseColour != null, "Parameter diffuseColour must not be null.");
            Contract.Requires<ArgumentNullException>(lightDirection != null, "Parameter lightDirection must not be null.");
            Contract.Requires<ArgumentNullException>(specularColour != null, "Parameter specularColour must not be null.");*/
            using (DataStream data = new DataStream(System.Runtime.InteropServices.Marshal.SizeOf(typeof(TransparencyCBuffer)), true, true))
            {
                data.Write(opacity);
                data.Position = 0;
                context.UpdateSubresource(new DataBox(0, 0, data), transparencyConstantBuffer, 0);
                context.PixelShader.SetConstantBuffer(transparencyConstantBuffer, 0);
            }
        }

        protected override void Dispose(bool disposable)
        {
            Contract.Ensures(transparencyConstantBuffer == null, "This function must dispose of the lightConstantBuffer variable.");
            if (transparencyConstantBuffer != null)
            {
                transparencyConstantBuffer.Dispose();
                transparencyConstantBuffer = null;
            }
            base.Dispose(disposable);
        }
    }
}
