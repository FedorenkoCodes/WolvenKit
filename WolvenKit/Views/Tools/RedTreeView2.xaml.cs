using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Splat;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.ScrollAxis;
using Syncfusion.UI.Xaml.TreeGrid;
using Syncfusion.UI.Xaml.TreeGrid.Helpers;
using WolvenKit.App.ViewModels.Documents;
using WolvenKit.App.ViewModels.Shell;
using WolvenKit.App.ViewModels.Shell.RedTypes;
using WolvenKit.Views.Shell;

namespace WolvenKit.Views.Tools;
/// <summary>
/// Interaktionslogik für RedTreeView2.xaml
/// </summary>
public partial class RedTreeView2 : UserControl
{
    public static readonly DependencyProperty EnableSearchProperty = DependencyProperty.Register(
        nameof(EnableSearch), typeof(bool), typeof(RedTreeView2), new PropertyMetadata(true));

    public bool EnableSearch
    {
        get => (bool)GetValue(EnableSearchProperty);
        set => SetValue(EnableSearchProperty, value);
    }

    public static readonly DependencyProperty EnableBreadcrumbProperty = DependencyProperty.Register(
        nameof(EnableBreadcrumb), typeof(bool), typeof(RedTreeView2), new PropertyMetadata(true));

    public bool EnableBreadcrumb
    {
        get => (bool)GetValue(EnableBreadcrumbProperty);
        set => SetValue(EnableBreadcrumbProperty, value);
    }

    public static readonly DependencyProperty EnableDescriptionProperty = DependencyProperty.Register(
        nameof(EnableDescription), typeof(bool), typeof(RedTreeView2), new PropertyMetadata(true));

    public bool EnableDescription
    {
        get => (bool)GetValue(EnableDescriptionProperty);
        set => SetValue(EnableDescriptionProperty, value);
    }

