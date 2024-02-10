using DZ4_10._02._2024_.Models;
using Microsoft.AspNetCore.Mvc;


namespace DZ4_10._02._2024_.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult ContactsView()
        {
            Contact contact = new Contact
            {
                NameContact = "John Doe",
                Telephone = "123-456-7890",
                AltTelephone = "987-654-3210",
                Email = "john.doe@example.com",
                Description = "This is a test contact."
            };
            return View("~/Views/Contacts/ContactsView.cshtml",contact);
        }
        [HttpPost]
        public IActionResult ContactsView(Contact contact)
        {
            if (string.IsNullOrEmpty(contact.NameContact) ||
                string.IsNullOrEmpty(contact.Telephone) ||
                string.IsNullOrEmpty(contact.Email))
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View("~/Views/Contacts/ContactsView.cshtml",contact);
            }
            if (contact.NameContact.Length < 5)
            {
                ViewBag.ErrorMessage = "The name is too short.";
                return View("~/Views/Contacts/ContactsView.cshtml", contact);
            }
            if (contact.Telephone.Length < 10)
            {
                ViewBag.ErrorMessage = "The telephone is too short.";
                return View("~/Views/Contacts/ContactsView.cshtml", contact);
            }
            if (contact.Email.Length < 5)
            {
                ViewBag.ErrorMessage = "The email is too short.";
                return View("~/Views/Contacts/ContactsView.cshtml", contact);
            }
            return RedirectToAction("Index");
        }
    }
}
