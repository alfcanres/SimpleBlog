using DataTransferObjects.Interfaces;


namespace BusinessLogicLayer.Response
{
    public class ResponsePagedListDTO<T> : IResponsePagedListDTO<T>
    {
        public IEnumerable<T> List => _list;
        public int RecordCount => _recordCount;
        public int CurrentPage => _currentPage;
        public int PageCount => _pageCount;

        private readonly IEnumerable<T> _list;
        private readonly int _recordCount;
        private readonly int _currentPage;
        private readonly int _pageCount;

        public ResponsePagedListDTO(IEnumerable<T> list, int recordCount, IPagerDTO pagerDTO)
        {
            this._list = list;
            this._recordCount = recordCount;
            this._currentPage = pagerDTO.CurrentPage;
            double pageCountDoub = Math.Ceiling(Convert.ToDouble((recordCount / Convert.ToDouble(pagerDTO.RecordsPerPage))));
            this._pageCount = Convert.ToInt32(pageCountDoub);
        }


        public static async Task<ResponsePagedListDTO<ReadDTO>> GetResponseFromQueryAsync<ReadDTO>(QueryStrategyBase<ReadDTO> queryStrategy, IPagerDTO pager) 
        {
            var list = await queryStrategy.GetResultsAsync();
            var recordCount = await queryStrategy.CountResultsAsync();
            var currentPage = pager.CurrentPage;
            double pageCountDoub = Math.Ceiling(Convert.ToDouble((recordCount / Convert.ToDouble(pager.RecordsPerPage))));
            var pageCount = Convert.ToInt32(pageCountDoub);

            ResponsePagedListDTO<ReadDTO> newList = new ResponsePagedListDTO<ReadDTO>(list, recordCount, pager);

            return newList;

        }


    }
}
