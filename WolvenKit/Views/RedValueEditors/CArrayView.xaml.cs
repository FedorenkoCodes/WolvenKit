using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WolvenKit.App.ViewModels.Shell.RedTypes;

namespace WolvenKit.Views.RedValueEditors;
/// <summary>
/// Interaktionslogik für CArrayView.xaml
/// </summary>
public partial class CArrayView : UserControl
{
    public CArrayView()
    {
        InitializeComponent();
    }

    private void DeleteAllItems_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not CArrayViewModel vm)
        {
            return;
        }

        vm.Clear();
    }
}
