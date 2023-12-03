using System;
using System.Globalization;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CDateTimeViewModel : RedTypeViewModel<CDateTime>
{
    public CDateTimeViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CDateTime data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = ((DateTime)_data).ToString(CultureInfo.InvariantCulture);
}