namespace ILB.Contacts
{
    public interface IContactAdministrationService
    {
        void Create(CreateContactCommand createContactCommand);
        void Update(UpdateContactCommand updateCommand);
    }
}