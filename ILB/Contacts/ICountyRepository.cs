using System.Collections.Generic;

namespace ILB.Contacts
{
    public interface ICountyRepository
    {
        IList<County> GetAll();
        County GetById(int countyId);
    }
}