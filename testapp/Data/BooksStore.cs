using TestApp.Models;

namespace TestApp.Data
{
    public static class BooksStore
    {
        public static List<Book> books = new List<Book>
        {
                new Book { Id = 1, Title = "Sura", Description = "Suras book."},
                new Book { Id = 2, Title = "Muna", Description = "Munas book." }
        };

    }
};
