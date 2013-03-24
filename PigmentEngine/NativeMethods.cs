using System;
using System.Runtime.InteropServices;

namespace Pigment
{
    public static class NativeMethods
    {
        /// <summary>
        /// Queries the performance frequency.
        /// </summary>
        /// <param name="performanceFrequency">The performance frequency.</param>
        /// <returns></returns>
        //[System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool QueryPerformanceFrequency(ref long performanceFrequency);

        /// <summary>
        /// Queries the performance counter.
        /// </summary>
        /// <param name="performanceCount">The performance count.</param>
        /// <returns></returns>
        //[System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        private static extern bool QueryPerformanceCounter(ref long performanceCount);

        /// <summary>
        /// Gets the desktop window.
        /// </summary>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = false)]
        private static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// Gets the desktop window.
        /// </summary>
        /// <returns></returns>
        public static IntPtr DesktopWindow()
        {
            return GetDesktopWindow();
        }

        /// <summary>
        /// Gets the number of ticks fired per second.
        /// </summary>
        /// <returns>The number of ticks per second.</returns>
       public static long GetPerformanceFrequency()
        {
            long frequency = 0;
            QueryPerformanceFrequency(ref frequency);
            return frequency;
        }

       /// <summary>
       /// Gets the number of ticks that have passed since the start.
       /// </summary>
       /// <returns>The number of ticks since start.</returns>
        public static long GetPerformanceCounter()
       {
           long count = 0;
           QueryPerformanceCounter(ref count);
           return count;
       }
    }
}