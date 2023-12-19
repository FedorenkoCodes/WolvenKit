using System;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CEnumViewModel : RedTypeViewModel<IRedEnum>
{
    private string _selectedValue;

    public string[] EnumValues { get; set; }

    public string SelectedValue
    {
        get => _selectedValue;
        set
        {
            if (value == _selectedValue)
            {
                return;
            }

            _selectedValue = value;
            DataObject = CEnum.Parse(_data!.GetInnerType(), value);
            OnPropertyChanged();
        }
    }

    public CEnumViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedEnum? data) : base(parent, redPropertyInfo, data)
    {
        ExtensionIcon = "SymbolEnum";

        EnumValues = Enum.GetNames(redPropertyInfo.InnerType!);

        _selectedValue = "";
        if (EnumValues.Length > 0)
        {
            _selectedValue = EnumValues[0];
        }
        
        if (data != null)
        {
            _selectedValue = data.ToEnumString();

            DisplayValue = _selectedValue;
        }
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = _selectedValue;
}