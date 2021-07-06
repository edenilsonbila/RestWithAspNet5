using CalculadoraRest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraRest.Repository
{
    public interface IRepository
    {
        Book Create(Book Book);
        Book Update(Book Book);

        Book FindByID(long id);

        List<Book> FindAll();

        void Delete(long id);

        bool Exists(long id);
    }
}
