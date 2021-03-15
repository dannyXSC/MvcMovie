using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public Movie()
        {
        }

        public Movie(int i,string t,DateTime r,string g,decimal p)
        {
            Id = i;
            Title = t;
            ReleaseDate = r;
            Genre = g;
            Price = p;
        }

    }
}