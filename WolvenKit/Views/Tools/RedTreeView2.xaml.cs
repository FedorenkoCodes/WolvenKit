using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.ScrollAxis;
using Syncfusion.UI.Xaml.TreeGrid;
using WolvenKit.App.ViewModels.Shell.RedTypes;
using WolvenKit.RED4.Types;
using WolvenKit.Views.Shell;

namespace WolvenKit.Views.Tools;
/// <summary>
/// Interaktionslogik für RedTreeView2.xaml
/// </summary>
public partial class RedTreeView2 : UserControl
{
    public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
        nameof(ItemSource), typeof(IRedType), typeof(RedTreeView2), new PropertyMetadata(null, OnItemSourceChanged));

    private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not RedTreeView2 view)
        {
            return;
        }

        view.SourceViewModel.Clear();

        if (view.ItemSource != null)
        {
            view.SourceViewModel.Add(RedTypeHelper.Create(view.ItemSource));
        }
    }

    public IRedType ItemSource
    {
        get => (IRedType)GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem), typeof(object), typeof(RedTreeView2));

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

    public ObservableCollection<RedTypeViewModel> SourceViewModel { get; } = new();

    public RedTreeView2()
    {
        InitializeComponent();

        Navigator.SelectedItemChanged += Navigator_OnSelectedItemChanged;
        Navigator.TextPathChanged += Navigator_OnTextPathChanged;

        TreeView.SelectionChanged += TreeView_OnSelectionChanged;
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
        var parts = e.Path.Split('\\');

        RedTypeViewModel item = null;
        IList<RedTypeViewModel> items = new List<RedTypeViewModel> { RedTypeHelper.Create(ItemSource) };
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

    public ObservableCollection<SearchResult> SearchResults { get; } = new();

    public record SearchResult(string Name, RedTypeViewModel Data);

    private IEnumerable<RedTypeViewModel> FindItems(RedTypeViewModel redTypeViewModel, string text)
    {
        if (redTypeViewModel.PropertyName.Contains(text, StringComparison.InvariantCultureIgnoreCase))
        {
            yield return redTypeViewModel;
        }

        if (redTypeViewModel.DisplayValue.Contains(text, StringComparison.InvariantCultureIgnoreCase))
        {
            yield return redTypeViewModel;
        }

        if (redTypeViewModel.Properties.Count > 0)
        {
            foreach (var child in redTypeViewModel.Properties)
            {
                foreach (var childProperty in FindItems(child, text))
                {
                    yield return childProperty;
                }

            }
        }
    }

    private string BuildPath(RedTypeViewModel redTypeViewModel)
    {
        var parts = new List<string>();

        do
        {
            parts.Add(redTypeViewModel.PropertyName);
            redTypeViewModel = redTypeViewModel.Parent;
        } while (redTypeViewModel != null);

        parts.Reverse();

        return string.Join('\\', parts);
    }

    private void SearchTextBox_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (SourceViewModel is not { } root)
        {
            return;
        }

        if (e.Key == Key.Enter)
        {
            SearchResults.Clear();
            foreach (var redTypeViewModel in FindItems(root[0], SearchTextBox.Text))
            {
                SearchResults.Add(new SearchResult($"{{{BuildPath(redTypeViewModel)}}} {redTypeViewModel.DisplayValue}", redTypeViewModel));
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
}
