using SlimDX;
using SlimDX.Direct3D11;

namespace Pigment.Engine.Rendering
{
    /// <summary>
    /// Helper class for render-to-texture functionality to save time
    /// </summary>
    public class RenderTextureHelper : RenderTextureBase
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
        public RenderTextureHelper(Device device, int texWidth, int texHeight)
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

        public override void Dispose()
        {
            shaderResourceView.Dispose();
            renderTargetView.Dispose();
            Texture.Dispose();
        }

        public override void WriteToFile(DeviceContext context)
        {
            Texture2D.ToFile(context, Texture, ImageFileFormat.Jpg, "texture.jpg");
        }
    }
}