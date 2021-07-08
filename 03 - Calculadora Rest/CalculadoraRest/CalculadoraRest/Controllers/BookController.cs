using CalculadoraRest.Business;
using CalculadoraRest.Data.VO;
using CalculadoraRest.Hypermedia.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CalculadoraRest.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Authorize("Bearer")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class BookController : ControllerBase
    {

        private readonly ILogger<BookController> _logger;
        private IBookBusiness _bookService;

        public BookController(ILogger<BookController> logger, IBookBusiness bookBusiness)
        {
            _logger = logger;
            _bookService = bookBusiness;
        }

        [HttpGet]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_bookService.FindAll());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var book = _bookService.FindByID(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }


        [HttpPost]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] BookVO book)
        {
            if (book == null)
                return BadRequest();

            return Ok(_bookService.Create(book));
        }


        [HttpPut]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] BookVO book)
        {
            if (book == null)
                return BadRequest();

            return Ok(_bookService.Update(book));
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _bookService.Delete(id);

            return NoContent();
        }

        private bool IsNumeric(string strNumber)
        {
            double number;
            bool isNumber = double.TryParse(strNumber, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out number);
            return isNumber;
        }

        private decimal ConvertToDecimal(string strNumber)
        {
            decimal decimalValue;
            if (decimal.TryParse(strNumber, out decimalValue))
                return decimalValue;

            return 0;
        }
    }
}


