using System;
using System.Runtime.InteropServices;

namespace MediaPlayer
{
    /// <summary>
    /// Volume control wrapper
    /// </summary>
    internal static class VolumeControlWrapper
    {
        /// <summary>
        /// Sets the volume
        /// </summary>
        /// <param name="hwo"></param>
        /// <param name="dwVolume"></param>
        /// <returns></returns>
        [DllImport("winmm.dll", EntryPoint = "waveOutSetVolume")]
        public static extern int WaveOutSetVolume(IntPtr hwo, uint dwVolume);

        /// <summary>
        /// plays a testSound after setting the volume
        /// </summary>
        /// <param name="pszSound"></param>
        /// <param name="hmod"></param>
        /// <param name="fdwSound"></param>
        /// <returns></returns>
        [DllImport("winmm.dll", SetLastError = true)]
        public static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);
    }
}
