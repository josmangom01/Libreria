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
    public class AutoresController : ControllerBase {
        private readonly LibreriaContext _context;

        public AutoresController(LibreriaContext context) {
            _context = context;
        }

        // GET: api/Autores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor_Libros>>> GetAutor() {
            if (_context.Autor == null) return NotFound();

            var Autores = from a in _context.Autor
                          select new Autor_Libros {
                              Id = a.Id,
                              Name = a.Name,
                              Libros = (from Libro in _context.Libro
                                      where Libro.Autor.Name.Equals(a.Name)
                                      select new LibroSinAutor {
                                          Id = Libro.Id,
                                          Title = Libro.Title,
                                          Chapters = Libro.Chapters,
                                          Pages = Libro.Pages,
                                          Price = Libro.Price
                                      }).ToList()
                          };


            var Libros = from Libro in _context.Libro
                        where Libro.Autor.Name.Equals("Pedro Ramírez")
                        select Libro;


            return await Autores.ToListAsync();
        }

        // ** CREAR UN AUTOR **
        // POST: api/Autores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Autor>> PostAutor(Autor autor) {
            if (_context.Autor == null) {
                return Problem("Entity set 'LibreriaContext.Autor'  is null.");
            }
            _context.Autor.Add(autor);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAutor", new { id = autor.Id }, autor);
        }

        // ?? ELIMINAR UN AUTOR ??
        // DELETE: api/Autores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutor(int id) {
            if (_context.Autor == null) return NotFound();
            var autor = await _context.Autor.FindAsync(id);
            if (autor == null) return NotFound();
            _context.Autor.Remove(autor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
