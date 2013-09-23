namespace ILB.Contacts
{
    public class UpdateContactCommand : CreateContactCommand
    {
        public UpdateContactCommand()
        {
            
        }

        public UpdateContactCommand(Contact contact)
        {
            Id = contact.Id;
            FirstName = contact.FirstName;
            Surname = contact.Surname;
            Address1 = contact.Address1;
            Address2 = contact.Address2;
            CountyId = contact.County.Id;
            CountryId = contact.Country.Id;
        }

        public int Id { get; set; }
    }
}