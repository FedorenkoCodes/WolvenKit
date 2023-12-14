using System.Windows.Controls;
using Syncfusion.UI.Xaml.Grid;

namespace WolvenKit.Views.RedValueEditors
{
    /// <summary>
    /// Interaktionslogik für RedBaseClassEditor.xaml
    /// </summary>
    public partial class RedBaseClassEditor : UserControl
    {
        private readonly GridRowSizingOptions _gridRowResizingOptions = new();

        public RedBaseClassEditor()
        {
            InitializeComponent();
        }

        private void PropertyGrid_OnQueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            PropertyGrid.GridColumnSizer.GetAutoRowHeight(e.RowIndex, _gridRowResizingOptions, out var autoHeight);

            e.Height = autoHeight >= 30 ? autoHeight : 30;
            e.Handled = true;
        }
    }
}
