using System;
using System.Collections.Generic;
using System.Text;

namespace Pigment
{
    /// <summary>Timing and frames per second class</summary> 
    public class Timer
    {
        private long ticksPerSecond;
        private long lastTime;
        private long lastFPSUpdate;
        private long FPSUpdateInterval;
        private uint numFrames;
        private float runningTime;
        private double timeElapsed;
        private float fps;
        private bool timerStopped;

        /// <summary>
        /// Creates a new Timer
        /// </summary>
        public Timer()
        {
            // Find the frequency, or amount of ticks per second 
            ticksPerSecond = NativeMethods.GetPerformanceFrequency();
            timerStopped = true;
            // Update the FPS every half second. 
            FPSUpdateInterval = ticksPerSecond >> 1;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void Start()
        {
            if (!timerStopped)
            {
                return;
            }
            lastTime = NativeMethods.GetPerformanceCounter();
            timerStopped = false;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void Stop()
        {
            if (timerStopped)
            {
                return;
            }
            long stopTime = 0;
            stopTime = NativeMethods.GetPerformanceCounter();
            runningTime += (float)(stopTime - lastTime) / (float)ticksPerSecond;
            timerStopped = true;
        }

        /// <summary>
        /// Updates the timer.
        /// </summary>
        public void Update()
        {
            if (timerStopped)
            {
                return;
            }

            // Get the current time 
            long currentTime = NativeMethods.GetPerformanceCounter();

            // Update time elapsed since last frame 
            timeElapsed = (double)(currentTime - lastTime) / (double)ticksPerSecond;
            runningTime += (float)timeElapsed;

            // Update FPS 
            numFrames++;
            if (currentTime - lastFPSUpdate >= FPSUpdateInterval)
            {
                float currentTimeSec = (float)currentTime / (float)ticksPerSecond;
                float lastTimeSec = (float)lastFPSUpdate / (float)ticksPerSecond;
                fps = (float)numFrames / (currentTimeSec - lastTimeSec);
                lastFPSUpdate = currentTime;
                numFrames = 0;
            }

            lastTime = currentTime;

        }

        /// <summary>
        /// Get the elapsed time in seconds since last update without updating. If the timer is stopped, returns 0.
        /// </summary>
        /// <returns>The time since the last update</returns>
        public double Peek()
        {
            if (timerStopped)
            {
                return 0;
            }

            // Get the current time 
            long currentTime = NativeMethods.GetPerformanceCounter();

            // Get time elapsed since last frame 
            return (double)(currentTime - lastTime) / (double)ticksPerSecond;
        }

        /// <summary>
        /// Is the timer stopped?
        /// </summary>
        /// <value>
        ///   <c>true</c> if stopped; otherwise, <c>false</c>.
        /// </value>
        public bool Stopped
        {
            get { return timerStopped; }
        }

        /// <summary>
        /// Frames per second
        /// </summary>
        /// <value>
        /// The FPS.
        /// </value>
        public float FPS
        {
            get { return fps; }
        }

        /// <summary>
        /// Elapsed time in seconds since last update. If the timer is stopped, returns 0.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        public double ElapsedTime
        {
            get
            {
                if (timerStopped)
                {
                    return 0;
                }
                return timeElapsed;
            }
        }

        /// <summary>
        /// Total running time.
        /// </summary>
        /// <value>
        /// The running time.
        /// </value>
        public float RunningTime
        {
            get
            {
                return runningTime;
            }
        }

    }
}
