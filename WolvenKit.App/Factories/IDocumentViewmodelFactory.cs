using WolvenKit.App.ViewModels.Documents;
using WolvenKit.App.ViewModels.Shell;
using WolvenKit.Core.Interfaces;

namespace WolvenKit.App.Factories;
public interface IDocumentViewmodelFactory
{
    public RedDocumentViewModel RedDocumentViewModel(AppViewModel appViewModel, IGameFile gameFile, bool isReadOnly = false);

    public WScriptDocumentViewModel WScriptDocumentViewModel(string path);

    public TweakXLDocumentViewModel TweakXLDocumentViewModel(string path);
}
