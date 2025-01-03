using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MVCRentaVehiculos.Data;
using MVCRentaVehiculos.Models;

namespace MVCRentaVehiculos.Controllers
{
    public class VehiculoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VehiculoController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Vehiculo
        public async Task<IActionResult> Index(string buscar)
        {
            var vehiculo = from c in _context.Vehiculo.Include(c => c.Marca).Include(c => c.Tipo) select c;
            if (!string.IsNullOrEmpty(buscar))
            {
                vehiculo = vehiculo.Where(s => s.Tipo.Nombre.Contains(buscar) || s.Marca.Nombre.Contains(buscar) || s.Placa.Contains(buscar));
            }
            return View(await vehiculo.ToListAsync());
        }

        // GET: Vehiculo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vehiculo == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculo
                .Include(v => v.Marca)
                .Include(v => v.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        //// GET: Vehiculo/Create
        //public IActionResult Create()
        //{
        //    ViewData["MarcaId"] = new SelectList(_context.Marca, "Id", "Nombre");
        //    ViewData["TipoId"] = new SelectList(_context.Tipo, "Id", "Nombre");
        //    return View();
        //}

        //// POST: Vehiculo/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Color,Placa,TipoId,MarcaId")] Vehiculo vehiculo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(vehiculo);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MarcaId"] = new SelectList(_context.Marca, "Id", "Nombre", vehiculo.MarcaId);
        //    ViewData["TipoId"] = new SelectList(_context.Tipo, "Id", "Nombre", vehiculo.TipoId);
        //    return View(vehiculo);
        //}

        //// GET: Vehiculo/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null || _context.Vehiculo == null)
        //    {
        //        return NotFound();
        //    }

        //    var vehiculo = await _context.Vehiculo.FindAsync(id);
        //    if (vehiculo == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["MarcaId"] = new SelectList(_context.Marca, "Id", "Nombre", vehiculo.MarcaId);
        //    ViewData["TipoId"] = new SelectList(_context.Tipo, "Id", "Nombre", vehiculo.TipoId);
        //    return View(vehiculo);
        //}

        //// POST: Vehiculo/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Color,Placa,TipoId,MarcaId")] Vehiculo vehiculo)
        //{
        //    if (id != vehiculo.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(vehiculo);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!VehiculoExists(vehiculo.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["MarcaId"] = new SelectList(_context.Marca, "Id", "Nombre", vehiculo.MarcaId);
        //    ViewData["TipoId"] = new SelectList(_context.Tipo, "Id", "Nombre", vehiculo.TipoId);
        //    return View(vehiculo);
        //}

        //get Crear y Editar (Upsert)
        public IActionResult Upsert(int? id)
        {

            IEnumerable<SelectListItem> tipoLista = _context.Tipo.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString()

            });
            IEnumerable<SelectListItem> marcaLista = _context.Marca.Select(c => new SelectListItem
            {
                Text = c.Nombre,
                Value = c.Id.ToString()

            });
            ViewBag.TipoLista = tipoLista;
            ViewBag.MarcaLista = marcaLista;
            Vehiculo vehiculo = new Vehiculo();
            if (id == null)
            {
                //Crear
                TempData["success"] = "Creado Satisfactoriamente";
                return View(vehiculo);
            }
            else
            {
                //editar
                vehiculo = _context.Vehiculo.Find(id);
                if (vehiculo == null)
                {
                    return NotFound();
                }
                TempData["success"] = "Editado Satisfactoriamente";
                return View(vehiculo);
            }
        }
        //post Crear y Editar (Upsert)
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (vehiculo.Id == 0)
                {
                    //crear
                    string upload = webRootPath + WC.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    vehiculo.ImagenUrl = fileName + extension;
                    _context.Vehiculo.Add(vehiculo);
                }
                else
                {
                    //editar
                    var objVehiculo = _context.Vehiculo.Find(vehiculo.Id);

                    if (files.Count > 0) //Usuario esta actualizando la imagen
                    {
                        string upload = webRootPath + WC.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var anteriorFile = Path.Combine(upload, objVehiculo.ImagenUrl);
                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        vehiculo.ImagenUrl = fileName + extension;
                    }
                    else //El usuario está actualizando otros datos, no la imagen
                    {
                        vehiculo.ImagenUrl = objVehiculo.ImagenUrl;
                    }
                    _context.Update(vehiculo);
                }
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehiculo);
        }

        // GET: Vehiculo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vehiculo == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculo
                .Include(v => v.Marca)
                .Include(v => v.Tipo)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // POST: Vehiculo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vehiculo == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vehiculo'  is null.");
            }
            var vehiculo = await _context.Vehiculo.FindAsync(id);
            if (vehiculo != null)
            {
                _context.Vehiculo.Remove(vehiculo);
            }

            await _context.SaveChangesAsync();
            TempData["success"] = "Eliminado Satisfactoriamente";
            return RedirectToAction(nameof(Index));
        }

        private bool VehiculoExists(int id)
        {
            return _context.Vehiculo.Any(e => e.Id == id);
        }
    }
}
