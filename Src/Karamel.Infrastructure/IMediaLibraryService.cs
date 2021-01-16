using Business;

namespace Karamel.Infrastructure
{
    /// <summary>
    /// Interface for the MediaLibrary
    /// </summary>
    public interface IMediaLibraryService
    {
        /// <summary>
        /// Gets the next song at random
        /// </summary>
        /// <returns>a random song from the library</returns>
        Song GetRandomNextSong();

        /// <summary>
        /// Gets the next song in a sequential mode
        /// </summary>
        /// <returns>next song from the library</returns>
        Song GetNextSong();
    }
}
