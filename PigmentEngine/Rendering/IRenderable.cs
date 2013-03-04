using SlimDX.Direct3D11;

namespace Pigment.Engine.Rendering
{
    /// <summary>
    /// Renderable interface.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Draws this object to the specified context
        /// </summary>
        /// <param name="context">The context.</param>
        void Draw(DeviceContext context);
    }
}