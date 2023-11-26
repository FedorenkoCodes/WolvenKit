using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CArrayViewModel : RedTypeViewModel<IRedArray>
{
    public CArrayViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedArray? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data == null)
        {
            return;
        }

        for (var i = 0; i < _data.Count; i++)
        {
            var data = (IRedType?)_data[i];

            RedTypeViewModel entry;
            if (data != null)
            {
                entry = RedTypeHelper.Create(this, new RedPropertyInfo(data) { Index = i }, data);
            }
            else
            {
                entry = RedTypeHelper.Create(this, new RedPropertyInfo(_data.InnerType) { Index = i }, data);
            }

            entry.PropertyName = $"[{i}]";

            Properties.Add(entry);
        }
    }
}