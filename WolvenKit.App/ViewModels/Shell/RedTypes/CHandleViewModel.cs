using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WolvenKit.App.ViewModels.Dialogs;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CHandleViewModel : RedTypeViewModel<IRedHandle>
{
    public CHandleViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedHandle? data) : base(parent, redPropertyInfo, data)
    {
        ExtensionIcon = "References";

        var innerValue = data?.GetValue();
        if (innerValue != null)
        {
            DisplayType += $" \u2192 {RedReflection.GetRedTypeFromCSType(innerValue.GetType())}";
        }
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        var val = _data?.GetValue();
        if (val == null)
        {
            return;
        }

        Properties.Add(RedTypeHelper.Create(this, new RedPropertyInfo(val), val));
    }

    public override IList<KeyValuePair<string, Action>> GetSupportedActions()
    {
        var result = base.GetSupportedActions();

        if (_data?.GetValue() == null)
        {
            result.Insert(0, new KeyValuePair<string, Action>("Create class", SetClass));
        }
        else
        {
            result.Insert(0, new KeyValuePair<string, Action>("Find all references", FindAllReferences));
            result.Insert(1, new KeyValuePair<string, Action>("Replace class", SetClass));
            result.Insert(2, new KeyValuePair<string, Action>("Clear", () =>
            {
                _data!.SetValue(null);
                FetchProperties();
            }));
        }

        return result;
    }

    private void FindAllReferences()
    {
        var rootItem = GetRootItem();

        if (rootItem.RootContext is not { } rootContext)
        {
            return;
        }

        rootContext.SearchResults ??= new ObservableCollection<SearchResult>();

        rootContext.SearchResults.Clear();
        foreach (var property in rootItem.GetAllProperties())
        {
            if (property is CHandleViewModel { DataObject: IRedHandle handle })
            {
                if (ReferenceEquals(handle.GetValue(), _data!.GetValue()))
                {
                    rootContext.SearchResults.Add(new SearchResult($"{{{property.BuildXPath()}}}", property));
                }
            }

            if (property is CWeakHandleViewModel { DataObject: IRedWeakHandle weakHandle })
            {
                if (ReferenceEquals(weakHandle.GetValue(), _data!.GetValue()))
                {
                    rootContext.SearchResults.Add(new SearchResult($"{{{property.BuildXPath()}}}", property));
                }
            }
        }
    }

    private async void SetClass()
    {
        var existing = new ObservableCollection<string>(AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => RedPropertyInfo.InnerType!.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).Select(x => x.Name));
        if (existing.Count == 0)
        {
            throw new Exception();
        }

        if (_data == null)
        {
            DataObject = RedTypeManager.CreateRedType(RedPropertyInfo.GetFullType());
        }

        if (existing.Count == 1)
        {
            _data!.SetValue(RedTypeManager.Create(existing[0]));
            FetchProperties();
        }

        if (existing.Count > 1)
        {
            var appViewModel = RedTypeHelper.GetAppViewModel();

            await appViewModel.SetActiveDialog(new CreateClassDialogViewModel(existing, false)
            {
                DialogHandler = (model =>
                {
                    appViewModel.CloseDialogCommand.Execute(null);
                    if (model is CreateClassDialogViewModel createClassDialogViewModel &&
                        !string.IsNullOrEmpty(createClassDialogViewModel.SelectedClass))
                    {
                        _data!.SetValue(RedTypeManager.Create(createClassDialogViewModel.SelectedClass));
                        FetchProperties();
                    }
                })
            });
        }
    }

    protected override void OnPropertyChanged(string? propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(IsExpanded) && Properties.Count == 1)
        {
            Properties[0].IsExpanded = IsExpanded;
        }
    }
}