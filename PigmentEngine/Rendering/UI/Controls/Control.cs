using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pigment.Engine.Rendering.UI.Controls
{
    public abstract class Control
    {
        public enum ControlState
        {
            Normal,
            Over,
            Down,
            Disabled
        }

        public enum TextAlignment
        {
            Left,
            Right,
            Top,
            Bottom,
            Center
        }


    }
}
