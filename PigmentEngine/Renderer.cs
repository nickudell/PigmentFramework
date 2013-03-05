using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX.Direct3D11;
using Buffer = SlimDX.Direct3D11.Buffer;
using SlimDX;
using SlimDX.D3DCompiler;
using SlimDX.DXGI;
using Pigment.Engine.Rendering.UI.Font;
using Pigment.Engine.Input;
using System.Diagnostics.Contracts;
using Pigment.Engine.Sound;
using Pigment.WPF;

namespace Pigment.Engine.Rendering
{
    class Renderer : IDisposable
    {
        D3DHelper dx;

        private SlimDX.Matrix world;

        private SlimDX.Matrix ortho;

        /// <summary>
        /// The camera
        /// </summary>
        private Camera camera;

        /// <summary>
        /// The colour shader
        /// </summary>
        //private BumpShader bumpShader;
        private FogShader fogShader;

        private ColourTextureShader colourTextureShader;

        private Light light;

        private Bitmap bitmap;

        private FontEngine fontEngine;

        private Pigment.Engine.Input.Input input;

        /// <summary>
        /// The meshes to render
        /// </summary>
        private List<Mesh<VertexPosTexNormTanBinorm>> meshes;

        public Texture2D SharedTexture
        {
            get
            {
                return dx.SharedTexture;
            }
        }

        public Renderer()
        {
            Contract.Ensures(dx != null);
            dx = new D3DHelper(640, 480);
            LoadShaders();
            LoadMeshes();
            Sound.Audio sound = new Sound.Audio();
            Wave wave = new Wave("Sound/Music/M.wav");
            sound.AddSound(wave);
            CreateMatrices();
            fontEngine = new FontEngine(dx.D3DDevice, "font.fnt", "font.png", dx.WindowWidth, dx.WindowHeight);
            input = new Pigment.Engine.Input.Input();
            dx.Context.Flush();
        }

        private void LoadMeshes()
        {
            Contract.Ensures(meshes != null, "Meshes must be instantiated by this function");

            meshes = new List<Mesh<VertexPosTexNormTanBinorm>>();
            meshes.Add(Mesh<VertexPosTexNormTanBinorm>.FromObj(dx.D3DDevice, System.IO.Directory.GetCurrentDirectory() + "/Meshes/OBJ/GrayBlock.obj"));
        }

        private void LoadShaders()
        {
            //Contract.Ensures(bumpShader != null, "lightShader must be instantiated by this function");
            Contract.Ensures(colourTextureShader != null, "colourTextureShader must be instantiated by this function");

            fogShader = new FogShader(dx.D3DDevice);
            colourTextureShader = new ColourTextureShader(dx.D3DDevice);
            
        }

        private void CreateMatrices()
        {
            Contract.Ensures(camera != null, "Camera must be instantiated by this function.");
            Contract.Ensures(world != null, "World must be instantiated by this function.");
            Contract.Ensures(ortho != null, "Ortho must be instantiated by this function.");

            float screenNear = 0.1f;
            float screenDepth = 100f;
            camera = new Camera(dx.D3DDevice, dx.WindowWidth, dx.WindowHeight);
            camera.Position = new SlimDX.Vector3(0, 0.5f, -2.2f);
            world = SlimDX.Matrix.Identity;
            ortho = SlimDX.Matrix.OrthoLH(dx.WindowWidth, dx.WindowHeight, screenNear, screenDepth);
        }

        public delegate void TickDelegate(double timestep);

        public event TickDelegate OnTick;

        [Serializable]
        public class InstanceVariableException : Exception
        {
            public InstanceVariableException() : base ("Instance Variable is incorrect", null)
            {

            }

            public InstanceVariableException(string message, Exception innerException) : base(message, innerException)
            {

            }
        }

        public void MoveRight(float force)
        {
            camera.Position = new Vector3( camera.Position.X - (float)(Math.Sin(-camera.Angle.Y + (Math.PI / 2)) * force),camera.Position.Y,camera.Position.Z - (float)(Math.Cos(-camera.Angle.Y + (Math.PI / 2)) * force));
        }

