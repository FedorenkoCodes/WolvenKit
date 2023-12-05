using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class RedTypeHelper
{
    private readonly AppViewModel _appViewModel;

    public RedTypeHelper(AppViewModel appViewModel)
    {
        _appViewModel = appViewModel;
    }

    public RedTypeViewModel Create(IRedType data) => Create(null, new RedPropertyInfo(data), data);

    public RedTypeViewModel Create(RedTypeViewModel? parent, RedPropertyInfo propertyInfo, IRedType? data, bool fetchData = true)
    {
        RedTypeViewModel? result = null;

        if (typeof(CMesh).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CMeshViewModel(parent, propertyInfo, (CMesh?)data);
        }
        else if (typeof(CResource).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CResourceViewModel(parent, propertyInfo, (CResource?)data);
        }
        else if (typeof(Vector4).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new Vector4ViewModel(parent, propertyInfo, (Vector4?)data);
        }
        else if (typeof(RedBaseClass).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new RedBaseClassViewModel(parent, propertyInfo, (RedBaseClass?)data);
        }
        else if (typeof(IRedHandle).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CHandleViewModel(parent, propertyInfo, (IRedHandle?)data);
            ((CHandleViewModel)result).AppViewModel = _appViewModel;
            ((CHandleViewModel)result).RedTypeHelper = this;
        }
        else if (typeof(IRedArray).IsAssignableFrom(propertyInfo.BaseType))
        {
            if (propertyInfo.InnerType == typeof(CKeyValuePair))
            {
                result = new CDictionaryViewModel(parent, propertyInfo, (CArray<CKeyValuePair>?)data);
            }
            else
            {
                result = new CArrayViewModel(parent, propertyInfo, (IRedArray?)data);
            }

            ((CArrayViewModel)result).AppViewModel = _appViewModel;
            ((CArrayViewModel)result).RedTypeHelper = this;
        }
        else if (typeof(IRedEnum).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CEnumViewModel(parent, propertyInfo, (IRedEnum?)data);
        }
        else if (typeof(IRedRef).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new ResourceReferenceViewModel(parent, propertyInfo, (IRedRef?)data);
        }
        else if (typeof(IRedLegacySingleChannelCurve).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CLegacySingleChannelCurveViewModel(parent, propertyInfo, (IRedLegacySingleChannelCurve?)data);
            ((CLegacySingleChannelCurveViewModel)result).RedTypeHelper = this;
        }
        else if (typeof(CName).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CNameViewModel(parent, propertyInfo, (CName)data!);
        }
        else if (typeof(CString).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CStringViewModel(parent, propertyInfo, (CString)data!);
        }
        else if (typeof(CBool).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CBoolViewModel(parent, propertyInfo, (CBool)data!);
        }
        else if (typeof(CUInt8).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CUInt8ViewModel(parent, propertyInfo, (CUInt8)data!);
        }
        else if (typeof(CInt8).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CInt8ViewModel(parent, propertyInfo, (CInt8)data!);
        }
        else if (typeof(CUInt16).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CUInt16ViewModel(parent, propertyInfo, (CUInt16)data!);
        }
        else if (typeof(CInt16).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CInt16ViewModel(parent, propertyInfo, (CInt16)data!);
        }
        else if (typeof(CUInt32).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CUInt32ViewModel(parent, propertyInfo, (CUInt32)data!);
        }
        else if (typeof(CInt32).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CInt32ViewModel(parent, propertyInfo, (CInt32)data!);
        }
        else if (typeof(CUInt64).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CUInt64ViewModel(parent, propertyInfo, (CUInt64)data!);
        }
        else if (typeof(CInt64).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CInt64ViewModel(parent, propertyInfo, (CInt64)data!);
        }
        else if (typeof(CFloat).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CFloatViewModel(parent, propertyInfo, (CFloat)data!);
        }
        else if (typeof(CDouble).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CDoubleViewModel(parent, propertyInfo, (CDouble)data!);
        }
        else if (typeof(CDateTime).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CDateTimeViewModel(parent, propertyInfo, (CDateTime)data!);
        }
        else if (typeof(CRUID).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CRUIDViewModel(parent, propertyInfo, (CRUID)data!);
        }

        result ??= new DefaultPropertyViewModel(parent, propertyInfo, data);

        if (result is IRedBaseClassViewModel redBaseClassViewModel)
        {
            redBaseClassViewModel.RedTypeHelper = this;
        }

        if (fetchData)
        {
            result.FetchProperties();
            result.UpdateDisplayValue();
        }

        return result;
    }
}