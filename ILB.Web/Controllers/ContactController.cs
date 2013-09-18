using System.Web.Mvc;
using ILB.ApplicationServices;
using ILB.Contacts;
using ILB.Infrastructure;
using ILb.Infrastructure;

namespace ILB.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController()
        {
            // You'd really DI this is from autofac.
            _contactService = new ContactService(new CountyRepository(), new CountryRepository(), new ContactRepository(), new ValidationService(), new ContactAdministrationService(new CountyRepository(), new CountryRepository(), new ContactRepository()));
        }

        public ActionResult Index()
        {
            return View(_contactService.GetAll());
        }

        public ActionResult Create()
        {
            return View(_contactService.CreateContact());
        }

        [HttpPost]
        public ActionResult Create(CreateContactCommand command)
        {
            command = _contactService.CreateContact(command);
            return ModelState.IsValid ? RedirectToAction("Index") : (ActionResult)View(command);
        }

        public ActionResult Update(int id)
        {
            return View(_contactService.UpdateContact(id));
        }

        [HttpPost]
        public ActionResult Update(UpdateContactCommand command)
        {
            command = _contactService.UpdateContact(command);
            return ModelState.IsValid ? RedirectToAction("Index") : (ActionResult)View(command);
        }
    }
}
