using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class CUInt8ViewModel : RedTypeViewModel<CUInt8>
{
    public byte MinValue => byte.MinValue;
    public byte MaxValue => byte.MaxValue;
    public int DecimalDigits => 0;

    public byte BindingValue
    {
        get => (CUInt8)DataObject!;
        set => DataObject = (CUInt8)value;
    }

    public CUInt8ViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CUInt8 data) : base(parent, redPropertyInfo, data)
    {
        UpdateDisplayValue();
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CUInt16ViewModel : RedTypeViewModel<CUInt16>
{
    public ushort MinValue => ushort.MinValue;
    public ushort MaxValue => ushort.MaxValue;
    public int DecimalDigits => 0;

    public ushort BindingValue
    {
        get => (CUInt16)DataObject!;
        set => DataObject = (CUInt16)value;
    }

    public CUInt16ViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CUInt16 data) : base(parent, redPropertyInfo, data)
    {
        UpdateDisplayValue();
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CUInt32ViewModel : RedTypeViewModel<CUInt32>
{
    public uint MinValue => uint.MinValue;
    public uint MaxValue => uint.MaxValue;
    public int DecimalDigits => 0;

    public uint BindingValue
    {
        get => (CUInt32)DataObject!;
        set => DataObject = (CUInt32)value;
    }

    public CUInt32ViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CUInt32 data) : base(parent, redPropertyInfo, data)
    {
        UpdateDisplayValue();
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CUInt64ViewModel : RedTypeViewModel<CUInt64>
{
    public ulong MinValue => ulong.MinValue;
    public ulong MaxValue => ulong.MaxValue;
    public int DecimalDigits => 0;

    public ulong BindingValue
    {
        get => (CUInt64)DataObject!;
        set => DataObject = (CUInt64)value;
    }

    public CUInt64ViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CUInt64 data) : base(parent, redPropertyInfo, data)
    {
        UpdateDisplayValue();
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}