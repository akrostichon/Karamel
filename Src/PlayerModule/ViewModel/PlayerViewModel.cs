using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;
using Business;
using Karamel.Infrastructure;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using PlayerControl.View;

namespace PlayerControl.ViewModel
{
    /// <summary>
    /// View Model for the player (control)
    /// </summary>
    public class PlayerViewModel : NotificationObject, IPlayerViewModel, IDisposable, IPlayerService
    {
        #region Attributes

        /// <summary>
        /// volume as a double value
        /// </summary>
        private double _volume = 10;
        
        /// <summary>
        /// whether a random song is taken as the next song if the playlist is empty
        /// or the next song in the media library is taken
        /// </summary>
        private bool _sequentialPlayback;

        /// <summary>
        /// should the playback be stopped after one song has been played
        /// </summary>
        private bool _stopPlaybackAfterSong;
        
        /// <summary>
        /// should the singers and other special functionality be shown
        /// </summary>
        private bool _expertMode;

        /// <summary>
        /// Whether the remaining or the played time is shown
        /// </summary>
        private bool _showRemainingTime;

        /// <summary>
        /// a pair of datetime (when a song position has been queried and the song position at this moment)
        /// This is not updated everytime.
        /// </summary>
        private Tuple<DateTime, int> _lastSongPositionTimePair;

        /// <summary>
        /// whether the playback is paused
        /// </summary>
        private bool _playbackPaused;

        /// <summary>
        /// Connection to the media player
        /// </summary>
        private readonly IMediaPlayer _mediaPlayer;
        
        /// <summary>
        /// Unity container
        /// </summary>
        private readonly IUnityContainer _unityContainer;

        /// <summary>
        /// service for the playlist
        /// </summary>
        private IPlaylistService _playlistService;

        /// <summary>
        /// timer for view elements that have to be updated
        /// </summary>
        private readonly DispatcherTimer _songProgressTimer;
        
        /// <summary>
        /// stores the time when the user last jumped inside the song position
        /// </summary>
        private DateTime _lastSetPositionCall;

        /// <summary>
        /// Song that is currently being played
        /// </summary>
        private PlaylistItem _currentSong;
        
        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor that links the View to the ViewModel
        /// </summary>
        /// <param name="playerView">view of the player (control)</param>
        /// <param name="unityContainer">unity container</param>
        public PlayerViewModel(IPlayerView playerView, IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
            View = playerView;

            _mediaPlayer = _unityContainer.Resolve<IMediaPlayer>();

            PlayCommand = new DelegateCommand(Play);
            PauseCommand = new DelegateCommand(Pause, CanPause);
            StopCommand = new DelegateCommand(Stop);
            NextSongCommand = new DelegateCommand(() => Next());
            PreviousSongCommand = new DelegateCommand(Previous, CanPlayPreviousSong);
            ShowAboutBoxCommand = new DelegateCommand(ShowAboutBox);
            ShowHelpCommand = new DelegateCommand(ShowHelp);
            SequentialPlayback = SolutionWideSettings.Instance.SequentialPlayback;
            StopPlaybackAfterSong = SolutionWideSettings.Instance.StopPlaybackAfterSong;
            ExpertMode = SolutionWideSettings.Instance.ExpertMode;
            Volume = SolutionWideSettings.Instance.Volume;
            ShowRemainingTime = SolutionWideSettings.Instance.ShowRemainingTime;
            
            View.ViewModel = this;

            _songProgressTimer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
            _songProgressTimer.Tick += SongProgressTimerTick;
            _songProgressTimer.Start();

        }
        
        #endregion Constructor

        #region Properties

        /// <summary>
        /// View of the Player (control)
        /// </summary>
        public IView View{ get; set; }

        /// <summary>
        /// gets or sets the volume - bound to volume slider
        /// </summary>
        public double Volume { get { return _volume; } 
        set
        {
            if (_volume != value)
            {
                _volume = value;
                RaisePropertyChanged(nameof(Volume));
                ChangeVolume(_volume);
                SolutionWideSettings.Instance.Volume = _volume;
            }
        } 
        }

