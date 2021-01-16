using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;
using Business;
using Karamel.Infrastructure;
using MediaPlayer.View;

namespace MediaPlayer
{
    /// <summary>
    /// Controller class for the cdg player
    /// </summary>
    internal class CdgPlayerController : IPlayerController, IDisposable
    {
        #region Attributes

        /// <summary>
        /// handle of the cdg library
        /// </summary>
        private static IntPtr _cdgLibDllHandle = IntPtr.Zero;

        /// <summary>
        /// With of the cdg window
        /// </summary>
        private double _cdgWindowWidth;

        /// <summary>
        /// Height of the cdg window
        /// </summary>
        private double _cdgWindowHeight;

        /// <summary>
        /// the video player window in which the cdg player should be shown
        /// </summary>
        private readonly VideoPlayerWindowView _videoPlayerWindow;
        /// <summary>
        /// timer for setting the cdg position
        /// </summary>
        private readonly DispatcherTimer _setCdgPositionTimer;
        /// <summary>
        /// required for playing zipped files
        /// </summary>
        private ZipFileHelper _zipFileHelper = null;

        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor, binds size of cdgPlayerWindow to the size of the video player window
        /// </summary>
        /// <param name="videoPlayerWindow"></param>
        public CdgPlayerController(VideoPlayerWindowView videoPlayerWindow)
        {
            _videoPlayerWindow = videoPlayerWindow;
            videoPlayerWindow.SizeChanged += CDGPlayerWindowOnSizeChanged;
            _cdgWindowHeight = videoPlayerWindow.Height;
            _cdgWindowWidth = videoPlayerWindow.Width;
            _setCdgPositionTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
            _setCdgPositionTimer.Tick += SetCdgPositionTimer_Tick;
        }
        
        #endregion Constructor

        #region Events

        /// <summary>
        /// timer event to synchronize cdg playback position with mp3 position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetCdgPositionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (IsCdgLibraryInitialized())
                {
                    double audioPlayerPosition = _videoPlayerWindow.VideoPlayer.Position.TotalMilliseconds;
                    CdgLibraryWrapper.CDGPlayerPosSet(_cdgLibDllHandle, (int)audioPlayerPosition);
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion Events

        #region Dispose

        /// <summary>
        /// deregister events and close the player on dispose
        /// </summary>
        public void Dispose()
        {
            if (_videoPlayerWindow != null)
            {
                _videoPlayerWindow.SizeChanged -= CDGPlayerWindowOnSizeChanged;
            }
            Close();
        }

        #endregion Dispose

        #region Events

        /// <summary>
        /// When the cdg player window is resized and the 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="sizeChangedEventArgs"></param>
        private void CDGPlayerWindowOnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            _cdgWindowHeight = ((Window)sender).Height;
            _cdgWindowWidth = ((Window)sender).Width;

            if (IsCdgLibraryInitialized())
            {
                if (sizeChangedEventArgs.HeightChanged)
                {
                    CdgLibraryWrapper.CDGPlayerHeightSet(_cdgLibDllHandle, (int)_cdgWindowHeight);
                }
                else
                {
                    CdgLibraryWrapper.CDGPlayerWidthSet(_cdgLibDllHandle, (int)_cdgWindowWidth);
                }
            }
        }

        #endregion Events

        /// <summary>
        /// Whether a song is currently being played
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return IsCdgLibraryInitialized();
        }

        /// <summary>
        /// Stops the playback
        /// </summary>
        public void Stop()
        {
            if (IsCdgLibraryInitialized())
            {
                _setCdgPositionTimer.Stop();
                CdgLibraryWrapper.CDGPlayerHide(_cdgLibDllHandle);
                CdgLibraryWrapper.CDGPlayerClose(_cdgLibDllHandle);
                _cdgLibDllHandle = IntPtr.Zero;
                _videoPlayerWindow.VideoPlayer.Stop();
                if (_zipFileHelper != null)
                {
                    _zipFileHelper.Dispose();
                    _zipFileHelper = null;
                }
            }
        }

        /// <summary>
        /// Pauses the playback
        /// </summary>
        public void Pause()
        {
            if (IsCdgLibraryInitialized())
            {
                CdgLibraryWrapper.CDGPlayerPause(_cdgLibDllHandle);
                _videoPlayerWindow.VideoPlayer.Pause();
            }
        }

        /// <summary>
        /// Starts the playback again after it has been paused
        /// </summary>
        public void Unpause()
        {
            if (IsCdgLibraryInitialized())
            {
                CdgLibraryWrapper.CDGPlayerPlay(_cdgLibDllHandle);
                _videoPlayerWindow.VideoPlayer.Play();
            }
        }

