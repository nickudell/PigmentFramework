using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pigment.Engine.Rendering;
using System.Diagnostics.Contracts;
using System.IO;
using Pigment.WPF;

namespace Pigment.Engine
{

    public class TileSet3D<T>
        where T: VertexPos
    {
        private Dictionary<AdjacencyFlags, Mesh<T>> tileSet;

        public void Load(string path)
        {
            Contract.Requires<FileNotFoundException>(File.Exists(path),"path");
            tileSet = new Dictionary<AdjacencyFlags,Mesh<T>>();

            AdjacencyFlags adjacencies = ParseAdjacencyString("flx");

            throw new NotImplementedException();
        }

        private AdjacencyFlags ParseAdjacencyString(string adjacencyString)
        {
            Contract.Requires<ArgumentException>(adjacencyString.Length==3,"adjacencyString has incorrect length");
            AdjacencyFlags result = AdjacencyFlags.None;
            if (adjacencyString[0] == 'f')
            {
                result = result | AdjacencyFlags.Front;
            }
            else if (adjacencyString[0] == 'b')
            {
                result = result | AdjacencyFlags.Back;
            }
            if (adjacencyString[1] == 'l')
            {
                result = result | AdjacencyFlags.Left;
            }
            else if (adjacencyString[1] == 'r')
            {
                result = result | AdjacencyFlags.Right;
            }
            if (adjacencyString[2] == 'u')
            {
                result = result | AdjacencyFlags.Top;
            }
            else if (adjacencyString[2] == 'd')
            {
                result = result | AdjacencyFlags.Bottom;
            }
            return result;
        }

        public Pigment.Engine.Rendering.Mesh<T> GetTile(AdjacencyFlags flags)
        {
            Contract.Requires<ArgumentException>(flags!= AdjacencyFlags.None,"Flags can not be only None.");
            Contract.Requires<ArgumentException>(flags != (AdjacencyFlags.Front | AdjacencyFlags.Back | AdjacencyFlags.Top | AdjacencyFlags.Bottom | AdjacencyFlags.Left | AdjacencyFlags.Right), "All surrounding flags supplied, should be invisible.");
            Contract.Requires<ArgumentException>(flags != (AdjacencyFlags.Front | AdjacencyFlags.Back | AdjacencyFlags.Top | AdjacencyFlags.Bottom | AdjacencyFlags.Left | AdjacencyFlags.Right | AdjacencyFlags.None), "All surrounding flags supplied, should be invisible.");
            throw new NotImplementedException();
        }
    }
}
