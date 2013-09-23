using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ILB.ApplicationServices;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using ILB.Wpf.Annotations;

namespace ILB.Wpf
{
    public class CreateContactViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private readonly UpdateContactQueryResult createContact;
        private readonly IValidationService validationService;

        public CreateContactViewModel(UpdateContactQueryResult createContact, IValidationService validationService)
        {
            this.createContact = createContact;
            this.validationService = validationService;
        }

        public string FirstName
        {
            get { return createContact.Command.FirstName; }
            set
            {
                createContact.Command.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get { return createContact.Command.Surname; }
            set 
            {
                createContact.Command.Surname = value;
                OnPropertyChanged();
            }
        }

        public string Address1
        {
            get { return createContact.Command.Address1; }
            set { createContact.Command.Address1 = value; }
        }

        public string Address2
        {
            get { return createContact.Command.Address2; }
            set
            {
                createContact.Command.Address2 = value;
                OnPropertyChanged();
            }
        }

        public int CountyId
        {
            get { return createContact.Command.CountyId; }
            set { createContact.Command.CountyId = value; }
        }

        public int CountryId
        {
            get { return createContact.Command.CountryId; }
            set
            {
                createContact.Command.CountryId = value;
                OnPropertyChanged();
            }
        }

        public IList<County> Counties
        {
            get { return createContact.Counties; }
            set { createContact.Counties = value; }
        }

        public IList<Country> Countries
        {
            get { return createContact.Countries; }
            set { createContact.Countries = value; }
        }

        public string this[string columnName]
        {
            get
            {
                var result = validationService.Validate(createContact);
                var firstOrDefault = result.Results.FirstOrDefault(validationResult => validationResult.MemberNames.Any(m => m == columnName));
                Error = firstOrDefault == null ? "" : firstOrDefault.ErrorMessage;
                return Error;
            }
        }

        public string Error { get; private set; }

        public CreateContactCommand Command
        {
            get { return createContact.Command; }
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