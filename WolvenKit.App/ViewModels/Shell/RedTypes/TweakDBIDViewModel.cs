using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class TweakDBIDViewModel : RedTypeViewModel<TweakDBID>
{
    public string? BindingValue
    {
        get => _data.GetResolvedText();
        set => DataObject = (TweakDBID)value!;
    }

    public TweakDBIDViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, TweakDBID data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = _data.ToString();
}