using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Sound
{
    /// <summary>
    /// Musical moods
    /// </summary>
    public enum Mood
    {
        /// <summary>
        /// Calm, usually quiet mood. Used for exploring safe areas.
        /// </summary>
        Placid,
        /// <summary>
        /// Tense, mostly quiet mood. For when there's something out there and it's dangerous.
        /// </summary>
        Tense,
        /// <summary>
        /// Action music, for when battle erupts
        /// </summary>
        Action,
        /// <summary>
        /// Sad music, for when there has been a loss (e.g. game over)
        /// </summary>
        Loss
    }

    /// <summary>
    /// Node which also handles phrasing for music
    /// </summary>
    public class MoodNode : IGraphNode<Mood>
    {
        /// <summary>
        /// The musical phrase
        /// </summary>
        private Phrase phrase;
        /// <summary>
        /// Gets the musical phrase.
        /// </summary>
        /// <value>
        /// The phrase.
        /// </value>
        public Phrase Phrase
        {
            get { return phrase; }
        }

        /// <summary>
        /// The mood
        /// </summary>
        private Mood mood;

        /// <summary>
        /// Gets or sets the mood.
        /// </summary>
        /// <value>
        /// The mood.
        /// </value>
        public Mood Mood
        {
            get { return mood; }
            set { mood = value; }
        }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodNode" /> class.
        /// </summary>
        /// <param name="mood">The mood.</param>
        /// <param name="phrase">The phrase.</param>
        public MoodNode(Mood mood, Phrase phrase) : this(mood,phrase,new List<Edge>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodNode" /> class.
        /// </summary>
        /// <param name="mood">The mood.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="adjacencies">The adjacent nodes with costs.</param>
        public MoodNode(Mood mood, Phrase phrase, List<Edge> adjacencies)
        {
            Contract.Requires<ArgumentNullException>(phrase != null, "phrase");
            this.mood = mood;
            this.phrase = phrase;
            this.adjacencies = adjacencies;
            foreach (Mood enumMood in Enum.GetValues(typeof(Mood)).Cast<Mood>())
            {
                NextHopTo.Add(enumMood, null);
            }
        }

        /// <summary>
        /// The adjacencies
        /// </summary>
        private List<Edge> adjacencies;

        /// <summary>
        /// Gets or sets the outward-facing edges of this node
        /// </summary>
        /// <value>
        /// The adjacencies.
        /// </value>
        public List<Edge> Adjacencies
        {
            get
            {
                return adjacencies;
            }
            set
            {
                adjacencies = value;
            }
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        public bool Equals(IGraphNode other)
        {
            MoodNode moodOther = other as MoodNode;
            if (moodOther != null)
            {
                return moodOther.Phrase.Equals(Phrase);
            }
            return false;
        }

        /// <summary>
        /// The next hop to a particular mood.
        /// </summary>
        private Dictionary<Mood, Edge> nextHopTo;

        /// <summary>
        /// Gets or sets the next hop to a node with the matching mood.
        /// </summary>
        /// <value>
        /// The next hop to a particular mood.
        /// </value>
        public Dictionary<Mood, Edge> NextHopTo
        {
            get
            {
                return nextHopTo;
            }
            set
            {
                nextHopTo = value;
            }
        }

        public bool Equals(Mood other)
        {
            return mood.Equals(other);
        }
    }
}
