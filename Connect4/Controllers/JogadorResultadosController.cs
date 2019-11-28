using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Connect4.Data;
using Connect4.Models;

namespace Connect4.Controllers
{
    public class JogadorResultadosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JogadorResultadosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: JogadorResultados
        public async Task<IActionResult> Index()
        {
            return View(await _context.JogadorResultados.ToListAsync());
        }

        // GET: JogadorResultados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jogadorResultados = await _context.JogadorResultados
                .SingleOrDefaultAsync(m => m.Id == id);
            if (jogadorResultados == null)
            {
                return NotFound();
            }

            return View(jogadorResultados);
        }

        // GET: JogadorResultados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JogadorResultados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Pontos")] JogadorResultados jogadorResultados)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jogadorResultados);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jogadorResultados);
        }

        // GET: JogadorResultados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jogadorResultados = await _context.JogadorResultados.SingleOrDefaultAsync(m => m.Id == id);
            if (jogadorResultados == null)
            {
                return NotFound();
            }
            return View(jogadorResultados);
        }

        // POST: JogadorResultados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Pontos")] JogadorResultados jogadorResultados)
        {
            if (id != jogadorResultados.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jogadorResultados);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JogadorResultadosExists(jogadorResultados.Id))
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
            return View(jogadorResultados);
        }

        // GET: JogadorResultados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jogadorResultados = await _context.JogadorResultados
                .SingleOrDefaultAsync(m => m.Id == id);
            if (jogadorResultados == null)
            {
                return NotFound();
            }

            return View(jogadorResultados);
        }

        // POST: JogadorResultados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jogadorResultados = await _context.JogadorResultados.SingleOrDefaultAsync(m => m.Id == id);
            _context.JogadorResultados.Remove(jogadorResultados);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JogadorResultadosExists(int id)
        {
            return _context.JogadorResultados.Any(e => e.Id == id);
        }
    }
}
