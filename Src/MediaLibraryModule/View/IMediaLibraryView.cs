using Business;
using Karamel.Infrastructure;

namespace MediaLibrary.View
{
    /// <summary>
    /// View interface for the media library
    /// </summary>
    public interface IMediaLibraryView : IView
    {
        Song GetNextSongAfterSelection();
    }
}
