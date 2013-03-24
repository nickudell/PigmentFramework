using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using SlimDX.DXGI;
using SlimDX.Windows;
using Buffer = SlimDX.Direct3D11.Buffer;
using Device = SlimDX.Direct3D11.Device;
using SlimDX.D3DCompiler;

namespace Pigment.Engine.Rendering
{
    public class D3DHelper : IDisposable
    {
        /// <summary>
        /// The D3D device
        /// </summary>
        public Device D3DDevice;

        /// <summary>
        /// The context
        /// </summary>
        public DeviceContext Context;
        RenderTargetView SampleRenderView;
        DepthStencilView SampleDepthView;
        Texture2D DepthTexture;
        /// <summary>
        /// The window width
        /// </summary>
        public int WindowWidth;
        /// <summary>
        /// The window height
        /// </summary>
        public int WindowHeight;

        /// <summary>
        /// Gets or sets the shared texture.
        /// </summary>
        /// <value>
        /// The shared texture.
        /// </value>
        public Texture2D SharedTexture
        {
            get;
            set;
        }

        private DepthStencilState depthStencilState;
        private DepthStencilState disabledDepthStencilState;

        private RasterizerState rasterState;

        private bool vsync;

        /// <summary>
        /// The depth enabled
        /// </summary>
        private bool depthEnabled;
        /// <summary>
        /// Gets or sets a value indicating whether [depth enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [depth enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool DepthEnabled
        {
            get
            {
                return depthEnabled;
            }
            set
            {
                if (value != depthEnabled)
                {
                    if (value)
                    {
                        Context.OutputMerger.DepthStencilState = depthStencilState;
                    }
                    else
                    {
                        Context.OutputMerger.DepthStencilState = disabledDepthStencilState;
                    }
                    depthEnabled = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="D3DHelper"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// Width must be greater than 0.
        /// or
        /// Height must be greater than 0.
        /// </exception>
        public D3DHelper(int width, int height)
        {
            if (width > 0)
            {
                WindowWidth = width;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Width must be greater than 0.");
            }
            if (height > 0)
            {
                WindowHeight = height;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Height must be greater than 0.");
            }

            InitD3D();
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
                if (SampleRenderView != null)
                {
                    SampleRenderView.Dispose();
                    SampleRenderView = null;
                }

                if (SampleDepthView != null)
                {
                    SampleDepthView.Dispose();
                    SampleDepthView = null;
                }

                if (SharedTexture != null)
                {
                    SharedTexture.Dispose();
                    SharedTexture = null;
                }

                if (DepthTexture != null)
                {
                    DepthTexture.Dispose();
                    DepthTexture = null;
                }

                if (Context != null)
                {
                    Context.Dispose();
                    Context = null;
                }

                if (D3DDevice != null)
                {
                    D3DDevice.Dispose();
                    D3DDevice = null;
                }
            }
        }

        public void FinishRender()
        {
            Context.Flush();
        }

        public void SetupRender(Color4 backgroundColour)
        {
            Context.OutputMerger.SetTargets(SampleDepthView, SampleRenderView);
            Context.Rasterizer.SetViewports(new Viewport(0, 0, WindowWidth, WindowHeight, 0.0f, 1.0f));

            Context.ClearDepthStencilView(SampleDepthView, DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1.0f, 0);
            Context.ClearRenderTargetView(SampleRenderView, backgroundColour);
        }

        void InitD3D()
        {
            D3DDevice = new Device(DriverType.Hardware, DeviceCreationFlags.Debug | DeviceCreationFlags.BgraSupport, FeatureLevel.Level_11_0);
            Context = D3DDevice.ImmediateContext;
            
            createRenderTarget();
            createDepthBuffer();

            createRasterizerState();

            createBlendState();
        }

        private void createRasterizerState()
        {
            RasterizerStateDescription rsd = new RasterizerStateDescription()
            {
                //CullMode = CullMode.None,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,

                //FillMode = FillMode.Wireframe,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = false,
                IsFrontCounterclockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0.0f
            };
            rasterState = RasterizerState.FromDescription(D3DDevice, rsd);
            D3DDevice.ImmediateContext.Rasterizer.State = rasterState;
        }

        private static void createBlendState()
        {
            BlendStateDescription bsd = new BlendStateDescription();
            
            bsd.RenderTargets[0] = new RenderTargetBlendDescription()
            {
                BlendEnable = true,
                SourceBlend = BlendOption.SourceAlpha,
                DestinationBlend = BlendOption.InverseSourceAlpha,
                BlendOperation = BlendOperation.Add,
                SourceBlendAlpha = BlendOption.Zero,
                DestinationBlendAlpha = BlendOption.Zero,
                BlendOperationAlpha = BlendOperation.Add,
                RenderTargetWriteMask = ColorWriteMaskFlags.All

            };
            /*

            BlendState bs = BlendState.FromDescription(D3DDevice, bsd);

            D3DDevice.ImmediateContext.OutputMerger.BlendState = bs;*/
        }

        private void createDepthBuffer()
        {
            Texture2DDescription depthdesc = new Texture2DDescription();
            depthdesc.BindFlags = BindFlags.DepthStencil;
            depthdesc.Format = Format.D32_Float_S8X24_UInt;
            depthdesc.Width = WindowWidth;
            depthdesc.Height = WindowHeight;
            depthdesc.MipLevels = 1;
            depthdesc.SampleDescription = new SampleDescription(1, 0);
            depthdesc.Usage = ResourceUsage.Default;
            depthdesc.OptionFlags = ResourceOptionFlags.None;
            depthdesc.CpuAccessFlags = CpuAccessFlags.None;
            depthdesc.ArraySize = 1;

            DepthTexture = new Texture2D(D3DDevice, depthdesc);

            SampleDepthView = new DepthStencilView(D3DDevice, DepthTexture);

            DepthStencilStateDescription depthStencilDesc = new DepthStencilStateDescription()
            {
                IsDepthEnabled = true,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,

                IsStencilEnabled = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,

                FrontFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                },

                BackFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                }
            };

            depthStencilState = DepthStencilState.FromDescription(D3DDevice, depthStencilDesc);

            DepthStencilStateDescription disabledDepthStencilDesc = new DepthStencilStateDescription()
            {
                IsDepthEnabled = false,
                DepthWriteMask = DepthWriteMask.All,
                DepthComparison = Comparison.Less,

                IsStencilEnabled = true,
                StencilReadMask = 0xFF,
                StencilWriteMask = 0xFF,

                FrontFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Increment,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                },

                BackFace = new DepthStencilOperationDescription()
                {
                    FailOperation = StencilOperation.Keep,
                    DepthFailOperation = StencilOperation.Decrement,
                    PassOperation = StencilOperation.Keep,
                    Comparison = Comparison.Always
                }
            };

            disabledDepthStencilState = DepthStencilState.FromDescription(D3DDevice, disabledDepthStencilDesc);
        }

        private void createRenderTarget()
        {
            Texture2DDescription colordesc = new Texture2DDescription();
            colordesc.BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource;
            colordesc.Format = Format.B8G8R8A8_UNorm;
            colordesc.Width = WindowWidth;
            colordesc.Height = WindowHeight;
            colordesc.MipLevels = 1;
            colordesc.SampleDescription = new SampleDescription(1, 0);
            colordesc.Usage = ResourceUsage.Default;
            colordesc.OptionFlags = ResourceOptionFlags.Shared;
            colordesc.CpuAccessFlags = CpuAccessFlags.None;
            colordesc.ArraySize = 1;

            SharedTexture = new Texture2D(D3DDevice, colordesc);

            SampleRenderView = new RenderTargetView(D3DDevice, SharedTexture);
        }
    }
}
