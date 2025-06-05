
using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects
{
    public class PagerParams 
    {
        private string _searchKeyWord = "";
        public string SearchKeyWord
        {
            set
            {
                _searchKeyWord = value;
            }
            get
            {
                return _searchKeyWord;
            }
        }

        private int _currentPage = 1;    


        private int _recordsPerPage = 10;

        public PagerParams()
        {
            
        }

        public int RecordsPerPage
        {
            get
            {
                return _recordsPerPage;
            }
            set
            {
                _recordsPerPage = value;
            }
        }

        public int CurrentPage { get => _currentPage; set => _currentPage = value; }

        public string ToQueryString()
        {
            return $"?CurrentPage={CurrentPage}&RecordsPerPage={RecordsPerPage}&SearchKeyWord={SearchKeyWord}"
                .ToLower(); 
        }
    }
}
