namespace WebApp.Models
{
    public class PagerViewModel
    {
        public int CurrentPage { get; set; }
        public int RecordsPerPage { get; set; }
        public int PageCount { get; set; }
        public string SearchKeyWord { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
