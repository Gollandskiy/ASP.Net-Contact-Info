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

        public ContactController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult ContactsView()
        {
            List<Contact> contacts = new List<Contact>
            {
                new Contact
                {
                    NameContact = "John Doe",
                    Telephone = "123-456-7890",
                    AltTelephone = "987-654-3210",
                    Email = "john.doe@example.com",
                    Description = "This is a test contact."
                }
            };
            return View("~/Views/Contacts/ContactsView.cshtml", contacts);
        }

        [HttpPost]
        public IActionResult ContactsView(List<Contact> contacts)
        {
            if (contacts == null || contacts.Count == 0)
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View("~/Views/Contacts/ContactsView.cshtml", contacts);
            }
            foreach (var contact in contacts)
            {
                if (string.IsNullOrEmpty(contact.NameContact) ||
                    string.IsNullOrEmpty(contact.Telephone) ||
                    string.IsNullOrEmpty(contact.Email))
                {
                    ViewBag.ErrorMessage = "Please fill in all required fields.";
                    return View("~/Views/Contacts/ContactsView.cshtml", contacts);
                }
                if (contact.NameContact.Length < 5)
                {
                    ViewBag.ErrorMessage = "The name is too short.";
                    return View("~/Views/Contacts/ContactsView.cshtml", contacts);
                }
                if (contact.Telephone.Length < 10)
                {
                    ViewBag.ErrorMessage = "The telephone is too short.";
                    return View("~/Views/Contacts/ContactsView.cshtml", contacts);
                }
                if (contact.Email.Length < 5)
                {
                    ViewBag.ErrorMessage = "The email is too short.";
                    return View("~/Views/Contacts/ContactsView.cshtml", contacts);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult SaveContactsToFile(List<Contact> contacts)
        {
            try
            {
                Console.WriteLine("Метод SaveContactsToFile был вызван");
                Console.WriteLine($"Количество контактов: {contacts.Count}");

                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "contacts.json");

                string json = JsonConvert.SerializeObject(contacts);

                System.IO.File.WriteAllText(filePath, json);

                ViewBag.SuccessMessage = "Контакты успешно сохранены в файл.";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ошибка при сохранении контактов: {ex.Message}";
            }

            return ContactsView();
        }

        public IActionResult ImportContactsFromFile()
        {
            try
            {
                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "contacts.json");

                string json = System.IO.File.ReadAllText(filePath);

                List<Contact> contacts = JsonConvert.DeserializeObject<List<Contact>>(json);

                ViewBag.SuccessMessage = "Контакты успешно импортированы из файла.";
                return View("~/Views/Contacts/ContactsView.cshtml", contacts);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ошибка при импорте контактов: {ex.Message}";
                return ContactsView();
            }
        }
    }
}