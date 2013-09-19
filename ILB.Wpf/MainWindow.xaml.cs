using System.Windows;
using ILB.ApplicationServices;
using ILB.Contacts;
using ILB.Infrastructure;
using ILb.Infrastructure;

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
            var contactService = new ContactService(new CountyRepository(), new CountryRepository(), new ContactRepository(), new ValidationService(), new ContactAdministrationService(new CountyRepository(), new CountryRepository(), new ContactRepository()));
            this.DataContext = new ContactEditorViewModel(contactService);
        }
    }
}
