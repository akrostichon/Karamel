using System;
using System.Windows.Controls;
using Business;
using MediaPlayer.View;

namespace MediaPlayer
{
    internal class VideoPlayerController : IPlayerController
    {
        #region Attributes

        /// <summary>
        /// the video player window in which the video player should be shown
        /// </summary>
        private readonly VideoPlayerWindowView _videoPlayerWindow;

        #endregion Attributes

        #region Constructor

        /// <summary>
        /// Constructor, binds size of cdgPlayerWindow to the size of the video player window
        /// </summary>
        /// <param name="videoPlayerWindow"></param>
        public VideoPlayerController(VideoPlayerWindowView videoPlayerWindow)
        {
            _videoPlayerWindow = videoPlayerWindow;
        }

        #endregion Constructor

        public bool IsPlaying()
        {
            return _videoPlayerWindow.VideoPlayer.Source != null;
        }

        public void Stop()
        {
            _videoPlayerWindow.VideoPlayer.Stop();
        }

        public void Pause()
        {
            _videoPlayerWindow.VideoPlayer.Pause();
        }

        /// <summary>
        /// Starts the playback again after it has been paused
        /// </summary>
        public void Unpause()
        {
            _videoPlayerWindow.VideoPlayer.Play();
        }

        public void Play(PlaylistItem song)
        {
            if (IsPlaying())
            {
                Stop();
            }
            _videoPlayerWindow.VideoPlayer.Source = new Uri(song.Song.FilePath);
            _videoPlayerWindow.VideoPlayer.LoadedBehavior = MediaState.Manual;
            _videoPlayerWindow.VideoPlayer.UnloadedBehavior = MediaState.Manual;
            _videoPlayerWindow.VideoPlayer.Play();
        }

        public int GetDurationOfPlayedSong()
        {
            if (_videoPlayerWindow.VideoPlayer.NaturalDuration.HasTimeSpan)
            {
                return (int) _videoPlayerWindow.VideoPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
            }
            return 0;
        }

        public int GetSongPosition()
        {
            return (int) _videoPlayerWindow.VideoPlayer.Position.TotalMilliseconds;
        }

        public void Close()
        {
            _videoPlayerWindow.VideoPlayer.Close();
        }

        public void SetSongPosition(int newSongPositionInMs)
        {
            _videoPlayerWindow.VideoPlayer.Position = TimeSpan.FromMilliseconds(newSongPositionInMs);
        }
    }
}
