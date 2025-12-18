namespace BookXMLParsing.Application.DTO
{
    public class BookDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string? Genre { get; set; }
        public int Year { get; set; }
        public string? Publisher { get; set; }
    }
}
