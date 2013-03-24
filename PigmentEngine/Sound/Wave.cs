using SlimDX.Multimedia;
using SlimDX.XAudio2;
using System;
using System.Diagnostics.Contracts;

namespace Pigment.Engine.Sound
{
    public class Wave : IDisposable
    {
        /// <summary>
        /// Gets the internal wave data.
        /// </summary>
        /// <value>
        /// The internal wave data.
        /// </value>
        public WaveStream Data { get; private set; }

        /// <summary>
        /// Gets the audio buffer.
        /// </summary>
        /// <value>
        /// The audio buffer.
        /// </value>
        public AudioBuffer Buffer { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wave"/> class, loading a wave file and creating an audio buffer from it.
        /// </summary>
        /// <param name="path">The path to the wave file.</param>
        public Wave(string path)
        {
            Contract.Requires<ArgumentException>(System.IO.File.Exists(path),"Parameter path must match an existing file");
            Data = new WaveStream(path);

            Buffer = new AudioBuffer()
            {
                AudioData = Data,
                AudioBytes = (int)Data.Length,
                Flags = BufferFlags.EndOfStream
            };
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="managed"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool managed)
        {
            if (managed)
            {
                Buffer.Dispose();
                Data.Dispose();
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