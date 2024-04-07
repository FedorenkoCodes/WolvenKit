using System.Linq;
using System;
using System.Collections.Generic;
using WolvenKit.RED4.Archive.Buffer;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class worldStreamingSectorViewModel : CResourceViewModel<worldStreamingSector>
{
    public worldStreamingSectorViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, worldStreamingSector? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data == null)
        {
            return;
        }

        foreach (var propertyInfo in _typeInfo.PropertyInfos.OrderBy(x => x.RedName))
        {
            ArgumentNullException.ThrowIfNull(propertyInfo.RedName);

            RedTypeViewModel entry;
            if (propertyInfo.RedName == "nodeData")
            {
                entry = new worldNodeDataBufferViewModel(this, new RedPropertyInfo(propertyInfo), (DataBuffer?)_data.GetProperty(propertyInfo.RedName));
                entry.RedTypeHelper = RedTypeHelper;

                entry.Refresh(true);
            }
            else
            {
                entry = RedTypeHelper.Create(this, new RedPropertyInfo(propertyInfo), _data.GetProperty(propertyInfo.RedName));
            }

            entry.PropertyName = propertyInfo.RedName;

            Properties.Add(entry);
        }
    }

    protected internal override void UpdateDisplayDescription()
    {
        var nodeData = GetPropertyFromPath("nodeData");

        if (nodeData != null)
        {
            foreach (var nodeDataProperty in nodeData.Properties)
            {
                var data = (worldNodeData)nodeDataProperty.DataObject!;

                nodeDataProperty.DisplayDescription = $"[{data.NodeIndex}] {_data!.Nodes[data.NodeIndex].Chunk!.DebugName}";
            }
        }
    }
}

public class worldNodeDataBufferViewModel : DataBufferViewModel
{
    public bool ShowProperties { get; set; } = true;

    public worldNodeDataBufferViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, DataBuffer? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data?.Data is not worldNodeDataBuffer worldNodeDataBuffer)
        {
            return;
        }

        for (var i = 0; i < worldNodeDataBuffer.Entries.Count; i++)
        {
            var entry = RedTypeHelper.Create(this, new RedPropertyInfo(worldNodeDataBuffer.Entries[i]), worldNodeDataBuffer.Entries[i], null, true, IsReadOnly);

            entry.ArrayIndex = i;
            entry.PropertyName = $"[{i}]";

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

    public override IList<ContextMenuItem> GetSupportedActions()
    {
        var result = new List<ContextMenuItem>
        {
            new("Export NodeData to JSON", ExportNodeData)
        };

        if (!IsReadOnly)
        {
            result.Add(new("Import from JSON to worldStreamingSector", () => ImportWorldNodeDataTask(true)));
            result.Add(new ("Import from JSON (no coords update)", () => ImportWorldNodeDataTask(false)));
        }

        return result;
    }

    private void ExportNodeData()
    {
    }

    private void ImportWorldNodeDataTask(bool updateCoords)
    {
        
    }
}