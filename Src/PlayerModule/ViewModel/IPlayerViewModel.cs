using Karamel.Infrastructure;
using Microsoft.Practices.Prism.Commands;

namespace PlayerControl.ViewModel
{
    /// <summary>
    /// Interface for the PlayerViewModel
    /// </summary>
    public interface IPlayerViewModel : IViewModel
    {
        DelegateCommand PlayCommand { get; set; }
        DelegateCommand PauseCommand { get; set; }
        DelegateCommand StopCommand { get; set; }
        DelegateCommand NextSongCommand { get; set; }
        DelegateCommand PreviousSongCommand { get; set; }
        bool PlaybackPaused { get; }
        void SetSongPosition(int newSongPositionInMs);
    }
}
