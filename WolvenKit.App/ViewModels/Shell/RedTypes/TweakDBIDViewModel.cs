using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WolvenKit.App.Models;
using WolvenKit.Common.Services;
using WolvenKit.Core.Extensions;
using WolvenKit.RED4.TweakDB;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public class TweakDBIDViewModel : RedTypeViewModel<TweakDBID>
{
    public string? BindingValue
    {
        get => _data.GetResolvedText();
        set => DataObject = (TweakDBID)value!;
    }

    public TweakDBIDViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, TweakDBID data) : base(parent, redPropertyInfo, data)
    {
    }

    protected internal override void FetchProperties()
    {
        Properties.Clear();

        if (_data == TweakDBID.Empty)
        {
            return;
        }

        if (!_data.IsResolvable || !_data.ResolvedText!.Contains("_inline"))
        {
            return;
        }

        if (TweakDBService.GetRecord(_data) is { } record)
        {
            var typeInfo = RedReflection.GetTypeInfo(record);

            foreach (var propertyInfo in typeInfo.PropertyInfos.OrderBy(x => x.RedName))
            {
                ArgumentNullException.ThrowIfNull(propertyInfo.RedName);

                var entry = RedTypeHelper.Create(this, new RedPropertyInfo(propertyInfo), record.GetProperty(propertyInfo.RedName), null, true, true);
                entry.PropertyName = propertyInfo.RedName;

                Properties.Add(entry);
            }
        }

        if (TweakDBService.GetFlat(_data) is { } flat)
        {
            Properties.Add(RedTypeHelper.Create(this, new RedPropertyInfo(flat), flat));
        }
    }

    protected internal override void UpdateDisplayValue() => DisplayValue = _data.ToString();

    public override IList<KeyValuePair<string, Action>> GetSupportedActions()
    {
        var result = base.GetSupportedActions();

        result.Insert(0, new KeyValuePair<string, Action>("Override Value", CreateTXLOverride));
        result.Insert(1, new KeyValuePair<string, Action>("Copy to Clipboard", CopyTXLOverride));

        return result;
    }

    private async void CreateTXLOverride()
    {
        var txl = await GetTXL();
    }

    private async void CopyTXLOverride()
    {
        var txl = await GetTXL();
    }

    private async Task<TweakXL?> GetTXL()
    {
        if (_data == TweakDBID.Empty)
        {
            return null;
        }

        await RedTypeHelper.LoadTweakDB();
        
        var tdbEntry = TweakDBService.GetFlat(_data);
        tdbEntry ??= TweakDBService.GetRecord(_data);

        var txl = new TweakXL { ID = _data };

        if (TweakDBService.TryGetType(_data, out var type))
        {
            txl.Type = type.Name;
        }

        if (tdbEntry is gamedataTweakDBRecord record)
        {
            record.GetPropertyNames().ForEach(name => txl.Properties.Add(name, record.GetProperty(name).NotNull()));
        }

        return txl;
    }
}