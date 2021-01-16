using Karamel.Infrastructure;
using MediaPlayer.View;
using Microsoft.Practices.Prism.ViewModel;

namespace MediaPlayer.ViewModel
{
    /// <summary>
    /// View model for the video player window
    /// </summary>
    public class VideoPlayerWindowViewModel : NotificationObject, IVideoPlayerWindowViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor which initiates the commands and sets the view
        /// </summary>
        /// <param name="videoPlayerWindowView">view</param>
        public VideoPlayerWindowViewModel(IVideoPlayerWindowView videoPlayerWindowView)
        {
            View = videoPlayerWindowView;
            View.ViewModel = this;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// video player window view
        /// </summary>
        public IView View{ get; set; }

        #endregion Properties

        #region Implementation of IVideoControl

        public void Pause()
        {
            
        }

        #endregion Implementation of IVideoControl
    }
}