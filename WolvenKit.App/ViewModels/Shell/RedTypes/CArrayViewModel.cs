using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WolvenKit.App.ViewModels.Dialogs;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CArrayViewModel : RedTypeViewModel<IRedArray>
{
    internal AppViewModel AppViewModel { get; set; } = null!;
    internal RedTypeHelper RedTypeHelper { get; set; } = null!;

    public bool ShowProperties { get; set; } = false;

    public CArrayViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedArray? data) : base(parent, redPropertyInfo, data)
    {
        ExtensionIcon = "SymbolArray";
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data == null)
        {
            return;
        }

        for (var i = 0; i < _data.Count; i++)
        {
            var data = (IRedType?)_data[i];

            RedTypeViewModel entry;
            if (data != null)
            {
                entry = RedTypeHelper.Create(this, new RedPropertyInfo(data) { Index = i }, data);
            }
            else
            {
                entry = RedTypeHelper.Create(this, new RedPropertyInfo(_data.InnerType) { Index = i }, data);
            }

            entry.PropertyName = $"[{i}]";
            entry.ArrayIndex = i;

            Properties.Add(entry);
        }
    }

    protected internal override object GetValue()
    {
        if (ShowProperties)
        {
            return Properties;
        }
        return base.GetValue();
    }

    protected internal override void SetValue(RedTypeViewModel value)
    {
        if (value.IsValueType)
        {
            _data![value.ArrayIndex] = value.DataObject;
        }
    }

    public override IList<MenuItem> GetSupportedActions() =>
        new List<MenuItem>
        {
            CreateMenuItem("New item", (sender, args) =>
            {
                if (RedPropertyInfo.InnerType!.IsAssignableTo(typeof(RedBaseClass)))
                {
                    AddClass();
                }
                else
                {
                    _data!.Add(RedTypeManager.CreateRedType(RedPropertyInfo.InnerType!));
                }

                FetchProperties();
            }),
            CreateMenuItem("Clear item(s)", (sender, args) =>
            {
                _data!.Clear();
                FetchProperties();
            })
        };

    private async void AddClass()
    {
        var existing = new ObservableCollection<string>(AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => RedPropertyInfo.InnerType!.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).Select(x => x.Name));
        if (existing.Count == 0)
        {
            throw new Exception();
        }

        if (existing.Count == 1)
        {
            _data!.Add(RedTypeManager.Create(existing[0]));
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
                        _data!.Add(RedTypeManager.Create(createClassDialogViewModel.SelectedClass));
                        FetchProperties();
                    }
                })
            });
        }
    }
}