﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using WolvenKit.App.ViewModels.Dialogs;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CDictionaryViewModel : CArrayViewModel
{
    private ObservableCollection<RedTypeViewModel> _viewCache = new();

    public CDictionaryViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CArray<CKeyValuePair>? data) : base(parent, redPropertyInfo, data)
    {
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
            var data = (CKeyValuePair?)_data[i];

            CKeyValuePairViewModel entry;
            if (data != null)
            {
                entry = new CKeyValuePairViewModel(this, new RedPropertyInfo(data) { Index = i }, data);
                entry.PropertyName = data.Key.GetResolvedText()!;
            }
            else
            {
                // TODO
                entry = new CKeyValuePairViewModel(this, new RedPropertyInfo(_data.InnerType) { Index = i }, data);
                entry.PropertyName = $"[{i}]";
            }

            entry.ArrayIndex = i;
            entry.RedTypeHelper = RedTypeHelper;
            entry.FetchProperties();

            Properties.Add(entry);
        }
    }

    protected internal override object GetValue()
    {
        _viewCache.Clear();

        foreach (var property in Properties)
        {
            var entry = RedTypeHelper.Create(this, property.Properties[0].RedPropertyInfo, property.Properties[0].DataObject);
            entry.PropertyName = property.PropertyName;

            _viewCache.Add(entry);
        }

        return _viewCache;
    }

    public override IList<MenuItem> GetSupportedActions() =>
        new List<MenuItem>
        {
            CreateMenuItem("New item", (sender, args) =>
            {
                AddClass();
            }),
            CreateMenuItem("Clear item(s)", (sender, args) =>
            {
                _data!.Clear();
                FetchProperties();
            })
        };

    private async void AddClass()
    {
        var existing = new ObservableCollection<string>
        {
            "Color",
            "CFloat",
            "CName",
            "Vector4",
            "rRef:ITexture",
            "rRef:CFoliageProfile",
            "rRef:CGradient",
            "rRef:CHairProfile",
            "rRef:Multilayer_Mask",
            "rRef:Multilayer_Setup",
            "rRef:CSkinProfile",
            "rRef:CTerrainSetup",
        };

        await AppViewModel.SetActiveDialog(new CreateClassDialogViewModel(existing, false)
        {
            DialogHandler = (model =>
            {
                AppViewModel.CloseDialogCommand.Execute(null);
                if (model is CreateClassDialogViewModel createClassDialogViewModel &&
                    !string.IsNullOrEmpty(createClassDialogViewModel.SelectedClass))
                {
                    _data!.Add(new CKeyValuePair(CName.Empty, RedTypeManager.CreateRedType(createClassDialogViewModel.SelectedClass)));
                    FetchProperties();
                }
            })
        });
    }
}