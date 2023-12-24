using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WolvenKit.App.ViewModels.Dialogs;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CArrayViewModel : RedTypeViewModel<IRedArray>, IMultiActionSupport
{
    public bool ShowProperties { get; set; } = false;

    public CArrayViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedArray? data) : base(parent, redPropertyInfo, data)
    {
        ExtensionIcon = "SymbolArray";

        DisplayType = $"{RedPropertyInfo.RedTypeName}";
        if (_data != null)
        {
            DisplayType += $" [{_data.Count}]";
        }
    }

    protected internal override void UpdateDisplayValue()
    {
        DisplayType = $"{RedPropertyInfo.RedTypeName}";
        if (_data != null)
        {
            DisplayType += $" [{_data.Count}]";
        }
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
            entry = RedTypeHelper.Create(this, new RedPropertyInfo(data) { Index = index }, data, null, true, IsReadOnly);
        }
        else
        {
            entry = RedTypeHelper.Create(this, new RedPropertyInfo(innerType!) { Index = index }, data, null, true, IsReadOnly);
        }

        entry.PropertyName = $"[{index}]";
        entry.ArrayIndex = index;

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
            OnPropertyChanged(nameof(DataObject));
        }
    }

    public IList<KeyValuePair<string, Action<IList<object>>>> GetSupportedMultiActions() =>
        new List<KeyValuePair<string, Action<IList<object>>>>
        {
            new("Remove items", RemoveItems)
        };

    public override IList<KeyValuePair<string, Action>> GetSupportedActions()
    {
        var result = base.GetSupportedActions();

        if (!IsReadOnly)
        {
            result.Insert(0, new KeyValuePair<string, Action>("New item", AddItem));
            result.Insert(1, new KeyValuePair<string, Action>("Clear item(s)", Clear));
        }

        return result;
    }

    public void AddItem()
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

            OnPropertyChanged(nameof(DataObject));
            Select(item);
        }
    }

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

            OnPropertyChanged(nameof(DataObject));
            Select(item);
        }

        if (existing.Count > 1)
        {
            await RedTypeHelper.GetAppViewModel().SetActiveDialog(new CreateClassDialogViewModel(existing, false)
            {
                DialogHandler = (model =>
                {
                    RedTypeHelper.GetAppViewModel().CloseDialogCommand.Execute(null);
                    if (model is CreateClassDialogViewModel createClassDialogViewModel &&
                        !string.IsNullOrEmpty(createClassDialogViewModel.SelectedClass))
                    {
                        var data = RedTypeManager.Create(createClassDialogViewModel.SelectedClass);
                        _data!.Add(data);

                        var item = AddItem(Properties.Count, data, null, true);

                        OnPropertyChanged(nameof(DataObject));
                        Select(item);
                    }
                })
            });
        }
    }

    public void RemoveItem(RedTypeViewModel redTypeViewModel)
    {
        _data!.RemoveAt(redTypeViewModel.ArrayIndex);
        Properties.RemoveAt(redTypeViewModel.ArrayIndex);

        for (var i = 0; i < Properties.Count; i++)
        {
            Properties[i].PropertyName = $"[{i}]";
            Properties[i].ArrayIndex = i;
        }

        OnPropertyChanged(nameof(DataObject));

        if (IsExpanded && Properties.Count > 0)
        {
            Select(Properties[^1]);
        }
        else
        {
            Select();
        }
    }

    public void RemoveItems(IList<object> selectedItems)
    {
        foreach (var redTypeViewModel in selectedItems.Cast<RedTypeViewModel>().OrderByDescending(x => x.ArrayIndex))
        {
            _data!.RemoveAt(redTypeViewModel.ArrayIndex);
            Properties.RemoveAt(redTypeViewModel.ArrayIndex);
        }

        for (var i = 0; i < Properties.Count; i++)
        {
            Properties[i].PropertyName = $"[{i}]";
            Properties[i].ArrayIndex = i;
        }

        OnPropertyChanged(nameof(DataObject));
        Select();
    }

    public void Clear()
    {
        _data!.Clear();
        Properties.Clear();

        OnPropertyChanged(nameof(DataObject));
        Select();
    }
}