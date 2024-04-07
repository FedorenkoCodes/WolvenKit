using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            entry.IsReadOnly = IsReadOnly;

            entry.Refresh(true);

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

    public override IList<ContextMenuItem> GetSupportedActions()
    {
        var result = base.GetSupportedActions();

        if (!IsReadOnly)
        {
            result.Insert(0, new ContextMenuItem("New item", AddClass));
            result.Insert(1, new ContextMenuItem("Clear item(s)", () =>
            {
                _data!.Clear();
                Refresh(true);
            }));
        }

        return result;
    }

    private async void AddClass()
    {
        var existing = new List<TypeEntry>
        {
            new("Color", "Color description", typeof(CColor)),
            new("CpuNameU64", "", typeof(CName)),
            new("Cube", "Reference to cube xbm", typeof(CResourceReference<ITexture>)),
            new("DynamicTexture", "Reference to dtex file", typeof(CResourceReference<ITexture>)),
            new("FoliageParameters", "Reference to fp file", typeof(CResourceReference<CFoliageProfile>)),
            new("Gradient", "Reference to gradient file", typeof(CResourceReference<CGradient>)),
            new("HairParameters", "Reference to hp file", typeof(CResourceReference<CHairProfile>)),
            new("MultilayerMask", "Reference to mlmask file", typeof(CResourceReference<Multilayer_Mask>)),
            new("MultilayerSetup", "Reference to mlsetup file", typeof(CResourceReference<Multilayer_Setup>)),
            new("Scalar", "", typeof(CFloat)),
            new("SkinParameters", "Reference to sp file", typeof(CResourceReference<CSkinProfile>)),
            //new("StructBuffer", "", null), // still not sure what this does
            new("TerrainSetup", "Reference to terrainsetup file", typeof(CResourceReference<CTerrainSetup>)),
            new("Texture", "Reference to xbm file", typeof(CResourceReference<ITexture>)),
            new("TextureArray", "Reference to texarray file", typeof(CResourceReference<ITexture>)),
            new("Vector", "", typeof(Vector4)),
        };

        await RedTypeHelper.GetAppViewModel().SetActiveDialog(new TypeSelectorDialogViewModel(existing)
        {
            DialogHandler = (model =>
            {
                RedTypeHelper.GetAppViewModel().CloseDialogCommand.Execute(null);

                if (model is TypeSelectorDialogViewModel { SelectedEntry.UserData: Type selectedType })
                {
                    _data!.Add(new CKeyValuePair(CName.Empty, RedTypeManager.CreateRedType(selectedType)));
                    Refresh(true);
                }
            })
        });
    }
}