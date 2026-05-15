using Microsoft.EntityFrameworkCore;
using MoviesApp.Models;

namespace MoviesApp.Data;

public class MovieDbContext : DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options ) : base(options)
    {
        
    }
    
    public DbSet<Movie>  Movies { get; set; }
}