using AutoMapper;
using BookXMLParsing.Application.Contracts.Persistence;
using BookXMLParsing.Application.DTO;
using BookXMLParsing.Domain.Common;
using MediatR;

namespace BookXMLParsing.Application.Features.Query
{
    public class GetAllBooksHandler : IRequestHandler<GetAllBooks, ApiResponse<List<ResponseDTO>>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public GetAllBooksHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<List<ResponseDTO>>> Handle(GetAllBooks request, CancellationToken cancellationToken)
        {
                var books = await _bookRepository.GetAllBookAsync();

                var result = (books != null && books.Count > 0) ? _mapper.Map<List<ResponseDTO>>(books) : new List<ResponseDTO>();

                return new ApiResponse<List<ResponseDTO>>
                {
                    Success = true,
                    Message = result.Count > 0 ? "Books retrieved successfully" : "No books found",
                    Data = result
                };
        }
    }
}