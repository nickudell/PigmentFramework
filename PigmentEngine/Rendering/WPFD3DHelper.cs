using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flaxen.SlimDXControlLib;
using SlimDX.Direct3D11;
using SlimDX;

namespace Pigment.Engine.Rendering
{
    public class WPFD3DHelper : RenderEngine
    {
        /// <summary>
        /// The swap chain
        /// </summary>
        private SlimDX.DXGI.SwapChain swapChain;

        /// <summary>
        /// The render target for the screen
        /// </summary>
        private RenderTargetView renderTarget;

        /// <summary>
        /// The Direct3D11 device to use
        /// </summary>
        public Device Device
        {
            get
            {
                return base.Device;
            }
        }

        /// <summary>
        /// The device context to render to
        /// </summary>
        public DeviceContext Context { get; private set; }

        private Texture2D depthStencilBuffer;

        private DepthStencilState depthStencilState;
        private DepthStencilState disabledDepthStencilState;

        private DepthStencilView depthStencilView;

        private RasterizerState rasterState;

        private bool vsync;

        private bool depthEnabled;
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

        public override void Initialize(SlimDXControl control)
        {
            Control = control;
        }

        public WPFD3DHelper(RenderWindow window, bool vsync, bool fullscreen, float screenDepth, float screenNear)
        {
            this.vsync = vsync;
            int numerator = 0;
            int denominator = 1;
            SlimDX.DXGI.SwapChainDescription scd = new SlimDX.DXGI.SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new SlimDX.DXGI.ModeDescription()
                {
                    Width = WindowWidth,
                    Height = WindowHeight,
                    Format = SlimDX.DXGI.Format.R8G8B8A8_UNorm,
                    RefreshRate = new Rational(numerator, denominator),
                    ScanlineOrdering = SlimDX.DXGI.DisplayModeScanlineOrdering.Unspecified,
                    Scaling = SlimDX.DXGI.DisplayModeScaling.Unspecified
                },
                Usage = SlimDX.DXGI.Usage.RenderTargetOutput,
                OutputHandle = window.Handle,
                SampleDescription = new SlimDX.DXGI.SampleDescription()
                {
                    Count = 1,
                    Quality = 0
                },
                IsWindowed = true,
                SwapEffect = SlimDX.DXGI.SwapEffect.Discard,
                Flags = SlimDX.DXGI.SwapChainFlags.None
            };
            Device device;
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, scd, out device, out swapChain);
            base.Device = device;
            Context = device.ImmediateContext;
            using (var factory = swapChain.GetParent<SlimDX.DXGI.Factory>())
            {
                factory.SetWindowAssociation(window.Handle, SlimDX.DXGI.WindowAssociationFlags.IgnoreAltEnter);
            }
            window.KeyDown += (o, e) =>
            {
                if (e.Alt && e.KeyCode == System.Windows.Forms.Keys.Enter)
                    swapChain.IsFullScreen = !swapChain.IsFullScreen;
            };
            Context.Rasterizer.SetViewports(new Viewport(0.0f, 0.0f, window.ClientSize.Width, window.ClientSize.Height));

            using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
                renderTarget = new RenderTargetView(Device, resource);

            buildDepthBuffer(window.ClientSize.Width, window.ClientSize.Height);
            DepthEnabled = true;
            Context.OutputMerger.SetTargets(depthStencilView, renderTarget);

            // handle form size changes
            window.UserResized += (o, e) =>
            {
                renderTarget.Dispose();
                depthStencilBuffer.Dispose();
                buildDepthBuffer(window.ClientSize.Width, window.ClientSize.Height);
                swapChain.ResizeBuffers(2, 0, 0, SlimDX.DXGI.Format.R8G8B8A8_UNorm, SlimDX.DXGI.SwapChainFlags.AllowModeSwitch);
                using (var resource = Resource.FromSwapChain<Texture2D>(swapChain, 0))
                    renderTarget = new RenderTargetView(Device, resource);

                Context.OutputMerger.SetTargets(depthStencilView, renderTarget);
            };

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
            rasterState = RasterizerState.FromDescription(Device, rsd);
            Device.ImmediateContext.Rasterizer.State = rasterState;

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
            BlendState bs = BlendState.FromDescription(device, bsd);
            device.ImmediateContext.OutputMerger.BlendState = bs;


            //swapChain.IsFullScreen = fullscreen;
        }

        private void buildDepthBuffer(int width, int height)
        {
            Texture2DDescription depthBufferDesc = new Texture2DDescription()
            {
                Width = width,
                Height = height,
                MipLevels = 1,
                ArraySize = 1,
                Format = SlimDX.DXGI.Format.D24_UNorm_S8_UInt,
                SampleDescription = new SlimDX.DXGI.SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.DepthStencil,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            depthStencilBuffer = new Texture2D(Device, depthBufferDesc);

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

            depthStencilState = DepthStencilState.FromDescription(Device, depthStencilDesc);

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

            disabledDepthStencilState = DepthStencilState.FromDescription(Device, disabledDepthStencilDesc);

            DepthStencilViewDescription depthStencilViewDesc = new DepthStencilViewDescription()
            {
                Format = SlimDX.DXGI.Format.D24_UNorm_S8_UInt,
                Dimension = DepthStencilViewDimension.Texture2D,
                MipSlice = 0
            };

            depthStencilView = new DepthStencilView(Device, depthStencilBuffer, depthStencilViewDesc);
        }

        public void BeginScene(Color4 resetColour)
        {
            Context.ClearRenderTargetView(renderTarget, resetColour);
            Context.ClearDepthStencilView(depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        public void EndScene()
        {
            if (vsync)
            {
                swapChain.Present(1, SlimDX.DXGI.PresentFlags.None);
            }
            else
            {
                swapChain.Present(0, SlimDX.DXGI.PresentFlags.None);
            }
        }

        public void Dispose()
        {
            swapChain.IsFullScreen = false;

            rasterState.Dispose();
            depthStencilView.Dispose();
            depthStencilState.Dispose();
            depthStencilBuffer.Dispose();
            renderTarget.Dispose();
            Context.Dispose();
            Device.Dispose();
            swapChain.Dispose();
        }
    }
}
