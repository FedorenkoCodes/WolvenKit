using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class ResourceReferenceViewModel : RedTypeViewModel<IRedRef>
{
    public string? BindingPath
    {
        get => _data!.DepotPath.GetResolvedText();
        set => UpdatePath(value);
    }

    public InternalEnums.EImportFlags BindingFlag
    {
        get => _data!.Flags;
        set => UpdateFlag(value);
    }

    public ResourceReferenceViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedRef? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = _data!.DepotPath.IsResolvable ? _data!.DepotPath.GetResolvedText()! : _data!.DepotPath.GetRedHash().ToString();

    private void UpdatePath(string? path)
    {
        if (path == null)
        {
            return;
        }

        DataObject = RedTypeManager.CreateRedType(RedPropertyInfo.BaseType, (ResourcePath)path, _data!.Flags);
    }

    private void UpdateFlag(InternalEnums.EImportFlags flags)
    {
        DataObject = RedTypeManager.CreateRedType(RedPropertyInfo.BaseType, _data!.DepotPath, flags);
    }
}