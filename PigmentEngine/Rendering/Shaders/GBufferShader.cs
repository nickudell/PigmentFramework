﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace Pigment.Engine.Rendering.Shaders
{
	public class GBufferShader : LightShader
	{
		/// <summary>
		/// InputLayout maker class for bumpmap Shaders
		/// </summary>
		private class GBufferInputLayoutMaker : IInputLayoutProvider
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
					new InputElement("NORMAL",0,SlimDX.DXGI.Format.R32G32B32_Float,0),
					new InputElement("TANGENT",0,SlimDX.DXGI.Format.R32G32B32_Float,0),
					new InputElement("BINORMAL",0,SlimDX.DXGI.Format.R32G32B32_Float,0)
				});
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GBufferShader"/> class.
		/// </summary>
		/// <param name="device">The device.</param>
		public GBufferShader(Device device) : this(device, "shaders/gbuffer.fx","shaders/gbuffer.fx", new GBufferInputLayoutMaker())
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GBufferShader"/> class.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <param name="vertexShaderPath">The vertex shader path.</param>
		/// <param name="pixelShaderPath">The pixel shader path.</param>
		public GBufferShader(Device device, string vertexShaderPath, string pixelShaderPath) : this(device,vertexShaderPath, pixelShaderPath, new GBufferInputLayoutMaker())
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GBufferShader"/> class.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <param name="vertexShaderPath">The vertex shader path.</param>
		/// <param name="pixelShaderPath">The pixel shader path.</param>
		/// <param name="inputLayoutMaker">The input layout maker.</param>
		protected GBufferShader(Device device, string vertexShaderPath, string pixelShaderPath, IInputLayoutProvider inputLayoutMaker) : base(device,vertexShaderPath,pixelShaderPath,inputLayoutMaker)
		{
		}
	}
}
