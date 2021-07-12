using CalculadoraRest.Data.VO;
using CalculadoraRest.Hypermedia.Utils;
using System.Collections.Generic;

namespace CalculadoraRest.Business
{
    public interface IPersonBusiness
    {
        PersonVO Create(PersonVO person);
        PersonVO Update(PersonVO person);

        PersonVO FindByID(long id);

        List<PersonVO> FindByName(string firtName, string lastName);

        List<PersonVO> FindAll();

        PersonVO Disable(long id);

        void Delete(long id);

        PagedSearchVO<PersonVO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);

    }
}
