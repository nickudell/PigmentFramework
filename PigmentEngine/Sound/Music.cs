using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.IO;
using System.Xml;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Sound
{
    public class Music
    {
        /// <summary>
        /// The music graph
        /// </summary>
        private Graph<Mood> musicGraph;

        /// <summary>
        /// Initializes a new instance of the <see cref="Music"/> class.
        /// </summary>
        /// <param name="musicFileName">Path of the music file.</param>
        public Music(string musicFileName)
        {
            Contract.Requires<FileNotFoundException>(File.Exists(musicFileName));
            Load(musicFileName);
        }

        public Music()
        {
            musicGraph = new Graph<Mood>();
        }

        /// <summary>
        /// Loads the specified music file.
        /// </summary>
        /// <param name="musicFileName">Path of the music file.</param>
        private void Load(string musicFileName)
        {
            List<MoodNode> nodes = new List<MoodNode>();
            XmlSerializer phraseDeserializer = new XmlSerializer(typeof(Phrase));
            XPathDocument doc = new XPathDocument(musicFileName);
            XPathNavigator navi = doc.CreateNavigator();
            foreach (XPathNavigator item in navi.Select("Phrase"))
            {
                Phrase phrase = (Phrase)phraseDeserializer.Deserialize(new StringReader(item.OuterXml));
                nodes.Add(new MoodNode(phrase.Mood, phrase));
            }
            musicGraph = new Graph<Mood>();
            foreach (XPathNavigator item in navi.Select("Edge"))
            {
                //Get from item
                string fromPath = item.SelectSingleNode("From").Value;
                MoodNode fromNode = nodes.Find((e) => { return (e.Phrase.FileName == fromPath); });
                //Get to item
                string toPath = item.SelectSingleNode("To").Value;
                MoodNode toNode = nodes.Find((e) => { return (e.Phrase.FileName == toPath); });
                musicGraph.AddDirectedEdge(fromNode, toNode, (int)fromNode.Phrase.Duration.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Saves the specified music file
        /// </summary>
        /// <param name="musicFileName">Path of the music file.</param>
        private void Save(string musicFileName)
        {
            XmlSerializer phraseSerializer = new XmlSerializer(typeof(Phrase));
            using (XmlWriter writer = XmlWriter.Create(musicFileName))
            {
                writer.WriteStartDocument();

                    writer.WriteStartElement("Phrases");
                        foreach (MoodNode node in musicGraph.Nodes)
                        {
                            phraseSerializer.Serialize(writer,node.Phrase);
                        }
                    writer.WriteEndElement();

                    writer.WriteStartElement("Edges");
                        foreach (MoodNode node in musicGraph.Nodes)
                        {
                            foreach (Edge edge in node.Adjacencies)
                            {
                                writer.WriteStartElement("Edge");
                                writer.WriteElementString("From", node.Phrase.FileName);
                                writer.WriteElementString("To", ((MoodNode)(edge.Destination)).Phrase.FileName);
                                writer.WriteEndElement();
                            }
                        }
                    writer.WriteEndElement();

                writer.WriteEndDocument();
            }
        }
    }
}