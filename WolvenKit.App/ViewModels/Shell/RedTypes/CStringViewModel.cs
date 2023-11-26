using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CStringViewModel : RedTypeViewModel<CString>
{
    public string StringValue
    {
        get => (CString)DataObject!;
        set => DataObject = (CString)value;
    }

    public CStringViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CString data) : base(parent, redPropertyInfo, data)
    {
        DisplayValue = _data;
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = _data;
}