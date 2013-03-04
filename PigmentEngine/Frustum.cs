using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using Pigment.Engine;

namespace Pigment.WPF.Engine
{
    class Frustum
    {
        /// <summary>
        /// The planes that make up the frustum
        /// </summary>
        private Plane[] planes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Frustum"/> class.
        /// </summary>
        public Frustum()
        {
            planes = new Plane[6];
        }

        /// <summary>
        /// Builds the frustum from a screen depth and a camera object.
        /// </summary>
        /// <param name="screenDepth">The screen depth.</param>
        /// <param name="camera">The camera.</param>
        public void BuildFrustum(float screenDepth, Camera camera)
        {
            BuildFrustum(screenDepth, camera.ViewMatrix, camera.ProjectionMatrix);
        }

        /// <summary>
        /// Builds the frustum from a screen depth, view matrix and projection matrix.
        /// </summary>
        /// <param name="screenDepth">The screen depth.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="projection">The projection matrix.</param>
        public void BuildFrustum(float screenDepth, Matrix view, Matrix projection)
        {
            float zMin, r;
            //Calculate the minimum Z distance in the frustum.
            zMin = -projection.M43 / projection.M33;
            r = screenDepth / (screenDepth - zMin);
            projection.M33 = r;
            projection.M43 = -r * zMin;

            //Create the frustum matrix from the view and updated projection matrices
            Matrix matrix = Matrix.Multiply(view, projection);

            //Calculate frustum near plane
            planes[0].Normal.X = matrix.M14 + matrix.M13;
            planes[0].Normal.Y = matrix.M24 + matrix.M23;
            planes[0].Normal.Z = matrix.M34 + matrix.M33;
            planes[0].D = matrix.M44 + matrix.M43;
            planes[0].Normalize();

            //Calculate the frustum far plane
            planes[1].Normal.X = matrix.M14 - matrix.M13;
            planes[1].Normal.Y = matrix.M24 - matrix.M23;
            planes[1].Normal.Z = matrix.M34 - matrix.M33;
            planes[1].D = matrix.M44 - matrix.M43;
            planes[1].Normalize();

            //Calculate the frustum left plane
            planes[2].Normal.X = matrix.M14 + matrix.M11;
            planes[2].Normal.Y = matrix.M24 + matrix.M21;
            planes[2].Normal.Z = matrix.M34 + matrix.M31;
            planes[2].D = matrix.M44 + matrix.M41;
            planes[2].Normalize();

            //Calculate the frustum right plane
            planes[3].Normal.X = matrix.M14 - matrix.M11;
            planes[3].Normal.Y = matrix.M24 - matrix.M21;
            planes[3].Normal.Z = matrix.M34 - matrix.M31;
            planes[3].D = matrix.M44 - matrix.M41;
            planes[3].Normalize();

            //Calculate the frustum top plane
            planes[4].Normal.X = matrix.M14 - matrix.M12;
            planes[4].Normal.Y = matrix.M24 - matrix.M22;
            planes[4].Normal.Z = matrix.M34 - matrix.M32;
            planes[4].D = matrix.M44 - matrix.M42;
            planes[4].Normalize();

            //Calculate the frustum bottom plane
            planes[5].Normal.X = matrix.M14 + matrix.M12;
            planes[5].Normal.Y = matrix.M24 + matrix.M22;
            planes[5].Normal.Z = matrix.M34 + matrix.M32;
            planes[5].D = matrix.M44 + matrix.M42;
            planes[5].Normalize();
        }

        /// <summary>
        /// Checks if a point is inside the frustum.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>True if the point is inside the frustum, else false.</returns>
        public bool CheckPoint(Vector3 point)
        {
            foreach (Plane plane in planes)
            {
                if(Plane.DotCoordinate(plane,point) < 0f)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if a cube is inside the frustum
        /// </summary>
        /// <param name="center">The center of the cube.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>True if any corner of the cube is inside the frustum, else false.</returns>
        public bool CheckCube(Vector3 center, int radius)
        {
            foreach (Plane plane in planes)
            {
                if (Plane.DotCoordinate(plane, new Vector3(center.X - radius, center.Y - radius, center.Z - radius)) >=0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X + radius, center.Y - radius, center.Z - radius)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X - radius, center.Y + radius, center.Z - radius)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X + radius, center.Y + radius, center.Z - radius)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X - radius, center.Y - radius, center.Z + radius)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X - radius, center.Y + radius, center.Z + radius)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X + radius, center.Y - radius, center.Z + radius)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X + radius, center.Y + radius, center.Z + radius)) >= 0f)
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if a sphere is inside the frustum.
        /// </summary>
        /// <param name="center">The center of the sphere.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>True if the point is within one radius' length of the frustum.</returns>
        public bool CheckSphere(Vector3 center, float radius)
        {
            foreach (Plane plane in planes)
            {
                if (Plane.DotCoordinate(plane, center) < -radius)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Checks if a box is inside the frustum
        /// </summary>
        /// <param name="center">The center of the box.</param>
        /// <param name="size">The size of the box.</param>
        /// <returns>True if any corner of the box is inside the frustum, else false.</returns>
        public bool CheckBox(Vector3 center, Vector3 size)
        {
            foreach (Plane plane in planes)
            {
                if (Plane.DotCoordinate(plane, new Vector3(center.X - size.X, center.Y - size.Y, center.Z - size.Z)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X + size.X, center.Y - size.Y, center.Z - size.Z)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X - size.X, center.Y + size.Y, center.Z - size.Z)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X + size.X, center.Y + size.Y, center.Z - size.Z)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X - size.X, center.Y - size.Y, center.Z + size.Z)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X - size.X, center.Y + size.Y, center.Z + size.Z)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X + size.X, center.Y - size.Y, center.Z + size.Z)) >= 0f)
                {
                    continue;
                }
                if (Plane.DotCoordinate(plane, new Vector3(center.X + size.X, center.Y + size.Y, center.Z + size.Z)) >= 0f)
                {
                    continue;
                }
                return false;
            }
            return true;
        }
    }
}
