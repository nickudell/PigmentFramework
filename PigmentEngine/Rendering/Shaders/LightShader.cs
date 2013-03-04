using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Rendering
{
    public class LightShader : TextureShader
    {

        /// <summary>
        /// InputLayout maker class for Light Shaders
        /// </summary>
        private class LightInputMaker : IInputLayoutProvider
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
                    new InputElement("TEXCOORD",0,SlimDX.DXGI.Format.R32G32_Float,0),
                    new InputElement("NORMAL",0,SlimDX.DXGI.Format.R32G32B32_Float,0)
                });
            }
        }

        private struct LightCBuffer
        {
            Color4 ambientColour;
            Color4 diffuseColour;
            Vector3 lightDirection;
            float specularPower;
            Color4 specularColour;
        }

        private SlimDX.Direct3D11.Buffer lightConstantBuffer;

        private struct CameraCBuffer
        {
            Vector3 cameraPosition;
            float padding;
        }

        private SlimDX.Direct3D11.Buffer cameraConstantBuffer;

        public LightShader(Device device) : this(device, "shaders/light.fx","shaders/light.fx")
        {

        }

        public LightShader(Device device, string vertexShaderPath, string pixelShaderPath) : this(device,vertexShaderPath,pixelShaderPath,new LightInputMaker())
        {
            
        }

        protected LightShader(Device device, string vertexShaderPath, string pixelShaderPath, IInputLayoutProvider inputLayoutMaker) : base(device,vertexShaderPath,pixelShaderPath,inputLayoutMaker)
        {
            Contract.Ensures(lightConstantBuffer != null, "lightConstantBuffer must be instantiated by this function.");
            Contract.Ensures(cameraConstantBuffer != null, "cameraConstantBuffer must be instantiated by this function.");
            BufferDescription lightBufferDesc = new BufferDescription(System.Runtime.InteropServices.Marshal.SizeOf(typeof(LightCBuffer)), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            lightConstantBuffer = new SlimDX.Direct3D11.Buffer(device, lightBufferDesc);

            BufferDescription cameraBufferDesc = new BufferDescription(System.Runtime.InteropServices.Marshal.SizeOf(typeof(CameraCBuffer)), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
            cameraConstantBuffer = new SlimDX.Direct3D11.Buffer(device, cameraBufferDesc);
        }

        /// <summary>
        /// Sets the shader parameters.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="world">The world.</param>
        /// <param name="view">The view.</param>
        /// <param name="projection">The projection.</param>
        public virtual void SetLightParameters(DeviceContext context, Color4 ambientColour, Color4 diffuseColour, Vector3 lightDirection, float specularPower, Color4 specularColour)
        {
            /*Contract.Requires<ArgumentNullException>(context != null, "Parameter context must not be null.");
            Contract.Requires<ArgumentNullException>(ambientColour != null, "Parameter ambientColour must not be null.");
            Contract.Requires<ArgumentNullException>(diffuseColour != null, "Parameter diffuseColour must not be null.");
            Contract.Requires<ArgumentNullException>(lightDirection != null, "Parameter lightDirection must not be null.");
            Contract.Requires<ArgumentNullException>(specularColour != null, "Parameter specularColour must not be null.");*/
            using (DataStream data = new DataStream(System.Runtime.InteropServices.Marshal.SizeOf(typeof(LightCBuffer)), true, true))
            {
                data.Write(ambientColour);
                data.Write(diffuseColour);
                data.Write(lightDirection);
                data.Write(specularPower);
                data.Write(specularColour);
                data.Position = 0;
                context.UpdateSubresource(new DataBox(0, 0, data), lightConstantBuffer, 0);
                context.PixelShader.SetConstantBuffer(lightConstantBuffer, 0);
            }
        }

        public void SetCameraParameters(DeviceContext context, Camera camera)
        {
            Contract.Requires<ArgumentNullException>(context != null, "Parameter context must not be null.");
            Contract.Requires<ArgumentNullException>(camera != null, "Parameter camera must not be null.");
            using (DataStream data = new DataStream(System.Runtime.InteropServices.Marshal.SizeOf(typeof(CameraCBuffer)), true, true))
            {
                data.Write(camera.Position);
                data.Write(0f);
                data.Position = 0;
                context.UpdateSubresource(new DataBox(0, 0, data), cameraConstantBuffer, 0);
                context.VertexShader.SetConstantBuffer(cameraConstantBuffer, 1);
            }
        }

        protected override void Dispose(bool disposable)
        {
            Contract.Ensures(lightConstantBuffer == null, "This function must dispose of the lightConstantBuffer variable.");
            Contract.Ensures(cameraConstantBuffer == null, "This function must dispose of the cameraConstantBuffer variable.");
            if (lightConstantBuffer != null)
            {
                lightConstantBuffer.Dispose();
                lightConstantBuffer = null;
            }
            if (cameraConstantBuffer != null)
            {
                cameraConstantBuffer.Dispose();
                cameraConstantBuffer = null;
            }
            base.Dispose(disposable);
        }
    }
}
