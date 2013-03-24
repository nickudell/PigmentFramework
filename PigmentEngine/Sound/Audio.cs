using System.Collections.Generic;
using SlimDX.XAudio2;
using System;

namespace Pigment.Engine.Sound
{
    public class Audio : IDisposable
    {
        /// <summary>
        /// The XAudio2 controller
        /// </summary>
        private XAudio2 audio;
        /// <summary>
        /// The master voice
        /// </summary>
        private MasteringVoice master;
        /// <summary>
        /// The audio sources
        /// </summary>
        private List<SourceVoice> sources;

        /// <summary>
        /// Initializes a new instance of the <see cref="Audio"/> class.
        /// </summary>
        public Audio()
        {
            audio = new XAudio2();
            master = new MasteringVoice(audio);
            sources = new List<SourceVoice>();
        }

        /// <summary>
        /// Adds the wave to the source voices and starts playing it.
        /// </summary>
        /// <param name="wave">The wave.</param>
        public void AddSound(Wave wave)
        {
            SourceVoice source = new SourceVoice(audio, wave.Data.Format);
            source.Start();
            source.SubmitSourceBuffer(wave.Buffer);
            sources.Add(source);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool managed)
        {
            if (managed)
            {
                foreach (SourceVoice source in sources)
                {
                    source.Stop();
                    source.Dispose();
                }
                master.Dispose();
                audio.Dispose();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}