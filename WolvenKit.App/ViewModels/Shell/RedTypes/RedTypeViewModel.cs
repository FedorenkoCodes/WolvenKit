using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WolvenKit.App.ViewModels.Documents;
using WolvenKit.RED4.Types;
using static System.Net.Mime.MediaTypeNames;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public abstract class RedTypeViewModel : INotifyPropertyChanging, INotifyPropertyChanged, IDisposable
{
    protected bool _isExpanded;
    protected bool _isReadOnly;

    protected IRedType? _dataObject;
    protected string _propertyName = "";
    protected string _displayType = "";
    protected string _displayValue = "";
    protected string _displayDescription = "";

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
        set => SetField(ref _isReadOnly, value);
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

    public string XPath => BuildXPath();

    public int ArrayIndex { get; internal set; } = -1;

    public bool IsValueType { get; }

    public object Value => GetValue();

    public string ExtensionIcon { get; set; } = "SymbolClass";

    public ObservableCollection<RedTypeViewModel> Properties { get; protected set; } = new();

    public RedTypeViewModel(RedTypeViewModel? parent, RedPropertyInfo redPropertyInfo, IRedType? data)
    {
        Parent = parent;
        RedPropertyInfo = redPropertyInfo;
        _dataObject = data;

        DisplayType = RedPropertyInfo.RedTypeName;
        IsValueType = RedPropertyInfo.BaseType.IsValueType;
    }

    protected internal virtual void FetchProperties()
    {

    }

    protected internal virtual object GetValue() => new ObservableCollection<RedTypeViewModel> { this };

    protected internal virtual void UpdateDisplayValue()
    {

    }

    protected internal virtual void SetValue(RedTypeViewModel value) {}

    public virtual IList<MenuItem> GetSupportedActions()
    {
        var result = new List<MenuItem>();

        if (Parent is CArrayViewModel cArrayViewModel)
        {
            result.Add(CreateMenuItem("Remove item", RemoveItem_OnClick));

            void RemoveItem_OnClick(object sender, RoutedEventArgs routedEventArgs)
            {
                cArrayViewModel.RemoveItem(this);
            }
        }
        
        return result;
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

    #region INotifyProperty

    public event PropertyChangingEventHandler? PropertyChanging;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null) =>
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        if (propertyName == nameof(DataObject))
        {
            UpdateDisplayValue();

            Parent?.SetValue(this);
            Parent?.OnPropertyChanged(nameof(DataObject));
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

    protected MenuItem CreateMenuItem(string header, Action<object, RoutedEventArgs> onClick)
    {
        var menuItem = new MenuItem { Header = header };
        menuItem.Click += (sender, args) => onClick(sender, args);
        return menuItem;
    }

    protected RedTypeViewModel GetRootItem() => Parent != null ? Parent.GetRootItem() : this;

    protected void Select(RedTypeViewModel? selection = null)
    {
        var context = GetRootItem().RootContext;

        if (context != null && context.SelectedProperties != null)
        {
            selection ??= this;

            context.SelectedProperty = selection;
            context.SelectedProperties.Clear();
            context.SelectedProperties.Add(selection);
        }
    }

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

    public void Refresh()
    {
        FetchProperties();
        UpdateDisplayValue();
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