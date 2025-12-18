namespace BookXMLParsing.Application.DTO
{
    public class ResponseDTO
    {
        public List<BookDTO> validBooks { get; set; } = new();
        public List<InvalidBookDTO> invalidBooks { get; set; } = new();
    }
}
