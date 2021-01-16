using Business;

namespace Karamel.Infrastructure
{
    /// <summary>
    /// interface for the media player
    /// </summary>
    public interface IMediaPlayer
    {
        /// <summary>
        /// Checks whether a song is currently being played by the library
        /// </summary>
        /// <returns>true if a song is played</returns>
        bool IsPlaying();

        /// <summary>
        /// Stops playback
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses playback
        /// </summary>
        void Pause();

        /// <summary>
        /// Starts the playback again after it has been paused
        /// </summary>
        void Unpause();

        /// <summary>
        /// plays the given song
        /// </summary>
        /// <param name="song"></param>
        void Play(PlaylistItem song);

        /// <summary>
        /// Gets the duration of a song in milliseconds
        /// </summary>
        /// <returns>duration of song in milliseconds</returns>
        int GetDurationOfPlayedSong();

        /// <summary>
        /// Gets the position inside the song in milliseconds
        /// </summary>
        /// <returns>position inside song in milliseconds</returns>
        int GetSongPosition();

        /// <summary>
        /// Closes the player
        /// </summary>
        void Close();

        /// <summary>
        /// Gets/sets the volume
        /// </summary>
        uint Volume { get; set; }

        /// <summary>
        /// Sets the song position inside the played song
        /// </summary>
        /// <param name="newSongPositionInMs">new position in milliSeconds</param>
        void SetSongPosition(int newSongPositionInMs);
    }
}
