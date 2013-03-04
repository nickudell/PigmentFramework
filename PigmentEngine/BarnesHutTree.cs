using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SlimDX;
namespace Pigment.Engine.Octree
{
    public class BarnesHutTree<I> : Pigment.Engine.Octree.OcTree<I>, IMassed
        where I: IPositioned, IMassed
    {
        public double Mass { get; private set; }

        public Vector3 centerOfMass { get; private set; }

        private BarnesHutTree<I> parent;

        public override void Add(I item)
        {
            base.Add(item);
            UpdateCenterOfMass();
        }

        public void UpdateCenterOfMass()
        {
            double sumMass = 0;
            Vector3 massByPos = new Vector3(0,0,0);
            foreach (BarnesHutTree<I> node in this.children.Values)
            {
                sumMass += node.Mass;
                massByPos += node.Position * (float)node.Mass;
            }
            centerOfMass = massByPos / (float)sumMass;
            if (parent != null)
            {
                parent.UpdateCenterOfMass();
            }
        }

        public override void CreateChildren()
        {
            base.CreateChildren();
        }

        public BarnesHutTree(Vector3 position, Vector3 size, int maxContents) : base(position, size, maxContents)
        {
            this.parent = null;
        }

        public BarnesHutTree(Vector3 position, Vector3 size, int maxContents, BarnesHutTree<I> parent)
            : this(position,size,maxContents)
        {
            this.parent = parent;
        }
    }
}
