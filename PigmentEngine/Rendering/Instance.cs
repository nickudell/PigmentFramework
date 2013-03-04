using SlimDX;

namespace Pigment.Engine.Rendering
{
    /// <summary>
    /// A base class for classes which are instances of meshes
    /// </summary>
    public class Instance : IPositioned
    {
        /// <summary>
        /// The position
        /// </summary>
        public Vector3 Position { get; set; }
    }
}