namespace ILB.Contacts
{
    public class ContactAdministrationService : IContactAdministrationService
    {
        private readonly ICountyRepository countyRepository;
        private readonly ICountryRepository countryRepository;
        private readonly IContactRepository contactRepository;

        public ContactAdministrationService(ICountyRepository countyRepository, ICountryRepository countryRepository, IContactRepository contactRepository)
        {
            this.countyRepository = countyRepository;
            this.countryRepository = countryRepository;
            this.contactRepository = contactRepository;
        }

        public void Create(CreateContactCommand createContactCommand)
        {
            var contact = new Contact(createContactCommand);
            contact.County = countyRepository.GetById(createContactCommand.CountyId);
            contact.Country = countryRepository.GetById(createContactCommand.CountyId);
            contactRepository.Save(contact);
        }

        public void Update(UpdateContactCommand updateCommand)
        {
            var contact = contactRepository.GetById(updateCommand.Id);
            contact.Update(updateCommand);
            contact.County = countyRepository.GetById(updateCommand.CountyId);
            contact.Country = countryRepository.GetById(updateCommand.CountyId);
            contactRepository.PersistAll();
        }
    }
}