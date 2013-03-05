using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace PigmentEngine.Sound
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
    /// 
    /// </summary>
    public class MoodNode
    {
        /// <summary>
        /// The mood of this phrase
        /// </summary>
        private Mood mood;

        /// <summary>
        /// Gets the mood of this phrase
        /// </summary>
        /// <value>
        /// The mood.
        /// </value>
        public Mood Mood
        {
            get { return mood; }
        }

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
        /// The next hop to take get to a different mood
        /// </summary>
        private Dictionary<Mood, MoodNode> nextHopTo;

        /// <summary>
        /// Gets or sets the next hop to.
        /// </summary>
        /// <value>
        /// The next hop to.
        /// </value>
        public Dictionary<Mood, MoodNode> NextHopTo
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

        /// <summary>
        /// The adjacencies
        /// </summary>
        private List<Tuple<MoodNode,int>> adjacenciesTo;

        /// <summary>
        /// Gets or sets the adjacencies.
        /// </summary>
        /// <value>
        /// The adjacencies.
        /// </value>
        public List<Tuple<MoodNode,int>> AdjacenciesTo
        {
            get { return adjacenciesTo; }
            set { adjacenciesTo = value; }
        }

        private List<MoodNode> adjacenciesFrom;

        public List<MoodNode> AdjacenciesFrom
        {
            get { return adjacenciesFrom; }
            set { adjacenciesFrom = value; }
        }
        

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodNode" /> class.
        /// </summary>
        /// <param name="mood">The mood.</param>
        /// <param name="phrase">The phrase.</param>
        public MoodNode(Mood mood, Phrase phrase)
        {
            Contract.Ensures(adjacenciesTo != null, "adjacencies must not be null after this method executes.");
            Contract.Ensures(nextHopTo != null, "nextHopTo must not be null after this method executes.");
            this.mood = mood;
            this.phrase = phrase;
            adjacenciesTo = new List<Tuple<MoodNode, int>>();
            nextHopTo = new Dictionary<Mood, MoodNode>();
            foreach (Mood enumMood in  Enum.GetValues(typeof(Mood)).Cast<Mood>())
            {
                nextHopTo.Add(enumMood, null);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodNode" /> class.
        /// </summary>
        /// <param name="mood">The mood.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="adjacencies">The adjacent nodes with costs.</param>
        public MoodNode(Mood mood, Phrase phrase, List<Tuple<MoodNode, int>> adjacencies) : this(mood, phrase)
        {
            Contract.Requires<ArgumentNullException>(adjacencies != null, "adjacencies");
            this.adjacenciesTo = adjacencies;
        }
    }
}
