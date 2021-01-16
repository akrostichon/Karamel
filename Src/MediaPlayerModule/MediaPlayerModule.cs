using System;
using System.Windows;
using Business;
using Karamel.Infrastructure;
using MediaPlayer.View;
using MediaPlayer.ViewModel;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;

namespace MediaPlayer
{
    /// <summary>
    /// Module class for the Media library
    /// </summary>
    public class MediaPlayerModule : IModule, IMediaPlayer
    {
        #region Attributes

        /// <summary>
        /// unity container of this application
        /// </summary>
        private readonly IUnityContainer _container;

        /// <summary>
        /// stores the last set volume
        /// </summary>
        private uint _volume;
        
        /// <summary>
        /// either represents a cdg player controller or a video player controller
        /// </summary>
        private MediaPlayerFactory _mediaPlayerFactory;

        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="container">unity container</param>
        public MediaPlayerModule(IUnityContainer container)
        {
            _container = container;
            Application.Current.MainWindow.Closing += MainWindow_Closing;
        }
        
        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Initialize the module by registering types of the module in the unity container
        /// </summary>
        public void Initialize()
        {
            // register the audio library as a Singleton
            _container.RegisterType<IMediaPlayer, MediaPlayerModule>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IVideoPlayerWindowView, VideoPlayerWindowView>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IVideoPlayerWindowViewModel, VideoPlayerWindowViewModel>(new ContainerControlledLifetimeManager());
            
            var viewModel = _container.Resolve<VideoPlayerWindowViewModel>();
            var videoPlayerWindow = ((VideoPlayerWindowView) viewModel.View);
            videoPlayerWindow.WindowStartupLocation = WindowStartupLocation.Manual;
            videoPlayerWindow.Show();
        }
        
        #endregion Public Methods

        #region Events

        /// <summary>
        /// When the mainWindow is closing we also want to close this window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Close();
        }
        
        #endregion Events

        #region Implementation of IMediaLibraryControl
        
        /// <summary>
        /// Whether a song is currently being played
        /// </summary>
        /// <returns></returns>
        public bool IsPlaying()
        {
            return _mediaPlayerFactory != null && _mediaPlayerFactory.PlayerController.IsPlaying();
        }
        
        /// <summary>
        /// Stops the playback
        /// </summary>
        public void Stop()
        {
            if (_mediaPlayerFactory != null)
            {
                _mediaPlayerFactory.PlayerController.Stop();
            }
        }

        /// <summary>
        /// Pauses the playback
        /// </summary>
        public void Pause()
        {
            if (_mediaPlayerFactory != null)
            {
                _mediaPlayerFactory.PlayerController.Pause();
            }
        }

        /// <summary>
        /// Starts the playback again after it has been paused
        /// </summary>
        public void Unpause()
        {
            if (_mediaPlayerFactory != null)
            {
                _mediaPlayerFactory.PlayerController.Unpause();
            }
        }

        /// <summary>
        /// starts the playback of the given song
        /// </summary>
        /// <param name="playlistItem"></param>
        public void Play(PlaylistItem playlistItem)
        {
            if (_mediaPlayerFactory == null ||
                _mediaPlayerFactory.PlayerControllerType != _mediaPlayerFactory.GetPlayerControllerTypeForPlaylistItem(playlistItem))
            {
                if (_mediaPlayerFactory != null)
                {
                    _mediaPlayerFactory.PlayerController.Close();
                }
                
                var playerWindow = _container.Resolve<VideoPlayerWindowView>();
                _mediaPlayerFactory = new MediaPlayerFactory(playlistItem, playerWindow);
            }

            _mediaPlayerFactory.PlayerController.Play(playlistItem);
        }

        /// <summary>
        /// Gets the duration of a song in milliseconds
        /// </summary>
        /// <returns>duration of song in milliseconds</returns>
        public int GetDurationOfPlayedSong()
        {
            if (_mediaPlayerFactory != null)
            {
                return _mediaPlayerFactory.PlayerController.GetDurationOfPlayedSong();
            }
            return 0;
        }

        /// <summary>
        /// Gets the position inside the song in milliseconds
        /// </summary>
        /// <returns>position inside song in milliseconds</returns>
        public int GetSongPosition()
        {
            if (_mediaPlayerFactory != null)
            {
                return _mediaPlayerFactory.PlayerController.GetSongPosition();
            }
            return 0;
        }

        /// <summary>
        /// Sets the song position inside the played song
        /// </summary>
        /// <param name="newSongPositionInMs">new position in milliSeconds</param>
        public void SetSongPosition(int newSongPositionInMs)
        {
            if (_mediaPlayerFactory != null)
            {
                _mediaPlayerFactory.PlayerController.SetSongPosition(newSongPositionInMs);
            }
        }
        
        /// <summary>
        /// Gets/sets the volume
        /// </summary>
        public uint Volume
        {
            get { return _volume; }
            set 
            {
                _volume = value;

                // Set the volume
                VolumeControlWrapper.WaveOutSetVolume(IntPtr.Zero, _volume);
                VolumeControlWrapper.PlaySound("tada.wav", IntPtr.Zero, 0x2001);
            }
        }

        /// <summary>
        /// Closes the player and frees its resources
        /// </summary>
        public void Close()
        {
            if (_mediaPlayerFactory != null)
            {
                _mediaPlayerFactory.PlayerController.Close();
            }

            // the cdg player window has to be closed
            var playerWindow = _container.Resolve<VideoPlayerWindowView>();
            playerWindow.Close();
        }

        #endregion Implementation of IMediaLibraryControl
    }
}