        /// <summary>
        /// starts the playback of the given song
        /// </summary>
        /// <param name="song"></param>
        public void Play(PlaylistItem song)
        {
            if (IsCdgLibraryInitialized() == false)
            {
                try
                {
                    InitializeCdgLibrary(song);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("The song " + song.Song.Artist + ":" + song.Song.Title + " could not be played.", "Error");
                    return;
                }
                
            }

            _videoPlayerWindow.VideoPlayer.LoadedBehavior = MediaState.Manual;
            _videoPlayerWindow.VideoPlayer.UnloadedBehavior = MediaState.Manual;
            _videoPlayerWindow.VideoPlayer.Play();
            CdgLibraryWrapper.CDGPlayerShow(_cdgLibDllHandle);
            CdgLibraryWrapper.CDGPlayerPlay(_cdgLibDllHandle);
            _setCdgPositionTimer.Start();
        }

        /// <summary>
        /// Gets the duration of a song in milliseconds
        /// </summary>
        /// <returns>duration of song in milliseconds</returns>
        public int GetDurationOfPlayedSong()
        {
            if (_videoPlayerWindow.VideoPlayer.NaturalDuration.HasTimeSpan)
            {
                return (int)_videoPlayerWindow.VideoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
            }
            return 0;
        }

        /// <summary>
        /// Gets the position inside the song in milliseconds
        /// </summary>
        /// <returns>position inside song in milliseconds</returns>
        public int GetSongPosition()
        {
            return (int)_videoPlayerWindow.VideoPlayer.Position.TotalMilliseconds;
        }
        
        /// <summary>
        /// Sets the song position inside the played song
        /// </summary>
        /// <param name="newSongPositionInMs">new position in milliSeconds</param>
        public void SetSongPosition(int newSongPositionInMs)
        {
            if (IsPlaying())
            {
                _videoPlayerWindow.VideoPlayer.Position = TimeSpan.FromMilliseconds(newSongPositionInMs);
                int songPosInMs = CdgLibraryWrapper.CDGPlayerPosGet(_cdgLibDllHandle);
                int deltaSecs = (newSongPositionInMs - songPosInMs) / 1000;
                CdgLibraryWrapper.CDGPlayerSeek(_cdgLibDllHandle, deltaSecs);
            }
        }

        /// <summary>
        /// Closes the player and frees its resources
        /// </summary>
        public void Close()
        {
            if (IsCdgLibraryInitialized())
            {
                _setCdgPositionTimer.Stop();
                CdgLibraryWrapper.CDGPlayerHide(_cdgLibDllHandle);
                CdgLibraryWrapper.CDGPlayerClose(_cdgLibDllHandle);
                _videoPlayerWindow.VideoPlayer.Stop();
                _videoPlayerWindow.VideoPlayer.Close();
                _cdgLibDllHandle = IntPtr.Zero;
                if (_zipFileHelper != null)
                {
                    _zipFileHelper.Dispose();
                    _zipFileHelper = null;
                }
            }
        }

        #region Helper

        /// <summary>
        /// Checks whether we have a handle of the cdg library
        /// </summary>
        /// <returns></returns>
        private bool IsCdgLibraryInitialized()
        {
            return _cdgLibDllHandle != IntPtr.Zero;
        }

        /// <summary>
        /// Initializes the cdg library
        /// </summary>
        /// <param name="playlistItem"></param>
        private void InitializeCdgLibrary(PlaylistItem playlistItem)
        {
            IntPtr applicationHandle = new WindowInteropHelper(Application.Current.MainWindow).Handle;
            IntPtr parentWindowHandle = new WindowInteropHelper(_videoPlayerWindow).Handle;

            string mp3FilePath = playlistItem.Song.FilePath;
            string cdgFilePath = playlistItem.Song.CdgFilePath;
            if (playlistItem.Song.Extension.Equals("zip"))
            {
                _zipFileHelper = new ZipFileHelper();
                Song tmpUnzippedSong = _zipFileHelper.ExtractFileTemporarily(mp3FilePath);
                mp3FilePath = tmpUnzippedSong.FilePath;
                cdgFilePath = tmpUnzippedSong.CdgFilePath;
            }

            _cdgLibDllHandle = CdgLibraryWrapper.CDGPlayerOpenExt(applicationHandle,
                                                                  cdgFilePath,
                                                                  parentWindowHandle,
                                                                  0);

            _videoPlayerWindow.VideoPlayer.Source = new Uri(mp3FilePath);
            
            _cdgWindowHeight = _videoPlayerWindow.Height;
            _cdgWindowWidth = _videoPlayerWindow.Width;
            CdgLibraryWrapper.CDGPlayerHeightSet(_cdgLibDllHandle, (int)_cdgWindowHeight);
            CdgLibraryWrapper.CDGPlayerWidthSet(_cdgLibDllHandle, (int)_cdgWindowWidth);
        }
        
        #endregion Helper
    }
}
