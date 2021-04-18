using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VideoGameStore.Data;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    public class GamesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GamesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            return View(await _context.Game.OrderBy(j => j.gameTitle).ThenBy(j => j.gameCategory).ToListAsync());
        }

        // GET: Games/Details/5
        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.gameGuid == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("gameGuid,gameTitle,gameDescription,gameReleaseDate,gamePrice,gameCategory,gameDeveloper,gamePublisher")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.gameGuid = Guid.NewGuid();
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("gameGuid,gameTitle,gameDescription,gameReleaseDate,gamePrice,gameCategory,gameDeveloper,gamePublisher")] Game game)
        {
            if (id != game.gameGuid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.gameGuid))
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
            return View(game);
        }

        // GET: Games/Delete/5   
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.gameGuid == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var game = await _context.Game.FindAsync(id);
            _context.Game.Remove(game);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(Guid id)
        {
            return _context.Game.Any(e => e.gameGuid == id);
        }


        public async Task<IActionResult> ShowSearchResults(string searchPhrase)
        {
            bool isSearchEmpty;
            if (searchPhrase == null) { isSearchEmpty = true; } else { isSearchEmpty = false; }

            string selectedSearchCategory = Request.Form["searchDropdown"];

            if(!String.IsNullOrEmpty(searchPhrase) && String.Equals(selectedSearchCategory, "Sort By"))
            {
                if (_context.Game.Where(j => j.gameTitle.Contains(searchPhrase)).Any())
                {
                    return View("Index", await _context.Game.Where(j => j.gameTitle.Contains(searchPhrase)).ToListAsync());
                }
                else if (_context.Game.Where(j => j.gameCategory.Contains(searchPhrase)).Any())
                {
                    return View("Index", await _context.Game.Where(j => j.gameCategory.Contains(searchPhrase)).ToListAsync());
                }
                else
                {
                    TempData["searchBoxErrors"] = "There were no results for what you searchced for";
                    return View("Index", await _context.Game.OrderBy(j => j.gameTitle).ThenBy(j => j.gameCategory).ToListAsync());
                }
            }
            else if (String.IsNullOrEmpty(searchPhrase) && !String.IsNullOrEmpty(selectedSearchCategory) || !String.IsNullOrEmpty(selectedSearchCategory) && !String.IsNullOrEmpty(searchPhrase))
            {
                switch (isSearchEmpty)
                {
                    case true:
                        switch (selectedSearchCategory)
                        {
                            case "A-Z":
                                return View("Index", await _context.Game.OrderBy(j => j.gameTitle).ToListAsync());
                            case "Z-A":
                                return View("Index", await _context.Game.OrderByDescending(j => j.gameTitle).ToListAsync());
                            case "Newest":
                                return View("Index", await _context.Game.OrderBy(j => j.gameReleaseDate).ToListAsync());
                            case "Cheapest":
                                return View("Index", await _context.Game.OrderBy(j => j.gamePrice).ThenBy(j => j.gameTitle).ToListAsync());
                            case "Expensive":
                                return View("Index", await _context.Game.OrderByDescending(j => j.gamePrice).ThenBy(j => j.gameTitle).ToListAsync());
                            default:
                                return View(await _context.Game.OrderBy(j => j.gameTitle).ThenBy(j => j.gameCategory).ToListAsync());
                                
                        }
                        
                    case false:
                        switch (selectedSearchCategory)
                        {
                            case "A-Z":
                                return View("Index", await _context.Game.OrderBy(j => j.gameTitle).Where(j => j.gameTitle.Contains(searchPhrase)).ToListAsync());
                            case "Z-A":
                                return View("Index", await _context.Game.OrderByDescending(j => j.gameTitle).Where(j => j.gameTitle.Contains(searchPhrase)).ToListAsync());
                            case "Newest":
                                return View("Index", await _context.Game.OrderBy(j => j.gameReleaseDate).Where(j => j.gameTitle.Contains(searchPhrase)).ToListAsync());
                            case "Cheapest":
                                return View("Index", await _context.Game.OrderBy(j => j.gamePrice).ThenBy(j => j.gameTitle).Where(j => j.gameTitle.Contains(searchPhrase)).ToListAsync());
                            case "Expensive":
                                return View("Index", await _context.Game.OrderByDescending(j => j.gamePrice).ThenBy(j => j.gameTitle).Where(j => j.gameTitle.Contains(searchPhrase)).ToListAsync());
                            default:
                                return View(await _context.Game.OrderBy(j => j.gameTitle).ThenBy(j => j.gameCategory).ToListAsync());
                        }                                              
                }
             
                     
            }
            else
            {
                TempData["searchBoxErrors"] = "You must enter something in order to search for something";
                return View("Index", await _context.Game.OrderBy(j => j.gameTitle).ThenBy(j => j.gameCategory).ToListAsync());
            }
        }

    }
}
