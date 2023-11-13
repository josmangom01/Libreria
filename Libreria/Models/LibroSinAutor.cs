namespace Libreria.Models
{
    public class LibroSinAutor
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public int Chapters { get; set; }

        public int Pages { get; set; }

        public double Price { get; set; }
    }
}
