using System.Globalization;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public interface INumberViewModel
{

}

public class CUInt8ViewModel : RedTypeViewModel<CUInt8>, INumberViewModel
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
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CInt8ViewModel : RedTypeViewModel<CInt8>, INumberViewModel
{
    public sbyte MinValue => sbyte.MinValue;
    public sbyte MaxValue => sbyte.MaxValue;
    public int DecimalDigits => 0;

    public sbyte BindingValue
    {
        get => (CInt8)DataObject!;
        set => DataObject = (CInt8)value;
    }

    public CInt8ViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CInt8 data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CUInt16ViewModel : RedTypeViewModel<CUInt16>, INumberViewModel
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
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CInt16ViewModel : RedTypeViewModel<CInt16>, INumberViewModel
{
    public short MinValue => short.MinValue;
    public short MaxValue => short.MaxValue;
    public int DecimalDigits => 0;

    public short BindingValue
    {
        get => (CInt16)DataObject!;
        set => DataObject = (CInt16)value;
    }

    public CInt16ViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CInt16 data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CUInt32ViewModel : RedTypeViewModel<CUInt32>, INumberViewModel
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
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CInt32ViewModel : RedTypeViewModel<CInt32>, INumberViewModel
{
    public int MinValue => int.MinValue;
    public int MaxValue => int.MaxValue;
    public int DecimalDigits => 0;

    public int BindingValue
    {
        get => (CInt32)DataObject!;
        set => DataObject = (CInt32)value;
    }

    public CInt32ViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CInt32 data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CUInt64ViewModel : RedTypeViewModel<CUInt64>, INumberViewModel
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
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CInt64ViewModel : RedTypeViewModel<CInt64>, INumberViewModel
{
    public long MinValue => long.MinValue;
    public long MaxValue => long.MaxValue;
    public int DecimalDigits => 0;

    public long BindingValue
    {
        get => (CInt64)DataObject!;
        set => DataObject = (CInt64)value;
    }

    public CInt64ViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CInt64 data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString();
}

public class CFloatViewModel : RedTypeViewModel<CFloat>, INumberViewModel
{
    public float MinValue => float.MinValue;
    public float MaxValue => float.MaxValue;
    public int DecimalDigits => 9;

    public float BindingValue
    {
        get => (CFloat)DataObject!;
        set => DataObject = (CFloat)value;
    }

    public CFloatViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CFloat data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString("G9", CultureInfo.InvariantCulture);
}

public class CDoubleViewModel : RedTypeViewModel<CDouble>, INumberViewModel
{
    public double MinValue => double.MinValue;
    public double MaxValue => double.MaxValue;
    public int DecimalDigits => 17;

    public double BindingValue
    {
        get => (CDouble)DataObject!;
        set => DataObject = (CDouble)value;
    }

    public CDoubleViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, CDouble data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = BindingValue.ToString("G17", CultureInfo.InvariantCulture);
}