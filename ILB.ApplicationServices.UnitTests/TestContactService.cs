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
        private readonly IList<Contact> expectedContacts = new List<Contact>();
        private IContactRepository contactRepository;
        private QueryInvoker queryInvoker;

        [TestInitialize]
        public void Establish_Context()
        {
            contactRepository = Mock.Of<IContactRepository>();

            queryInvoker = new QueryInvoker(new ContactService(null,
                          null,
                          contactRepository,
                          new ValidationService(),
                          new ContactAdministrationService(null,
                                                           null,
                                                           contactRepository)));
            
            Mock.Get(contactRepository).Setup(q => q.GetAll()).Returns(expectedContacts);
        }

        //Can we do something to automate the object creation with values?
        //TODO use autofixture to create the data automatically
        [TestMethod]
        public void Should_return_all_contact()
        {
            var contacts = queryInvoker.Query<AllContactsQueryResult>().Contacts;
            Assert.AreEqual(contacts, expectedContacts);
        }

    }

    [TestClass]
    public class When_user_requests_create_contact_form
    {
        private ICountyRepository countyRepository;
        private ICountryRepository countryRepository;
        private QueryInvoker queryInvoker;
        private CreateContactQueryResult createContactResult;
        private IList<County> counties = new List<County>();
        private IList<Country> countries = new List<Country>();
            
            
        [TestInitialize]
        public void Establish_Context()
        {
            countyRepository = Mock.Of<ICountyRepository>();
            countryRepository = Mock.Of<ICountryRepository>();

            queryInvoker = new QueryInvoker(new ContactService(countyRepository,
                          countryRepository,
                          null,
                          new ValidationService(),
                          new ContactAdministrationService(countyRepository,
                                                           countryRepository,
                                                           null)));
            
            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(countries);

            createContactResult = queryInvoker.Query<CreateContactQueryResult>();
        }

        [TestMethod]
        public void Should_show_list_of_counties()
        {
            Assert.AreEqual(createContactResult.Counties, counties);
        }

        [TestMethod]
        public void Should_show_list_of_countries()
        {
            Assert.AreEqual(createContactResult.Countries, countries);
        }
        //TODO This shouldn't exist - why, how do we get rid of it
        [TestMethod]
        public void Should_contain_empty_create_contact_command()
        {
            Assert.IsInstanceOfType(createContactResult.Command, typeof(CreateContactCommand));
        }

    }

    [TestClass]
    public class When_user_requests_to_save_contact
    {
        private ICountyRepository countyRepository;
        private ICountryRepository countryRepository;
        private IContactRepository contactRepository;
        private CommandInvoker commandInvoker;
        private IList<County> counties = new List<County>();
        private IList<Country> countries = new List<Country>();
        private Contact contactCreated;

        //TODO use autofixture for the data
        [TestInitialize]
        public void Establish_Context()
        {
            contactRepository = Mock.Of<IContactRepository>();
            Mock.Get(contactRepository)
                    .Setup(q => q.Save(It.IsAny<Contact>()))
                    .Callback<Contact>(q => contactCreated = q);

            countyRepository = Mock.Of<ICountyRepository>();
            Mock.Get(countyRepository)
                .Setup(q => q.GetById(1))
                .Returns(new County(1, "Merseyside"));

            countryRepository = Mock.Of<ICountryRepository>();
            Mock.Get(countryRepository)
                .Setup(q => q.GetById(1))
                .Returns(new Country(1, "UK"));

            commandInvoker = new CommandInvoker(new ContactService(countyRepository,
                          countryRepository,
                          contactRepository,
                          new ValidationService(),
                          new ContactAdministrationService(countyRepository,
                                                           countryRepository,
                                                           contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(countries);

            commandInvoker.Execute<CreateContactCommand,CreateContactQueryResult>(new CreateContactCommand()
                {
                    FirstName = "Andrew",
                    Surname = "Stewart",
                    Address1 = "Address 1",
                    Address2 = "Address 2",
                    CountyId = 1,
                    CountryId = 1,
                });
        }

        //TODO Try and use autofixture to gen the data and test all of the methods
        [TestMethod]
        public void Should_create_contact_with_correct_firstname()
        {
            Assert.AreEqual(contactCreated.FirstName, "Andrew");
        }

        [TestMethod]
        public void Should_create_contact_with_correct_surname()
        {
            Assert.AreEqual(contactCreated.Surname, "Stewart");
        }

        [TestMethod]
        public void Should_create_contact_with_correct_address1()
        {
            Assert.AreEqual(contactCreated.Address1, "Address 1");
        }

        [TestMethod]
        public void Should_create_contact_with_correct_address2()
        {
            Assert.AreEqual(contactCreated.Address2, "Address 2");
        }

        [TestMethod]
        public void Should_create_contact_with_correct_CountyId()
        {
            Assert.AreEqual(contactCreated.County.Id, 1);
        }

        [TestMethod]
        public void Should_create_contact_with_correct_CountryId()
        {
            Assert.AreEqual(contactCreated.Country.Id, 1);
        }
    }

    [TestClass]
    public class When_user_requests_to_save_an_invalid_contact
    {
        private ICountyRepository countyRepository;
        private ICountryRepository countryRepository;
        private IContactRepository contactRepository;
        private CommandInvoker commandInvoker;
        private CreateContactQueryResult createContactResult;
        private IList<County> counties = new List<County>();
        private IList<Country> countries = new List<Country>();
        private Contact contactCreated;
        private CreateContactCommand createContactCommand;


        [TestInitialize]
        public void Establish_Context()
        {
            contactRepository = Mock.Of<IContactRepository>();
            Mock.Get(contactRepository)
                    .Setup(q => q.Save(It.IsAny<Contact>()))
                    .Callback<Contact>(q => contactCreated = q);

            countyRepository = Mock.Of<ICountyRepository>();
            Mock.Get(countyRepository)
                .Setup(q => q.GetById(1))
                .Returns(new County(1, "Merseyside"));

            countryRepository = Mock.Of<ICountryRepository>();
            Mock.Get(countryRepository)
                .Setup(q => q.GetById(1))
                .Returns(new Country(1, "UK"));

            commandInvoker = new CommandInvoker(new ContactService(countyRepository,
                          countryRepository,
                          contactRepository,
                          new ValidationService(),
                          new ContactAdministrationService(countyRepository,
                                                           countryRepository,
                                                           contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(countries);

            createContactCommand = new CreateContactCommand()
                {
                    FirstName = "", Surname = "", Address1 = "Address 1", Address2 = "Address 2", CountyId = 1, CountryId = 1,
                };
            createContactResult = commandInvoker.Execute<CreateContactCommand, CreateContactQueryResult>(createContactCommand);
        }

        [TestMethod]
        public void Should_not_save_changes()
        {
            Mock.Get(contactRepository).Verify(q => q.Save(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Should_show_list_of_counties()
        {
            Assert.AreEqual(createContactResult.Counties, counties);
        }

        [TestMethod]
        public void Should_show_list_of_countries()
        {
            Assert.AreEqual(createContactResult.Countries, countries);
        }

        [TestMethod]
        public void Should_contain_original_create_contact_command()
        {
            Assert.AreEqual(createContactResult.Command, createContactCommand);
        }
    }

    [TestClass]
    public class When_user_requests_update_contact_form
    {
        private IContactRepository contactRepository;
        private ICountyRepository countyRepository;
        private ICountryRepository countryRepository;
        private QueryInvoker queryInvoker;
        private UpdateContactQueryResult queryResult;
        private IList<County> counties = new List<County>();
        private IList<Country> countries = new List<Country>();


        [TestInitialize]
        public void Establish_Context()
        {
            contactRepository = Mock.Of<IContactRepository>();

            Mock.Get(contactRepository)
                .Setup(q => q.GetById(1))
                .Returns(new Contact(new CreateContactCommand
                    {
                        FirstName = "Bob",
                        Surname = "Holness",
                        Address1 = "1 Blockbuster way",
                        Address2 = "ITV Town",
                    })
                    {
                        County = new County(1, "Some County"),
                        Country = new Country(2, "UK"),
                    });

            countyRepository = Mock.Of<ICountyRepository>();
            countryRepository = Mock.Of<ICountryRepository>();

            queryInvoker = new QueryInvoker(new ContactService(countyRepository,
                          countryRepository,
                          contactRepository,
                          new ValidationService(),
                          new ContactAdministrationService(countyRepository,
                                                           countryRepository,
                                                           contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(countries);

            queryResult = queryInvoker.Query<UpdateContactQuery, UpdateContactQueryResult>(new UpdateContactQuery { Id = 1});
        }

        [TestMethod]
        public void Should_show_list_of_counties()
        {
            Assert.AreEqual(queryResult.Counties, counties);
        }

        [TestMethod]
        public void Should_show_list_of_countries()
        {
            Assert.AreEqual(queryResult.Countries, countries);
        }

        [TestMethod]
        public void Should_contain_empty_create_contact_command()
        {
            Assert.IsInstanceOfType(queryResult.Command, typeof(UpdateContactCommand));
        }

        [TestMethod]
        public void Should_save_contact_with_correct_firstname()
        {
            Assert.AreEqual(queryResult.Command.FirstName, "Bob");
        }

        [TestMethod]
        public void Should_save_contact_with_correct_surname()
        {
            Assert.AreEqual(queryResult.Command.Surname, "Holness");
        }

        [TestMethod]
        public void Should_save_contact_with_correct_address1()
        {
            Assert.AreEqual(queryResult.Command.Address1, "1 Blockbuster way");
        }

        [TestMethod]
        public void Should_save_contact_with_correct_address2()
        {
            Assert.AreEqual(queryResult.Command.Address2, "ITV Town");
        }

        [TestMethod]
        public void Should_save_contact_with_correct_CountyId()
        {
            Assert.AreEqual(queryResult.Command.CountyId, 1);
        }

        [TestMethod]
        public void Should_save_contact_with_correct_CountryId()
        {
            Assert.AreEqual(queryResult.Command.CountryId, 2);
        }
    }

    [TestClass]
    public class When_user_requests_to_save_exisiting_contact
    {
        private ICountyRepository countyRepository;
        private ICountryRepository countryRepository;
        private IContactRepository contactRepository;
        private CommandInvoker commandInvoker;
        private UpdateContactQueryResult commandResult;
        private IList<County> counties = new List<County>();
        private IList<Country> countries = new List<Country>();
        private Contact existingContact = new Contact(new CreateContactCommand());


        [TestInitialize]
        public void Establish_Context()
        {
            contactRepository = Mock.Of<IContactRepository>();
            Mock.Get(contactRepository)
                .Setup(q => q.GetById(1))
                .Returns(existingContact);


            countyRepository = Mock.Of<ICountyRepository>();
            Mock.Get(countyRepository)
                .Setup(q => q.GetById(1))
                .Returns(new County(1, "Merseyside"));

            countryRepository = Mock.Of<ICountryRepository>();
            Mock.Get(countryRepository)
                .Setup(q => q.GetById(1))
                .Returns(new Country(1, "UK"));

            commandInvoker = new CommandInvoker(new ContactService(countyRepository,
                          countryRepository,
                          contactRepository,
                          new ValidationService(),
                          new ContactAdministrationService(countyRepository,
                                                           countryRepository,
                                                           contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(countries);

            commandResult = commandInvoker.Execute<UpdateContactCommand, UpdateContactQueryResult>(new UpdateContactCommand
            {
                Id = 1,
                FirstName = "Andrew",
                Surname = "Stewart",
                Address1 = "Address 1",
                Address2 = "Address 2",
                CountyId = 1,
                CountryId = 1,
            });
        }

        [TestMethod]
        public void Should_save_contact_with_correct_firstname()
        {
            Assert.AreEqual(existingContact.FirstName, "Andrew");
        }

        [TestMethod]
        public void Should_save_contact_with_correct_surname()
        {
            Assert.AreEqual(existingContact.Surname, "Stewart");
        }

        [TestMethod]
        public void Should_save_contact_with_correct_address1()
        {
            Assert.AreEqual(existingContact.Address1, "Address 1");
        }

        [TestMethod]
        public void Should_save_contact_with_correct_address2()
        {
            Assert.AreEqual(existingContact.Address2, "Address 2");
        }

        [TestMethod]
        public void Should_save_contact_with_correct_CountyId()
        {
            Assert.AreEqual(existingContact.County.Id, 1);
        }

        [TestMethod]
        public void Should_save_contact_with_correct_CountryId()
        {
            Assert.AreEqual(existingContact.Country.Id, 1);
        }
    }

    [TestClass]
    public class When_user_requests_to_update_an_existing_contact_with_invalid_contact
    {
        private ICountyRepository countyRepository;
        private ICountryRepository countryRepository;
        private IContactRepository contactRepository;
        private CommandInvoker commandInvoker;
        private UpdateContactQueryResult result;
        private IList<County> counties = new List<County>();
        private IList<Country> countries = new List<Country>();
        private UpdateContactCommand updateCommand;


        [TestInitialize]
        public void Establish_Context()
        {
            contactRepository = Mock.Of<IContactRepository>();
            countyRepository = Mock.Of<ICountyRepository>();
            Mock.Get(countyRepository)
                .Setup(q => q.GetById(1))
                .Returns(new County(1, "Merseyside"));

            countryRepository = Mock.Of<ICountryRepository>();
            Mock.Get(countryRepository)
                .Setup(q => q.GetById(1))
                .Returns(new Country(1, "UK"));

            commandInvoker = new CommandInvoker(new ContactService(countyRepository,
                          countryRepository,
                          contactRepository,
                          new ValidationService(),
                          new ContactAdministrationService(countyRepository,
                                                           countryRepository,
                                                           contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(countries);

            updateCommand = new UpdateContactCommand()
            {
                Id = 1,
                FirstName = "",
                Surname = "",
                Address1 = "Address 1",
                Address2 = "Address 2",
                CountyId = 1,
                CountryId = 1,
            };
            result = commandInvoker.Execute<UpdateContactCommand, UpdateContactQueryResult>(updateCommand);
        }

        [TestMethod]
        public void Should_not_save_changes()
        {
            Mock.Get(contactRepository).Verify(q => q.Save(It.IsAny<Contact>()), Times.Never);
        }

        [TestMethod]
        public void Should_show_list_of_counties()
        {
            Assert.AreEqual(result.Counties, counties);
        }

        [TestMethod]
        public void Should_show_list_of_countries()
        {
            Assert.AreEqual(result.Countries, countries);
        }

        [TestMethod]
        public void Should_contain_original_update_contact_command()
        {
            Assert.AreEqual(result.Command, updateCommand);
        }
    }
}
