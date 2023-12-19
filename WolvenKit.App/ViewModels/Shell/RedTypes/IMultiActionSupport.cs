using System;
using System.Collections.Generic;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public interface IMultiActionSupport
{
    public IList<KeyValuePair<string, Action<IList<object>>>> GetSupportedMultiActions();
}