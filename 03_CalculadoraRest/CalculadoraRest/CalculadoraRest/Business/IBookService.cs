using CalculadoraRest.Data.VO;
using System.Collections.Generic;

namespace CalculadoraRest.Business
{
    public interface IBookBusiness
    {
        BookVO Create(BookVO book);
        BookVO Update(BookVO book);

        BookVO FindByID(long id);

        List<BookVO> FindAll();

        void Delete(long id);

    }
}
