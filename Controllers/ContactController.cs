using DZ4_10._02._2024_.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DZ4_10._02._2024_.Controllers
{
    public class ContactController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        private List<Contact> _contacts { get; set; }

        public ContactController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _contacts = new List<Contact>();
        }

        public IActionResult ContactsView()
        {
            _contacts.Add(new Contact
            {
                NameContact = "John Doe",
                Telephone = "123-456-7890",
                AltTelephone = "987-654-3210",
                Email = "john.doe@example.com",
                Description = "This is a test contact."
            });
            return View("~/Views/Contacts/ContactsView.cshtml", _contacts);
        }

        [HttpPost]
        public IActionResult ContactsView(List<Contact> contacts)
        {
            if (contacts == null || contacts.Count == 0)
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View("~/Views/Contacts/ContactsView.cshtml", _contacts);
            }
            foreach (var contact in contacts)
            {
                if (string.IsNullOrEmpty(contact.NameContact) ||
                    string.IsNullOrEmpty(contact.Telephone) ||
                    string.IsNullOrEmpty(contact.Email))
                {
                    ViewBag.ErrorMessage = "Please fill in all required fields.";
                    return View("~/Views/Contacts/ContactsView.cshtml", _contacts);
                }
                if (contact.NameContact.Length < 5)
                {
                    ViewBag.ErrorMessage = "The name is too short.";
                    return View("~/Views/Contacts/ContactsView.cshtml", _contacts);
                }
                if (contact.Telephone.Length < 10)
                {
                    ViewBag.ErrorMessage = "The telephone is too short.";
                    return View("~/Views/Contacts/ContactsView.cshtml", _contacts);
                }
                if (contact.Email.Length < 5)
                {
                    ViewBag.ErrorMessage = "The email is too short.";
                    return View("~/Views/Contacts/ContactsView.cshtml", _contacts);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult SaveContactsToFile()
        {
            try
            {
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "contacts.json");
                string json = JsonConvert.SerializeObject(_contacts);
                System.IO.File.WriteAllText(filePath, json);

                byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/json", "contacts.json");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ошибка при сохранении контактов: {ex.Message}";
                return RedirectToAction("ContactsView");
            }
        }

        public IActionResult ImportContactsFromFile()
        {
            try
            {
                ContactsView();
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "contacts.json");
                string json = System.IO.File.ReadAllText(filePath);
                List<Contact> _contacts = JsonConvert.DeserializeObject<List<Contact>>(json);
                ViewBag.SuccessMessage = "Контакты успешно импортированы из файла.";
                return View("~/Views/Contacts/ContactsView.cshtml", _contacts);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ошибка при импорте контактов: {ex.Message}";
                return ContactsView();
            }
        }
    }
}