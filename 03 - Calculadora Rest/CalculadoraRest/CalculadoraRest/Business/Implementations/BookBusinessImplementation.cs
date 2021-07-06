using CalculadoraRest.Data.Converter.Implementation;
using CalculadoraRest.Data.VO;
using CalculadoraRest.Model;
using CalculadoraRest.Repository;
using System.Collections.Generic;

namespace CalculadoraRest.Business.Implementations
{
    public class BookBusinessImplementation : IBookBusiness
    {
        private readonly IRepository<Book> _repository;
        private readonly BookConverter _converter;

        public BookBusinessImplementation(IRepository<Book> context)
        {
            _repository = context;
            _converter = new BookConverter();
        }

        public List<BookVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public BookVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }

        public BookVO Create(BookVO book)
        {
            var entity = _converter.Parse(book);
            entity = _repository.Create(entity);
            return _converter.Parse(entity);
        }

        public BookVO Update(BookVO book)
        {
            var entity = _converter.Parse(book);
            entity = _repository.Update(entity);
            return _converter.Parse(entity);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
