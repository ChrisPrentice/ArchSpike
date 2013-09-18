namespace ILB.Contacts
{
    public class ContactAdministrationService : IContactAdministrationService
    {
        private readonly ICountyRepository _countyRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IContactRepository _contactRepository;

        public ContactAdministrationService(ICountyRepository countyRepository, ICountryRepository countryRepository, IContactRepository contactRepository)
        {
            _countyRepository = countyRepository;
            _countryRepository = countryRepository;
            _contactRepository = contactRepository;
        }

        public void Create(CreateContactCommand createContactCommand)
        {
            var contact = new Contact(createContactCommand);
            contact.County = _countyRepository.GetById(createContactCommand.CountyId);
            contact.Country = _countryRepository.GetById(createContactCommand.CountyId);
            _contactRepository.Save(contact);
        }

        public void Update(UpdateContactCommand updateCommand)
        {
            var contact = _contactRepository.GetById(updateCommand.Id);
            contact.Update(updateCommand);
            contact.County = _countyRepository.GetById(updateCommand.CountyId);
            contact.Country = _countryRepository.GetById(updateCommand.CountyId);
            _contactRepository.PersistAll();
        }
    }
}