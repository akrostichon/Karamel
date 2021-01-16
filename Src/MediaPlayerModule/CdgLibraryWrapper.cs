using System;
using System.Runtime.InteropServices;

namespace MediaPlayer
{
    /// <summary>
    /// Wrapper for the unmanaged cdg library
    /// </summary>
    internal static class CdgLibraryWrapper
    {
        #region General

        /// <summary>
        /// Opens the connection to the cdg player and sets the song to be played
        /// </summary>
        /// <param name="appHandle">handle of this application</param>
        /// <param name="cdgFileName">cdg file path</param>
        /// <param name="audioFileName">mp3 file path</param>
        /// <param name="parentWindowHandle">
        /// handle of the window in which the cdg player should be shown.
        /// Set to zero if the player should be shown in its own window
        /// </param>
        /// <returns></returns>
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern IntPtr CDGPlayerOpen(IntPtr appHandle, string cdgFileName, string audioFileName, IntPtr parentWindowHandle);

        [DllImport(@"KCdgPlayer.dll")]
        internal static extern IntPtr CDGPlayerOpenExt(IntPtr appHandle, string cdgFileName, IntPtr parentWindowHandle, int internalAudioTimer);

        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerClose(IntPtr dllHandle);

        /// <summary>
        /// Player always has to be shown before it can play a song and after it has been opened
        /// </summary>
        /// <param name="dllHandle"></param>
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerShow(IntPtr dllHandle);

        /// <summary>
        /// player always has to be hidden before it can be closed
        /// </summary>
        /// <param name="dllHandle"></param>
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerHide(IntPtr dllHandle);

        /// <summary>
        /// Checks whether the player is visible
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <returns></returns>
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern int CDGPlayerVisible(IntPtr dllHandle);

        // Calls for applications using the DLL's internal audio player
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerPlay(IntPtr dllHandle);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerPause(IntPtr dllHandle);
        /// <summary>
        /// seeks to a specific point in the song (cdg and audio)
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <param name="deltaSecs">number of seconds that have to be skipped (+- direction)</param>
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerSeek(IntPtr dllHandle, int deltaSecs);
        /// <summary>
        /// Gets the duration of a song in milliseconds
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <returns>duration of song in milliseconds</returns>
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern int CDGPlayerLengthGet(IntPtr dllHandle);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern int CDGPlayerPosGet(IntPtr dllHandle);
        /// <summary>
        /// Sets the position of the CDG Player but only if the audio is played by an external player!
        /// </summary>
        /// <param name="dllHandle"></param>
        /// <param name="millisecs">song position in milliseconds</param>
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerPosSet(IntPtr dllHandle, int millisecs);

        #endregion General

        #region Window Configuration APIs

        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerDefaultColourSet(IntPtr dllHandle, int rgbColour);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern int CDGPlayerTopGet(IntPtr dllHandle);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern int CDGPlayerLeftGet(IntPtr dllHandle);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerTopSet(IntPtr dllHandle, int posTop);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerLeftSet(IntPtr dllHandle, int posLeft);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern int CDGPlayerWidthGet(IntPtr dllHandle);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern int CDGPlayerHeightGet(IntPtr dllHandle);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerWidthSet(IntPtr dllHandle, int width);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerHeightSet(IntPtr dllHandle, int height);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerCaptionSet(IntPtr dllHandle, string caption);

        #endregion Window Configuration APIs

        #region Transparent mode configuration APIs

        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerTransparencyBackgroundSet(IntPtr dllHandle, string fileName);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerTransparencyEnableSet(IntPtr dllHandle, int enable);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerTransparencyScrollEnableSet(IntPtr dllHandle, int enable);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerTransparencyBorderEnableSet(IntPtr dllHandle, int enable);

        #endregion Transparent mode configuration APIs

        // Resample filter mode APIs
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerResampleFilterSet(IntPtr dllHandle, int filterType);

        #region Refresh rate configuration APIs

        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerMaxScreenUpdatesPerSecSet(IntPtr dllHandle, int updatesPerSec);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerMaxForcedUpdatesPerSecSet(IntPtr dllHandle, int updatesPerSec);
        [DllImport(@"KCdgPlayer.dll")]
        internal static extern void CDGPlayerScrollFixedUpdatesPerSecSet(IntPtr dllHandle, int updatesPerSec);

        #endregion Refresh rate configuration APIs
    }
}
