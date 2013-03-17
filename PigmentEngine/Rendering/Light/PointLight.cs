using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace Pigment.Engine.Rendering.Light
{
    /// <summary>
    /// A light which emanates in a sphere from its position
    /// </summary>
    public class PointLight : Light
    {
        /// <summary>
        /// Gets or sets the radius.
        /// </summary>
        /// <value>
        /// The radius.
        /// </value>
        public float Radius { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLight"/> class.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="colour">The colour.</param>
        /// <param name="specularColour">The specular colour.</param>
        /// <param name="specularPower">The specular power.</param>
        public PointLight(Vector3 position, float radius, Color4 colour, Color4 specularColour, float specularPower)
            : base(colour, specularColour, specularPower)
        {
            this.Radius = radius;
            this.Position = position;
        }
    }
}
