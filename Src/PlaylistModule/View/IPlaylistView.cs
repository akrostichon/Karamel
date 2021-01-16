using Business;
using Karamel.Infrastructure;

namespace Playlist.View
{
    public interface IPlaylistView : IView
    {
        void SelectItem(PlaylistItem playlistItem);
    }
}
