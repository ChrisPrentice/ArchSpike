using System.Windows;
using ILB.Infrastructure;

namespace ILB.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ContactEditorViewModel(new CommandInvoker(), new QueryInvoker());
        }
    }
}
