using System.Windows;
using System.Windows.Controls;
using WolvenKit.App.ViewModels.Shell.RedTypes;

namespace WolvenKit.Views.RedValueEditors;

public class RedValueEditorSelector : DataTemplateSelector
{
    public DataTemplate CBoolEditor { get; set; }
    public DataTemplate CEnumEditor { get; set; }
    public DataTemplate CNumberEditor { get; set; }
    public DataTemplate CStringEditor { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is CBoolViewModel)
        {
            return CBoolEditor;
        }

        if (item is CEnumViewModel)
        {
            return CEnumEditor;
        }

        if (item is CStringViewModel)
        {
            return CStringEditor;
        }

        if (item is CUInt8ViewModel or CUInt16ViewModel or CUInt32ViewModel or CUInt64ViewModel)
        {
            return CNumberEditor;
        }

        return base.SelectTemplate(item, container);
    }
}