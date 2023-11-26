using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class DefaultPropertyViewModel : RedTypeViewModel<IRedType>
{
    public DefaultPropertyViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedType? data) : base(parent, redPropertyInfo, data)
    {
    }
}