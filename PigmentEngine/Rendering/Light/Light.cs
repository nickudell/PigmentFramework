using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using Pigment.WPF;
using Pigment.Engine.Rendering.Matter;
using Pigment.Engine.Rendering.Matter.Vertices;

namespace Pigment.Engine.Rendering.Light
{
    public abstract class Light
    {
        public Color4 Colour { get; set; }

        public Color4 SpecularColour { get; set; }

        public float SpecularPower { get; set; }

        public Mesh<VertexPos> BoundingShape { get; protected set; }

        public Light(Color4 colour, Color4 specularColour, float specularPower)
        {
            Colour = colour;
            SpecularPower = specularPower;
            SpecularColour = specularColour;
        }
    }
}
