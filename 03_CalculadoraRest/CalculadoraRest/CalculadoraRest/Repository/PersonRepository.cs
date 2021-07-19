using CalculadoraRest.Model;
using CalculadoraRest.Model.Context;
using CalculadoraRest.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraRest.Repository
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(MySqlContext context) : base(context)
        {

        }
        public Person Disable(long id)
        {
            if (!_context.Persons.Any(p => p.Id.Equals(id))) return null;

            var user = _context.Persons.SingleOrDefault(p => p.Id.Equals(id));

            if (user != null)
            {
                user.Enabled = false;
                try
                {
                    _context.Entry(user).CurrentValues.SetValues(user);
                    _context.SaveChanges();
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return user;
        }

        public List<Person> FindByName(string firtName, string lastName)
        {
            if (!string.IsNullOrEmpty(firtName) && !string.IsNullOrEmpty(lastName))
            {
                return _context.Persons
               .Where(p => p.FirtName.Contains(firtName)
               && p.LastName.Contains(lastName)).ToList();
            }else
            if (!string.IsNullOrEmpty(firtName))
            {
                return _context.Persons
               .Where(p => p.FirtName.Contains(firtName)).ToList();
            }
            else
            if (!string.IsNullOrEmpty(lastName))
            {
                return _context.Persons
               .Where(p => p.LastName.Contains(lastName)).ToList();
            }

            return null;
        }
    }
}
