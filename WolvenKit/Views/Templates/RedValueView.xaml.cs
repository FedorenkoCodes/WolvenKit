using System.Windows;
using System.Windows.Controls;
using WolvenKit.App.ViewModels.Shell.RedTypes;

namespace WolvenKit.Views.Templates
{
    /// <summary>
    /// Interaktionslogik für RedValueView.xaml
    /// </summary>
    public partial class RedValueView : UserControl
    {
        public static readonly DependencyProperty ItemSourceProperty = DependencyProperty.Register(
            nameof(ItemSource), typeof(RedTypeViewModel), typeof(RedValueView), new PropertyMetadata(null, OnItemSourceChanged));

        private static void OnItemSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not RedValueView view)
            {
                return;
            }
        }

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