        public void MoveForward(float force)
        {
            camera.Position = new Vector3(camera.Position.X - (float)(Math.Cos(-camera.Angle.Y + (Math.PI / 2)) * force), camera.Position.Y, camera.Position.Z + (float)(Math.Sin(-camera.Angle.Y + (Math.PI / 2)) * force));
        }

        public void MoveUp(float distance)
        {
            camera.Position = new Vector3(camera.Position.X,camera.Position.Y + distance,camera.Position.Z);
        }

        public void LookUp(float angle)
        {
            camera.Angle = new Vector3(camera.Angle.X+ angle,camera.Angle.Y,camera.Angle.Z);
        }

        public void LookRight(float angle)
        {
            camera.Angle = new Vector3(camera.Angle.X, camera.Angle.Y + angle, camera.Angle.Z);
        }

        public void Render(double timeStep)
        {
            Contract.Requires<ArgumentException>(timeStep > 0, "Parameter timeStep must be greater than 0");
            dx.SetupRender(new Color4(1,0.8f,0.8f,0.8f));

            //Process controls
            foreach (System.Windows.Forms.Keys key in input.Pressed.Keys)
            {
                switch(key)
                {
                    case System.Windows.Forms.Keys.W:
                        MoveUp((float)(15 * timeStep));
                        break;
                    case System.Windows.Forms.Keys.S:
                        MoveUp((float)(-15 * timeStep));
                        break;
                    case System.Windows.Forms.Keys.A:
                        MoveRight((float)(-15 * timeStep));
                        break;
                    case System.Windows.Forms.Keys.D:
                        MoveRight((float)(15 * timeStep));
                        break;
                }
            }

            MoveForward((float)(15 * timeStep * input.MouseWheelDelta));
            if (OnTick != null)
            {
                OnTick(0d);
            }

            //set the shader
            fogShader.SetupShader(dx.Context);
            world = SlimDX.Matrix.Multiply(world, SlimDX.Matrix.RotationY(0.01f));

            //set the world, view and projection matrices
            fogShader.SetWVPMatrices(dx.Context, world, camera.ViewMatrix, camera.ProjectionMatrix);
            //bumpShader.SetCameraParameters(dx.Context, camera);
            //bumpShader.SetLightParameters(dx.Context, light);
            fogShader.SetFogParameters(dx.Context, 0, 2, new Color3(0.5f, 0.5f, 0.5f));
            //render the meshes
            foreach (Mesh<VertexPosTexNormTanBinorm> mesh in meshes)
            {
                fogShader.SetTextures(dx.Context, mesh.Textures);
                mesh.Draw(dx.Context);
            }
            //Render the 2D assets
            dx.DepthEnabled = false;
            colourTextureShader.SetupShader(dx.Context);
            colourTextureShader.SetTexture(dx.Context, fontEngine.Texture);
            colourTextureShader.SetWVPMatrices(dx.Context, SlimDX.Matrix.Identity, camera.ViewMatrix, ortho);
            //fontEngine.DrawIndexed(dx.Context);
            dx.DepthEnabled = true;

            dx.FinishRender();
        }

        protected virtual void Dispose(bool disposing)
        {
            //Contract.Ensures(bumpShader == null, "lightShader must be disposed by this function.");
            Contract.Ensures(colourTextureShader == null, "colourTextureShader must be disposed by this function.");
            Contract.Ensures(bitmap == null, "bitmap must be disposed by this function.");
            Contract.Ensures(fontEngine == null, "fontEngine must be disposed by this function.");
            Contract.Ensures(dx == null, "dx must be disposed by this function.");

            if (disposing)
            {
                if (fogShader != null)
                {
                    fogShader.Dispose();
                    fogShader = null;
                }
                if (colourTextureShader != null)
                {
                    colourTextureShader.Dispose();
                    colourTextureShader = null;
                }
                if(bitmap != null)
                {
                    bitmap.Dispose();
                    bitmap = null;
                }
                if(fontEngine != null)
                {
                    fontEngine.Dispose();
                    fontEngine = null;
                }
                if (dx != null)
                {
                    dx.Dispose();
                    dx = null;
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
