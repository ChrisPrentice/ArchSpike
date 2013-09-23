using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ILB.ApplicationServices;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using ILB.Wpf.Annotations;

namespace ILB.Wpf
{
    public class ContactEditorViewModel : INotifyPropertyChanged    
    {
        private readonly IQueryInvoker queryInvoker;
        private IList<Contact> contacts;
        private CreateContactViewModel currentContact;
        private Contact selectedContact;

        public ContactEditorViewModel(ICommandInvoker commandInvoker, IQueryInvoker queryInvoker)
        {
            this.queryInvoker = queryInvoker;
            Contacts = queryInvoker.Query<AllContactsQueryResult>().Contacts;

            SaveCommand = new DelegateCommand(() =>
                {
                    if (CurrentContact.Command is UpdateContactCommand)
                    {
                        commandInvoker.Execute<UpdateContactCommand, UpdateContactQueryResult>((UpdateContactCommand) CurrentContact.Command);
                    }
                    else
                    {
                        commandInvoker.Execute<CreateContactCommand, CreateContactQueryResult>(CurrentContact.Command);
                    }

                    Contacts = queryInvoker.Query<AllContactsQueryResult>().Contacts;
                });

            NewCommand = new DelegateCommand(() =>
                {
                    var modifyContactQueryResult = queryInvoker.Query<UpdateContactQueryResult>();
                    CurrentContact = new CreateContactViewModel(modifyContactQueryResult, new ValidationService());
                });
            NewCommand.Execute(null);
        }

        public ICommand NewCommand { get; set; }

        public CreateContactViewModel CurrentContact
        {
            get { return currentContact; }
            set
            {
                currentContact = value;
                OnPropertyChanged();
            }
        }

        public Contact SelectedContact
        {
            get { return selectedContact; }
            set
            {
                selectedContact = value;
                var modifyContactQueryResult = queryInvoker.Query<UpdateContactQuery, UpdateContactQueryResult>(new UpdateContactQuery {Id = selectedContact.Id});
                CurrentContact = new CreateContactViewModel(modifyContactQueryResult, new ValidationService());
                OnPropertyChanged();
            }
        }

        public DelegateCommand SaveCommand { get; set; }

        public IList<Contact> Contacts
        {
            get { return contacts; }
            set
            {
                contacts = value;
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