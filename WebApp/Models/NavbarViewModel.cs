namespace WebApp.Models
{
    public class NavbarViewModel
    {
        public int RecordsPerPage { get; set; }
        public string SearchKeyWord { get; set; }
        public bool ShowCreateNewButton { get; set; } = false;

        public string ActionName { get; set; }
        public string ControllerName { get; set; }
    }
}
