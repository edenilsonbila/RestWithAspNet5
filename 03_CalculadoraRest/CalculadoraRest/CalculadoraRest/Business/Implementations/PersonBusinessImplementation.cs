using CalculadoraRest.Data.Converter.Implementation;
using CalculadoraRest.Data.VO;
using CalculadoraRest.Hypermedia.Utils;
using CalculadoraRest.Model;
using CalculadoraRest.Model.Context;
using CalculadoraRest.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalculadoraRest.Business.Implementations
{
    public class PersonBusinessImplementation : IPersonBusiness
    {
        private readonly IPersonRepository _repository;
        private readonly PersonConverter _converter;

        public PersonBusinessImplementation(IPersonRepository repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
        }

        public List<PersonVO> FindAll()
        {
            return _converter.Parse(_repository.FindAll());
        }

        public PagedSearchVO<PersonVO> FindWithPagedSearch(
            string name, string sortDirection, int pageSize, int page)
        {
           
            var sort = (!string.IsNullOrEmpty(sortDirection) && !sortDirection.Equals("desc")) ? "asc" : "desc";
            var size = (pageSize < 1) ? 10 : pageSize;
            var offset = page > 0 ? (page - 1) * size : 0;

            string query = @"select * from person p where 1 = 1 ";
            if (!string.IsNullOrEmpty(name)) query += $"and p.first_name like '%{name}%' ";
            query += $" order by p.first_name {sort} limit {size} offset {offset}";

            string countQuery = @"select count(*) from Person p where 1 = 1 ";
            if (!string.IsNullOrEmpty(name)) countQuery += $" and p.first_name like '%{name}%' ";


            var persons = _repository.FindWithPagedSearch(query);
            int totalResults = _repository.GetCount(countQuery);

            return new PagedSearchVO<PersonVO> { 
            CurrentPage = offset,
            List = _converter.Parse(persons),
            PageSize = size,
            SortDirections = sort,
            TotalResults = totalResults
            };
        }

        public PersonVO FindByID(long id)
        {
            return _converter.Parse(_repository.FindByID(id));
        }

        public PersonVO Create(PersonVO person)
        {
            try
            {
                var personEntity = _converter.Parse(person);
                personEntity = _repository.Create(personEntity);
                return _converter.Parse(personEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PersonVO Update(PersonVO person)
        {
            try
            {
                var personEntity = _converter.Parse(person);
                personEntity = _repository.Update(personEntity);
                return _converter.Parse(personEntity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PersonVO Disable(long id)
        {
            var personEntity = _repository.Disable(id);
            return _converter.Parse(personEntity);
        }

        public void Delete(long id)
        {
            try
            {
                _repository.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<PersonVO> FindByName(string firtName, string lastName)
        {
            return _converter.Parse(_repository.FindByName(firtName,lastName));
        }

      
    }
}