        /// <summary>
        /// service for the playlist
        /// </summary>
        public IPlaylistService PlaylistService
        {
            get { return _playlistService ?? (_playlistService = _unityContainer.Resolve<IPlaylistService>()); }
        }

        /// <summary>
        /// the song that should be played next or is currently being played
        /// </summary>
        public PlaylistItem CurrentSong
        {
            get { return _currentSong; }
            set
            {
                if (_currentSong != null)
                {
                    _currentSong.IsCurrentlyPlaying = false;
                }
                _currentSong = value;
                if (_currentSong != null)
                {
                    _currentSong.IsCurrentlyPlaying = true;
                }
            }
            
        }

        /// <summary>
        /// whether the playback is paused at this moment
        /// </summary>
        public bool PlaybackPaused
        {
            get { return _playbackPaused; }
            set
            {
                if (_playbackPaused != value)
                {
                    _playbackPaused = value;
                    RaisePropertyChanged(nameof(PlaybackPaused));
                }
            }
        }

        /// <summary>
        /// whether a random song is taken as the next song if the playlist is empty
        /// or the next song in the media library is taken
        /// </summary>
        public bool SequentialPlayback
        {
            get { return _sequentialPlayback; }
            set
            {
                if (_sequentialPlayback != value)
                {
                    _sequentialPlayback = value;
                    SolutionWideSettings.Instance.SequentialPlayback = value;
                    RaisePropertyChanged(nameof(SequentialPlayback));
                }
            }
        }

        /// <summary>
        /// should the playback be stopped after one song has been played
        /// </summary>
        public bool StopPlaybackAfterSong
        {
            get { return _stopPlaybackAfterSong; }
            set
            {
                if (_stopPlaybackAfterSong != value)
                {
                    _stopPlaybackAfterSong = value;
                    SolutionWideSettings.Instance.StopPlaybackAfterSong = value;
                    RaisePropertyChanged(nameof(StopPlaybackAfterSong));
                }
            }
        }
        
        /// <summary>
        /// whether a random song is taken as the next song if the playlist is empty
        /// or the next song in the media library is taken
        /// </summary>
        public bool ExpertMode
        {
            get { return _expertMode; }
            set
            {
                if (_expertMode != value)
                {
                    _expertMode = value;
                    SolutionWideSettings.Instance.ExpertMode = value;
                    RaisePropertyChanged(nameof(ExpertMode));
                }
            }
        }

        /// <summary>
        /// Whether the remaining or the played time is shown
        /// </summary>
        public bool ShowRemainingTime
        {
            get { return _showRemainingTime; }
            set
            {
                if (_showRemainingTime != value)
                {
                    _showRemainingTime = value;
                    SolutionWideSettings.Instance.ShowRemainingTime = value;
                    RaisePropertyChanged(nameof(ShowRemainingTime));
                }
            }
        }

        #endregion Properties

        #region Events

        /// <summary>
        /// dispatcher timer tick - the view has to be updated
        /// (Song position, left time, ...)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongProgressTimerTick(object sender, EventArgs e)
        {
            int positionInMs = _mediaPlayer.GetSongPosition();
            int durationOfSongInMs = _mediaPlayer.GetDurationOfPlayedSong();
            ((IPlayerView)View).SetSongProgressSlider(positionInMs, durationOfSongInMs);


            string totalDuration = TimeSpan.FromMilliseconds(durationOfSongInMs).ToString(@"mm\:ss");
            string currentTime = TimeSpan.FromMilliseconds(positionInMs).ToString(@"mm\:ss");
            string timeTo = "-" + TimeSpan.FromMilliseconds(durationOfSongInMs - positionInMs).ToString(@"mm\:ss");

            string progressOf = currentTime + " | " + totalDuration;
            ((IPlayerView)View).SetSongProgressToggleButton(progressOf, timeTo);

            if (positionInMs > 0 && PlaybackPaused == false && _lastSongPositionTimePair != null)
            {
                bool haveToHandleEndOfSong = (positionInMs >= durationOfSongInMs);

                bool fiveHundredMsSinceLastUpdate = _lastSongPositionTimePair != null && (DateTime.Now - _lastSongPositionTimePair.Item1).TotalMilliseconds > 500;
                if (_lastSongPositionTimePair == null || fiveHundredMsSinceLastUpdate)
                {
                    _lastSongPositionTimePair = new Tuple<DateTime, int>(DateTime.Now, positionInMs);
                }
                
                // sometimes the karaoke library will state that the song still has some milliseconds to play
                // but has already stopped playing
                if (haveToHandleEndOfSong == false &&
                    fiveHundredMsSinceLastUpdate &&
                    _lastSongPositionTimePair != null &&
                    _lastSongPositionTimePair.Item2 == positionInMs
                    )
                {
                    haveToHandleEndOfSong = true;
                }

                if (haveToHandleEndOfSong)
                {
                    HandleEndOfSong();
                }
            }
        }
        
