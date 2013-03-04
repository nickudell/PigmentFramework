using Pigment.WPF;
using SlimDX.Direct3D11;
using System.Collections.Generic;
namespace Pigment.Engine.Rendering
{

    /// <summary>
    /// An instanced mesh
    /// </summary>
    /// <typeparam name="V">The vertex type of this mesh</typeparam>
    /// <typeparam name="I">The instance type of this mesh</typeparam>
    public class InstancedMesh<V, I> : Mesh<V>
        where V : VertexPos
        where I : Instance
    {
        /// <summary>
        /// Gets or sets the instances.
        /// </summary>
        /// <value>
        /// The instances.
        /// </value>
        public I[] Instances { get; set; }

        /// <summary>
        /// Gets the instance stride.
        /// </summary>
        /// <value>
        /// The instance stride.
        /// </value>
        public int InstanceStride { get; private set; }

        /// <summary>
        /// Gets the instance buffer.
        /// </summary>
        /// <value>
        /// The instance buffer.
        /// </value>
        public Buffer InstanceBuffer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstancedMesh{I}" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="vertices">The vertices.</param>
        /// <param name="vertexTopology">The vertex topology.</param>
        /// <param name="instances">The instances.</param>
        public InstancedMesh(Device device, List<V> vertices, PrimitiveTopology vertexTopology, string[] textureFileNames, I[] instances)
            : base(device, vertices, vertexTopology, textureFileNames)
        {
            Instances = instances;
        }
    }
}