using SlimDX;
using SlimDX.Direct3D11;

namespace Pigment.Engine.Rendering
{
    /// <summary>
    /// Base class for render-to-texture helper classes
    /// </summary>
    public abstract class RenderTextureBase
    {
        /// <summary>
        /// Save the texture to a file on the HDD
        /// </summary>
        /// <param name="context">The device context (used for the Texture2D's in-build saving function)</param>
        public abstract void WriteToFile(DeviceContext context);

        /// <summary>
        /// Set this class' texture as the supplied context's render target and set the supplied depthStencilView too
        /// </summary>
        /// <param name="context">The DeviceContext to which we set this texture as the render target</param>
        /// <param name="depthStencilView">The depth stencil we also wish to set to the context</param>
        public abstract void SetRenderTarget(DeviceContext context, DepthStencilView depthStencilView);

        /// <summary>
        /// Set this class' texture as the supplied context's render target
        /// </summary>
        /// <param name="context">The DeviceContext to which we set this texture as the render target</param>
        public abstract void SetRenderTarget(DeviceContext context);

        /// <summary>
        /// Clear the render target of the supplied context to the supplied colour
        /// </summary>
        /// <param name="context">The context whose render target we are clearing</param>
        /// <param name="red">The red component of the colour we wish to clear the render target to</param>
        /// <param name="green">The green component of the colour we wish to clear the render target to</param>
        /// <param name="blue">The blue component of the colour we wish to clear the render target to</param>
        /// <param name="alpha">The alpha component of the colour we wish to clear the render target to</param>
        public void ClearRenderTarget(DeviceContext context, float red, float green, float blue, float alpha)
        {
            ClearRenderTarget(context, new Color4(alpha, red, green, blue));
        }

        /// <summary>
        /// Clear the render target of the supplied context to the supplied colour
        /// </summary>
        /// <param name="context">The context whose render target we are clearing</param>
        /// <param name="color">The colour we wish to clear the render target to</param>
        public abstract void ClearRenderTarget(DeviceContext context, Color4 color);

        /// <summary>
        /// Dispose of all the resources this object holds.
        /// </summary>
        public abstract void Dispose();
    }
}