using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Index()
    {
        var movie = await _context.Movies.ToListAsync();
        return View(movie);
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