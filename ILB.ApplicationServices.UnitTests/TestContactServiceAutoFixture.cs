using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILB.ApplicationServices.Contacts;
using ILB.Contacts;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace ILB.ApplicationServices.UnitTests
{
    public class When_displaying_contacts_1
    {
        [Theory, AutoContactsData]
        public void Should_return_all_contact([Frozen]IContactRepository contactRepository, IList<Contact> contacts, ContactService sut)
        {
            Mock.Get(contactRepository).Setup(q => q.GetAll()).Returns(contacts);
            
            var queryInvoker = new QueryInvoker(sut);
            var actual = queryInvoker.Query<AllContactsQueryResult>().Contacts;

            var expected = contacts;
            Assert.Equal(expected, actual);
        }
    }

    public class When_user_requests_to_save_contact_1
    {
        private Contact savedContact;

        [Theory, AutoContactsData]
        public void Should_create_contact_with_correct_information(
            [Frozen] IContactRepository contactRepository,
            [Frozen]IValidationService validationService,
            ContactService sut,
            CreateContactCommand createContactCommand)
        {

            var newContact = new Contact(createContactCommand);
            Mock.Get(contactRepository).Setup(q => q.Save(It.IsAny<Contact>()))
                .Callback<Contact>(q => savedContact = q);

            var validationResults = new ValidationResults(new Collection<ValidationResult>());

            Mock.Get(validationService).Setup(q => q.Validate(createContactCommand)).Returns(validationResults);

            var commandInvoker = new CommandInvoker(sut);
            commandInvoker.Execute<CreateContactCommand, CreateContactQueryResult>(createContactCommand);

            Assert.Equal(newContact, savedContact);
        }
    }
}


/* [Theory, AutoContactsData]
 public void Should_create_contact_with_correct_information([Frozen]CreateContactCommand createCommand, ContactService sut)
 {
     var expected = createCommand;
            
     var commandInvoker = new CommandInvoker(sut);
     var actual = commandInvoker.Execute<CreateContactCommand, CreateContactQueryResult>(createCommand).Command;
            
     Assert.Equal(expected, actual);
 }
 private Contact savedContact;

 /*  Castle.DynamicProxy.InvalidProxyConstructorArgumentsException
     Can not instantiate proxy of class: ILB.ValidationResults.
     Could not find a parameterless constructor.#1#
 [Theory, AutoContactsData]
 public void Should_create_contact_with_correct_information(
     [Frozen]IContactRepository contactRepository,
     ContactService sut, 
     CreateContactCommand createContactCommand)
 {
     var newContact = new Contact(createContactCommand);
     Mock.Get(contactRepository).Setup(q => q.Save(It.IsAny<Contact>()))
             .Callback<Contact>(q => savedContact = q);

     var commandInvoker = new CommandInvoker(sut);
     commandInvoker.Execute<CreateContactCommand, CreateContactQueryResult>(createContactCommand);

     Assert.Equal(newContact, savedContact);
 }

 [Theory, AutoContactsData]
 public void Should_create_contact_with_correct_information(
     [Frozen]IContactRepository contactRepository,
     ContactService sut,
     CreateContactCommand createContactCommand,
     IFixture fixture)
 {
      How can we user this --  Fixture.Inject???  or do we supply it as a resutl of our Mock in a Mock setup.
     ValidationResults validationResult = new ValidationResults(ValidationResult.Success);
            
     var newContact = new Contact(createContactCommand);
     Mock.Get(contactRepository).Setup(q => q.Save(It.IsAny<Contact>()))
             .Callback<Contact>(q => savedContact = q);

     var commandInvoker = new CommandInvoker(sut);
     commandInvoker.Execute<CreateContactCommand, CreateContactQueryResult>(createContactCommand);

     Assert.Equal(newContact, savedContact);
// }*/
