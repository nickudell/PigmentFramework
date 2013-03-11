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
    /// Node which also handles phrasing for music
    /// </summary>
    public class MoodNode : Node<Mood>
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
        /// Initializes a new instance of the <see cref="MoodNode" /> class.
        /// </summary>
        /// <param name="mood">The mood.</param>
        /// <param name="phrase">The phrase.</param>
        public MoodNode(Mood mood, Phrase phrase) : base()
        {
            Contract.Requires<ArgumentNullException>(mood != null, "mood");
            Contract.Requires<ArgumentNullException>(phrase != null, "phrase");
            this.Content = mood;
            this.phrase = phrase;
            foreach (Mood enumMood in Enum.GetValues(typeof(Mood)).Cast<Mood>())
            {
                NextHopTo.Add(enumMood, null);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoodNode" /> class.
        /// </summary>
        /// <param name="mood">The mood.</param>
        /// <param name="phrase">The phrase.</param>
        /// <param name="adjacencies">The adjacent nodes with costs.</param>
        public MoodNode(Mood mood, Phrase phrase, List<Tuple<Node<Mood>, int>> adjacencies) : base(adjacencies)
        {
            Contract.Requires<ArgumentNullException>(phrase != null, "phrase");
            this.Content = mood;
            this.phrase = phrase;
            foreach (Mood enumMood in Enum.GetValues(typeof(Mood)).Cast<Mood>())
            {
                NextHopTo.Add(enumMood, null);
            }
        }
    }
}
