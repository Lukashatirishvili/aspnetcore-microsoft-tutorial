using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Data;
using MoviesApp.Models;

namespace MoviesApp.Controllers;

public class MoviesController : Controller
{
    private readonly MovieDbContext _context;
    
    public MoviesController(MovieDbContext context)
    {
        _context = context;
    }

    private bool MovieExists(int id)
    {
        return _context.Movies.Any(e => e.Id == id);
    }
    
    // GET
    // Search
    // GET: Movies
    public async Task<IActionResult> Index(string movieGenre, string searchString)
    {
        if (_context.Movies == null)
        {
            return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
        }

        // Use LINQ to get list of genres.
        IQueryable<string> genreQuery = from m in _context.Movies
            orderby m.Genre
            select m.Genre;
        
        var movies = from m in _context.Movies
            select m;

        if (!string.IsNullOrEmpty(searchString))
        {
            movies = movies.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
        }

        if (!string.IsNullOrEmpty(movieGenre))
        {
            movies = movies.Where(x => x.Genre == movieGenre);
        }

        var movieGenreVM = new MovieGenreViewModel
        {
            Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
            Movies = await movies.ToListAsync()
        };

        return View(movieGenreVM);
    }
    
    [HttpPost]
    public string Index(string searchString, bool notUsed)
    {
        return "From [HttpPost]Index: filter on " + searchString;
    }
    
    // Edit GET method
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var movie = await _context.Movies.FindAsync(id);


        if (movie == null)
        {
            return NotFound();
        }
        
        return View(movie);
    }
    
    // Edit post method
    [HttpPost]
    [ValidateAntiForgeryToken]  
    public async Task<IActionResult> Edit(int id, [Bind("Id,    Title,ReleaseData,Genre,Price")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var movie = await _context.Movies.FindAsync(id);
        
        if (movie == null)
        {
            return NotFound();
        }
        
        return View(movie);
    }
    
    

}