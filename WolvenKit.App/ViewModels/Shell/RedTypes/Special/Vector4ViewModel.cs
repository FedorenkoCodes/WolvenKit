using System.Globalization;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class Vector4ViewModel : RedBaseClassViewModel<Vector4>
{
    public Vector4ViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, Vector4? data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue()
    {
        if (_data == null)
        {
            DisplayValue = "null";
            return;
        }

        DisplayValue = $"X: {_data.X.ToString(CultureInfo.InvariantCulture)} | Y: {_data.Y.ToString(CultureInfo.InvariantCulture)} | Z: {_data.Z.ToString(CultureInfo.InvariantCulture)} | W: {_data.W.ToString(CultureInfo.InvariantCulture)}";
    }
}