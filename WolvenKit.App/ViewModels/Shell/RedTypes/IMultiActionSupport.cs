using System.Collections.Generic;
using System.Windows.Controls;

namespace WolvenKit.App.ViewModels.Shell.RedTypes;

public interface IMultiActionSupport
{
    public IList<MenuItem> GetSupportedMultiActions(IList<object> selectedItems);
}