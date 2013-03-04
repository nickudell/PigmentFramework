using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace Pigment.Engine.Rendering
{
    public class PointLight : Light
    {
        public float Radius { get; set; }

        public Vector3 Position { get; set; }

        public PointLight(Vector3 position, float radius, Color4 colour, Color4 specularColour, float specularPower)
            : base(colour, specularColour, specularPower)
        {
            this.Radius = radius;
            this.Position = position;
        }
    }
}
