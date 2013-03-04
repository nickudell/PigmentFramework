using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.D3DCompiler;
using SlimDX;
using SlimDX.Direct3D11;

namespace Pigment.Engine.Rendering
{
    /// <summary>
    /// Simple colour shader
    /// </summary>
    public class ColourShader  : WVPTransformShader
    {
        /// <summary>
        /// Helper class for making InputLayout for Colour shaders
        /// </summary>
        private class ColourInputMaker : IInputLayoutProvider
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
                return new InputLayout(device, inputSignature, new[]
                { 
                    new InputElement("POSITION", 0, SlimDX.DXGI.Format.R32G32B32_Float, 0), 
                    new InputElement("COLOR",0,SlimDX.DXGI.Format.R32G32B32_Float,0)
                });
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ColourShader" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public ColourShader(Device device) : this(device,"shaders/colour.fx", "shaders/colour.fx")
        {

        }

        public ColourShader(Device device, string vertexShaderPath, string pixelShaderPath) : this(device, vertexShaderPath,pixelShaderPath,new ColourInputMaker())
        {

        }

        protected ColourShader(Device device, string vertexShaderPath, string pixelShaderPath, IInputLayoutProvider inputLayoutMaker) : base(device,vertexShaderPath,pixelShaderPath,inputLayoutMaker)
        {

        }
    }
}
