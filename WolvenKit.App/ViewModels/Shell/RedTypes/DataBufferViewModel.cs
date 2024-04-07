using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using WolvenKit.RED4.Archive.Buffer;
using WolvenKit.RED4.Archive.IO;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class BaseBufferViewModel<T> : RedTypeViewModel<T> where T : IRedBufferWrapper
{
    public BaseBufferViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, T? data) : base(parent, redPropertyInfo, data)
    {
    }

    public override IList<ContextMenuItem> GetSupportedActions()
    {
        var result = base.GetSupportedActions();

        result.Insert(0, new ContextMenuItem("Save Buffer To Disk", SaveBufferToDisk));

        if (!IsReadOnly)
        {
            result.Insert(1, new ContextMenuItem("Load Buffer From Disk", LoadBufferFromDisk));
        }

        return result;
    }

    private void SaveBufferToDisk()
    {
        if (_data == null || _data.Buffer.MemSize == 0)
        {
            return;
        }

        var dlg = new SaveFileDialog
        {
            FileName = $"{PropertyName}.bin",
            Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*"
        };
        if (dlg.ShowDialog() == true)
        {
            File.WriteAllBytes(dlg.FileName, _data.Buffer.GetBytes());
        }
    }

    private void LoadBufferFromDisk()
    {
        var dlg = new OpenFileDialog()
        {
            FileName = "buffer.bin",
            Filter = "bin files (*.bin)|*.bin|All files (*.*)|*.*"
        };
        if (dlg.ShowDialog() != true)
        {
            return;
        }

        DataObject ??= new DataBuffer();

        _data!.Buffer.SetBytes(File.ReadAllBytes(dlg.FileName));

        var reader = _data.Buffer.CreateReader();
        if (reader != null)
        {
            if (reader is RedPackageReader pReader)
            {
                var rootType = GetRootItem().DataObject!.GetType();

                if (rootType == typeof(gamePersistentStateDataResource))
                {
                    pReader.Settings.RedPackageType = RedPackageType.SaveResource;
                }
                else if (rootType == typeof(inkWidgetLibraryResource))
                {
                    pReader.Settings.RedPackageType = RedPackageType.InkLibResource;
                }
                else if (rootType == typeof(appearanceAppearanceResource))
                {
                    pReader.Settings.ImportsAsHash = true;
                }

                //pReader.LoggerService = LoggerService;
            }

            reader.ReadBuffer(_data!.Buffer);

            // TODO[RTVM]: Only for meshMeshMaterialBuffer.rawData, refactor
            Parent?.Refresh(true);
        }
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data?.Data?.Data == null)
        {
            return;
        }

        Properties.Add(RedTypeHelper.Create(this, new RedPropertyInfo(_data.Data.Data), _data.Data.Data));
    }
}

public class DataBufferViewModel : BaseBufferViewModel<DataBuffer>
{
    public DataBufferViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, DataBuffer? data) : base(parent, redPropertyInfo, data)
    {
    }
}

public class SharedDataBufferViewModel : BaseBufferViewModel<SharedDataBuffer>
{
    public SharedDataBufferViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, SharedDataBuffer? data) : base(parent, redPropertyInfo, data)
    {
    }
}

public class SerializationDeferredDataBufferViewModel : BaseBufferViewModel<SerializationDeferredDataBuffer>
{
    public SerializationDeferredDataBufferViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, SerializationDeferredDataBuffer? data) : base(parent, redPropertyInfo, data)
    {
    }
}