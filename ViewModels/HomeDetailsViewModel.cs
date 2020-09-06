using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.ViewModels
{
    public class HomeDetailsViewModel
    {

        public Book Book { get; set; }
        public string PageTitle
        {
            get; set;

        }
    }
}
