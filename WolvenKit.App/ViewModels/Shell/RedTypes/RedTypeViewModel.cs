using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using WolvenKit.RED4.Types;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public abstract class RedTypeViewModel : INotifyPropertyChanging, INotifyPropertyChanged, IDisposable
{
    protected bool _isExpanded;

    protected IRedType? _dataObject;
    protected string _propertyName = "";
    protected string _displayType = "";
    protected string _displayValue = "";
    protected string _displayDescription = "";

    public RedTypeViewModel? Parent { get; }
    public RedPropertyInfo RedPropertyInfo { get; }

    public bool IsExpanded
    {
        get => _isExpanded;
        set => SetField(ref _isExpanded, value);
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

    internal int ArrayIndex { get; set; } = -1;

    public bool IsValueType { get; }

    public object Value => GetValue();

    public string ExtensionIcon { get; set; } = "SymbolClass";

    public ObservableCollection<RedTypeViewModel> Properties { get; } = new();

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

    public virtual IList<MenuItem> GetSupportedActions() => new List<MenuItem>();

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