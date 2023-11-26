using System;
using System.Reflection;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CHandleViewModel : RedTypeViewModel<IRedHandle>
{
    public CHandleViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedHandle? data) : base(parent, redPropertyInfo, data)
    {
        var innerValue = data?.GetValue();
        if (innerValue != null)
        {
            DisplayType += $" \u2192 {RedReflection.GetRedTypeFromCSType(innerValue.GetType())}";
        }
    }

    protected internal override void FetchProperties()
    {
        var val = _data?.GetValue();
        if (val == null)
        {
            return;
        }

        var typeInfo = RedReflection.GetTypeInfo(val);

        Properties.Clear();
        Properties.Add(RedTypeHelper.Create(this, new RedPropertyInfo(val), val));
    }
}