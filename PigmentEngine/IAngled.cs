using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace Pigment.Engine
{
    /// <summary>
    /// Enforces a gettable angle of rotation in 3 axes.
    /// </summary>
    public interface IAngled
    {
        /// <summary>
        /// Gets the angle of the object.
        /// </summary>
        /// <value>
        /// The angle.
        /// </value>
        Vector3 Angle { get; }
    }
}
