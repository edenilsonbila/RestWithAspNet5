using CalculadoraRest.Data.Converter.Contract;
using CalculadoraRest.Data.VO;
using CalculadoraRest.Model;
using System.Collections.Generic;
using System.Linq;

namespace CalculadoraRest.Data.Converter.Implementation
{
    public class PersonConverter : IParser<PersonVO, Person>, IParser<Person, PersonVO>
    {
        public Person Parse(PersonVO origin)
        {
            if (origin == null) return null;

            return new Person
            {
                Id = origin.Id,
                Address = origin.Address,
                FirtName = origin.FirtName,
                Gender = origin.Gender,
                LastName = origin.LastName
            };
        }

        public PersonVO Parse(Person origin)
        {
            if (origin == null) return null;

            return new PersonVO
            {
                Id = origin.Id,
                Address = origin.Address,
                FirtName = origin.FirtName,
                Gender = origin.Gender,
                LastName = origin.LastName
            };
        }

        public List<Person> Parse(List<PersonVO> origin)
        {
            if (origin == null) return null;

            return origin.Select(_ => Parse(_)).ToList();
        }

        public List<PersonVO> Parse(List<Person> origin)
        {
            if (origin == null) return null;

            return origin.Select(_ => Parse(_)).ToList();
        }
    }
}
