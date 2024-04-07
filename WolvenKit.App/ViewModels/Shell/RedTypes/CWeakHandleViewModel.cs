using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SharpDX;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CWeakHandleViewModel : RedTypeViewModel<IRedWeakHandle>
{
    public CWeakHandleViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedWeakHandle? data) : base(parent, redPropertyInfo, data)
    {
        ExtensionIcon = "References";

        var innerValue = data?.GetValue();
        if (innerValue != null)
        {
            DisplayType += $" \u2192 {RedReflection.GetRedTypeFromCSType(innerValue.GetType())}";
        }
    }

    public override IList<ContextMenuItem> GetSupportedActions()
    {
        var result = base.GetSupportedActions();

        if (_data?.GetValue() != null)
        {
            result.Insert(0, new ContextMenuItem("Go to source", GoToSource));
        }

        return result;
    }

    private void GoToSource()
    {
        var rootItem = GetRootItem();

        if (rootItem.RootContext is not { } rootContext)
        {
            return;
        }

        foreach (var property in rootItem.GetAllProperties())
        {
            if (property is not CHandleViewModel { DataObject: IRedHandle handle })
            {
                continue;
            }

            if (ReferenceEquals(handle.GetValue(), _data!.GetValue()))
            {
                rootContext.SelectedProperty = property;
                return;
            }
        }
    }
}