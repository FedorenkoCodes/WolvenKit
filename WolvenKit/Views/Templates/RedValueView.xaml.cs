using System.Windows;
using System.Windows.Controls;
using Syncfusion.UI.Xaml.Grid;
using WolvenKit.App.ViewModels.Shell.RedTypes;

namespace WolvenKit.Views.Templates
{
    /// <summary>
    /// Interaktionslogik für RedValueView.xaml
    /// </summary>
    public partial class RedValueView : UserControl
    {
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
            nameof(ItemSource), typeof(RedTypeViewModel), typeof(RedValueView), new PropertyMetadata(null, OnItemSourceChanged));

        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RedValueView view)
            {
                return;
            }
        }

        public RedTypeViewModel ItemSource
        {
            get => (RedTypeViewModel)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }

        public RedValueView()
        {
            InitializeComponent();
        }

        private readonly GridRowSizingOptions _gridRowResizingOptions = new();

        private void PropertyGrid_OnQueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            PropertyGrid.GridColumnSizer.GetAutoRowHeight(e.RowIndex, _gridRowResizingOptions, out var autoHeight);

            e.Height = autoHeight >= 30 ? autoHeight : 30;
            e.Handled = true;
        }
    }
}
