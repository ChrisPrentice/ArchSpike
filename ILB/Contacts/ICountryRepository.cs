using System.Collections.Generic;

namespace ILB.Contacts
{
    public interface ICountryRepository
    {
        IList<Country> GetAll();
        Country GetById(int countyId);
    }
}