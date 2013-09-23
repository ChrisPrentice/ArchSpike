using System.Collections.Generic;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ILB.ApplicationServices.UnitTests
{
    [TestClass]
    public class When_displaying_contacts
    {
        private ContactService contactService;
        private IList<Contact> expectedContacts = new List<Contact>();
        private IContactRepository contactRepository;

        [TestInitialize]
        public void Establish_Context()
        {
            contactRepository = Mock.Of<IContactRepository>();
            Mock.Get(contactRepository).Setup(q => q.GetAll()).Returns(expectedContacts);

            contactService = new ContactService(null, null, contactRepository, null, null);
        }

        [TestMethod]
        public void Should_return_all_contact()
        {
            //var contacts = contactService.GetAll();
            //Assert.AreEqual(contacts, expectedContacts);
        }
    }
}
