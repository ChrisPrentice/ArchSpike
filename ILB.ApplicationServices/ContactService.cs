using System.Collections.Generic;
using ILB.Contacts;

namespace ILB.ApplicationServices
{
    public class ContactService : IContactService
    {
        private readonly ICountyRepository _countyRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IContactRepository _contactRepository;
        private readonly IValidationService _validationService;
        private readonly IContactAdministrationService _contactAdministrationService;

        public ContactService(ICountyRepository countyRepository, ICountryRepository countryRepository, IContactRepository contactRepository, IValidationService validationService, IContactAdministrationService contactAdministrationService)
        {
            _countyRepository = countyRepository;
            _countryRepository = countryRepository;
            _contactRepository = contactRepository;
            _validationService = validationService;
            _contactAdministrationService = contactAdministrationService;
        }

        public IList<Contact> GetAll()
        {
            return _contactRepository.GetAll();
        }

        public CreateContactCommand CreateContact()
        {
            var counties = _countyRepository.GetAll();
            var countries = _countryRepository.GetAll();
            return new CreateContactCommand(counties, countries);
        }

        public CreateContactCommand CreateContact(CreateContactCommand createContactCommand)
        {
            if (_validationService.Validate(createContactCommand))
            {
                _contactAdministrationService.Create(createContactCommand);
            }

            createContactCommand.Counties = _countyRepository.GetAll();
            createContactCommand.Countries = _countryRepository.GetAll();
            return createContactCommand;
        }

        public UpdateContactCommand UpdateContact(int contactId)
        {
            var counties = _countyRepository.GetAll();
            var countries = _countryRepository.GetAll();
            var contact = _contactRepository.GetById(contactId);
            return new UpdateContactCommand(contact, counties, countries);
        }

        public UpdateContactCommand UpdateContact(UpdateContactCommand updateCommand)
        {
            if (_validationService.Validate(updateCommand))
            {
                _contactAdministrationService.Update(updateCommand);
                return updateCommand;
            }

            updateCommand.Counties = _countyRepository.GetAll();
            updateCommand.Countries = _countryRepository.GetAll();
            return updateCommand;
        }
    }
}