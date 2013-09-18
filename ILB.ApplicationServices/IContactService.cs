using System.Collections.Generic;
using ILB.Contacts;

namespace ILB.ApplicationServices
{
    public interface IContactService
    {
        IList<Contact> GetAll();
        CreateContactCommand CreateContact();
        CreateContactCommand CreateContact(CreateContactCommand createContactCommand);
        UpdateContactCommand UpdateContact(int contactId);
        UpdateContactCommand UpdateContact(UpdateContactCommand updateCommand);
    }
}