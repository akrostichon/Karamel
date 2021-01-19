using System.Windows.Controls;
using System.Windows.Input;
using Business;
using Playlist.ViewModel;

namespace Playlist.View
{
    /// <summary>
    /// interaction logic for PlaylistView.xaml
    /// </summary>
    public partial class PlaylistView : UserControl, IPlaylistView
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PlaylistView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ViewModel is Data context
        /// </summary>
        public Karamel.Infrastructure.IViewModel ViewModel
        {
            get { return (IPlaylistViewModel)DataContext; }
            set { DataContext = (IPlaylistViewModel)value; }
        }


        #region Implementation of IPlaylistView
        
        /// <summary>
        /// Selects an item inside the grid
        /// </summary>
        /// <param name="playlistItem"></param>
        public void SelectItem(PlaylistItem playlistItem)
        {
            int idxToSelect = PlaylistGrid.Items.IndexOf(playlistItem);
            if (idxToSelect >= 0)
            {
                PlaylistGrid.SelectedIndex = idxToSelect;
                PlaylistGrid.ScrollIntoView(PlaylistGrid.Items[idxToSelect]);
            }
        }

        /// <summary>
        /// double clicking plays the clicked item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaylistGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            if (row != null)
            {
                PlaylistItem playlistItem = row.Item as PlaylistItem;
                if (playlistItem != null)
                {
                    // TODO this means a user may play any item in the playlist and we will go on not with the next item but with the first
                    ((IPlaylistViewModel) ViewModel).PlayItem(playlistItem);
                }
            }
        }

        #endregion Implementation of IPlaylistView

    }
}
