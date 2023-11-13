namespace Libreria.Models
{
    public class Autor_Libros
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public virtual ICollection<LibroSinAutor> Libros { get; set; } = new List<LibroSinAutor>();
    }
}
