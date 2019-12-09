using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Connect4.Data;
using Connect4.Models;
using Microsoft.AspNetCore.Authorization;

namespace Connect4.Controllers
{
    public class TorneiosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TorneiosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Torneios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Torneio.ToListAsync());
        }

        [Authorize]
        // GET: Torneios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Torneios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,NomeTorneio,QuantidadeJogadores,Premiacao,Inicio")] Torneio torneio)
        {
            if (ModelState.IsValid)
            {
                torneio.Dono = User.Identity.Name;
                _context.Add(torneio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(torneio);
        }

        // GET: Torneios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var torneio = await _context.Torneio.SingleOrDefaultAsync(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }
            return View(torneio);
        }

        public IActionResult SelecionarJogadores(int id)
        {
            var torneio = _context.Torneio.Include(t => t.Jogadores)
                .SingleOrDefault(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }

            SelecionarUsuarioViewModel viewModel = new SelecionarUsuarioViewModel();

            List<int> jogadores = new List<int>();
            if (torneio.Jogadores != null)
            {
                jogadores = torneio.Jogadores.Select(j => j.Id).ToList();
            }
            ViewBag.Jogadores =
                new SelectList(_context.JogadorPessoas.Include(j => j.Usuario).ToList(),
                nameof(JogadorPessoa.Id),
                nameof(JogadorPessoa.Nome),
                jogadores
                );
            viewModel.JogadoresIds = jogadores;
            return View(viewModel);
        }

        public IActionResult JogadorResultados(int id)
        {
            var torneio = _context.Torneio.Include(t => t.Jogadores).SingleOrDefault(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }
            List<JogadorResultados> jogadorResultados = new List<JogadorResultados>();
            if (torneio.Jogadores != null)
            {
                jogadorResultados = _context.JogadorResultados.Include(j => j.Jogador).Where(j => j.Torneio.Id == id).ToList();
            }
            foreach (var item in jogadorResultados)
            {
                if(item.Jogador is JogadorPessoa)
                {
                    item.Jogador = _context.JogadorPessoas
                                .Include(j => j.Usuario)
                                .Where(j => j.Id == item.Jogador.Id)
                                .FirstOrDefault();
                }
            }
            ViewBag.JogadorResultados = jogadorResultados;
            return View(jogadorResultados);
        }

        [HttpPost]
        public IActionResult SelecionarJogadores(
            int id,
            [Bind(nameof(SelecionarUsuarioViewModel.JogadoresIds))] SelecionarUsuarioViewModel viewModel)
        {
            var torneio = _context.Torneio.SingleOrDefault(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }

            var jogadores = _context.JogadorPessoas.Where(
                jp => viewModel.JogadoresIds.Exists(j => j == jp.Id))
                .ToList();
            foreach (var item in jogadores)
            {
                torneio.Jogadores.Add(item);
            }
            _context.SaveChanges();

            if (torneio.QuantidadeJogadores == torneio.Jogadores.Count())
            {
                for (int i = 0; i < torneio.QuantidadeJogadores - 1; i++)
                {
                    for (int j = 0; j < (torneio.QuantidadeJogadores - 1) - i; j++)
                    {
                        Jogo Jogo = new Jogo();
                        Jogo.Jogador1 = torneio.Jogadores[i];
                        Jogo.Jogador2 = torneio.Jogadores[j + i + 1];
                        torneio.Jogos.Add(Jogo);
                        _context.Add(Jogo);
                    }
                }
            }
            for (int i = 0; i < torneio.QuantidadeJogadores; i++)
            {
                JogadorResultados JogadorResultados = new JogadorResultados();
                JogadorResultados.Torneio = torneio;
                JogadorResultados.Jogador = torneio.Jogadores[i];
                _context.Add(JogadorResultados);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // POST: Torneios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeTorneio,QuantidadeJogadores,Premiacao,Inicio")] Torneio torneio)
        {
            if (id != torneio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(torneio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TorneioExists(torneio.Id))
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
            return View(torneio);
        }

        // GET: Torneios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var torneio = await _context.Torneio
                .SingleOrDefaultAsync(m => m.Id == id);
            if (torneio == null)
            {
                return NotFound();
            }
            if (User.Identity.Name != torneio.Dono)
            {
                return Forbid();
            }
            return View(torneio);
        }

        // POST: Torneios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var torneio = await _context.Torneio.SingleOrDefaultAsync(m => m.Id == id);
            if (User.Identity.Name != torneio.Dono)
            {
                return Forbid();
            }
            _context.Torneio.Remove(torneio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TorneioExists(int id)
        {
            return _context.Torneio.Any(e => e.Id == id);
        }
    }
}
