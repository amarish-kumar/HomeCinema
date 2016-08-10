using System;
using System.Collections.Generic;

namespace HomeCinema.Entities
{
    public class Movie : IEntityBase
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
        public string Director { get; set; }
        public string Writer { get; set; }
        public string Producer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public byte Rating { get; set; }
        public string TrailerUrl { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}