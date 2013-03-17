using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;

namespace Pigment.Engine.Rendering.Shaders
{
    /// <summary>
    /// Shader that renders transformed vertices with a texture
    /// </summary>
    public class ColourTextureShader : TextureShader
    {
        /// <summary>
        /// Helper class for building InputLayouts for the ColourTexture shaders
        /// </summary>
        private class ColourTextureInputLayoutMaker : IInputLayoutProvider
        {

            /// <summary>
            /// Builds an input layout based on what this shader requires, and the shader signature passed to it
            /// </summary>
            /// <param name="device">The D3D Device to create the layout with</param>
            /// <param name="inputSignature">The shader input signature to verify the input layout against</param>
            /// <returns>
            /// An input layout for this shader
            /// </returns>
            public InputLayout MakeInputLayout(Device device, ShaderSignature inputSignature)
            {
                return new InputLayout(device, inputSignature, new[] { 
                new InputElement("POSITION", 0, SlimDX.DXGI.Format.R32G32B32_Float, 0), 
                new InputElement("COLOR",0,SlimDX.DXGI.Format.R32G32B32A32_Float,0),
                new InputElement("TEXCOORD",0,SlimDX.DXGI.Format.R32G32_Float,0)
                });
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TextureShader" /> class and creates the sampler.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="vertexShaderPath">The vertex shader path.</param>
        /// <param name="pixelShaderPath">The pixel shader path.</param>
        public ColourTextureShader(Device device)
            : this(device, "shaders/colourtexture.fx", "shaders/colourtexture.fx")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureShader" /> class and creates the sampler.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="vertexShaderPath">The vertex shader path.</param>
        /// <param name="pixelShaderPath">The pixel shader path.</param>
        public ColourTextureShader(Device device, string vertexShaderPath, string pixelShaderPath)
            : this(device, vertexShaderPath, pixelShaderPath, new ColourTextureInputLayoutMaker())
        {
        }

        protected ColourTextureShader(Device device, string vertexShaderPath, string pixelShaderPath, IInputLayoutProvider inputLayoutMaker) : base(device,vertexShaderPath,pixelShaderPath,inputLayoutMaker)
        {

        }
    }
}
