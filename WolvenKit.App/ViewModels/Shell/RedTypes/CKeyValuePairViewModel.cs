using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CKeyValuePairViewModel : RedTypeViewModel<CKeyValuePair>
{
    public CKeyValuePairViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CKeyValuePair? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data == null)
        {
            return;
        }

        Properties.Add(RedTypeHelper.Create(this, new RedPropertyInfo(_data.Value), _data.Value));
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = _data!.Value.ToString()!;

    protected internal override object GetValue() => Properties;
}