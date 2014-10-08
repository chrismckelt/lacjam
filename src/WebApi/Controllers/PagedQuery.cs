namespace Lacjam.WebApi.Controllers
{
    public class PagedQuery
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string Q { get; set; }
    }
}