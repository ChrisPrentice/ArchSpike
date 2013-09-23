using System.Collections.Generic;
using ILB.Contacts;

namespace ILB.ApplicationServices.Contacts
{
    public class ContactService : IHandleQuery<AllContactsQueryResult>, 
                                    IHandleQuery<UpdateContactQueryResult>,
                                    IHandleQuery<CreateContactQueryResult>,
                                    IHandleQuery<UpdateContactQuery, UpdateContactQueryResult>,
                                    IHandleCommand<CreateContactCommand, CreateContactQueryResult>,
                                    IHandleCommand<UpdateContactCommand, UpdateContactQueryResult>
    {
        private readonly IContactAdministrationService contactAdministrationService;
        private readonly IContactRepository contactRepository;
        private readonly ICountryRepository countryRepository;
        private readonly ICountyRepository countyRepository;
        private readonly IValidationService validationService;

        public ContactService(ICountyRepository countyRepository, ICountryRepository countryRepository,
                              IContactRepository contactRepository, IValidationService validationService,
                              IContactAdministrationService contactAdministrationService)
        {
            this.countyRepository = countyRepository;
            this.countryRepository = countryRepository;
            this.contactRepository = contactRepository;
            this.validationService = validationService;
            this.contactAdministrationService = contactAdministrationService;
        }

        AllContactsQueryResult IHandleQuery<AllContactsQueryResult>.Query()
        {
            return new AllContactsQueryResult
                    {
                        Contacts = contactRepository.GetAll()
                    };
        }


        CreateContactQueryResult IHandleQuery<CreateContactQueryResult>.Query()
        {
            IList<County> counties = countyRepository.GetAll();
            IList<Country> countries = countryRepository.GetAll();
            return new CreateContactQueryResult(counties, countries);
        }

        public UpdateContactQueryResult Query()
        {
            IList<County> counties = countyRepository.GetAll();
            IList<Country> countries = countryRepository.GetAll();
            return new UpdateContactQueryResult(counties, countries);
        }

        public CreateContactQueryResult Execute(CreateContactCommand createContactCommand)
        {
            if (validationService.Validate(createContactCommand).IsValid)
            {
                contactAdministrationService.Create(createContactCommand);
            }
            var counties = countyRepository.GetAll();
            var countries = countryRepository.GetAll();

            return new CreateContactQueryResult(counties, countries)
                {
                    Command = createContactCommand
                };
        }

        public UpdateContactQueryResult Query(UpdateContactQuery query)
        {
            IList<County> counties = countyRepository.GetAll();
            IList<Country> countries = countryRepository.GetAll();
            Contact contact = contactRepository.GetById(query.Id);
            return new UpdateContactQueryResult(counties, countries)
                {
                    Command = new UpdateContactCommand(contact)
                };
        }

        public UpdateContactQueryResult Execute(UpdateContactCommand updateCommand)
        {
            if (validationService.Validate(updateCommand).IsValid)
            {
                contactAdministrationService.Update(updateCommand);
            }

            var counties = countyRepository.GetAll();
            var countries = countryRepository.GetAll();

            return new UpdateContactQueryResult(counties, countries)
            {
                Command = updateCommand
            };
        }

    }
}