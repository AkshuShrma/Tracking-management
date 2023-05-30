using Tracker.Models;

namespace Tracker.Repository
{
    public interface IBookRepo
    {
        Task<List<Book>> GetBooks();
        Task<Book> GetBookById(int id);
        Task AddBook(Book book);
        Task UpdateBook( Book book);
        Task DeleteBook(int id);
    }
}
