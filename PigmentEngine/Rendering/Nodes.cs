using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
using System.Windows;

namespace Pigment.Engine.Rendering.UI.Controls
{
    public class ControlNode
    {
        public string Name { get; set; }
        public List<ImageNode> Images = new List<ImageNode>();
    }

    public class ImageNode
    {
        public string Name {get; set;}
        public Color4 colour { get; set; }
        public Rect Rectangle { get; set; }
    }
}
