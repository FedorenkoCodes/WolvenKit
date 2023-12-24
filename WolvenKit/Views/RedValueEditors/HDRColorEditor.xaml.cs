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
using ColorPicker.Models;
using WolvenKit.Views.Tools;

namespace WolvenKit.Views.RedValueEditors;
/// <summary>
/// Interaktionslogik für HDRColorEditor.xaml
/// </summary>
public partial class HDRColorEditor : UserControl
{
    public static readonly DependencyProperty ColorStateProperty = DependencyProperty.Register(
        nameof(ColorState), typeof(ColorState), typeof(HDRColorEditor));

    public ColorState ColorState
    {
        get => (ColorState)GetValue(ColorStateProperty);
        set => SetValue(ColorStateProperty, value);
    }
    
    public HDRColorEditor()
    {
        InitializeComponent();
    }
}
