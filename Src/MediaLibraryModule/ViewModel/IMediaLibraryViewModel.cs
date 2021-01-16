using System.Collections;
using Karamel.Infrastructure;

namespace MediaLibrary.ViewModel
{
    /// <summary>
    /// Interface for the specific view model of the media library
    /// </summary>
    public interface IMediaLibraryViewModel : IViewModel
    {
        /// <summary>
        /// Enqueues a number of songs
        /// </summary>
        /// <param name="selectedSongs"></param>
        /// <param name="startPlayingIfListIsEmpty"></param>
        void Enqueue(IList selectedSongs, bool startPlayingIfListIsEmpty = false);
    }
}
