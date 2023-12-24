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
using WolvenKit.RED4.Types;
using WolvenKit.Views.Editors;

namespace WolvenKit.Views.RedValueEditors;
/// <summary>
/// Interaktionslogik für CLegacySingleChannelCurveView.xaml
/// </summary>
public partial class CLegacySingleChannelCurveView : UserControl
{
    public CLegacySingleChannelCurveView()
    {
        InitializeComponent();
    }

    private void CurveEditorButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not CLegacySingleChannelCurveViewModel context)
        {
            return;
        }

        if (context.DataObject is not IRedLegacySingleChannelCurve data)
        {
            data = (IRedLegacySingleChannelCurve)RedTypeManager.CreateRedType(context.RedPropertyInfo.GetFullType());
        }

        var curveEditorWindow = new CurveEditorWindow(data);
        var r = curveEditorWindow.ShowDialog();
        if (r ?? true)
        {
            var c = curveEditorWindow.GetCurve();
            if (c is not null)
            {
                if (c.Points.Count == 0)
                {
                    context.DataObject = null;
                }
                else
                {
                    data.InterpolationType = c.Type;

                    data.Clear();
                    foreach (var point in c.Points)
                    {
                        data.Add((float)point.Item1, point.Item2);
                    }

                    context.DataObject = data;
                }

                context.Refresh();
            }
        }
    }
}
