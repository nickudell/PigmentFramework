using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using SlimDX.Direct3D11;
using Pigment.Engine.Rendering.Matter;

namespace Pigment.Engine
{
    public class Camera :IPositioned, IMoveable, IAngled, IRotateable
    {
        /// <summary>
        /// The position
        /// </summary>
        private Vector3 position;

        /// <summary>
        /// Whether the position / angle of the camera has changed
        /// </summary>
        private bool changed;
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                changed = true;
                position = value;
            }
        }

        /// <summary>
        /// The angle
        /// </summary>
        private Vector3 angle;
        /// <summary>
        /// Gets or sets the angle of the object.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        public Vector3 Angle
        {
            get
            {
                return angle;
            }
            set
            {
                changed = true;
                angle = value;
            }
        }

        /// <summary>
        /// The view matrix
        /// </summary>
        private Matrix viewMatrix;
        /// <summary>
        /// Gets the view matrix.
        /// </summary>
        /// <value>
        /// The view matrix.
        /// </value>
        public Matrix ViewMatrix
        {
            get 
            {
                if (changed)
                {
                    viewMatrix = CalculateViewMatrix();
                }
                return viewMatrix;
            }
        }

        /// <summary>
        /// Gets the projection matrix.
        /// </summary>
        /// <value>
        /// The projection matrix.
        /// </value>
        public Matrix ProjectionMatrix { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public Camera(Device device, int windowWidth, int windowHeight)
        {
            ProjectionMatrix = Matrix.PerspectiveFovLH(30, (float)windowWidth / (float)windowHeight, 1, 10);
            viewMatrix = Matrix.LookAtLH(new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
        }

        /// <summary>
        /// Calculates the view matrix.
        /// </summary>
        /// <returns></returns>
        private Matrix CalculateViewMatrix()
        {
            Vector3 up = new Vector3(0, 1, 0);
            Vector3 lookAt = new Vector3(0, 0, 1);
            Vector3 rot = angle * 0.0174532925f;
            
            Matrix rotationMatrix = Matrix.RotationYawPitchRoll(rot.X, rot.Y, rot.Z);
            
            lookAt = Vector3.TransformCoordinate(lookAt, rotationMatrix);
            up = Vector3.TransformCoordinate(up, rotationMatrix);

            lookAt = position + lookAt;
            //lookAt = new Vector3(0, 0, 0);
            return Matrix.LookAtLH(position, lookAt, up);
        }
    }
}
