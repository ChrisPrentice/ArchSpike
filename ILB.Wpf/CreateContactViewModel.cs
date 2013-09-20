using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ILB.Contacts;
using ILB.Wpf.Annotations;

namespace ILB.Wpf
{
    public class CreateContactViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private readonly CreateContactCommand _createContact;
        private readonly IValidationService _validationService;

        public CreateContactViewModel(CreateContactCommand createContact, IValidationService validationService)
        {
            _createContact = createContact;
            _validationService = validationService;
        }

        public string FirstName
        {
            get { return _createContact.FirstName; }
            set
            {
                _createContact.FirstName = value;
                this.OnPropertyChanged();
            }
        }

        public string Surname
        {
            get { return _createContact.Surname; }
            set 
            {
                _createContact.Surname = value;
                this.OnPropertyChanged();
            }
        }

        public string Address1
        {
            get { return _createContact.Address1; }
            set { _createContact.Address1 = value; }
        }

        public string Address2
        {
            get { return _createContact.Address2; }
            set
            {
                _createContact.Address2 = value;
                this.OnPropertyChanged();
            }
        }

        public int CountyId
        {
            get { return _createContact.CountyId; }
            set { _createContact.CountyId = value; }
        }

        public int CountryId
        {
            get { return _createContact.CountryId; }
            set
            {
                _createContact.CountryId = value;
                this.OnPropertyChanged();
            }
        }

        public IList<County> Counties
        {
            get { return _createContact.Counties; }
            set { _createContact.Counties = value; }
        }

        public IList<Country> Countries
        {
            get { return _createContact.Countries; }
            set { _createContact.Countries = value; }
        }

        public string this[string columnName]
        {
            get
            {
                var result = _validationService.Validate(_createContact);
                var firstOrDefault = result.Results.FirstOrDefault(validationResult => validationResult.MemberNames.Any(m => m == columnName));
                return firstOrDefault == null ? "" : firstOrDefault.ErrorMessage;
            }
        }

        public string Error { get; private set; }

        public CreateContactCommand Command
        {
            get { return _createContact; }
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