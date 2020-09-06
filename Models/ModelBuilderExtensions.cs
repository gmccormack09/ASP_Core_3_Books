using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book() { Id = 1, Name = "Blood Meridian", Author = "Cormac McCarthy", Genre = Genre.Western, PhotoPath = "BloodMeridian.png" },
                new Book() { Id = 2, Name = "Biography of Malcolm X", Author= "Malcolm X", Genre = Genre.Autobiography, PhotoPath= "MalcolmXBook.png"},
                new Book() { Id = 3, Name = "Beyond Good and Evil", Author= "Friedrich Nietzsche", Genre = Genre.Philosophy}               
            );
        }
    }
}
