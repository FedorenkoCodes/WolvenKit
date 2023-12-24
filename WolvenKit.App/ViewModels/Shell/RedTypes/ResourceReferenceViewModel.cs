using System;
using System.Collections.Generic;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class ResourceReferenceViewModel : RedTypeViewModel<IRedRef>
{
    private string _selectedValue = "Default";

    public string[] EnumValues => new[]
    {
        "Default",
        "Obligatory",
        "Template",
        "Soft",
        "Embedded",
        "Inplace"
    };

    public string? BindingPath
    {
        get => _data!.DepotPath.GetResolvedText();
        set => UpdatePath(value);
    }

    public string BindingFlag
    {
        get => _selectedValue;
        set
        {
            if (value == _selectedValue)
            {
                return;
            }

            _selectedValue = value;
            UpdatePath(value);
        }
    }

    public ResourceReferenceViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedRef? data) : base(parent, redPropertyInfo, data)
    {
        ExtensionIcon = "FileSymlinkFile";

        if (data != null)
        {
            _selectedValue = data.Flags.ToString();

            DisplayValue = _selectedValue;
        }
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = _data!.DepotPath.IsResolvable ? _data!.DepotPath.GetResolvedText()! : _data!.DepotPath.GetRedHash().ToString();

    public override IList<KeyValuePair<string, Action>> GetSupportedActions()
    {
        var result = base.GetSupportedActions();

        result.Insert(0, new KeyValuePair<string, Action>("Go to file", GoToFile));
        result.Insert(1, new KeyValuePair<string, Action>("Add file to project", AddFileToProject));

        return result;
    }

    private void GoToFile()
    {
        if (_data == null)
        {
            return;
        }

        RedTypeHelper.GetAppViewModel().OpenFileFromDepotPath(_data.DepotPath);
    }

    private void AddFileToProject()
    {
        if (_data == null)
        {
            return;
        }

        RedTypeHelper.GetGameController().AddFileToModModal(_data.DepotPath);
    }

    private void UpdatePath(string? path)
    {
        if (path == null)
        {
            return;
        }

        DataObject = RedTypeManager.CreateRedType(RedPropertyInfo.BaseType, (ResourcePath)path, _data!.Flags);
    }

    private void UpdateFlag(string? flag)
    {
        if (flag == null)
        {
            return;
        }

        DataObject = RedTypeManager.CreateRedType(RedPropertyInfo.BaseType, _data!.DepotPath, Enum.Parse<InternalEnums.EImportFlags>(flag));
    }
}