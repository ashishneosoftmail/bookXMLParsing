using BookXMLParsing.Application.Features.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookXMLParsing.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// GetAllBooks Parses XML and sends response with valid and invalid object.
        /// </summary>
        /// <returns>JSON</returns>
        [HttpGet("valid")]
        public async Task<IActionResult> GetAllBooks()
        {
            var result = await _mediator.Send(new GetAllBooks());
            return Ok(result);
        }
    }
}