using MediatR;
using BookXMLParsing.Domain.Common;
using BookXMLParsing.Application.DTO;

namespace BookXMLParsing.Application.Features.Query
{
    public record GetAllBooks : IRequest<ApiResponse<List<ResponseDTO>>>;
}

