using System.Windows;
using Calculator.ViewModels;

namespace Calculator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private CalculatorViewModel ViewModel => (CalculatorViewModel)DataContext;

    public MainWindow()
    {
        InitializeComponent();

        DataContext = new CalculatorViewModel();
    }
}