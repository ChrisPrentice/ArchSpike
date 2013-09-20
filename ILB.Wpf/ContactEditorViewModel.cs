using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ILB.ApplicationServices;
using ILB.Contacts;
using ILB.Wpf.Annotations;

namespace ILB.Wpf
{
    public class ContactEditorViewModel : INotifyPropertyChanged    
    {
        private readonly IContactService _contactService;
        private IList<Contact> _contacts;
        private CreateContactViewModel _currentCommand;
        private Contact _selectedContact;

        public ContactEditorViewModel(IContactService contactService)
        {
            _contactService = contactService;
            Contacts = contactService.GetAll();
            SaveCommand = new DelegateCommand(() =>
                {
                    contactService.CreateContact(CurrentCommand.Command);
                    Contacts = contactService.GetAll();
                });
            NewCommand = new DelegateCommand(() => CurrentCommand = new CreateContactViewModel(contactService.CreateContact(), new ValidationService()));
            NewCommand.Execute(null);
        }

        protected ICommand NewCommand { get; set; }

        public CreateContactViewModel CurrentCommand
        {
            get { return _currentCommand; }
            set
            {
                _currentCommand = value;
                OnPropertyChanged();
            }
        }

        public Contact SelectedContact
        {
            get { return _selectedContact; }
            set
            {
                _selectedContact = value;
                CurrentCommand = new CreateContactViewModel(_contactService.UpdateContact(_selectedContact.Id), new ValidationService());
                OnPropertyChanged();
            }
        }

        public DelegateCommand SaveCommand { get; set; }

        public IList<Contact> Contacts
        {
            get { return _contacts; }
            set
            {
                _contacts = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}