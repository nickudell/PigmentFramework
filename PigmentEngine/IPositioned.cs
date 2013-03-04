using SlimDX;

namespace Pigment
{

    /// <summary>
    /// Interface which enforces a publically gettable Vector3 Position
    /// </summary>
    public interface IPositioned
    {
        /// <summary>
        /// Gets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        Vector3 Position { get;}
    }
}