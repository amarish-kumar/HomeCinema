using HomeCinema.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HomeCinema.Data.Configurations
{
    public class Movieconfiguraiton : EntityTypeConfiguration<Movie>
    {
        public Movieconfiguraiton()
        {
            Property(m => m.Title).IsRequired().HasMaxLength(100);
            Property(m => m.GenreId).IsRequired();
            Property(m => m.Director).IsRequired().HasMaxLength(100);
            Property(m => m.Producer).IsRequired().HasMaxLength(50);
            Property(m => m.Writer).IsRequired().HasMaxLength(50);
            Property(m => m.Producer).IsRequired().HasMaxLength(50);
            Property(m => m.Rating).IsRequired();
            Property(m => m.Description).IsRequired().HasMaxLength(2000);
            Property(m => m.TrailerURI).IsRequired().HasMaxLength(200);
            HasMany(m => m.Stocks).WithRequired().HasForeignKey(s => s.MovieId);
        }
    }
}