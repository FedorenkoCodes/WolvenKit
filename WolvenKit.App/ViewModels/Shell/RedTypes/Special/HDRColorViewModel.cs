using System.Linq;
using System;
using System.Windows.Media;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class HDRColorViewModel : RedBaseClassViewModel<HDRColor>
{
    private Color _color;

    public Color Color
    {
        get => _color;
        set
        {
            if (value == _color)
            {
                return;
            }

            OnPropertyChanging();
            _color = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(Brush));
            UpdateHDRColor();
        }
    }

    public SolidColorBrush Brush => new(Color.FromArgb(255, Color.R, Color.G, Color.B));

    public HDRColorViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, HDRColor? data) : base(parent, redPropertyInfo, data)
    {
        if (_data != null)
        {
            _color = Color.FromScRgb(1, _data.Red, _data.Green, _data.Blue);
        }
    }

    private void UpdateHDRColor()
    {
        DataObject ??= new HDRColor();

        GetPropertyByName("Red")!.DataObject = (CFloat)_color.ScR;
        GetPropertyByName("Green")!.DataObject = (CFloat)_color.ScG;
        GetPropertyByName("Blue")!.DataObject = (CFloat)_color.ScB;

        Refresh(false);
    }
}