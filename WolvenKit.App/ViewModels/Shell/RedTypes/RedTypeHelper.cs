using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public static class RedTypeHelper
{
    public static RedTypeViewModel Create(IRedType data) => Create(null, new RedPropertyInfo(data), data);

    public static RedTypeViewModel Create(RedTypeViewModel? parent, RedPropertyInfo propertyInfo, IRedType? data, bool fetchData = true)
    {
        RedTypeViewModel? result = null;

        if (typeof(CResource).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CResourceViewModel(parent, propertyInfo, (CResource?)data);
        } 
        else if (typeof(RedBaseClass).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new RedBaseClassViewModel(parent, propertyInfo, (RedBaseClass?)data);
        }
        else if (typeof(IRedHandle).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CHandleViewModel(parent, propertyInfo, (IRedHandle?)data);
        }
        else if (typeof(IRedArray).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CArrayViewModel(parent, propertyInfo, (IRedArray?)data);
        }
        else if (typeof(IRedEnum).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CEnumViewModel(parent, propertyInfo, (IRedEnum?)data);
        }
        else if (typeof(CString).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CStringViewModel(parent, propertyInfo, (CString)data!);
        }

        result ??= new DefaultPropertyViewModel(parent, propertyInfo, data);

        if (fetchData)
        {
            result.FetchProperties();
        }

        return result;
    }
}