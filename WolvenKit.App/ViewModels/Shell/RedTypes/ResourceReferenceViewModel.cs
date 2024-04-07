using System;
using System.Collections.Generic;
using System.Linq;
using WolvenKit.Core.Interfaces;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class ResourceReferenceViewModel : RedTypeViewModel<IRedRef>
{
    private string _selectedValue = "Default";

    private bool _lookupDone;
    private IGameFile? _projectFile = null;
    private List<IGameFile> _modFiles = new();
    private List<IGameFile> _baseFiles = new();

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
            UpdateFlag(value);
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

    public override IList<ContextMenuItem> GetSupportedActions()
    {
        var result = base.GetSupportedActions();

        if (_data?.Flags == InternalEnums.EImportFlags.Embedded)
        {
            result.Insert(0, new ContextMenuItem("Embedded File!"));
            return result;
        }

        if (!_lookupDone)
        {
            FindAllFiles();
        }

        var openFileMenu = new ContextMenuItem("Open file...");
        var addFileMenu = new ContextMenuItem("Add file...");
        
        if (_projectFile != null)
        {
            openFileMenu.Children.Add(new ContextMenuItem("From project", () => OpenFile(_projectFile)));
        }

        foreach (var gameFile in _modFiles)
        {
            openFileMenu.Children.Add(new ContextMenuItem($"From mod ({gameFile.GetArchive().Name})", () => OpenFile(gameFile)));
            addFileMenu.Children.Add(new ContextMenuItem($"From mod ({gameFile.GetArchive().Name})", () => OpenFile(gameFile)));
        }

        foreach (var gameFile in _baseFiles)
        {
            openFileMenu.Children.Add(new ContextMenuItem($"From base ({gameFile.GetArchive().Name})", () => OpenFile(gameFile)));
            addFileMenu.Children.Add(new ContextMenuItem($"From base ({gameFile.GetArchive().Name})", () => OpenFile(gameFile)));
        }

        result.Insert(0, openFileMenu);
        result.Insert(1, addFileMenu);

        return result;
    }

    private void OpenFile(IGameFile gameFile)
    {
        RedTypeHelper.GetAppViewModel().OpenGameFile(gameFile);
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

    private void FindAllFiles()
    {
        if (_data == null || _data.DepotPath == ResourcePath.Empty)
        {
            return;
        }

        var archiveManager = RedTypeHelper.GetArchiveManager();

        if (archiveManager.ProjectArchive != null)
        {
            _projectFile = archiveManager.ProjectArchive.Files.FirstOrDefault(x => x.Key == _data.DepotPath).Value;
        }

        foreach (var archive in archiveManager.ModArchives.Items)
        {
            if (archive.Files.TryGetValue(_data.DepotPath, out var gameFile))
            {
                _modFiles.Add(gameFile);
            }
        }

        foreach (var archive in archiveManager.Archives.Items)
        {
            if (archive.Files.TryGetValue(_data.DepotPath, out var gameFile))
            {
                _baseFiles.Add(gameFile);
            }
        }

        _lookupDone = true;
    }
}