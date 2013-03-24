using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Pigment.Engine.Sound
{
    public class Phrase
    {
        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private TimeSpan duration;

        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        private Wave wave;
        
        [XmlIgnore]
        public Wave Wave
        {
            get { return wave; }
            set { wave = value; }
        }

        private Mood mood;

        public Mood Mood
        {
            get { return mood; }
            set { mood = value; }
        }
        
    }
}
