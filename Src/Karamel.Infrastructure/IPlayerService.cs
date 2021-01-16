using Business;

namespace Karamel.Infrastructure
{
    /// <summary>
    /// Interface for the player
    /// </summary>
    public interface IPlayerService
    {
        void Play(PlaylistItem playlistItem);
    }
}
