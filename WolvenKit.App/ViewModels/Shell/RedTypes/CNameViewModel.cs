using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CNameViewModel : RedTypeViewModel<CName>
{
    public string? BindingValue
    {
        get => _data.GetResolvedText();
        set => DataObject = (CName)value!;
    }

    public CNameViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CName data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue()
    {
        DisplayValue = _data.IsResolvable ? _data.GetResolvedText()! : _data.GetRedHash().ToString();
    }
}