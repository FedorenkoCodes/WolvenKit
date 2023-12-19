using System.Linq;
using DynamicData;
using Microsoft.Msagl.Core.Layout;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CLegacySingleChannelCurveViewModel : RedTypeViewModel<IRedLegacySingleChannelCurve>
{
    public RedTypeHelper RedTypeHelper { get; set; } = null!;

    public CLegacySingleChannelCurveViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedLegacySingleChannelCurve? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data == null)
        {
            return;
        }

        var curvePoints = _data.GetCurvePoints().ToArray();

        for (var i = 0; i < curvePoints.Length; i++)
        {
            var curvePoint = curvePoints[i];

            var entry = new CLegacySingleChannelCurvePointViewModel(this, new RedPropertyInfo(curvePoint), curvePoint)
            {
                ArrayIndex = i, 
                PropertyName = $"[{i}]", 
                RedTypeHelper = RedTypeHelper
            };

            entry.FetchProperties();

            Properties.Add(entry);
        }
    }
}

public class CLegacySingleChannelCurvePointViewModel : RedTypeViewModel<IRedCurvePoint>
{
    public RedTypeHelper RedTypeHelper { get; set; } = null!;

    public CLegacySingleChannelCurvePointViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedCurvePoint? data) : base(parent, redPropertyInfo, data)
    {
        UpdateDisplayValue();
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data == null)
        {
            return;
        }

        var point = RedTypeHelper.Create(this, new RedPropertyInfo(_data.GetPoint()), _data.GetPoint());
        point.PropertyName = "Point";
        Properties.Add(point);

        var val = RedTypeHelper.Create(this, new RedPropertyInfo(_data.GetValue()), _data.GetValue());
        val.PropertyName = "Value";
        Properties.Add(val);
    }

    protected internal override void SetValue(RedTypeViewModel value)
    {
        if (value.PropertyName == "Point")
        {
            _data!.SetPoint((CFloat)value.DataObject!);
        }

        if (value is { PropertyName: "Value", IsValueType: true })
        {
            _data!.SetValue(value.DataObject!);
        }
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = $"Point: {_data!.GetPoint()} | Value: {_data.GetValue()}";
}