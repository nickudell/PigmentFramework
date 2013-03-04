using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;

namespace Pigment.Engine.Rendering
{
    /// <summary>
    /// Base class for simpler shader interaction
    /// </summary>
    public abstract class ShaderBase : IDisposable
    {
        /// <summary>
        /// The vertex shader
        /// </summary>
        private VertexShader vertexShader;
        /// <summary>
        /// The pixel shader
        /// </summary>
        private PixelShader pixelShader;
        /// <summary>
        /// The input layout
        /// </summary>
        private InputLayout inputLayout;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderBase" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="vertexShaderPath">The vertex shader file path.</param>
        /// <param name="pixelShaderPath">The pixel shader file path.</param>
        protected ShaderBase(Device device, string vertexShaderPath, string pixelShaderPath, IInputLayoutProvider inputLayoutMaker)
        {
            ShaderSignature inputSignature;
            using (ShaderBytecode bytecode = ShaderBytecode.CompileFromFile(vertexShaderPath, "VShader", "vs_4_0", ShaderFlags.None, EffectFlags.None))
            {
                vertexShader = new VertexShader(device, bytecode);
                inputSignature = ShaderSignature.GetInputSignature(bytecode);
            }
            using (ShaderBytecode bytecode = ShaderBytecode.CompileFromFile(pixelShaderPath, "PShader", "ps_4_0", ShaderFlags.None, EffectFlags.None))
                pixelShader = new PixelShader(device, bytecode);
            inputLayout = inputLayoutMaker.MakeInputLayout(device, inputSignature);
        }

        /// <summary>
        /// Sets up the shader on the GPU.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual void SetupShader(DeviceContext context)
        {
            context.InputAssembler.InputLayout = inputLayout;
            context.VertexShader.Set(vertexShader);
            context.PixelShader.Set(pixelShader);
        }

        protected virtual void Dispose(bool disposable)
        {
            if (vertexShader != null)
            {
                vertexShader.Dispose();
                vertexShader = null;
            }
            if (pixelShader != null)
            {
                pixelShader.Dispose();
                pixelShader = null;
            }
            if (inputLayout != null)
            {
                inputLayout.Dispose();
                vertexShader = null;
            }
        }

        /// <summary>
        /// Releases resources
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
