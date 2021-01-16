using System.Windows;
using Karamel.Infrastructure;

namespace MediaLibrary.View
{
    /// <summary>
    /// Interaction logic for FolderSelectionView.xaml
    /// </summary>
    public partial class FolderSelectionView : Window, IView
    {
        #region Constructor

        public FolderSelectionView()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Implementation of IView
        
        /// <summary>
        /// ViewModel is Data context
        /// </summary>
        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion Implementation of IView

        /// <summary>
        /// the ok button should close the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickDefault(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
