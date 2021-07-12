using CalculadoraRest.Data.VO;
using System.Collections.Generic;

namespace CalculadoraRest.Business
{
    public interface IPersonBusiness
    {
        PersonVO Create(PersonVO person);
        PersonVO Update(PersonVO person);

        PersonVO FindByID(long id);

        List<PersonVO> FindAll();

        PersonVO Disable(long id);

        void Delete(long id);

    }
}
