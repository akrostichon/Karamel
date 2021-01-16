using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Karamel.Infrastructure;
using MediaPlayer.Properties;

namespace MediaPlayer.View
{
    /// <summary>
    /// interaction logic for VideoPlayerWindowView.xaml
    /// </summary>
    public partial class VideoPlayerWindowView : Window, IVideoPlayerWindowView
    {
        #region Constructor

        /// <summary>
        /// initialization
        /// </summary>
        public VideoPlayerWindowView()
        {
            InitializeComponent();
            LoadVideoWindowSettings();
        }

        #endregion Constructor

        #region Implementation of IView

        /// <summary>
        /// view model which is the data context
        /// </summary>
        public IViewModel ViewModel { get; set; }

        #endregion Implementation of IView

        /// <summary>
        /// When the window is doubleClicked it should be maximized.
        /// If it already is maximized it should return to its normal size
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// When the user holds and moves the mouse above the window it should be dragged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        /// <summary>
        /// when the window is closing it saves its position and state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VideoPlayerWindowView_OnClosing(object sender, CancelEventArgs e)
        {
            SaveVideoWindowSettings();
        }

        #region Save and load window settings

        /// <summary>
        /// Loads the video settings and adjusts them to the actual screen size
        /// </summary>
        private void LoadVideoWindowSettings()
        {
            Width = VideoWindowSettings.Default.Width;
            Height = VideoWindowSettings.Default.Height;

            Top = VideoWindowSettings.Default.Top;
            Left = VideoWindowSettings.Default.Left;
            WindowState = VideoWindowSettings.Default.WindowState;

            AdjustPositionToScreenSize();
        }

        /// <summary>
        /// adjusts the stored position to the size of the screen
        /// </summary>
        public void AdjustPositionToScreenSize()
        {

            if (Width > SystemParameters.VirtualScreenWidth)
            {
                Width = SystemParameters.VirtualScreenWidth;
            }

            if (Height > SystemParameters.VirtualScreenHeight)
            {
                Height = SystemParameters.VirtualScreenHeight;
            }
            if (Top + Height / 2 > SystemParameters.VirtualScreenHeight)
            {
                Top = SystemParameters.VirtualScreenHeight - Height;
            }

            if (Left + Width / 2 > SystemParameters.VirtualScreenWidth)
            {
                Left = SystemParameters.VirtualScreenWidth - Width;
            }

            if (Top < 0)
            {
                Top = 0;
            }

            if (Left < 0)
            {
                Left = 0;
            }
        }

        /// <summary>
        /// Saves the window position and size in the settings
        /// </summary>
        public void SaveVideoWindowSettings()
        {
            if (WindowState != WindowState.Minimized)
            {
                VideoWindowSettings.Default.Width = Width;
                VideoWindowSettings.Default.Height = Height;
                VideoWindowSettings.Default.Top = Top;
                VideoWindowSettings.Default.Left = Left;
                VideoWindowSettings.Default.Save();
            }
        }

        #endregion save and load window settings
    }
}
