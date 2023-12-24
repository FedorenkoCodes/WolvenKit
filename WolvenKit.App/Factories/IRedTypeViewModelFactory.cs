using WolvenKit.App.ViewModels.Shell;
using WolvenKit.App.ViewModels.Shell.RedTypes;

namespace WolvenKit.App.Factories;

public interface IRedTypeViewModelFactory
{
    RedTypeHelper RedTypeHelper(AppViewModel appViewModel);
}