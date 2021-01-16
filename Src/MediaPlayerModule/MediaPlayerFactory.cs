using Business;
using MediaPlayer.View;

namespace MediaPlayer
{
    /// <summary>
    /// Media player factory - either represents a cdg player controller or a video player controller
    /// </summary>
    internal class MediaPlayerFactory
    {
        #region attributes

        /// <summary>
        /// the player controller (either cdg or video)
        /// </summary>
        private readonly IPlayerController _playerController;
        
        #endregion attributes

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="playlistItem"></param>
        /// <param name="videoPlayerWindow"></param>
        internal MediaPlayerFactory(PlaylistItem playlistItem, VideoPlayerWindowView videoPlayerWindow)
        {
            PlayerControllerType = GetPlayerControllerTypeForPlaylistItem(playlistItem);
            if (PlayerControllerType == PlayerControllerType.Mp3G)
            {
                _playerController = new CdgPlayerController(videoPlayerWindow);
            }
            else
            {
                _playerController = new VideoPlayerController(videoPlayerWindow);
            }
        }

        #region Properties
        
        /// <summary>
        /// the player controller (either cdg or video)
        /// </summary>
        internal IPlayerController PlayerController { get { return _playerController; } }

        /// <summary>
        /// player controller type (is the song that is played a video or an mp3+g)
        /// </summary>
        internal PlayerControllerType PlayerControllerType { get; private set; }

        #endregion Properties

        #region PlayerController

        /// <summary>
        /// Gets the correct playerController type for the kind of media file behind the playlist item
        /// </summary>
        /// <param name="playlistItem"></param>
        /// <returns>Mp3g or Video player Controller type</returns>
        internal PlayerControllerType GetPlayerControllerTypeForPlaylistItem(PlaylistItem playlistItem)
        {
            if (playlistItem.Song.Extension.Equals("mp3") || playlistItem.Song.Extension.Equals("zip"))
            {
                return PlayerControllerType.Mp3G;
            }
            return PlayerControllerType.Video;
        }

        #endregion PlayerController

    }
}
