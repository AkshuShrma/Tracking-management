using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Tracker.Data;
using Tracker.Models;

namespace Tracker.Repository
{
    public class BookRepo : IBookRepo
    {
        private readonly ApplicationDbContext _context;

        public BookRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddBook(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(int id)
        {
            var booksInDb = await _context.Books.FindAsync(id);
            _context.Books.Remove(booksInDb);
            await _context.SaveChangesAsync();
        }

        public async Task<Book> GetBookById(int id)
        {
            var emp = await (_context.Books.Where(v => v.BookId == id).FirstOrDefaultAsync());
            return emp;
        }

        public async Task<List<Book>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            return books;
        }

        public async Task UpdateBook( Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }
    }
}
