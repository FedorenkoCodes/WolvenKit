using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Syncfusion.UI.Xaml.ScrollAxis;
using Syncfusion.UI.Xaml.TreeGrid.Helpers;
using Syncfusion.UI.Xaml.TreeGrid;
using WolvenKit.App.ViewModels.Shell.RedTypes;
using static WolvenKit.Views.Tools.RedTreeView2;
using Syncfusion.UI.Xaml.Grid;
using WolvenKit.Views.Shell;
using WolvenKit.App.ViewModels.Shell;

namespace WolvenKit.Views.Tools;
/// <summary>
/// Interaktionslogik für RedTreeView2Compact.xaml
/// </summary>
public partial class RedTreeView2Compact : UserControl
{
    public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
        nameof(ItemSource), typeof(ObservableCollection<RedTypeViewModel>), typeof(RedTreeView2Compact));

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem), typeof(object), typeof(RedTreeView2Compact), new PropertyMetadata(OnSelectedItemChanged));

    private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RedTreeView2 view)
        {
            return;
        }

        var rowIndex = view.TreeView.ResolveToRowIndex(view.SelectedItem);
        var columnIndex = view.TreeView.GetLastColumnIndex();
        view.TreeView.ScrollInView(new RowColumnIndex(rowIndex, columnIndex));
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register(
        nameof(SelectedItems), typeof(ObservableCollection<object>), typeof(RedTreeView2Compact));

    public ObservableCollection<object> SelectedItems
    {
        get => (ObservableCollection<object>)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    public ObservableCollection<RedTypeViewModel> ItemSource
    {
        get => (ObservableCollection<RedTypeViewModel>)GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }

    public RedTreeView2Compact()
    {
        InitializeComponent();

        TreeView.SelectionChanged += TreeView_OnSelectionChanged;
        TreeView.TreeGridContextMenuOpening += TreeView_OnTreeGridContextMenuOpening;
        TreeView.SelectionController = new GridSelectionControllerExt(TreeView);
    }

    private void TreeView_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
    {
        
    }

    private void TreeView_OnTreeGridContextMenuOpening(object sender, TreeGridContextMenuEventArgs e)
    {
        if (SelectedItems.Count == 0)
        {
            e.Handled = true;
            return;
        }

        e.ContextMenu.Items.Clear();

        var redTypeViewModel = (RedTypeViewModel)SelectedItems[0];

        if (SelectedItems.Count == 1)
        {
            foreach (var supportedAction in redTypeViewModel.GetSupportedActions())
            {
                e.ContextMenu.Items.Add(BuildMenuItem(supportedAction));
            }
        }

        if (SelectedItems.Count > 1 && redTypeViewModel.Parent is IMultiActionSupport multiActionSupport)
        {
            foreach (var supportedMultiAction in multiActionSupport.GetSupportedMultiActions())
            {
                var menuItem = new MenuItem { Header = supportedMultiAction.Key };
                menuItem.Click += (_, _) => supportedMultiAction.Value(SelectedItems);

                e.ContextMenu.Items.Add(menuItem);
            }
        }

        if (e.ContextMenu.Items.Count == 0)
        {
            e.Handled = true;
        }

        MenuItem BuildMenuItem(ContextMenuItem supportedAction)
        {
            var menuItem = new MenuItem { Header = supportedAction.Name };

            if (supportedAction.Action != null)
            {
                menuItem.Click += (_, _) => supportedAction.Action();
            }

            foreach (var subItem in supportedAction.Children)
            {
                menuItem.Items.Add(BuildMenuItem(subItem));
            }

            return menuItem;
        }
    }
}
