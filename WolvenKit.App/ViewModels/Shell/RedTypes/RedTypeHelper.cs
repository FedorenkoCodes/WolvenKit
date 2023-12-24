using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WolvenKit.App.Controllers;
using WolvenKit.App.Services;
using WolvenKit.App.ViewModels.Documents;
using WolvenKit.Common.Services;
using WolvenKit.Core.Extensions;
using WolvenKit.Core.Interfaces;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class RedTypeHelper
{
    private readonly AppViewModel _appViewModel;
    private readonly ILoggerService _loggerService;
    private readonly ISettingsManager _settingsManager;
    private readonly IGameControllerFactory _gameControllerFactory;
    private readonly ITweakDBService _tweakDbService;

    public RedTypeHelper(AppViewModel appViewModel, ILoggerService loggerService, ISettingsManager settingsManager, IGameControllerFactory gameControllerFactory, ITweakDBService tweakDbService)
    {
        _appViewModel = appViewModel;
        _loggerService = loggerService;
        _settingsManager = settingsManager;
        _gameControllerFactory = gameControllerFactory;
        _tweakDbService = tweakDbService;
    }

    public RedTypeViewModel Create(IRedType data, bool fetchData = true, bool isReadOnly = false) => Create(null, new RedPropertyInfo(data), data, null, fetchData, isReadOnly);

    public RedTypeViewModel Create(RDTDataViewModel parentDataContext, IRedType data) => Create(null, new RedPropertyInfo(data), data, parentDataContext);

    private RedTypeViewModel InternalCreateRedTypeViewModel(RedTypeViewModel? parent, RedPropertyInfo propertyInfo, IRedType? data)
    {
        if (typeof(CMesh).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CMeshViewModel(parent, propertyInfo, (CMesh?)data);
        }
        if (typeof(worldStreamingSector).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new worldStreamingSectorViewModel(parent, propertyInfo, (worldStreamingSector?)data);
        }
        if (typeof(CResource).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CResourceViewModel(parent, propertyInfo, (CResource?)data);
        }
        if (typeof(HDRColor).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new HDRColorViewModel(parent, propertyInfo, (HDRColor?)data);
        }
        if (typeof(Vector4).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new Vector4ViewModel(parent, propertyInfo, (Vector4?)data);
        }
        if (typeof(RedBaseClass).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new RedBaseClassViewModel(parent, propertyInfo, (RedBaseClass?)data);
        }
        if (typeof(DataBuffer).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new DataBufferViewModel(parent, propertyInfo, (DataBuffer?)data);
        }
        if (typeof(SharedDataBuffer).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new SharedDataBufferViewModel(parent, propertyInfo, (SharedDataBuffer?)data);
        }
        if (typeof(SerializationDeferredDataBuffer).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new SerializationDeferredDataBufferViewModel(parent, propertyInfo, (SerializationDeferredDataBuffer?)data);
        }
        if (typeof(gamedataLocKeyWrapper).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new gamedataLocKeyWrapperViewModel(parent, propertyInfo, (gamedataLocKeyWrapper?)data);
        }
        if (typeof(IRedHandle).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CHandleViewModel(parent, propertyInfo, (IRedHandle?)data);
        }
        if (typeof(IRedWeakHandle).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CWeakHandleViewModel(parent, propertyInfo, (IRedWeakHandle?)data);
        }
        if (typeof(IRedArray).IsAssignableFrom(propertyInfo.BaseType))
        {
            if (propertyInfo.InnerType == typeof(CKeyValuePair))
            {
                return new CDictionaryViewModel(parent, propertyInfo, (CArray<CKeyValuePair>?)data);
            }

            return new CArrayViewModel(parent, propertyInfo, (IRedArray?)data);
        }
        if (typeof(IRedEnum).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CEnumViewModel(parent, propertyInfo, (IRedEnum?)data);
        }
        if (typeof(IRedRef).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new ResourceReferenceViewModel(parent, propertyInfo, (IRedRef?)data);
        }
        if (typeof(IRedLegacySingleChannelCurve).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CLegacySingleChannelCurveViewModel(parent, propertyInfo, (IRedLegacySingleChannelCurve?)data);
        }
        if (typeof(CName).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CNameViewModel(parent, propertyInfo, (CName)data!);
        }
        if (typeof(CString).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CStringViewModel(parent, propertyInfo, (CString)data!);
        }
        if (typeof(NodeRef).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new NodeRefViewModel(parent, propertyInfo, (NodeRef)data!);
        }
        if (typeof(TweakDBID).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new TweakDBIDViewModel(parent, propertyInfo, (TweakDBID)data!);
        }
        if (typeof(CBool).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CBoolViewModel(parent, propertyInfo, (CBool)data!);
        }
        if (typeof(CUInt8).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CUInt8ViewModel(parent, propertyInfo, (CUInt8)data!);
        }
        if (typeof(CInt8).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CInt8ViewModel(parent, propertyInfo, (CInt8)data!);
        }
        if (typeof(CUInt16).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CUInt16ViewModel(parent, propertyInfo, (CUInt16)data!);
        }
        if (typeof(CInt16).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CInt16ViewModel(parent, propertyInfo, (CInt16)data!);
        }
        if (typeof(CUInt32).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CUInt32ViewModel(parent, propertyInfo, (CUInt32)data!);
        }
        if (typeof(CInt32).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CInt32ViewModel(parent, propertyInfo, (CInt32)data!);
        }
        if (typeof(CUInt64).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CUInt64ViewModel(parent, propertyInfo, (CUInt64)data!);
        }
        if (typeof(CInt64).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CInt64ViewModel(parent, propertyInfo, (CInt64)data!);
        }
        if (typeof(CFloat).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CFloatViewModel(parent, propertyInfo, (CFloat)data!);
        }
        if (typeof(CDouble).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CDoubleViewModel(parent, propertyInfo, (CDouble)data!);
        }
        if (typeof(CDateTime).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CDateTimeViewModel(parent, propertyInfo, (CDateTime)data!);
        }
        if (typeof(CRUID).IsAssignableFrom(propertyInfo.BaseType))
        {
            return new CRUIDViewModel(parent, propertyInfo, (CRUID)data!);
        }

        return new DefaultPropertyViewModel(parent, propertyInfo, data);
    }

    public RedTypeViewModel Create(RedTypeViewModel? parent, RedPropertyInfo propertyInfo, IRedType? data, RDTDataViewModel? parentDataContext = null, bool fetchData = true, bool isReadOnly = false)
    {
        var result = InternalCreateRedTypeViewModel(parent, propertyInfo, data);

        result.IsReadOnly = isReadOnly;
        result.RedTypeHelper = this;
        result.RootContext = parentDataContext;

        if (fetchData)
        {
            result.Refresh(true);
        }

        return result;
    }

    // TODO: Remove this...
    public CArrayViewModel CreateQueryTree(TweakDBID queryId)
    {
        var arr = new CArrayViewModel(null, new RedPropertyInfo(typeof(CArray<TweakDBID>)), null);
        var query = TweakDBService.GetQuery(queryId).NotNull();
        for (var i = 0; i < query.Count; i++)
        {
            var tvm = new TweakDBIDViewModel(arr, new RedPropertyInfo(typeof(TweakDBID)), query[i]);
            tvm.RedTypeHelper = this;
            tvm.IsReadOnly = true;
            tvm.ArrayIndex = i;
            tvm.Refresh();

            arr.Properties.Add(tvm);
        }
        arr.RedTypeHelper = this;
        arr.IsReadOnly = true;
        arr.ShowProperties = true;
        arr.Refresh();

        return arr;
    }

    public AppViewModel GetAppViewModel() => _appViewModel;
    
    public ILoggerService GetLoggerService() => _loggerService;
    
    public IGameController GetGameController() => _gameControllerFactory.GetController();
    
    public async Task LoadTweakDB()
    {
        if (!_tweakDbService.IsLoaded)
        {
            var dbPath = Path.Combine(_settingsManager.GetRED4GameRootDir(), "r6", "cache", "tweakdb_ep1.bin");
            if (!File.Exists(dbPath))
            {
                dbPath = Path.Combine(_settingsManager.GetRED4GameRootDir(), "r6", "cache", "tweakdb.bin");
            }

            _loggerService.Info("Loading TweakDB...");
            await _tweakDbService.LoadDBAsync(dbPath);
            _loggerService.Info("TweakDB loaded");
        }
    }
}