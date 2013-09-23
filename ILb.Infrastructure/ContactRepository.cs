using System.Collections.Generic;
using System.Linq;
using ILB.Contacts;

namespace ILb.Infrastructure
{
    /// <summary>
    /// Your going to have to pretend this is an EF layer
    /// </summary>
    public class ContactRepository : IContactRepository
    {
        private static int id;
        private static readonly IList<Contact> Contacts = new List<Contact>();

        public void Save(Contact contact)
        {
            id++;
            contact.Id = id;
            Contacts.Add(contact);
        }

        public void PersistAll()
        {

        }

        public Contact GetById(int contactId)
        {
            return Contacts.FirstOrDefault(q => q.Id == contactId);
        }

        public IList<Contact> GetAll()
        {
            return Contacts.OrderBy(q => q.Surname).ToList();
        }
    }
}