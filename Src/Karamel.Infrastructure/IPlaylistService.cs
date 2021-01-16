using System.Collections.Generic;
using Business;

namespace Karamel.Infrastructure
{
    /// <summary>
    /// Service for the playlist
    /// </summary>
    public interface IPlaylistService
    {
        /// <summary>
        /// Adds a single song in Simple (without singer) or KaraokeBar (with singer) mode
        /// </summary>
        /// <param name="song">song that should be enqueued</param>
        /// <param name="startPlayingIfEmptyPlaylist"></param>
        /// <param name="singer">singer or null</param>
        void EnqueueSong(Song song, bool startPlayingIfEmptyPlaylist = false, Singer singer = null);

        /// <summary>
        /// Adds a song which is performed by a list of singers in karaokeBarMode
        /// </summary>
        /// <param name="song">song that should be enqueued</param>
        /// <param name="singers">singers performing the song</param>
        void EnqueueSong(Song song, IEnumerable<Singer> singers);

        /// <summary>
        /// Adds a number of songs without singers within the simple mode
        /// </summary>
        /// <param name="songs">songs that should be enqueued</param>
        void EnqueueSongs(List<Song> songs);

        /// <summary>
        /// Gets the next playlist item
        /// </summary>
        /// <param name="randomPlayback"></param>
        /// <param name="currentPlaylistItem"></param>
        /// <param name="fetchFromMediaLibraryIfEndReached"></param>
        /// <returns>
        /// the next playlist item
        /// </returns>
        PlaylistItem GetNextPlaylistItem(bool randomPlayback, PlaylistItem currentPlaylistItem, bool fetchFromMediaLibraryIfEndReached);

        /// <summary>
        /// Gets the previous playlist item
        /// </summary>
        /// <param name="currentPlaylistItem"></param>
        /// <returns>
        /// the previous playlist item
        /// </returns>
        PlaylistItem GetPreviousPlaylistItem(PlaylistItem currentPlaylistItem);
        
        /// <summary>
        /// Checks whether there is a playlist item before the current one
        /// </summary>
        /// <param name="currentPlaylistItem"></param>
        /// <returns></returns>
        bool HasPreviousPlaylistItem(PlaylistItem currentPlaylistItem);

        /// <summary>
        /// Clears the playlist
        /// </summary>
        void ClearPlaylist();

        
    }
}
