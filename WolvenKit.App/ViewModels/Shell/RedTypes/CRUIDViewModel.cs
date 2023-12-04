using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CRUIDViewModel : RedTypeViewModel<CRUID>, INumberViewModel
{
    public ulong MinValue => ulong.MinValue;
    public ulong MaxValue => ulong.MaxValue;
    public int DecimalDigits => 0;

    public ulong BindingValue
    {
        get => (CRUID)DataObject!;
        set => DataObject = (CRUID)value;
    }

    public CRUIDViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CRUID data) : base(parent, redPropertyInfo, data)
    {
        ExtensionIcon = "SymbolNumeric";
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}