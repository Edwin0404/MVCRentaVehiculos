using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCRentaVehiculos.Data;
using MVCRentaVehiculos.Models;

namespace MVCRentaVehiculos.Controllers
{
    public class TypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tipo
        public async Task<IActionResult> Index(string buscar)
        {
            var tipo = from Tipo in _context.Tipo select Tipo;
            if (!String.IsNullOrEmpty(buscar))
            {
                tipo = tipo.Where(s => s.Nombre!.Contains(buscar));
            }
            return View(await tipo.ToListAsync());
        }

        // GET: Tipo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tipo == null)
            {
                return NotFound();
            }

            var tipo = await _context.Tipo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipo == null)
            {
                return NotFound();
            }

            return View(tipo);
        }

        // GET: Tipo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tipo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Tipo tipo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipo);
                await _context.SaveChangesAsync();
                TempData["success"] = "Creado Satisfactoriamente";
                return RedirectToAction(nameof(Index));
            }
            return View(tipo);
        }

        // GET: Tipo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tipo == null)
            {
                return NotFound();
            }

            var tipo = await _context.Tipo.FindAsync(id);
            if (tipo == null)
            {
                return NotFound();
            }
            return View(tipo);
        }

        // POST: Tipo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Tipo tipo)
        {
            if (id != tipo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipo);
                    TempData["success"] = "Editado Satisfactoriamente";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoExists(tipo.Id))
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
            return View(tipo);
        }

        // GET: Tipo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tipo == null)
            {
                return NotFound();
            }

            var tipo = await _context.Tipo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tipo == null)
            {
                return NotFound();
            }

            return View(tipo);
        }

        // POST: Tipo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tipo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tipo'  is null.");
            }
            var tipo = await _context.Tipo.FindAsync(id);
            if (tipo != null)
            {
                _context.Tipo.Remove(tipo);
            }
            
            await _context.SaveChangesAsync();
            TempData["success"] = "Eliminado Satisfactoriamente";
            return RedirectToAction(nameof(Index));
        }

        private bool TipoExists(int id)
        {
          return _context.Tipo.Any(e => e.Id == id);
        }
    }
}
