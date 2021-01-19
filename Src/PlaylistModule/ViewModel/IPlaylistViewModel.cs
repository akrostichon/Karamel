using System.Collections.ObjectModel;
using Business;
using Karamel.Infrastructure;

namespace Playlist.ViewModel
{
    public interface IPlaylistViewModel : IViewModel
    {
        /// <summary>
        /// list of items witin the playlist
        /// </summary>
        ObservableCollection<PlaylistItem> PlaylistItems { get; }


        /// <summary>
        /// list of items witin the playlist history
        /// </summary>
        ObservableCollection<PlaylistItem> HistoryPlaylistItems { get; }

        /// <summary>
        /// Plays the selected playlistItem
        /// </summary>
        /// <param name="selectedItem"></param>
        void PlayItem(PlaylistItem selectedItem);
    }
}
