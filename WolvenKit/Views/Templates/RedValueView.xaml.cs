using System.Windows;
using System.Windows.Controls;
using Syncfusion.UI.Xaml.Grid;
using WolvenKit.App.ViewModels.Shell.RedTypes;

namespace WolvenKit.Views.Templates
{
    /// <summary>
    /// Interaktionslogik für RedValueView.xaml
    /// </summary>
    public partial class RedValueView : UserControl
    {
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
            nameof(ItemSource), typeof(RedTypeViewModel), typeof(RedValueView));

        public RedTypeViewModel ItemSource
        {
            get => (RedTypeViewModel)GetValue(ItemSourceProperty);
            set => SetValue(ItemSourceProperty, value);
        }

        public RedValueView()
        {
            InitializeComponent();
        }
    }
}
