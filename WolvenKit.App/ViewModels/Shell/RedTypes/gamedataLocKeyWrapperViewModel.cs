using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class gamedataLocKeyWrapperViewModel : RedTypeViewModel<gamedataLocKeyWrapper>
{
    public sbyte MinValue => sbyte.MinValue;
    public sbyte MaxValue => sbyte.MaxValue;
    public int DecimalDigits => 0;

    public ulong BindingValue
    {
        get => _data!.Key;
        set => _data!.Key = value;
    }

    public gamedataLocKeyWrapperViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, gamedataLocKeyWrapper? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = _data != null ? _data.Key.ToString() : "null";
}