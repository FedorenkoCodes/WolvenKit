using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CResourceViewModel : RedBaseClassViewModel<CResource>
{
    public CResourceViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CResource? data) : base(parent, redPropertyInfo, data)
    {
        PropertyName = "FILE ROOT";
    }
}