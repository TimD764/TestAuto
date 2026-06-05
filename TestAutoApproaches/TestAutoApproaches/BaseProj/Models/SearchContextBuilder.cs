
namespace TestAutoApproaches.Models
{
    public class SearchContextBuilder
    {
        private readonly SearchContext _context = new SearchContext();

        public SearchContextBuilder WithSearchTerm(string term)
        {
            _context.SearchTerm = term;
            // formatting
            _context.ExpectedSubString = term.Replace(" ", "+");
            return this;
        }

        public SearchContext Build()
        {
            return _context;
        }
    }
}