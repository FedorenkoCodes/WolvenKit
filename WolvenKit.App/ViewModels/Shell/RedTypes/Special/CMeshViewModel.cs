using System.Windows.Forms;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CMeshViewModel : CResourceViewModel<CMesh>
{
    public CMeshViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CMesh? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue()
    {
        base.UpdateDisplayValue();

        if (_data == null)
        {
            return;
        }

        string[] localKeys =
        {
            "localMaterialBuffer.materials", 
            "preloadLocalMaterialInstances", 
        };

        string[] externalKeys =
        {
            "externalMaterials",
            "preloadExternalMaterials",
        };

        foreach (var materialEntryProperty in GetPropertyFromPath("materialEntries")!.Properties)
        {
            var materialEntry = (CMeshMaterialEntry)materialEntryProperty.DataObject!;

            ushort idx = materialEntry.Index;

            foreach (var key in materialEntry.IsLocalInstance ? localKeys : externalKeys)
            {
                var list = GetPropertyFromPath(key);
                if (list is not null && list.Properties.Count > idx)
                {
                    list.Properties[idx].DisplayDescription = materialEntry.Name.GetResolvedText()!;
                }
            }
        }
    }
}