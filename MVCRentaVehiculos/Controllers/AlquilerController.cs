using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCRentaVehiculos.Data;
using MVCRentaVehiculos.Models;
using static Humanizer.On;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;

namespace MVCRentaVehiculos.Controllers
{
    public class AlquilerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverter _converter;

        public AlquilerController(ApplicationDbContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        public IActionResult MostrarPDFenPagina()
        {
            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");
            url_pagina = $"{url_pagina}/Alquiler/Reportes";


            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings(){
                        Page = url_pagina
                    }
                }

            };

            var archivoPDF = _converter.Convert(pdf);


            return File(archivoPDF, "application/pdf");
        }

        public IActionResult DescargarPDF()
        {
            string pagina_actual = HttpContext.Request.Path;
            string url_pagina = HttpContext.Request.GetEncodedUrl();
            url_pagina = url_pagina.Replace(pagina_actual, "");
            url_pagina = $"{url_pagina}/Alquiler/Reportes";


            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings()
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings(){
                        Page = url_pagina
                    }
                }
            };

            var archivoPDF = _converter.Convert(pdf);
            string nombrePDF = "reporte" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";

            return File(archivoPDF, "application/pdf", nombrePDF);
        }

        public async Task<IActionResult> Reportes()
        {

            var alquiler = from c in _context.Alquiler.Include(c => c.Vehiculo).Include(c => c.Cliente) select c;

            return View(await alquiler.ToListAsync());
        }

        // GET: Alquiler
        public async Task<IActionResult> Index(String fechaIni, String fechaFin, String buscar)
        {
            ViewData["AlquilerId"] = new SelectList(_context.Cliente, "Id", "NombreCompleto");
            var alquileres = from c in _context.Alquiler.Include(c => c.Vehiculo).Include(c => c.Cliente) select c;
            if (!String.IsNullOrEmpty(fechaIni) && !String.IsNullOrEmpty(fechaFin))
            {
                alquileres = alquileres.Where(s => s.FechaAlquiler >= DateTime.Parse(fechaIni) && s.FechaDevolucion <= DateTime.Parse(fechaFin) || s.Cliente.Nombres.Contains(buscar));
            }
            if (!String.IsNullOrEmpty(buscar))
            {
                alquileres = alquileres.Where(s => s.Cliente.Apellidos.Contains(buscar) || s.Cliente.Nombres.Contains(buscar));
            }
            return View(await alquileres.ToListAsync());
        }

        // GET: Alquiler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Alquiler == null)
            {
                return NotFound();
            }

            var alquiler = await _context.Alquiler
                .Include(a => a.Cliente)
                .Include(a => a.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alquiler == null)
            {
                return NotFound();
            }

            return View(alquiler);
        }

        // GET: Alquiler/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "NombreCompleto");
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculo, "Id", "Placa");
            return View();
        }

        // POST: Alquiler/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaAlquiler,FechaDevolucion,VehiculoId,ClienteId,Observaciones,Importe")] Alquiler alquiler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(alquiler);
                await _context.SaveChangesAsync();
                TempData["success"] = "Creado Satisfactoriamente";
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "NombreCompleto", alquiler.ClienteId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculo, "Id", "Placa", alquiler.VehiculoId);
            return View(alquiler);
        }

        // GET: Alquiler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Alquiler == null)
            {
                return NotFound();
            }

            var alquiler = await _context.Alquiler.FindAsync(id);
            if (alquiler == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Apellidos", alquiler.ClienteId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculo, "Id", "Color", alquiler.VehiculoId);
            return View(alquiler);
        }

        // POST: Alquiler/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaAlquiler,FechaDevolucion,VehiculoId,ClienteId,Observaciones,Importe")] Alquiler alquiler)
        {
            if (id != alquiler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alquiler);
                    TempData["success"] = "Editado Satisfactoriamente";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlquilerExists(alquiler.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "Id", "Apellidos", alquiler.ClienteId);
            ViewData["VehiculoId"] = new SelectList(_context.Vehiculo, "Id", "Color", alquiler.VehiculoId);
            return View(alquiler);
        }

        // GET: Alquiler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Alquiler == null)
            {
                return NotFound();
            }

            var alquiler = await _context.Alquiler
                .Include(a => a.Cliente)
                .Include(a => a.Vehiculo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alquiler == null)
            {
                return NotFound();
            }

            return View(alquiler);
        }

        // POST: Alquiler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Alquiler == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Alquiler'  is null.");
            }
            var alquiler = await _context.Alquiler.FindAsync(id);
            if (alquiler != null)
            {
                _context.Alquiler.Remove(alquiler);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Eliminado Satisfactoriamente";
            return RedirectToAction(nameof(Index));
        }

        private bool AlquilerExists(int id)
        {
            return _context.Alquiler.Any(e => e.Id == id);
        }
    }
}
