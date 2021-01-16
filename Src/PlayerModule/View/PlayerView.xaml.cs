using System.Windows.Controls;
using System.Windows.Input;
using PlayerControl.ViewModel;

namespace PlayerControl.View
{
    /// <summary>
    /// interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl, IPlayerView
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public PlayerView()
        {
            InitializeComponent();
        }

        #endregion Constructor

        /// <summary>
        /// View model
        /// </summary>
        public Karamel.Infrastructure.IViewModel ViewModel
        {
            get { return (IPlayerViewModel) DataContext; }
            set { DataContext = value; }
        }

        #region Implementation of IPlayerView

        /// <summary>
        /// Sets the artist and the title inside the "currently playing" section
        /// </summary>
        /// <param name="artist"></param>
        /// <param name="title"></param>
        public void SetArtistAndTitle(string artist, string title)
        {
            ArtistTitleTextBlock.Text = artist + " - " + title;
            
        }

        /// <summary>
        /// Sets the duration of the song that is played inside
        /// all views that show the song progress
        /// </summary>
        /// <param name="durationOfSongInMs">duration in milliseconds</param>
        public void SetSongDuration(int durationOfSongInMs)
        {
            SongProgressSlider.Maximum = durationOfSongInMs;
        }

        /// <summary>
        /// Sets the song position in milliseconds
        /// </summary>
        /// <param name="positionInMs"></param>
        /// <param name="durationOfSongInMs"></param>
        public void SetSongProgressSlider(int positionInMs, int durationOfSongInMs)
        {
            SongProgressSlider.Maximum = durationOfSongInMs;
            SongProgressSlider.Value = positionInMs;
        }

        /// <summary>
        /// The song progress button either shows the time till the song is finished or the current time and total duration
        /// </summary>
        /// <param name="progressOf">format for current time and total duration</param>
        /// <param name="timeLeft">format for left time till the song is finished</param>
        public void SetSongProgressToggleButton(string progressOf, string timeLeft)
        {
            if (SongProgressButton.IsChecked.HasValue && SongProgressButton.IsChecked.Value)
            {
                SongProgressButton.Content = timeLeft;
            }
            else
            {
                SongProgressButton.Content = progressOf;
            }
        }

        #endregion Implementation of IPlayerView

        #region Events

        /// <summary>
        /// Gets the song position (in milliseconds) from the given mouse position
        /// </summary>
        /// <param name="mousePosition"></param>
        /// <returns>song position in milliseconds</returns>
        private int GetSongPositionFromMousePosition(double mousePosition)
        {
            double ratio = mousePosition / SongProgressSlider.ActualWidth;
            double progressBarValue = ratio * SongProgressSlider.Maximum;
            return (int) progressBarValue;
        }
        
        /// <summary>
        /// When the user has clicked and now moves the progressbar position he wants to change
        /// the time inside the song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongProgressSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && SongProgressSlider.Maximum > 0)
            {
                UpdateSongPositionFromMousePosition(e);
            }
        }

        /// <summary>
        /// When the user clicks the progressBar he wants to change the position inside the song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SongProgressSlider_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (SongProgressSlider.Maximum > 0)
            {
                UpdateSongPositionFromMousePosition(e);
            }
        }

        /// <summary>
        /// Updates the song position with the help of the mouse position
        /// </summary>
        /// <param name="e"></param>
        private void UpdateSongPositionFromMousePosition(MouseEventArgs e)
        {
            double mousePosition = e.GetPosition(SongProgressSlider).X;
            int newSongPositionInMs = GetSongPositionFromMousePosition(mousePosition);
            if (newSongPositionInMs >= 0 && newSongPositionInMs < SongProgressSlider.Maximum)
            {
                SongProgressSlider.Value = newSongPositionInMs;
                ((IPlayerViewModel)ViewModel).SetSongPosition(newSongPositionInMs);
            }
        }


        #endregion Events
    }
}
