using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CBoolViewModel : RedTypeViewModel<CBool>
{
    public bool BindingValue
    {
        get => (CBool)DataObject!;
        set => DataObject = (CBool)value;
    }

    public CBoolViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CBool data) : base(parent, redPropertyInfo, data)
    {
        UpdateDisplayValue();
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = _data ? "True" : "False";
}