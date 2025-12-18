using BookXMLParsing.Application.DTO;
namespace BookXMLParsing.Application.Contracts.Persistence
{
    public interface IBookRepository
    {
        Task<List<ResponseDTO>> GetAllBookAsync();
    }
}
