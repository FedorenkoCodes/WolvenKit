using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class NodeRefViewModel : RedTypeViewModel<NodeRef>
{
    public string? BindingValue
    {
        get => _data.GetResolvedText();
        set => DataObject = (NodeRef)value!;
    }

    public NodeRefViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, NodeRef data) : base(parent, redPropertyInfo, data)
    {
        ExtensionIcon = "SymbolString";
    }

    protected internal override void UpdateDisplayValue()
    {
        DisplayValue = _data.IsResolvable ? _data.GetResolvedText()! : _data.GetRedHash().ToString();
    }
}