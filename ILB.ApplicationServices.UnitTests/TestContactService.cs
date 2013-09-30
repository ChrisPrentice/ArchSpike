#region

using System.Collections.Generic;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

#endregion

namespace ILB.ApplicationServices.UnitTests
{
    public class WhenDisplayingContacts
    {
        private readonly IList<Contact> _expectedContacts = new List<Contact>();
        private readonly QueryInvoker _queryInvoker;

        public WhenDisplayingContacts()
        {
            var contactRepository = Mock.Of<IContactRepository>();

            _queryInvoker = new QueryInvoker(new ContactService(null,
                null,
                contactRepository,
                new ValidationService(),
                new ContactAdministrationService(null,
                    null,
                    contactRepository)));

            Mock.Get(contactRepository).Setup(q => q.GetAll()).Returns(_expectedContacts);
        }

        //Can we do something to automate the object creation with values?
        //TODO use autofixture to create the data automatically
        [Fact]
        public void Should_return_all_contact()
        {
            IList<Contact> contacts = _queryInvoker.Query<AllContactsQueryResult>().Contacts;
            Assert.Equal(contacts, _expectedContacts);
        }
    }

    public class WhenUserRequestsCreateContactForm
    {
        private readonly IList<County> _counties = new List<County>();
        private readonly IList<Country> _countries = new List<Country>();
        private readonly CreateContactQueryResult _createContactResult;

