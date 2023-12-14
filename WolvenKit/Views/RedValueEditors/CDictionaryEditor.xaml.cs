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

namespace WolvenKit.Views.RedValueEditors;
/// <summary>
/// Interaktionslogik für CDictionaryEditor.xaml
/// </summary>
public partial class CDictionaryEditor : UserControl
{
    private readonly GridRowSizingOptions _gridRowResizingOptions = new();

    public CDictionaryEditor()
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
