using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Karamel.Infrastructure
{
    public class DataGridWithMultipleSelection : DataGrid
    {

        public DataGridWithMultipleSelection()
        {
            SelectionChanged += MultiSelectGrid_SelectionChanged;
        }

        void MultiSelectGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsList = SelectedItems;
        }
        #region SelectedItemsList

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
                DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(DataGridWithMultipleSelection), new PropertyMetadata(null));

        #endregion
    }

}
