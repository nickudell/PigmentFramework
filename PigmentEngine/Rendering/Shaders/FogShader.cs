using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;
using SlimDX.D3DCompiler;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Rendering.Shaders
{
	/// <summary>
	/// Shader that renders transformed vertices with a texture
	/// </summary>
	public class FogShader : TextureShader
	{
		private struct FogCBuffer
		{
			private float fogStart;

			public float FogStart
			{
				get { return fogStart; }
				set { fogStart = value; }
			}

			private float fogEnd;

			public float FogEnd
			{
				get { return fogEnd; }
				set { fogEnd = value; }
			}

			private Color3 fogColour;

			public Color3 FogColour
			{
				get { return fogColour; }
				set { fogColour = value; }
			}

			public Color3 Buffer;
			
			
		}

		private Buffer fogConstantBuffer;

		/// <summary>
		///  InputLayout maker class for Light Shaders
		/// </summary>
		private class FogShaderInputMaker : IInputLayoutProvider
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
		public FogShader(Device device)
			: this(device, "shaders/fog.fx", "shaders/fog.fx")
		{
		}

		public FogShader(Device device, string vertexShaderPath, string pixelShaderPath) : this(device,vertexShaderPath,pixelShaderPath,new FogShaderInputMaker())
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TextureShader" /> class and creates the sampler.
		/// </summary>
		/// <param name="device">The device.</param>
		/// <param name="vertexShaderPath">The vertex shader path.</param>
		/// <param name="pixelShaderPath">The pixel shader path.</param>
		protected FogShader(Device device, string vertexShaderPath, string pixelShaderPath, IInputLayoutProvider inputLayoutMaker)
			: base(device, vertexShaderPath, pixelShaderPath, inputLayoutMaker)
		{
			Contract.Ensures(fogConstantBuffer != null, "fogConstantBuffer must not be null after this method executes.");
			BufferDescription bufferDesc = new BufferDescription(System.Runtime.InteropServices.Marshal.SizeOf(typeof(FogCBuffer)), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
			fogConstantBuffer = new SlimDX.Direct3D11.Buffer(device, bufferDesc);
		}

		public void SetFogParameters(DeviceContext context, float start, float end, Color3 fogColour)
		{
			Contract.Requires<ArgumentNullException>(fogColour != null, "fogColour");
			using (DataStream data = new DataStream(System.Runtime.InteropServices.Marshal.SizeOf(typeof(FogCBuffer)), true, true))
			{
				data.Write(start);
				data.Write(end);
				data.Write(fogColour);
				data.Position = 0;
				context.UpdateSubresource(new DataBox(0, 0, data), fogConstantBuffer, 0);
				context.VertexShader.SetConstantBuffer(fogConstantBuffer, 0);
			}
		}

		protected override void Dispose(bool managed)
		{
			Contract.Ensures(fogConstantBuffer == null, "fogConstantBuffer must be null after this method executes.");
			if (managed)
			{
				//Check if fogConstantBuffer still exists and if so, dispose it and set it to null.
				if (fogConstantBuffer != null)
				{
					fogConstantBuffer.Dispose();
					fogConstantBuffer = null;
				}
			}
			base.Dispose(managed);
		}
	}
}
