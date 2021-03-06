﻿using SlimDX;
using SlimDX.Direct3D11;
using System;

namespace Pigment.Engine.Rendering.Textures
{
    public class MultiRenderTexture : RenderTextureBase
    {
        public Texture2D[] Texture { get; set; }

        RenderTargetView[] renderTargetView;
        ShaderResourceView[] shaderResourceView;

        public MultiRenderTexture(Device device, int texWidth, int texHeight, int numTex)
        {
            Texture2DDescription textureDescription = new Texture2DDescription()
                {
                    Width = texWidth,
                    Height = texHeight,
                    MipLevels = 1,
                    ArraySize = numTex,
                    Format = SlimDX.DXGI.Format.R32G32B32A32_Float,
                    SampleDescription = new SlimDX.DXGI.SampleDescription(1, 0),
                    BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    Usage = ResourceUsage.Default,
                };
            Texture = new Texture2D[numTex];
            renderTargetView = new RenderTargetView[numTex];
            shaderResourceView = new ShaderResourceView[numTex];
            for (int i = 0; i < numTex; i++)
            {
                Texture[i] = new Texture2D(device, textureDescription);
                RenderTargetViewDescription renderTargetViewDescription = new RenderTargetViewDescription()
                {
                    Format = textureDescription.Format,
                    Dimension = RenderTargetViewDimension.Texture2D,
                    MipSlice = 0,
                };

                renderTargetView[i] = new RenderTargetView(device, Texture[i], renderTargetViewDescription);

                ShaderResourceViewDescription shaderResourceViewDescription = new ShaderResourceViewDescription()
                {
                    Format = textureDescription.Format,
                    Dimension = ShaderResourceViewDimension.Texture2D,
                    MostDetailedMip = 0,
                    MipLevels = 1
                };

                shaderResourceView[i] = new ShaderResourceView(device, Texture[i], shaderResourceViewDescription);
            }
        }

        public override void WriteToFile(DeviceContext context)
        {
            for (int index = 0; index < Texture.Length; index++)
            {
                WriteToFile(context, index);
            }
        }

        public void WriteToFile(DeviceContext context, int index)
        {
            Texture2D.ToFile(context, Texture[index], ImageFileFormat.Jpg, "texture" + index + ".jpg");
        }

        public override void SetRenderTarget(DeviceContext context, DepthStencilView depthStencilView)
        {
            context.OutputMerger.SetTargets(depthStencilView, renderTargetView);
        }

        public override void SetRenderTarget(DeviceContext context)
        {
            context.OutputMerger.SetTargets(renderTargetView);
        }

        public void SetRenderTarget(DeviceContext context, DepthStencilView depthStencilView, int arrayIndex)
        {
            context.OutputMerger.SetTargets(depthStencilView, renderTargetView[arrayIndex]);
        }

        public override void ClearRenderTarget(DeviceContext context, Color4 color)
        {
            foreach (RenderTargetView view in renderTargetView)
            {
                context.ClearRenderTargetView(view, color);
            }
        }

        public void ClearRenderTarget(DeviceContext context, int arrayIndex, float red, float green, float blue, float alpha)
        {
            ClearRenderTarget(context, arrayIndex, new Color4(alpha, red, green, blue));
        }

        public void ClearRenderTarget(DeviceContext context, int arrayIndex, Color4 color)
        {
            context.ClearRenderTargetView(renderTargetView[arrayIndex], color);
        }

        public ShaderResourceView[] GetShaderResourceView()
        {
            return shaderResourceView;
        }

        public ShaderResourceView GetShaderResourceView(int arrayIndex)
        {
            return shaderResourceView[arrayIndex];
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
                for (int i = 0; i < shaderResourceView.Length; i++)
                {
                    //Check if shaderResourceView[i] still exists and if so, dispose it and set it to null.
                    if (shaderResourceView[i] != null)
                    {
                        shaderResourceView[i].Dispose();
                        shaderResourceView[i] = null;
                    }

                    //Check if renderTargetView[i] still exists and if so, dispose it and set it to null.
                    if (renderTargetView[i] != null)
                    {
                        renderTargetView[i].Dispose();
                        renderTargetView[i] = null;
                    }

                    //Check if Texture[i] still exists and if so, dispose it and set it to null.
                    if (Texture[i] != null)
                    {
                        Texture[i].Dispose();
                        Texture[i] = null;
                    }
                }
            }
        }
    }
}