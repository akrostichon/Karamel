using System.Windows;
using SingerLibrary.ViewModel;

namespace SingerLibrary.View
{
    /// <summary>
    /// interaction logic for SingerLibraryView.xaml
    /// </summary>
    public partial class SingerLibraryView : Window, ISingerLibraryView
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SingerLibraryView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// View model is data context
        /// </summary>
        public Karamel.Infrastructure.IViewModel ViewModel
        {
            get { return (ISingerLibraryViewModel)DataContext; }
            set { DataContext = value; }
        }

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