        #endregion Events

        #region Implementation of IPlayerViewModel

        /// <summary>
        /// Sets the song position inside the played song
        /// </summary>
        /// <param name="newSongPositionInMs">new position in milliSeconds</param>
        public void SetSongPosition(int newSongPositionInMs)
        {
            if ((DateTime.Now - _lastSetPositionCall).TotalMilliseconds < 500 ||
                _mediaPlayer.IsPlaying() == false)
            {
                return;
            }
            _lastSetPositionCall = DateTime.Now;
            _mediaPlayer.SetSongPosition(newSongPositionInMs);
        }

        #region Commands

        /// <summary>
        /// Delegate command for playing songs
        /// </summary>
        public DelegateCommand PlayCommand { get; set; }

        /// <summary>
        /// plays the currently selected song, if no song is selected it plays the first one inside the playlist,
        /// if no song is inside the playlist, the random function is used to get a song from the library
        /// </summary>
        private void Play()
        {
            if (_mediaPlayer.IsPlaying())
            {
                if (PlaybackPaused)
                {
                    Pause();
                    return;
                }
                _mediaPlayer.Stop();
            }
            if (CurrentSong == null && PlaylistService != null)
            {
                CurrentSong = PlaylistService.GetNextPlaylistItem(SequentialPlayback == false, null, true);
            }
            if (CurrentSong == null)
            {
                return;
            }

            _mediaPlayer.Play(CurrentSong);
            PlaybackPaused = false;
            
            PreviousSongCommand.RaiseCanExecuteChanged();

            PauseCommand.RaiseCanExecuteChanged();
            ((IPlayerView)View).SetArtistAndTitle(CurrentSong.Song.Artist, CurrentSong.Song.Title);
            int durationOfSongInMs = _mediaPlayer.GetDurationOfPlayedSong();
            ((IPlayerView)View).SetSongDuration(durationOfSongInMs);
        }

        /// <summary>
        /// command for pausing playback
        /// </summary>
        public DelegateCommand PauseCommand { get; set; }

