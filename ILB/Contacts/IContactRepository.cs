using System.Collections.Generic;

namespace ILB.Contacts
{
    public interface IContactRepository
    {
        void Save(Contact contact);
        void PersistAll();
        Contact GetById(int contactId);
        IList<Contact> GetAll();
    }
}
