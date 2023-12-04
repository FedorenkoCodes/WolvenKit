using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using WolvenKit.App.ViewModels.Dialogs;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CHandleViewModel : RedTypeViewModel<IRedHandle>
{
    internal AppViewModel AppViewModel { get; set; } = null!;
    internal RedTypeHelper RedTypeHelper { get; set; } = null!;

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

    public override IList<MenuItem> GetSupportedActions()
    {
        var actions = base.GetSupportedActions();

        if (_data?.GetValue() == null)
        {
            actions.Add(CreateMenuItem("Create class", (sender, args) => SetClass()));
        }
        else
        {
            actions.Add(CreateMenuItem("Replace class", (sender, args) => SetClass()));

            actions.Add(CreateMenuItem("Clear", (sender, args) =>
            {
                _data!.SetValue(null);
                FetchProperties();
            }));
        }

        return actions;
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
            await AppViewModel.SetActiveDialog(new CreateClassDialogViewModel(existing, false)
            {
                DialogHandler = (model =>
                {
                    AppViewModel.CloseDialogCommand.Execute(null);
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
}