        public WhenUserRequestsCreateContactForm()
        {
            var countyRepository = Mock.Of<ICountyRepository>();
            var countryRepository = Mock.Of<ICountryRepository>();

            var queryInvoker = new QueryInvoker(new ContactService(countyRepository,
                countryRepository,
                null,
                new ValidationService(),
                new ContactAdministrationService(countyRepository,
                    countryRepository,
                    null)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(_counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(_countries);

            _createContactResult = queryInvoker.Query<CreateContactQueryResult>();
        }

        [Fact]
        public void Should_show_list_of_counties()
        {
            Assert.Equal(_createContactResult.Counties, _counties);
        }

        [Fact]
        public void Should_show_list_of_countries()
        {
            Assert.Equal(_createContactResult.Countries, _countries);
        }

        //TODO This shouldn't exist - why, how do we get rid of it?
        [Fact]
        public void Should_contain_empty_create_contact_command()
        {
            Assert.IsType<CreateContactCommand>(_createContactResult.Command);
        }
    }

    public class WhenUserRequestsToSaveContact
    {
        private readonly IList<County> _counties = new List<County>();
        private readonly IList<Country> _countries = new List<Country>();
        private Contact _contactCreated;

        //TODO use autofixture for the data
        public WhenUserRequestsToSaveContact()
        {
            IContactRepository contactRepository = Mock.Of<IContactRepository>();
            Mock.Get(contactRepository)
                .Setup(q => q.Save(It.IsAny<Contact>()))
                .Callback<Contact>(q => _contactCreated = q);

            ICountyRepository countyRepository = Mock.Of<ICountyRepository>();
            Mock.Get(countyRepository)
                .Setup(q => q.GetById(1))
                .Returns(new County(1, "Merseyside"));

            ICountryRepository countryRepository = Mock.Of<ICountryRepository>();
            Mock.Get(countryRepository)
                .Setup(q => q.GetById(1))
                .Returns(new Country(1, "UK"));

            CommandInvoker commandInvoker = new CommandInvoker(new ContactService(countyRepository,
                countryRepository,
                contactRepository,
                new ValidationService(),
                new ContactAdministrationService(countyRepository,
                    countryRepository,
                    contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(_counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(_countries);

            commandInvoker.Execute<CreateContactCommand, CreateContactQueryResult>(new CreateContactCommand
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
        [Fact]
        public void Should_create_contact_with_correct_firstname()
        {
            Assert.Equal(_contactCreated.FirstName, "Andrew");
        }

        [Fact]
        public void Should_create_contact_with_correct_surname()
        {
            Assert.Equal(_contactCreated.Surname, "Stewart");
        }

        [Fact]
        public void Should_create_contact_with_correct_address1()
        {
            Assert.Equal(_contactCreated.Address1, "Address 1");
        }

        [Fact]
        public void Should_create_contact_with_correct_address2()
        {
            Assert.Equal(_contactCreated.Address2, "Address 2");
        }

        [Fact]
        public void Should_create_contact_with_correct_CountyId()
        {
            Assert.Equal(_contactCreated.County.Id, 1);
        }

        [Fact]
        public void Should_create_contact_with_correct_CountryId()
        {
            Assert.Equal(_contactCreated.Country.Id, 1);
        }
    }

    public class WhenUserRequestsToSaveAnInvalidContact
    {
        private readonly IContactRepository _contactRepository;
        private readonly IList<County> _counties = new List<County>();
        private readonly IList<Country> _countries = new List<Country>();
        private readonly CreateContactCommand _createContactCommand;
        private readonly CreateContactQueryResult _createContactResult;
        //private Contact _contactCreated;

        public WhenUserRequestsToSaveAnInvalidContact()
        {
            _contactRepository = Mock.Of<IContactRepository>();
            //Mock.Get(_contactRepository)
            //    .Setup(q => q.Save(It.IsAny<Contact>()))
            //    .Callback<Contact>(q => _contactCreated = q);

            var countyRepository = Mock.Of<ICountyRepository>();
            Mock.Get(countyRepository)
                .Setup(q => q.GetById(1))
                .Returns(new County(1, "Merseyside"));

            var countryRepository = Mock.Of<ICountryRepository>();
            Mock.Get(countryRepository)
                .Setup(q => q.GetById(1))
                .Returns(new Country(1, "UK"));

            var commandInvoker = new CommandInvoker(new ContactService(countyRepository,
                countryRepository,
                _contactRepository,
                new ValidationService(),
                new ContactAdministrationService(countyRepository,
                    countryRepository,
                    _contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(_counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(_countries);

            _createContactCommand = new CreateContactCommand
            {
                FirstName = "",
                Surname = "",
                Address1 = "Address 1",
                Address2 = "Address 2",
                CountyId = 1,
                CountryId = 1,
            };
            _createContactResult =
                commandInvoker.Execute<CreateContactCommand, CreateContactQueryResult>(_createContactCommand);
        }

        [Fact]
        public void Should_not_save_changes()
        {
            Mock.Get(_contactRepository).Verify(q => q.Save(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public void Should_show_list_of_counties()
        {
            Assert.Equal(_createContactResult.Counties, _counties);
        }

        [Fact]
        public void Should_show_list_of_countries()
        {
            Assert.Equal(_createContactResult.Countries, _countries);
        }

        [Fact]
        public void Should_contain_original_create_contact_command()
        {
            Assert.Equal(_createContactResult.Command, _createContactCommand);
        }
    }

    public class WhenUserRequestsUpdateContactForm
    {
        private readonly IList<County> _counties = new List<County>();
        private readonly IList<Country> _countries = new List<Country>();
        private readonly UpdateContactQueryResult _queryResult;


        public WhenUserRequestsUpdateContactForm()
        {
            var contactRepository = Mock.Of<IContactRepository>();

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

            var countyRepository = Mock.Of<ICountyRepository>();
            var countryRepository = Mock.Of<ICountryRepository>();

            QueryInvoker queryInvoker = new QueryInvoker(new ContactService(countyRepository,
                countryRepository,
                contactRepository,
                new ValidationService(),
                new ContactAdministrationService(countyRepository,
                    countryRepository,
                    contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(_counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(_countries);

            _queryResult =
                queryInvoker.Query<UpdateContactQuery, UpdateContactQueryResult>(new UpdateContactQuery {Id = 1});
        }

        [Fact]
        public void Should_show_list_of_counties()
        {
            Assert.Equal(_queryResult.Counties, _counties);
        }

        [Fact]
        public void Should_show_list_of_countries()
        {
            Assert.Equal(_queryResult.Countries, _countries);
        }

        [Fact]
        public void Should_contain_empty_create_contact_command()
        {
            Assert.IsType<UpdateContactCommand>(_queryResult.Command);
        }

        [Fact]
        public void Should_save_contact_with_correct_firstname()
        {
            Assert.Equal(_queryResult.Command.FirstName, "Bob");
        }

        [Fact]
        public void Should_save_contact_with_correct_surname()
        {
            Assert.Equal(_queryResult.Command.Surname, "Holness");
        }

        [Fact]
        public void Should_save_contact_with_correct_address1()
        {
            Assert.Equal(_queryResult.Command.Address1, "1 Blockbuster way");
        }

        [Fact]
        public void Should_save_contact_with_correct_address2()
        {
            Assert.Equal(_queryResult.Command.Address2, "ITV Town");
        }

        [Fact]
        public void Should_save_contact_with_correct_CountyId()
        {
            Assert.Equal(_queryResult.Command.CountyId, 1);
        }

        [Fact]
        public void Should_save_contact_with_correct_CountryId()
        {
            Assert.Equal(_queryResult.Command.CountryId, 2);
        }
    }

    public class WhenUserRequestsToSaveExisitingContact
    {
        private readonly IList<County> _counties = new List<County>();
        private readonly IList<Country> _countries = new List<Country>();
        private readonly Contact _existingContact = new Contact(new CreateContactCommand());

        public WhenUserRequestsToSaveExisitingContact()
        {
            var contactRepository = Mock.Of<IContactRepository>();
            Mock.Get(contactRepository)
                .Setup(q => q.GetById(1))
                .Returns(_existingContact);


            var countyRepository = Mock.Of<ICountyRepository>();
            Mock.Get(countyRepository)
                .Setup(q => q.GetById(1))
                .Returns(new County(1, "Merseyside"));

            var countryRepository = Mock.Of<ICountryRepository>();
            Mock.Get(countryRepository)
                .Setup(q => q.GetById(1))
                .Returns(new Country(1, "UK"));

            var commandInvoker = new CommandInvoker(new ContactService(countyRepository,
                countryRepository,
                contactRepository,
                new ValidationService(),
                new ContactAdministrationService(countyRepository,
                    countryRepository,
                    contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(_counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(_countries);

            commandInvoker.Execute<UpdateContactCommand, UpdateContactQueryResult>(new UpdateContactCommand
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

        [Fact]
        public void Should_save_contact_with_correct_firstname()
        {
            Assert.Equal(_existingContact.FirstName, "Andrew");
        }

        [Fact]
        public void Should_save_contact_with_correct_surname()
        {
            Assert.Equal(_existingContact.Surname, "Stewart");
        }

        [Fact]
        public void Should_save_contact_with_correct_address1()
        {
            Assert.Equal(_existingContact.Address1, "Address 1");
        }

        [Fact]
        public void Should_save_contact_with_correct_address2()
        {
            Assert.Equal(_existingContact.Address2, "Address 2");
        }

        [Fact]
        public void Should_save_contact_with_correct_CountyId()
        {
            Assert.Equal(_existingContact.County.Id, 1);
        }

        [Fact]
        public void Should_save_contact_with_correct_CountryId()
        {
            Assert.Equal(_existingContact.Country.Id, 1);
        }
    }

    public class WhenUserRequestsToUpdateAnExistingContactWithInvalidContact
    {
        private readonly IContactRepository _contactRepository;
        private readonly IList<County> _counties = new List<County>();
        private readonly IList<Country> _countries = new List<Country>();
        private readonly UpdateContactQueryResult _result;
        private readonly UpdateContactCommand _updateCommand;


        public WhenUserRequestsToUpdateAnExistingContactWithInvalidContact()
        {
            _contactRepository = Mock.Of<IContactRepository>();
            var countyRepository = Mock.Of<ICountyRepository>();
            Mock.Get(countyRepository)
                .Setup(q => q.GetById(1))
                .Returns(new County(1, "Merseyside"));

            var countryRepository = Mock.Of<ICountryRepository>();
            Mock.Get(countryRepository)
                .Setup(q => q.GetById(1))
                .Returns(new Country(1, "UK"));

            var commandInvoker = new CommandInvoker(new ContactService(countyRepository,
                countryRepository,
                _contactRepository,
                new ValidationService(),
                new ContactAdministrationService(countyRepository,
                    countryRepository,
                    _contactRepository)));

            Mock.Get(countyRepository).Setup(q => q.GetAll()).Returns(_counties);
            Mock.Get(countryRepository).Setup(q => q.GetAll()).Returns(_countries);

            _updateCommand = new UpdateContactCommand
            {
                Id = 1,
                FirstName = "",
                Surname = "",
                Address1 = "Address 1",
                Address2 = "Address 2",
                CountyId = 1,
                CountryId = 1,
            };
            _result = commandInvoker.Execute<UpdateContactCommand, UpdateContactQueryResult>(_updateCommand);
        }

        [Fact]
        public void Should_not_save_changes()
        {
            Mock.Get(_contactRepository).Verify(q => q.Save(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public void Should_show_list_of_counties()
        {
            Assert.Equal(_result.Counties, _counties);
        }

        [Fact]
        public void Should_show_list_of_countries()
        {
            Assert.Equal(_result.Countries, _countries);
        }

        [Fact]
        public void Should_contain_original_update_contact_command()
        {
            Assert.Equal(_result.Command, _updateCommand);
        }
    }
}