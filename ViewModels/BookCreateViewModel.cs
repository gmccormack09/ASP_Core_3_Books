using BugTracker.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.ViewModels
{
    public class BookCreateViewModel
    {
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public Genre? Genre { get; set; }
        public IFormFile Photo { get; set; }
    }
}
