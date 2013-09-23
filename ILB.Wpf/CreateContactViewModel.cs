using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using ILB.Wpf.Annotations;

namespace ILB.Wpf
{
    public class CreateContactViewModel : IDataErrorInfo, INotifyPropertyChanged
    {
        private readonly CreateContactCommand command;
        private readonly IValidationService validationService;
        private IList<Country> countries;
        private IList<County> counties;

        public CreateContactViewModel(CreateContactQueryResult createContact, IValidationService validationService)
        {
            this.command = createContact.Command;
            this.Counties = createContact.Counties;
            this.Countries = createContact.Countries;
            this.validationService = validationService;
        }

        public CreateContactViewModel(UpdateContactQueryResult createContact, IValidationService validationService)
        {
            this.command = createContact.Command;
            this.Counties = createContact.Counties;
            this.Countries = createContact.Countries;
            this.validationService = validationService;
        }

        public string FirstName
        {
            get { return command.FirstName; }
            set
            {
                command.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get { return command.Surname; }
            set 
            {
                command.Surname = value;
                OnPropertyChanged();
            }
        }

        public string Address1
        {
            get { return command.Address1; }
            set { command.Address1 = value; }
        }

        public string Address2
        {
            get { return command.Address2; }
            set
            {
                command.Address2 = value;
                OnPropertyChanged();
            }
        }

        public int CountyId
        {
            get { return command.CountyId; }
            set { command.CountyId = value; }
        }

        public int CountryId
        {
            get { return command.CountryId; }
            set
            {
                command.CountryId = value;
                OnPropertyChanged();
            }
        }

        public IList<County> Counties
        {
            get { return counties; }
            set 
            {
                counties = value;
                OnPropertyChanged();
            }
        }

        public IList<Country> Countries
        {
            get { return countries; }
            set
            {
                countries = value;
                OnPropertyChanged();
            }
        }

        public string this[string columnName]
        {
            get
            {
                var result = validationService.Validate(command);
                var firstOrDefault = result.Results.FirstOrDefault(validationResult => validationResult.MemberNames.Any(m => m == columnName));
                Error = firstOrDefault == null ? "" : firstOrDefault.ErrorMessage;
                return Error;
            }
        }

        public string Error { get; private set; }

        public CreateContactCommand Command
        {
            get { return command; }
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