        /// <summary>
        /// pauses the playback
        /// </summary>
        private void Pause()
        {
            _lastSongPositionTimePair = null;
            if (PlaybackPaused)
            {
                _mediaPlayer.Unpause();
            }
            else
            {
                _mediaPlayer.Pause();
            }
            
            PlaybackPaused = !PlaybackPaused;
            PauseCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// Whether anything is playing
        /// </summary>
        /// <returns></returns>
        private bool CanPause()
        {
            return _mediaPlayer.IsPlaying();
        }

        /// <summary>
        /// command for stopping playback and unloading the current song
        /// </summary>
        public DelegateCommand StopCommand { get; set; }

        /// <summary>
        /// stops the playback
        /// </summary>
        private void Stop()
        {
            _mediaPlayer.Stop();

            PauseCommand.RaiseCanExecuteChanged();
        }

        /// <summary>
        /// command for going on with the next song
        /// </summary>
        public DelegateCommand NextSongCommand { get; set; }

        /// <summary>
        /// plays the next song
        /// </summary>
        /// <param name="forceTakeFromMediaLibraryIfEnd"></param>
        private void Next(bool forceTakeFromMediaLibraryIfEnd = true)
        {
            if (PlaylistService != null)
            {
                bool takeFromMediaLibraryIfAtEndOfPlaylist = forceTakeFromMediaLibraryIfEnd ||
                                                             StopPlaybackAfterSong == false;
                CurrentSong = PlaylistService.GetNextPlaylistItem(SequentialPlayback == false, CurrentSong, takeFromMediaLibraryIfAtEndOfPlaylist);

                if (_mediaPlayer.IsPlaying() && PlaybackPaused == false && StopPlaybackAfterSong == false)
                {
                    Play();
                }
                else if (CurrentSong != null)
                {
                    Play();
                    Stop();
                }
            }
        }

        /// <summary>
        /// command for going back to the previous song
        /// </summary>
        public DelegateCommand PreviousSongCommand { get; set; }
        
        /// <summary>
        /// plays the previous song if there is one
        /// </summary>
        private void Previous()
        {
            if (SolutionWideSettings.Instance.RemoveSongFromPlaylistAfterFetch && _mediaPlayer.IsPlaying())
            {
                // restart current song
                Stop();
                Play();
                return;
            }
            if (PlaylistService != null)
            {
                CurrentSong = PlaylistService.GetPreviousPlaylistItem(CurrentSong);
            }
            
            PreviousSongCommand.RaiseCanExecuteChanged();

            if (_mediaPlayer.IsPlaying())
            {
                Play();
            }
        }

        /// <summary>
        /// Whether it is possible to play a previous song
        /// </summary>
        /// <returns></returns>
        private bool CanPlayPreviousSong()
        {
            if (SolutionWideSettings.Instance.RemoveSongFromPlaylistAfterFetch && _mediaPlayer.IsPlaying())
            {
                return true;
            }
            if (PlaylistService != null)
            {
                return PlaylistService.HasPreviousPlaylistItem(CurrentSong);
            }
            return false;
        }

        /// <summary>
        /// command for showing the about box
        /// </summary>
        public DelegateCommand ShowAboutBoxCommand { get; set; }

        /// <summary>
        /// shows the about box
        /// </summary>
        private void ShowAboutBox()
        {
            AboutBox aboutBox = new AboutBox(View as Window);
            aboutBox.ShowDialog();
        }
        
        /// <summary>
        /// command for showing the help
        /// </summary>
        public DelegateCommand ShowHelpCommand { get; set; }

        /// <summary>
        /// shows the help
        /// </summary>
        private void ShowHelp()
        {
            Process.Start("https://sourceforge.net/p/karamel/wiki/Help/");
        }

        #endregion Commands

        #region Volume

        /// <summary>
        /// Changes the volume
        /// </summary>
        /// <param name="volume">volume with a range of 0.0 to 1.0</param>
        private void ChangeVolume(double volume)
        {
            if (volume >= 0 && volume <= 10)
            {
                // Calculate the volume that's being set
                double newVolume = ushort.MaxValue * volume / 10.0;

                uint v = ((uint)newVolume) & 0xffff;
                uint vAll = v | (v << 16);
                _mediaPlayer.Volume = vAll;
            }
            else
            {
                throw new ArgumentOutOfRangeException("volume");
            }
        }

        #endregion Volume

        #endregion Implementation of IPlayerViewModel


        #region Implementation of IPlayerService

        /// <summary>
        /// plays the given playlist item
        /// </summary>
        /// <param name="playlistItem"></param>
        public void Play(PlaylistItem playlistItem)
        {
            if (playlistItem != null)
            {
                Stop();
                CurrentSong = playlistItem;
                Play();
            }
        }

        #endregion Implementation of IPlayerService

        #region Private Helper

        /// <summary>
        /// Handles the end of a song - either goes on playing or stops the playback
        /// </summary>
        private void HandleEndOfSong()
        {
            if (StopPlaybackAfterSong)
            {
                Stop();
            }
            Next(false);
        }


        #endregion Private Helper

        #region Dispose

        /// <summary>
        /// Frees resources and media channels
        /// </summary>
        public void Dispose()
        {
            _mediaPlayer.Close();
        }

        #endregion Dispose
    }
}
