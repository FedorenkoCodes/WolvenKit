using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using WolvenKit.App.ViewModels.Documents;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public abstract class RedTypeViewModel : INotifyPropertyChanging, INotifyPropertyChanged, IDisposable
{
    #region Fields

    protected bool _isExpanded;
    protected bool _isReadOnly;
    protected bool _isDefault;

    protected IRedType? _dataObject;
    protected string _propertyName = "";
    protected string _displayType = "";
    protected string _displayValue = "";
    protected string _displayDescription = "";

    #endregion Fields

    #region Propeties

    public RedTypeHelper RedTypeHelper { get; set; } = null!;

    public RedTypeViewModel? Parent { get; }
    public RedPropertyInfo RedPropertyInfo { get; }
    public RDTDataViewModel? RootContext { get; set; }

    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetField(ref _isExpanded, value);
    }

    public bool IsReadOnly
    {
        get => _isReadOnly;
        internal set => SetField(ref _isReadOnly, value);
    }

    public bool IsDefault
    {
        get => _isDefault;
        set => SetField(ref _isDefault, value);
    }

    public IRedType? DataObject
    {
        get => _dataObject;
        set => SetField(ref _dataObject, value);
    }

    public string PropertyName
    {
        get => _propertyName;
        set => SetField(ref _propertyName, value);
    }

    public string DisplayType
    {
        get => _displayType;
        protected set => SetField(ref _displayType, value);
    }

    public string DisplayValue
    {
        get => _displayValue;
        protected set => SetField(ref _displayValue, value);
    }

    public string DisplayDescription
    {
        get => _displayDescription;
        set => SetField(ref _displayDescription, value);
    }

    public string DisplayValueOrType => _displayValue != "" ? _displayValue : _displayType;
    
    public EDisplayFormat DisplayFormat
    {
        get
        {
            if (_displayValue != "")
            {
                if (!_isDefault)
                {
                    return EDisplayFormat.Value;
                }

                return EDisplayFormat.ValueDefault;
            }

            if (!_isDefault)
            {
                return EDisplayFormat.Type;
            }

            return EDisplayFormat.TypeDefault;
        }
    }

    public string XPath => BuildXPath();

    public int ArrayIndex { get; internal set; } = -1;

    public bool IsValueType { get; }

    public object Value => GetValue();

    public string ExtensionIcon { get; set; } = "SymbolClass";

    public ObservableCollection<RedTypeViewModel> Properties { get; protected set; } = new();

    #endregion Propeties

    #region Constructor

    public RedTypeViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedType? data)
    {
        Parent = parent;
        RedPropertyInfo = redPropertyInfo;
        _dataObject = data;

        DisplayType = RedPropertyInfo.RedTypeName;
        IsValueType = RedPropertyInfo.BaseType.IsValueType;
    }

    #endregion Constructor

    #region Methods

    protected internal virtual object GetValue() => new ObservableCollection<RedTypeViewModel> { this };

    protected internal virtual void SetValue(RedTypeViewModel value) { }

    protected internal virtual void FetchProperties()
    {

    }

    protected internal virtual void UpdateDisplayValue()
    {

    }

    protected internal virtual void UpdateDisplayDescription()
    {
        var propNames = new[] { "name" };

        foreach (var name in propNames)
        {
            var property = Properties.FirstOrDefault(x => x.PropertyName == name);
            if (property != null)
            {
                DisplayDescription = property.DisplayValue;
                break;
            }
        }
    }

    protected internal virtual void UpdateIsDefault()
    {
        if (RedPropertyInfo.ExtendedPropertyInfo != null)
        {
            IsDefault = RedPropertyInfo.ExtendedPropertyInfo.IsDefault(DataObject);
        }
    }

    public string BuildXPath()
    {
        var parts = new List<string>();

        var redTypeViewModel = this;
        do
        {
            parts.Add(redTypeViewModel.PropertyName);
            redTypeViewModel = redTypeViewModel.Parent;
        } while (redTypeViewModel != null);

        parts.Reverse();

        return string.Join('\\', parts);
    }

    protected RedTypeViewModel? GetPropertyByName(string name) => Properties.FirstOrDefault(x => x.PropertyName == name);

    protected RedTypeViewModel? GetPropertyFromPath(string path)
    {
        var parts = path.Split('.');

        var result = this;
        foreach (var part in parts)
        {
            var newResult = result.Properties.FirstOrDefault(x => x.PropertyName == part);
            if (newResult == null)
            {
                return null;
            }

            result = newResult;
        }

        return result;
    }

    public IEnumerable<RedTypeViewModel> GetAllProperties()
    {
        foreach (var child in Properties)
        {
            yield return child;

            foreach (var childProperty in child.GetAllProperties())
            {
                yield return childProperty;
            }
        }
    }

    public virtual IList<KeyValuePair<string, Action>> GetSupportedActions()
    {
        var result = new List<KeyValuePair<string, Action>>();

        if (!IsReadOnly && Parent is CArrayViewModel cArrayViewModel)
        {
            result.Add(new KeyValuePair<string, Action>("Remove item", RemoveItemOnClick));

            void RemoveItemOnClick()
            {
                cArrayViewModel.RemoveItem(this);
            }
        }

        return result;
    }

    protected RedTypeViewModel GetRootItem() => Parent != null ? Parent.GetRootItem() : this;

    protected void Select(RedTypeViewModel? selection = null)
    {
        var context = GetRootItem().RootContext;

        if (context is { SelectedProperties: not null })
        {
            selection ??= this;

            context.SelectedProperty = selection;
            context.SelectedProperties.Clear();
            context.SelectedProperties.Add(selection);
        }
    }

    public void Refresh(bool refreshProperties = false)
    {
        if (refreshProperties)
        {
            FetchProperties();
        }

        UpdateIsDefault();
        UpdateDisplayValue();
        UpdateDisplayDescription();
    }

    #endregion Methods

    #region INotifyProperty

    public event PropertyChangingEventHandler? PropertyChanging;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null) =>
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (propertyName == nameof(DataObject))
        {
            Refresh();

            Parent?.SetValue(this);

            if (RootContext is { Parent: { } document })
            {
                document.SetIsDirty(true);
            }
        }

        if (propertyName == nameof(DisplayValue) || propertyName == nameof(DisplayType))
        {
            OnPropertyChanged(nameof(DisplayValueOrType));
            OnPropertyChanged(nameof(DisplayFormat));
        }

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return false;
        }

        OnPropertyChanging(propertyName);
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    #endregion INotifyProperty

    #region IDisposable

    protected bool _disposedValue;

    ~RedTypeViewModel() => Dispose(false);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                foreach (var propertyViewModel in Properties)
                {
                    propertyViewModel.Dispose();
                }
            }

            _disposedValue = true;
        }
    }

    #endregion IDisposable
}

public abstract class RedTypeViewModel<TRedType> : RedTypeViewModel where TRedType : IRedType?
{
    protected TRedType? _data => (TRedType?)DataObject;

    protected RedTypeViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, TRedType? data) : base(parent, redPropertyInfo, data)
    {
    }
}