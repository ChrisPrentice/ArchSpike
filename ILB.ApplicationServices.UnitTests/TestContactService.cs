using System.Collections.Generic;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using Moq;
using Ploeh.AutoFixture.AutoMoq;
using Xunit;
using Assert = Xunit.Assert;
using Ploeh.AutoFixture.Xunit;

namespace ILB.ApplicationServices.UnitTests
{
    public class When_displaying_contacts
    {
        private readonly IList<Contact> expectedContacts = new List<Contact>();
        private IContactRepository contactRepository;
        private QueryInvoker queryInvoker;

        public When_displaying_contacts()
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
        [Fact]
        public void Should_return_all_contact()
        {
            var contacts = queryInvoker.Query<AllContactsQueryResult>().Contacts;
            Assert.Equal(contacts, expectedContacts);
        }

    }

    public class When_user_requests_create_contact_form
    {
        private ICountyRepository countyRepository;
        private ICountryRepository countryRepository;
        private QueryInvoker queryInvoker;
        private CreateContactQueryResult createContactResult;
        private IList<County> counties = new List<County>();
        private IList<Country> countries = new List<Country>();
            
        public When_user_requests_create_contact_form()
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

        [Fact]
        public void Should_show_list_of_counties()
        {
            Assert.Equal(createContactResult.Counties, counties);
        }

        [Fact]
        public void Should_show_list_of_countries()
        {
            Assert.Equal(createContactResult.Countries, countries);
        }
        //TODO This shouldn't exist - why, how do we get rid of it?
        [Fact]
        public void Should_contain_empty_create_contact_command()
        {
            Assert.IsType<CreateContactCommand>(createContactResult.Command);
        }

    }

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
        public When_user_requests_to_save_contact()
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
        [Fact]
        public void Should_create_contact_with_correct_firstname()
        {
            Assert.Equal(contactCreated.FirstName, "Andrew");
        }

        [Fact]
        public void Should_create_contact_with_correct_surname()
        {
            Assert.Equal(contactCreated.Surname, "Stewart");
        }

        [Fact]
        public void Should_create_contact_with_correct_address1()
        {
            Assert.Equal(contactCreated.Address1, "Address 1");
        }

        [Fact]
        public void Should_create_contact_with_correct_address2()
        {
            Assert.Equal(contactCreated.Address2, "Address 2");
        }

        [Fact]
        public void Should_create_contact_with_correct_CountyId()
        {
            Assert.Equal(contactCreated.County.Id, 1);
        }

        [Fact]
        public void Should_create_contact_with_correct_CountryId()
        {
            Assert.Equal(contactCreated.Country.Id, 1);
        }
    }

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

        public When_user_requests_to_save_an_invalid_contact()
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

        [Fact]
        public void Should_not_save_changes()
        {
            Mock.Get(contactRepository).Verify(q => q.Save(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public void Should_show_list_of_counties()
        {
            Assert.Equal(createContactResult.Counties, counties);
        }

        [Fact]
        public void Should_show_list_of_countries()
        {
            Assert.Equal(createContactResult.Countries, countries);
        }

        [Fact]
        public void Should_contain_original_create_contact_command()
        {
            Assert.Equal(createContactResult.Command, createContactCommand);
        }
    }

    public class When_user_requests_update_contact_form
    {
        private IContactRepository contactRepository;
        private ICountyRepository countyRepository;
        private ICountryRepository countryRepository;
        private QueryInvoker queryInvoker;
        private UpdateContactQueryResult queryResult;
        private IList<County> counties = new List<County>();
        private IList<Country> countries = new List<Country>();


        public When_user_requests_update_contact_form()
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

        [Fact]
        public void Should_show_list_of_counties()
        {
            Assert.Equal(queryResult.Counties, counties);
        }

        [Fact]
        public void Should_show_list_of_countries()
        {
            Assert.Equal(queryResult.Countries, countries);
        }

        [Fact]
        public void Should_contain_empty_create_contact_command()
        {
            Assert.IsType <UpdateContactCommand>(queryResult.Command);
        }

        [Fact]
        public void Should_save_contact_with_correct_firstname()
        {
            Assert.Equal(queryResult.Command.FirstName, "Bob");
        }

        [Fact]
        public void Should_save_contact_with_correct_surname()
        {
            Assert.Equal(queryResult.Command.Surname, "Holness");
        }

        [Fact]
        public void Should_save_contact_with_correct_address1()
        {
            Assert.Equal(queryResult.Command.Address1, "1 Blockbuster way");
        }

        [Fact]
        public void Should_save_contact_with_correct_address2()
        {
            Assert.Equal(queryResult.Command.Address2, "ITV Town");
        }

        [Fact]
        public void Should_save_contact_with_correct_CountyId()
        {
            Assert.Equal(queryResult.Command.CountyId, 1);
        }

        [Fact]
        public void Should_save_contact_with_correct_CountryId()
        {
            Assert.Equal(queryResult.Command.CountryId, 2);
        }
    }

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


 
        public  When_user_requests_to_save_exisiting_contact()
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

        [Fact]
        public void Should_save_contact_with_correct_firstname()
        {
            Assert.Equal(existingContact.FirstName, "Andrew");
        }

        [Fact]
        public void Should_save_contact_with_correct_surname()
        {
            Assert.Equal(existingContact.Surname, "Stewart");
        }

        [Fact]
        public void Should_save_contact_with_correct_address1()
        {
            Assert.Equal(existingContact.Address1, "Address 1");
        }

        [Fact]
        public void Should_save_contact_with_correct_address2()
        {
            Assert.Equal(existingContact.Address2, "Address 2");
        }

        [Fact]
        public void Should_save_contact_with_correct_CountyId()
        {
            Assert.Equal(existingContact.County.Id, 1);
        }

        [Fact]
        public void Should_save_contact_with_correct_CountryId()
        {
            Assert.Equal(existingContact.Country.Id, 1);
        }
    }

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


        public When_user_requests_to_update_an_existing_contact_with_invalid_contact()
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

        [Fact]
        public void Should_not_save_changes()
        {
            Mock.Get(contactRepository).Verify(q => q.Save(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public void Should_show_list_of_counties()
        {
            Assert.Equal(result.Counties, counties);
        }

        [Fact]
        public void Should_show_list_of_countries()
        {
            Assert.Equal(result.Countries, countries);
        }

        [Fact]
        public void Should_contain_original_update_contact_command()
        {
            Assert.Equal(result.Command, updateCommand);
        }
    }
}
