using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.D3DCompiler;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Rendering
{
    /// <summary>
    /// Shader that renders transformed vertices with a texture
    /// </summary>
    public class TextureShader : WVPTransformShader
    {
/// <summary>
///  InputLayout maker class for Light Shaders
/// </summary>
        private class TextureShaderInputMaker : IInputLayoutProvider
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
        /// The texture sampler
        /// </summary>
        private SamplerState sampler;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureShader" /> class and creates the sampler.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="vertexShaderPath">The vertex shader path.</param>
        /// <param name="pixelShaderPath">The pixel shader path.</param>
        public TextureShader(Device device)
            : this(device, "shaders/texture.fx", "shaders/texture.fx")
        {
        }

        public TextureShader(Device device, string vertexShaderPath, string pixelShaderPath) : this(device,vertexShaderPath,pixelShaderPath,new TextureShaderInputMaker())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureShader" /> class and creates the sampler.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="vertexShaderPath">The vertex shader path.</param>
        /// <param name="pixelShaderPath">The pixel shader path.</param>
        protected TextureShader(Device device, string vertexShaderPath, string pixelShaderPath, IInputLayoutProvider inputLayoutMaker)
            : base(device, vertexShaderPath, pixelShaderPath, inputLayoutMaker)
        {
            SamplerDescription sampleDesc = new SamplerDescription()
            {
                Filter = Filter.MinMagMipLinear,
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                MipLodBias = 0f,
                MaximumAnisotropy = 1,
                ComparisonFunction = Comparison.Always,
                BorderColor = new Color4(0, 0, 0, 0),
                MinimumLod = 0,
                MaximumLod = float.MaxValue
            };

            sampler = SamplerState.FromDescription(device, sampleDesc);
        }

        /// <summary>
        /// Sets the texture.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="texture">The texture.</param>
        public void SetTexture(DeviceContext context, Texture texture)
        {
            Contract.Requires<ArgumentNullException>(context != null, "context");
            Contract.Requires<ArgumentNullException>(texture != null, "texture");
            context.PixelShader.SetShaderResource(texture.TextureView, 0);
        }

        /// <summary>
        /// Sets the textures.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="texture">The textures.</param>
        public void SetTextures(DeviceContext context, Texture[] textures)
        {
            Contract.Requires<ArgumentNullException>(context != null, "context");
            Contract.Requires<ArgumentNullException>(textures != null, "textures");
            if (textures.Length == 1)
            {
                SetTexture(context, textures[0]);
            }
            else
            {
			    ShaderResourceView[] views = new ShaderResourceView[textures.Length];
			    for (int i = 0; i < textures.Length; i++)
			    {
			        views[i] = textures[i].TextureView;
			    }
			    context.PixelShader.SetShaderResources(views, 0, textures.Length);
            }
        }

        /// <summary>
        /// Sets up the shader on the GPU.
        /// </summary>
        /// <param name="context">The context.</param>
        public override void SetupShader(DeviceContext context)
        {
            Contract.Requires<ArgumentNullException>(context != null, "context");
            base.SetupShader(context);
            context.PixelShader.SetSampler(sampler, 0);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposable"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposable)
        {
            Contract.Ensures(!disposable || disposable ==(sampler==null));
            if (disposable)
            {
                if (sampler != null)
                {
                    sampler.Dispose();
                    sampler = null;
                }
            }
            base.Dispose(disposable);
        }
    }
}
