using WolvenKit.App.ViewModels.Documents;
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

    public RedTypeViewModel Create(RDTDataViewModel parentDataContext, IRedType data) => Create(null, new RedPropertyInfo(data), data, parentDataContext);

    public T? Create<T>(RDTDataViewModel parentDataContext, IRedType data) where T : RedTypeViewModel => default;

    public RedTypeViewModel Create(RedTypeViewModel? parent, RedPropertyInfo propertyInfo, IRedType? data, RDTDataViewModel? parentDataContext = null, bool fetchData = true)
    {
        RedTypeViewModel? result = null;

        if (typeof(CMesh).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CMeshViewModel(parent, propertyInfo, (CMesh?)data);
        }
        if (typeof(worldStreamingSector).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new worldStreamingSectorViewModel(parent, propertyInfo, (worldStreamingSector?)data);
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
        else if (typeof(DataBuffer).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new DataBufferViewModel(parent, propertyInfo, (DataBuffer?)data);
        }
        else if (typeof(SharedDataBuffer).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new SharedDataBufferViewModel(parent, propertyInfo, (SharedDataBuffer?)data);
        }
        else if (typeof(gamedataLocKeyWrapper).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new gamedataLocKeyWrapperViewModel(parent, propertyInfo, (gamedataLocKeyWrapper?)data);
        }
        else if (typeof(IRedHandle).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CHandleViewModel(parent, propertyInfo, (IRedHandle?)data);
        }
        else if (typeof(IRedWeakHandle).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CWeakHandleViewModel(parent, propertyInfo, (IRedWeakHandle?)data);
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
        }
        else if (typeof(CName).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CNameViewModel(parent, propertyInfo, (CName)data!);
        }
        else if (typeof(CString).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new CStringViewModel(parent, propertyInfo, (CString)data!);
        }
        else if (typeof(NodeRef).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new NodeRefViewModel(parent, propertyInfo, (NodeRef)data!);
        }
        else if (typeof(TweakDBID).IsAssignableFrom(propertyInfo.BaseType))
        {
            result = new TweakDBIDViewModel(parent, propertyInfo, (TweakDBID)data!);
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

        result.RedTypeHelper = this;
        result.RootContext = parentDataContext;

        if (fetchData)
        {
            result.FetchProperties();
            result.UpdateDisplayValue();
            result.UpdateDisplayDescription();
            result.UpdateIsDefault();
        }

        return result;
    }

    public AppViewModel GetAppViewModel() => _appViewModel;
}