using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Libreria.Data;
using Libreria.Models;

namespace Libreria.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class LibrosController : ControllerBase {
        private readonly LibreriaContext _context;

        public LibrosController(LibreriaContext context) {
            _context = context;
        }

        // ** OBTENER TODOS LOS LIBROS **
        // GET: api/Libros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetLibro() {

            var query = from a in _context.Libro
                        select new Libro {
                            Id = a.Id,
                            Title = a.Title,
                            Chapters = a.Chapters,
                            Pages = a.Pages,
                            Price = a.Price,
                            Autor = a.Autor
                        };

            if (_context.Libro == null) return NotFound();
            return await query.ToListAsync();
        }

        // ** CONSULTAR LIBRO **
        // GET: api/Libros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetLibro(int id) {
            if (_context.Libro == null) return NotFound();
            var libro = await _context.Libro.FindAsync(id);
            if (libro == null) return NotFound();
            var autor = await _context.Autor.FindAsync(libro.Id);
            libro.Autor = autor;
            return libro;
        }

        // ** NUEVO LIBRO **  Falta
        // POST: api/Libros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Libro>> PostLibro(LibroSinAutor libro, string IdAutor) {
            var autor = await _context.Autor.FindAsync(IdAutor);
            if (autor == null) return Problem("autor is null.");
            await _context.AddAsync( new Libro {
                Id = libro.Id,
                Title = libro.Title,
                Chapters = libro.Chapters,
                Pages = libro.Pages,
                Price = libro.Price,
                Autor = autor
            });
            _context.SaveChanges();
            return CreatedAtAction("GetLibro", new { id = libro.Id }, libro);
        }

        // ?? ELIMINAR LIBRO ??
        // DELETE: api/Libros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLibro(int id) {
            if (_context.Libro == null) return NotFound();
            var libro = await _context.Libro.FindAsync(id);
            if (libro == null) return NotFound();
            _context.Libro.Remove(libro);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("Filtrar")]
        public async Task<ActionResult<IEnumerable<Libro>>> Filtrar(string cadena) {
            var query = from a in _context.Libro
                        select new Libro
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Chapters = a.Chapters,
                            Pages = a.Pages,
                            Price = a.Price,
                            Autor = a.Autor
                        };
            return await query.Where(s => s.Title.Contains(cadena)).ToListAsync();
        }
    }
}
