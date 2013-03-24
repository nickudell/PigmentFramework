using SlimDX;
using SlimDX.Direct3D11;
using System;

namespace Pigment.Engine.Rendering.Textures
{
    /// <summary>
    /// Helper class for render-to-texture functionality to save time
    /// </summary>
    public class RenderTexture : RenderTextureBase, IDisposable
    {
        //The texture we are rendering to
        public Texture2D Texture { get; set; }

        //The texture's rendertargetview for binding the texture as a render target
        RenderTargetView renderTargetView;
        //the texture's shaderresourceview for binding the texture as a shader resource
        ShaderResourceView shaderResourceView;

        /// <summary>
        /// Constructor
        /// Creates the texture we will render to based on the supplied width and height
        /// </summary>
        /// <param name="device">The device we will create the texture with</param>
        /// <param name="texWidth"></param>
        /// <param name="texHeight"></param>
        public RenderTexture(Device device, int texWidth, int texHeight)
        {
            Texture2DDescription textureDescription = new Texture2DDescription()
                {
                    Width = texWidth,
                    Height = texHeight,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = SlimDX.DXGI.Format.R32G32B32A32_Float,
                    SampleDescription = new SlimDX.DXGI.SampleDescription(1, 0),
                    BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    Usage = ResourceUsage.Default,
                };
            Texture = new Texture2D(device, textureDescription);
            RenderTargetViewDescription renderTargetViewDescription = new RenderTargetViewDescription()
            {
                Format = textureDescription.Format,
                Dimension = RenderTargetViewDimension.Texture2D,
                MipSlice = 0,
            };

            renderTargetView = new RenderTargetView(device, Texture, renderTargetViewDescription);

            ShaderResourceViewDescription shaderResourceViewDescription = new ShaderResourceViewDescription()
            {
                Format = textureDescription.Format,
                Dimension = ShaderResourceViewDimension.Texture2D,
                MostDetailedMip = 0,
                MipLevels = 1
            };

            shaderResourceView = new ShaderResourceView(device, Texture, shaderResourceViewDescription);
        }

        public override void SetRenderTarget(DeviceContext context, DepthStencilView depthStencilView)
        {
            context.OutputMerger.SetTargets(depthStencilView, renderTargetView);
        }

        public override void ClearRenderTarget(DeviceContext context, Color4 color)
        {
            context.ClearRenderTargetView(renderTargetView, color);
        }

        public override void SetRenderTarget(DeviceContext context)
        {
            context.OutputMerger.SetTargets(renderTargetView);
        }

        public ShaderResourceView GetShaderResourceView()
        {
            return shaderResourceView;
        }

        public override void WriteToFile(DeviceContext context)
        {
            Texture2D.ToFile(context, Texture, ImageFileFormat.Jpg, "texture.jpg");
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool managed)
        {
            if (managed)
            {
                //Check if Texture still exists and if so, dispose it and set it to null.
                if (Texture != null)
                {
                    Texture.Dispose();
                    Texture = null;
                }

                //Check if renderTargetView still exists and if so, dispose it and set it to null.
                if (renderTargetView != null)
                {
                    renderTargetView.Dispose();
                    renderTargetView = null;
                }

                //Check if shaderResourceView still exists and if so, dispose it and set it to null.
                if (shaderResourceView != null)
                {
                    shaderResourceView.Dispose();
                    shaderResourceView = null;
                }
            }

        }
    }
}