using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace Pigment.Engine.Rendering.Matter
{
    /// <summary>
    /// Enforces a settable angle of rotation in three axes
    /// </summary>
    public interface IRotateable
    {
        /// <summary>
        /// Sets the angle of the object.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        Vector3 Angle { set; }
    }
}
