using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoGameStore.Data;
using VideoGameStore.Models;

namespace VideoGameStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrativeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdministrativeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Returns a page of all the games
        public async Task <IActionResult>AllGames()
        {
            return View(await _context.Game.OrderBy(j => j.gameTitle).ThenBy(j => j.gameCategory).ToListAsync());
        }

        //GET method for getting the edit page
        [HttpGet]
        public async Task<IActionResult> EditGame(Guid? id)
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

        //POST method for editing a game
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditGame(Guid id, [Bind("gameGuid,gameTitle,gameDescription,gameReleaseDate,gamePrice,gameCategory,gameDeveloper,gamePublisher")] Game game)
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
                return RedirectToAction("AllGames", "Administrative");
            }
            return View(game);
        }

        //GET method for getting the details page
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

        //GET method for getting the create page
        public IActionResult CreateGame()
        {
            return View();
        }

        //POST method for creating a game
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGame([Bind("gameGuid,gameTitle,gameDescription,gameReleaseDate,gamePrice,gameCategory,gameDeveloper,gamePublisher")] Game game)
        {
            if (ModelState.IsValid)
            {
                game.gameGuid = Guid.NewGuid();
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction("AllGames", "Administrative");
            }
            return View(game);
        }

        private bool GameExists(Guid id)
        {
            return _context.Game.Any(e => e.gameGuid == id);
        }
    }
}
