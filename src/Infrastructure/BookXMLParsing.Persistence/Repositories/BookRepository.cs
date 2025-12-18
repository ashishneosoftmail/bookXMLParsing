using BookXMLParsing.Application.Contracts.Persistence;
using BookXMLParsing.Application.DTO;
using BookXMLParsing.Application.Features.Query;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Xml;
using System.Xml.Linq;

namespace BookXMLParsing.Persistence.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<GetAllBooksHandler> _logger;

        public BookRepository(IConfiguration configuration, IWebHostEnvironment environment, ILogger<GetAllBooksHandler> logger)
        {
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
        }
        /// <summary>
        /// GetAllBookAsync task parses XML with Books data and sends resposne with valid and invalid books
        /// </summary>
        /// <returns>List</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public async Task<List<ResponseDTO>> GetAllBookAsync()
        {
            var validBooks = new List<BookDTO>();
            var invalidBooks = new List<InvalidBookDTO>();
            var response = new List<ResponseDTO>();

            try
            {
                // Get full path from configuration
                string xmlFileName = _configuration["XmlFileName"];

                if (string.IsNullOrWhiteSpace(xmlFileName))
                {
                    _logger.LogError("XML file name is not configured.");
                    throw new FileNotFoundException("XML file name is not configured in appsettings.");
                }

                string fullPath = Path.Combine(_environment.WebRootPath, xmlFileName);

                if (!File.Exists(fullPath))
                {
                    _logger.LogError("XML file not found at path: {FullPath}", fullPath);
                    throw new FileNotFoundException($"XML file not found at path: {fullPath}");
                }

                using var reader = XmlReader.Create(fullPath, new XmlReaderSettings { Async = true });

                _logger.LogInformation("Xml file loaded successfully");

                //Parse XML nodes
                while (await reader.ReadAsync())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "book")
                    {
                        string title = null, author = null, genre = null, publisher = null;
                        int year = 0;

                        using (var subReader = reader.ReadSubtree())
                        {
                            while (subReader.Read())
                            {
                                if (subReader.NodeType != XmlNodeType.Element) continue;

                                switch (subReader.Name)
                                {
                                    case "title":
                                        title = subReader.ReadElementContentAsString();
                                        break;
                                    case "author":
                                        author = subReader.ReadElementContentAsString();
                                        break;
                                    case "genre":
                                        genre = subReader.ReadElementContentAsString();
                                        break;
                                    case "year":
                                        year = subReader.ReadElementContentAsInt();
                                        break;
                                    case "publisher":
                                        publisher = subReader.ReadElementContentAsString();
                                        break;
                                }
                            }
                        }

                        // Validate year
                        if (year <= 0)
                        {
                            invalidBooks.Add(new InvalidBookDTO
                            {
                                Title = title,
                                Reason = "Invalid year"
                            });
                            continue;
                        }

                        // Validate publisher URL
                        if (!Uri.TryCreate(publisher, UriKind.Absolute, out var uri) ||
                            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                        {
                            invalidBooks.Add(new InvalidBookDTO
                            {
                                Title = title,
                                Reason = "Invalid publisher"
                            });
                            continue;
                        }

                        // Valid book
                        validBooks.Add(new BookDTO
                        {
                            Title = title,
                            Author = author,
                            Genre = genre,
                            Year = year,
                            Publisher = publisher
                        });
                    }
                }
                //Response with Valid and Invalid book data
                response.Add(new ResponseDTO
                {
                    validBooks = validBooks,
                    invalidBooks = invalidBooks
                });

                _logger.LogInformation("XML file parsed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred while parsing books data: " + Convert.ToString(ex.Message));
            }

            return response;
        }
    }
}