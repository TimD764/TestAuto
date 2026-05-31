namespace EhuAutomationNUnit.Models
{
    public class SearchContext
    {
        public string SearchTerm { get; set; } = string.Empty;
        public string ExpectedSubString { get; set; } = string.Empty;
    }

    public class SearchContextBuilder
    {
        private readonly SearchContext _context = new SearchContext();

        public SearchContextBuilder WithSearchTerm(string term)
        {
            _context.SearchTerm = term;
            // Automatically format the expected URL query string safely
            _context.ExpectedSubString = term.Replace(" ", "+");
            return this;
        }

        public SearchContext Build()
        {
            return _context;
        }
    }
}