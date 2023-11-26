using System;
using System.Linq;
using System.Reflection;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class RedBaseClassViewModel<T> : RedTypeViewModel<T> where T : RedBaseClass
{
    protected ExtendedTypeInfo _typeInfo;

    public RedBaseClassViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, T? data) : base(parent, redPropertyInfo, data)
    {
        if (RedPropertyInfo.ExtendedPropertyInfo != null)
        {
            _typeInfo = RedReflection.GetTypeInfo(RedPropertyInfo.ExtendedPropertyInfo.Type);
        }
        else if (_data != null)
        {
            _typeInfo = RedReflection.GetTypeInfo(_data);
        }
        else
        {
            throw new ArgumentNullException(nameof(_typeInfo));
        }
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

            var entry = RedTypeHelper.Create(this, new RedPropertyInfo(propertyInfo), _data.GetProperty(propertyInfo.RedName));
            entry.PropertyName = propertyInfo.RedName;

            Properties.Add(entry);
        }
    }

    protected internal override object GetValue() => Properties;

    protected internal override void SetValue(RedTypeViewModel value)
    {
        if (value.IsValueType)
        {
            _data?.SetProperty(value.PropertyName, value.DataObject);
        }
    }
}

public class RedBaseClassViewModel : RedBaseClassViewModel<RedBaseClass>
{
    public RedBaseClassViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, RedBaseClass? data) : base(parent, redPropertyInfo, data)
    {
    }
}