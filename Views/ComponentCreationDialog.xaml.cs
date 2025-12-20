using System.Windows;
using SampleApp.ViewModels;

namespace SampleApp.Views;
/// <summary>
/// ComponentCreationDialog.xaml の相互作用ロジック
/// </summary>
public partial class ComponentCreationDialog
{
    public ComponentCreationDialog(ComponentCreationViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }

    private void OkButton_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }
}
