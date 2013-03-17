using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;

namespace Pigment.Engine.Rendering.Shaders
{
    /// <summary>
    /// Enforces the creation of an InputLayout
    /// </summary>
    public interface IInputLayoutProvider
    {
        /// <summary>
        /// Builds an input layout based on what this shader requires, and the shader signature passed to it
        /// </summary>
        /// <param name="device">The D3D Device to create the layout with</param>
        /// <param name="inputSignature">The shader input signature to verify the input layout against</param>
        /// <returns>An input layout for this shader</returns>
        InputLayout MakeInputLayout(Device device, ShaderSignature inputSignature);
    }
}