    public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
        nameof(ItemSource), typeof(ObservableCollection<RedTypeViewModel>), typeof(RedTreeView2));

    public ObservableCollection<RedTypeViewModel> ItemSource
    {
        get => (ObservableCollection<RedTypeViewModel>)GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem), typeof(object), typeof(RedTreeView2), new PropertyMetadata(OnSelectedItemChanged));

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
        nameof(SelectedItems), typeof(ObservableCollection<object>), typeof(RedTreeView2));

    public ObservableCollection<object> SelectedItems
    {
        get => (ObservableCollection<object>)GetValue(SelectedItemsProperty);
        set => SetValue(SelectedItemsProperty, value);
    }

    public static readonly DependencyProperty SearchResultsProperty = DependencyProperty.Register(
        nameof(SearchResults), typeof(ObservableCollection<SearchResult>), typeof(RedTreeView2));

    public ObservableCollection<SearchResult> SearchResults
    {
        get => (ObservableCollection<SearchResult>)GetValue(SearchResultsProperty);
        set => SetValue(SearchResultsProperty, value);
    }

    internal RedTypeHelper _redTypeHelper;

    public RedTreeView2()
    {
        InitializeComponent();

        SearchResults = new ObservableCollection<SearchResult>();
        SearchResults.CollectionChanged += SearchResults_OnCollectionChanged;

        _redTypeHelper = Locator.Current.GetService<RedTypeHelper>();

        Navigator.SelectedItemChanged += Navigator_OnSelectedItemChanged;
        Navigator.TextPathChanged += Navigator_OnTextPathChanged;

        TreeView.SelectionChanged += TreeView_OnSelectionChanged;
        TreeView.TreeGridContextMenuOpening += TreeView_OnTreeGridContextMenuOpening;
        TreeView.SelectionController = new GridSelectionControllerExt(TreeView);
    }

    private void SearchResults_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => SearchExpander.SetCurrentValue(Expander.IsExpandedProperty, true);

    private bool HasSameParent()
    {
        var sameParent = true;

        RedTypeViewModel parent = null;
        foreach (var item in SelectedItems)
        {
            if (item is not RedTypeViewModel redTypeViewModel || redTypeViewModel.Parent == null)
            {
                sameParent = false;
                break;
            }

            if (parent == null)
            {
                parent = redTypeViewModel.Parent;
                continue;
            }

            if (!ReferenceEquals(parent, redTypeViewModel.Parent))
            {
                sameParent = false;
                break;
            }
        }

        return sameParent;
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

    private void Navigator_OnSelectedItemChanged(object sender, EventArgs e)
    {
        if (Navigator.SelectedItem is not { } redTypeViewModel)
        {
            return;
        }

        SelectTreeItem(redTypeViewModel);
    }

    private void Navigator_OnTextPathChanged(object sender, Breadcrumb.TextPathEventArgs e)
    {
        if (DataContext is not RDTDataViewModel { Properties: { } root })
        {
            return;
        }

        var parts = e.Path.Split('\\');

        RedTypeViewModel item = null;
        IList<RedTypeViewModel> items = root;
        foreach (var part in parts)
        {
            var prop = items.FirstOrDefault(x => x.PropertyName == part);
            if (prop != null)
            {
                item = prop;
                items = prop.Properties;
            }
            else
            {
                return;
            }
        }

        if (item != null)
        {
            SelectTreeItem(item);
            e.Handled = true;
        }
    }

    private void SelectTreeItem(RedTypeViewModel item)
    {
        var items = new List<RedTypeViewModel>();

        while (true)
        {
            items.Add(item);
            if (item.Parent == null)
            {
                break;
            }
            item = item.Parent;
        }

        items.Reverse();

        if (items.Count > 0)
        {
            int rowIndex;
            for (var i = 0; i < items.Count - 1; i++)
            {
                rowIndex = TreeView.ResolveToRowIndex(items[i]);
                TreeView.ExpandNode(rowIndex);
            }

            var lastItem = items[^1];
            TreeView.SetCurrentValue(SfGridBase.SelectedItemProperty, lastItem);

            rowIndex = TreeView.ResolveToRowIndex(lastItem);
            var columnIndex = TreeView.ResolveToStartColumnIndex();

            TreeView.ScrollInView(new RowColumnIndex(rowIndex, columnIndex));
        }
    }

    private void TreeView_OnSelectionChanged(object sender, GridSelectionChangedEventArgs e)
    {
        if (e.AddedItems.Count == 1 && e.AddedItems[0] is TreeGridRowInfo info)
        {
            var parts = new List<RedTypeViewModel>();

            var property = (RedTypeViewModel)info.RowData;
            do
            {
                parts.Add(property);
                property = property.Parent;
            } while (property != null);

            parts.Reverse();

            Navigator.SetCurrentValue(Breadcrumb.ItemsSourceProperty, parts);
        }
    }

    private IEnumerable<RedTypeViewModel> FindItems(RedTypeViewModel redTypeViewModel, string text)
    {
        foreach (var property in redTypeViewModel.GetAllProperties())
        {
            if (property.PropertyName.Contains(text, StringComparison.InvariantCultureIgnoreCase))
            {
                yield return property;
            }

            if (property.DisplayValue.Contains(text, StringComparison.InvariantCultureIgnoreCase))
            {
                yield return property;
            }
        }
    }

    private void SearchTextBox_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is not RDTDataViewModel { Properties: { } root })
            {
                return;
            }

            SearchResults ??= new ObservableCollection<SearchResult>();

            SearchResults.Clear();

            if (string.IsNullOrEmpty(SearchTextBox.Text))
            {
                return;
            }

            foreach (var redTypeViewModel in FindItems(root[0], SearchTextBox.Text))
            {
                SearchResults.Add(new SearchResult($"{{{redTypeViewModel.BuildXPath()}}} {redTypeViewModel.DisplayValue}", redTypeViewModel));
            }
        }
    }

    private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ListBoxItem item || item.Content is not SearchResult result)
        {
            return;
        }

        SelectTreeItem(result.Data);
    }

    public class GridSelectionControllerExt : TreeGridRowSelectionController
    {

        public GridSelectionControllerExt(SfTreeGrid treeGrid) : base(treeGrid)
        {
        }

        protected override void ProcessKeyDown(KeyEventArgs args)
        {
            if (args.Key == Key.Left)
            {
                if (TreeGrid.SelectedItem is RedTypeViewModel redTypeViewModel)
                {
                    if (redTypeViewModel.IsExpanded)
                    {
                        redTypeViewModel.IsExpanded = false;
                        return;
                    }

                    if (redTypeViewModel.Parent != null)
                    {
                        TreeGrid.SetCurrentValue(SfGridBase.SelectedItemProperty, redTypeViewModel.Parent);
                        return;
                    }
                }
            }

            if (args.Key == Key.Right)
            {
                if (TreeGrid.SelectedItem is RedTypeViewModel redTypeViewModel)
                {
                    if (redTypeViewModel.Properties.Count > 0)
                    {
                        redTypeViewModel.IsExpanded = true;
                    }
                }
            }

            base.ProcessKeyDown(args);
        }

        protected override void ProcessSelectedItemChanged(SelectionPropertyChangedHandlerArgs handle)
        {
            base.ProcessSelectedItemChanged(handle);
        }
    }
}
