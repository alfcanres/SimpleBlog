namespace DataTransferObjects
{
    public class ResponsePagedList<ReadDTO> where ReadDTO : class
    {

        private  IEnumerable<ReadDTO> _list;
        private int _recordCount;
        private int _currentPage;
        private int _pageCount;
        private int _recordsPerPage;
        private string _searchKeyWord;

        public ResponsePagedList()
        {
        }

        public ResponsePagedList(IEnumerable<ReadDTO> list, int recordCount, PagerParams pagerDTO)
        {
            this.List = list;
            this.RecordCount = recordCount;
            this.CurrentPage = pagerDTO.CurrentPage;
            this.RecordsPerPage = pagerDTO.RecordsPerPage;
            this.SearchKeyWord = pagerDTO.SearchKeyWord;
            double pageCountDoub = Math.Ceiling(Convert.ToDouble((recordCount / Convert.ToDouble(pagerDTO.RecordsPerPage))));
            this.PageCount = Convert.ToInt32(pageCountDoub);
        }

        public IEnumerable<ReadDTO> List { get => _list; set => _list = value; }
        public int RecordCount { get => _recordCount; set => _recordCount = value; }
        public int CurrentPage { get => _currentPage; set => _currentPage = value; }
        public int PageCount { get => _pageCount; set => _pageCount = value; }
        public int RecordsPerPage { get => _recordsPerPage; set => _recordsPerPage = value; }
        public string SearchKeyWord { get => _searchKeyWord; set => _searchKeyWord = value; }
    }
}
