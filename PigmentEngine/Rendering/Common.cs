using System;
namespace Common
{
    /// <summary>
    /// Pair of two generic items
    /// </summary>
    /// <typeparam name="G">Item1's type</typeparam>
    /// <typeparam name="H">Item2's type</typeparam>
    public class Pair<G, H>
    {
        /// <summary>
        /// Gets or sets the first item
        /// </summary>
        /// <value>
        /// The first item
        /// </value>
        public G Item1 { get; set; }
        /// <summary>
        /// Gets or sets the second item
        /// </summary>
        /// <value>
        /// The second item
        /// </value>
        public H Item2 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pair{H}" /> class.
        /// </summary>
        /// <param name="item1">The first item.</param>
        /// <param name="item2">The second item.</param>
        public Pair(G item1, H item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Pair<G, H> objPair = obj as Pair<G, H>;
            if (objPair != null)
            {
                if (objPair.Item1.Equals(Item1) && objPair.Item2.Equals(Item2))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "Item1: " + Item1.ToString() + ", Item2: " + Item2.ToString();
        }
    }

    /// <summary>
    /// Two paired integers, typically for coordinates
    /// </summary>
    public class Int2
    {
        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>
        /// The X coordinate.
        /// </value>
        public int X { get; set; }
        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        /// <value>
        /// The Y coordinate.
        /// </value>
        public int Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int2" /> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Int2(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Int2 objInt2 = obj as Int2;
            if (objInt2 != null)
            {
                if (objInt2.X == X && objInt2.Y == Y)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "X: " + X + ", Y: " + Y;
        }
    }

    /// <summary>
    /// Two paired integers, typically for coordinates
    /// </summary>
    public class Int3
    {
        /// <summary>
        /// Gets or sets the X coordinate.
        /// </summary>
        /// <value>
        /// The X coordinate.
        /// </value>
        public int X { get; set; }
        /// <summary>
        /// Gets or sets the Y coordinate.
        /// </summary>
        /// <value>
        /// The Y coordinate.
        /// </value>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the Z coordinate.
        /// </summary>
        /// <value>
        /// The Z coordinate.
        /// </value>
        public int Z { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Int3" /> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public Int3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Int3 objInt3 = obj as Int3;
            if (objInt3 != null)
            {
                if (objInt3.X == X && objInt3.Y == Y && objInt3.Z == Z)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "X: " + X + ", Y: " + Y + ", Z: " + Z;
        }
    }
}
