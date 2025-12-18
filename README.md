This repository contains a high-performance .NET API that parses a large XML file containing <book> records, validates them based on defined rules, and returns valid and invalid books in a single non-paginated response.

-----Problem Overview----
The API processes an XML file with thousands of <book> elements and:
Parses the XML efficiently
Applies validation rules
Returns structured JSON output
Avoids unnecessary memory allocations
Is suitable for large XML files on a local development machine

-----Validation Rules-----
A book is considered valid if:
year is a valid integer greater than 0
publisher is a well-formed HTTP or HTTPS URL
All other records are treated as invalid with a failure reason.

-----API Endpoint------
GET /books/valid
