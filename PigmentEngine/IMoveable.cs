using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;

namespace Pigment
{
    public interface IMoveable
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        Vector3 Position { set; }
    }
}
