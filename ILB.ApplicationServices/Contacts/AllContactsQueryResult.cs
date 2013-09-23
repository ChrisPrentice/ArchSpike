using System.Collections.Generic;
using ILB.Contacts;

namespace ILB.ApplicationServices.Contacts
{
    public class AllContactsQueryResult
    {
        public IList<Contact> Contacts { get; set; }
    }
}