using System.Windows;
using System.Windows.Controls;
using WolvenKit.App.ViewModels.Shell.RedTypes;

namespace WolvenKit.Views.RedValueEditors;

public class RedValueEditorSelector : DataTemplateSelector
{
    public DataTemplate CEnumEditor { get; set; }
    public DataTemplate CStringEditor { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is CEnumViewModel)
        {
            return CEnumEditor;
        }

        if (item is CStringViewModel)
        {
            return CStringEditor;
        }

        return base.SelectTemplate(item, container);
    }
}