using CalculadoraRest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraRest.Business
{
    public interface IBookBusiness
    {
        Book Create(Book book);
        Book Update(Book book);

        Book FindByID(long id);

        List<Book> FindAll();

        void Delete(long id);

    }
}
