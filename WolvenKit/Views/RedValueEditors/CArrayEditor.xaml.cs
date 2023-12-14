using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Syncfusion.UI.Xaml.Grid;
using WolvenKit.App.ViewModels.Shell.RedTypes;

namespace WolvenKit.Views.RedValueEditors
{
    /// <summary>
    /// Interaktionslogik für CArrayEditor.xaml
    /// </summary>
    public partial class CArrayEditor : UserControl
    {
        private readonly GridRowSizingOptions _gridRowResizingOptions = new();

        public CArrayEditor()
        {
            InitializeComponent();
        }

        private void PropertyGrid_OnQueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            PropertyGrid.GridColumnSizer.GetAutoRowHeight(e.RowIndex, _gridRowResizingOptions, out var autoHeight);

            e.Height = autoHeight >= 30 ? autoHeight : 30;
            e.Handled = true;
        }

        private void DeleteSelected_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not CArrayViewModel vm)
            {
                return;
            }

            vm.RemoveItems(PropertyGrid.SelectedItems);
        }

        private void DeleteAllItems_OnClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is not CArrayViewModel vm)
            {
                return;
            }

            vm.Clear();
        }
    }
}
