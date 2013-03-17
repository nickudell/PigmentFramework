using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;

namespace Pigment.Engine.Rendering.Textures
{
    /// <summary>
    /// A texture
    /// </summary>
    public class Texture : IDisposable
    {
        /// <summary>
        /// Gets the texture.
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        public ShaderResourceView TextureView { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture" /> class by loading a texture from file.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="filename">The filename.</param>
        public Texture(Device device, string filename)
        {
            TextureView = ShaderResourceView.FromFile(device, filename);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        protected virtual void Dispose(bool disposable)
        {
            if (disposable)
            {
                if (TextureView != null)
                {
                    TextureView.Dispose();
                    TextureView = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
