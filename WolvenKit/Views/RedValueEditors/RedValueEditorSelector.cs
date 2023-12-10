using System.Windows;
using System.Windows.Controls;
using WolvenKit.App.ViewModels.Shell.RedTypes;

namespace WolvenKit.Views.RedValueEditors;

public class RedValueEditorSelector : DataTemplateSelector
{
    public DataTemplate CBoolEditor { get; set; }
    public DataTemplate CEnumEditor { get; set; }
    public DataTemplate CKeyValuePairEditor { get; set; }
    public DataTemplate CNameEditor { get; set; }
    public DataTemplate CNumberEditor { get; set; }
    public DataTemplate CStringEditor { get; set; }
    public DataTemplate TweakDBIDEditor { get; set; }
    public DataTemplate ResourceReferenceEditor { get; set; }
    public DataTemplate CLegacySingleChannelCurveEditor { get; set; }

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

        if (item is CKeyValuePairViewModel)
        {
            return CKeyValuePairEditor;
        }

        if (item is CNameViewModel)
        {
            return CNameEditor;
        }

        if (item is CStringViewModel)
        {
            return CStringEditor;
        }

        if (item is TweakDBIDViewModel)
        {
            return TweakDBIDEditor;
        }

        if (item is INumberViewModel)
        {
            return CNumberEditor;
        }

        if (item is ResourceReferenceViewModel)
        {
            return ResourceReferenceEditor;
        }

        if (item is CLegacySingleChannelCurveViewModel)
        {
            return CLegacySingleChannelCurveEditor;
        }

        return base.SelectTemplate(item, container);
    }
}