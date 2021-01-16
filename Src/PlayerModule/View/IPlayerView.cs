using Karamel.Infrastructure;

namespace PlayerControl.View
{
    /// <summary>
    /// interface for the player view
    /// </summary>
    public interface IPlayerView : IView
    {
        /// <summary>
        /// sets the artist and title
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="title"></param>
        void SetArtistAndTitle(string artist, string title);

        /// <summary>
        /// Sets the duration of the song that is played inside
        /// all views that show the song progress
        /// </summary>
        /// <param name="durationOfSongInMs">duration in milliseconds</param>
        void SetSongDuration(int durationOfSongInMs);

        /// <summary>
        /// Sets the progress inside the song in milliseconds
        /// </summary>
        /// <param name="positionInMs"></param>
        /// <param name="durationOfSongInMs"></param>
        void SetSongProgressSlider(int positionInMs, int durationOfSongInMs);


        /// <summary>
        /// The song progress button either shows the time till the song is finished or the current time and total duration
        /// </summary>
        /// <param name="progressOf">format for current time and total duration</param>
        /// <param name="timeLeft">format for left time till the song is finished</param>
        void SetSongProgressToggleButton(string progressOf, string timeLeft);
    }
}
