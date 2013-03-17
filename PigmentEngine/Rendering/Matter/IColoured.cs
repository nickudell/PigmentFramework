using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace Pigment.Engine.Rendering.Matter
{
    public interface IColoured
    {
        Color4 Colour { get; set; }
    }
}
