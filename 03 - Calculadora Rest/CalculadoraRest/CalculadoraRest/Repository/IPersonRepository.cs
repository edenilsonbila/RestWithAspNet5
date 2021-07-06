using CalculadoraRest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraRest.Repository
{
    public interface IPersonRepository
    {
        Person Create(Person person);
        Person Update(Person person);

        Person FindByID(long id);

        List<Person> FindAll();

        void Delete(long id);

        bool Exists(long id);
    }
}
