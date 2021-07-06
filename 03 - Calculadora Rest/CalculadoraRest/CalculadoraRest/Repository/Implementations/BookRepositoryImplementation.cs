using CalculadoraRest.Model;
using CalculadoraRest.Model.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalculadoraRest.Repository.Implementations
{
    public class BookRepositoryImplementation : IBookRepository
    {
        private MySqlContext _context;

        public BookRepositoryImplementation(MySqlContext context)
        {
            _context = context;
        }

        public List<Book> FindAll()
        {
            return _context.Books.ToList();
        }

        public Book FindByID(long id)
        {
            return _context.Books.Find(id);
        }

        public Book Create(Book Book)
        {
            try
            {
                _context.Add(Book);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }

            return Book;
        }

        public Book Update(Book Book)
        {
            if (!Exists(Book.Id)) return null;

            var result = _context.Books.SingleOrDefault(_ => _.Id.Equals(Book.Id));

            if (result != null)
            {
                try
                {
                    _context.Entry(Book).CurrentValues.SetValues(Book);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return Book;
        }

        public void Delete(long id)
        {
            var result = _context.Books.SingleOrDefault(_ => _.Id.Equals(id));
            if (result != null)
            {
                try
                {
                    _context.Books.Remove(result);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }
        public bool Exists(long id)
        {
            return _context.Books.Any(_ => _.Id.Equals(id));
        }


        //private Book MockBook(int i)
        //{
        //    return new Book
        //    {
        //        Id = 1,
        //        FirtName = "Edenilson",
        //        Address = "Teste",
        //        Gender = "M",
        //        LastName = "Bila"
        //    };
        //}

        //private long IncrementAndGet()
        //{
        //    return Interlocked.Increment(ref count);
        //}
    }
}
