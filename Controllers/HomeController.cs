using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Models;
using BugTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IBookRepository _bookRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public HomeController(IBookRepository bookRepository, IWebHostEnvironment hostingEnvironment)
        {
            _bookRepository = bookRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            // retrieve all the employees
            var model = _bookRepository.GetAllBooks();
            // Pass the list of employees to the view
            ViewBag.Title = "Index";

            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Details(int? id)
        {

            Book book = _bookRepository.GetBook(id ?? 1);

            if (book == null)
            {
                Response.StatusCode = 404;
                return View("UserNotFound", id);
            }

            // Instantiate HomeDetailsViewModel and store Employee details and PageTitle
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Book = book,
                PageTitle = "Employee Details"
            };

            // Pass the ViewModel object to the View() helper method
            return View(homeDetailsViewModel);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(BookCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // If the Photos property on the incoming model object is not null and if count > 0,
                // then the user has selected at least one file to upload

                // has selected an image to upload.
                if (model.Photo != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Book newBook = new Book
                {
                    Name = model.Name,
                    Author = model.Author,
                    Genre = model.Genre,
                    PhotoPath = uniqueFileName
                };

                _bookRepository.Add(newBook);
                return RedirectToAction("details", new { id = newBook.Id });
            }
            return View();
        }


        [HttpGet]
        public ViewResult Edit(int id)
        {
            Book book = _bookRepository.GetBook(id);
            BookEditViewModel userEditViewModel = new BookEditViewModel
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Genre = book.Genre,
                ExistingPhotoPath = book.PhotoPath
            };
            return View(userEditViewModel);
        }


        // Through model binding, the action method parameter
        // EmployeeEditViewModel receives the posted edit form data
        [HttpPost]
        public IActionResult Edit(BookEditViewModel model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the employee being edited from the database
                Book book = _bookRepository.GetBook(model.Id);
                // Update the employee object with the data in the model object
                book.Name = model.Name;
                book.Author = model.Author;
                book.Genre = model.Genre;

                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.Photo != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the employee object which will be
                    // eventually saved in the database
                    book.PhotoPath = ProcessUploadedFile(model);
                }

                // Call update method on the repository service passing it the
                // employee object to update the data in the database table
                Book updatedUser = _bookRepository.Update(book);

                return RedirectToAction("index");
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _bookRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private string ProcessUploadedFile(BookCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
