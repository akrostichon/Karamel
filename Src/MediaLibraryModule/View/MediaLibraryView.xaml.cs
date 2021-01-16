using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Business;
using Karamel.Infrastructure;
using MediaLibrary.ViewModel;

namespace MediaLibrary.View
{
    /// <summary>
    /// interaction logic for MediaLibraryView.xaml
    /// </summary>
    public partial class MediaLibraryView : IMediaLibraryView
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MediaLibraryView()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// the view model
        /// </summary>
        public IViewModel ViewModel
        {
            get { return (IMediaLibraryViewModel)DataContext; }
            set { DataContext = value; }
        }

        #endregion Properties
        
        #region Events

        /// <summary>
        /// handle context menu item clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            IMediaLibraryViewModel viewModel = (IMediaLibraryViewModel) DataContext;
            if (sender.Equals(EnqueueSongsMenuItem))
            {
                viewModel.Enqueue(MediaLibrary.SelectedItems);
            }
        }

        /// <summary>
        /// Double click enqueues the selected song - if none was inside the playlist it is also played directly
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaLibrary_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            if (row != null)
            {
                bool startPlayingIfListIsEmpty = SolutionWideSettings.Instance.StopPlaybackAfterSong == false;
                ((IMediaLibraryViewModel)DataContext).Enqueue(MediaLibrary.SelectedItems, startPlayingIfListIsEmpty);
            }
        }
        
        #endregion Events

        #region Implementation of IMediaLibraryView

        /// <summary>
        /// Gets the next song after the currently selected one
        /// </summary>
        /// <returns></returns>
        public Song GetNextSongAfterSelection()
        {
            if (MediaLibrary.SelectedIndex >= 0 && MediaLibrary.Items.Count > MediaLibrary.SelectedIndex + 1)
            {
                MediaLibrary.SelectedIndex++;
            }
            else
            {
                MediaLibrary.SelectedIndex = 0;
            }
            return MediaLibrary.SelectedItem as Song;
        }
        
        #endregion Implementation of IMediaLibraryView
    }
}
