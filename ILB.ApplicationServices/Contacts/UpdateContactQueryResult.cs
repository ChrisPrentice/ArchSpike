using System.Collections.Generic;
using ILB.Contacts;

namespace ILB.ApplicationServices.Contacts
{
    public class UpdateContactQueryResult
    {
        public UpdateContactQueryResult(IList<County> counties, IList<Country> countries)
        {
            Counties = counties;
            Countries = countries;
            Command = new UpdateContactCommand();
        }

        public UpdateContactCommand Command { get; set; }
        public IList<County> Counties { get; set; }
        public IList<Country> Countries { get; set; }
    }
}