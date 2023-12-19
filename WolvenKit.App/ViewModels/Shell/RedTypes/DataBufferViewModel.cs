using WolvenKit.RED4.Archive.Buffer;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class DataBufferViewModel : RedTypeViewModel<DataBuffer>
{
    public DataBufferViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, DataBuffer? data) : base(parent, redPropertyInfo, data)
    {
    }
}

public class SharedDataBufferViewModel : RedTypeViewModel<SharedDataBuffer>
{
    public SharedDataBufferViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, SharedDataBuffer? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data?.Data?.Data == null)
        {
            return;
        }

        Properties.Add(RedTypeHelper.Create(this, new RedPropertyInfo(_data.Data.Data), _data.Data.Data));
    }
}