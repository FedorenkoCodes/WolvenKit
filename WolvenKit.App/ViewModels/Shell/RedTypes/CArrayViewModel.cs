using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WolvenKit.App.ViewModels.Dialogs;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CArrayViewModel : RedTypeViewModel<IRedArray>, IMultiActionSupport
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
            AddItem(i, (IRedType?)_data[i], _data.InnerType);
        }
    }

    protected RedTypeViewModel AddItem(int index, IRedType? data, Type? innerType = null, bool expand = false)
    {
        RedTypeViewModel entry;
        if (data != null)
        {
            entry = RedTypeHelper.Create(this, new RedPropertyInfo(data) { Index = index }, data);
        }
        else
        {
            entry = RedTypeHelper.Create(this, new RedPropertyInfo(innerType!) { Index = index }, data);
        }

        entry.PropertyName = $"[{index}]";
        entry.ArrayIndex = index;
        entry.IsReadOnly = IsReadOnly;

        Properties.Add(entry);

        if (expand)
        {
            IsExpanded = true;
        }

        return entry;
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

    public IList<MenuItem> GetSupportedMultiActions(IList<object> selectedItems)
    {
        return new List<MenuItem>
        {
            CreateMenuItem("Remove items", (sender, args) => { RemoveItems(selectedItems); })
        };
    }

    private new void Refresh()
    {
        FetchProperties();
        UpdateDisplayValue();

        if (Properties.Count > 0)
        {
            Select(Properties[^1]);
        }
        else
        {
            Select();
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
                    var data = RedTypeManager.CreateRedType(RedPropertyInfo.InnerType!);
                    _data!.Add(data);

                    var item = AddItem(Properties.Count, data, null, true);
                    Select(item);
                }
            }),
            CreateMenuItem("Clear item(s)", (sender, args) =>
            {
                Clear();
            })
        };

    private async void AddClass()
    {
        var existing = new ObservableCollection<string>(AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => RedPropertyInfo.InnerType!.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract).Select(x => RedReflection.GetTypeRedName(x)!));
        if (existing.Count == 0)
        {
            throw new Exception();
        }

        if (existing.Count == 1)
        {
            var data = RedTypeManager.Create(existing[0]);
            _data!.Add(data);

            var item = AddItem(Properties.Count, data, null, true);
            Select(item);
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
                        var data = RedTypeManager.Create(createClassDialogViewModel.SelectedClass);
                        _data!.Add(data);

                        var item = AddItem(Properties.Count, data, null, true);
                        Select(item);
                    }
                })
            });
        }
    }

    public void RemoveItem(RedTypeViewModel redTypeViewModel)
    {
        _data!.RemoveAt(redTypeViewModel.ArrayIndex);
        Refresh();
    }

    public void RemoveItems(IList<object> selectedItems)
    {
        foreach (var redTypeViewModel in selectedItems.Cast<RedTypeViewModel>().OrderByDescending(x => x.ArrayIndex))
        {
            _data!.RemoveAt(redTypeViewModel.ArrayIndex);
        }
        Refresh();
    }

    public void Clear()
    {
        _data!.Clear();
        Refresh();
    }
}