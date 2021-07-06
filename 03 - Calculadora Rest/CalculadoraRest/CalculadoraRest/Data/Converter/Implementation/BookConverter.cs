using CalculadoraRest.Data.Converter.Contract;
using CalculadoraRest.Data.VO;
using CalculadoraRest.Model;
using System.Collections.Generic;
using System.Linq;

namespace CalculadoraRest.Data.Converter.Implementation
{
    public class BookConverter : IParser<Book, BookVO>, IParser<BookVO, Book>
    {
        public Book Parse(BookVO origin)
        {
            if (origin == null) return null;

            return new Book
            {
                Id = origin.Id,
                Author = origin.Author,
                LauchDate = origin.LauchDate,
                Price = origin.Price,
                Title = origin.Title
            };
        }

        public BookVO Parse(Book origin)
        {
            if (origin == null) return null;

            return new BookVO
            {
                Id = origin.Id,
                Author = origin.Author,
                LauchDate = origin.LauchDate,
                Price = origin.Price,
                Title = origin.Title
            };
        }

        public List<Book> Parse(List<BookVO> origin)
        {
            if (origin == null) return null;

            return origin.Select(_ => Parse(_)).ToList();
        }

        public List<BookVO> Parse(List<Book> origin)
        {
            if (origin == null) return null;

            return origin.Select(_ => Parse(_)).ToList();
        }
    }
}
