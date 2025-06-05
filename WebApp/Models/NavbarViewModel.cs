namespace WebApp.Models
{
    public class NavbarViewModel
    {
        public int RecordsPerPage { get; set; }
        public string SearchKeyWord { get; set; }
        public bool ShowCreateNewButton { get; set; }  = false;
    }
}
