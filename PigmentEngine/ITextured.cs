using SlimDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pigment.WPF
{
    public interface ITextured
    {
        Vector2 TexCoords { get; set; }
    }
}
