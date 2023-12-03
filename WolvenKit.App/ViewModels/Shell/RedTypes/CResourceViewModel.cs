using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CResourceViewModel<T> : RedBaseClassViewModel<T> where T : CResource
{
    public CResourceViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, T? data) : base(parent, redPropertyInfo, data)
    {
        PropertyName = "FILE ROOT";
    }
}

public class CResourceViewModel : CResourceViewModel<CResource>
{
    public CResourceViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CResource? data) : base(parent, redPropertyInfo, data)
    {
    }
}