using DataTransferObjects.Interfaces;

namespace BusinessLogicLayer.Response
{
    public class ResponseListDTO<ReadDTO> : IResponseListDTO<ReadDTO>
    {
        public IEnumerable<ReadDTO> List => _list;
        public int RecordCount => _recordCount;
        
        private readonly IEnumerable<ReadDTO> _list;
        private readonly int _recordCount;

        public ResponseListDTO(IEnumerable<ReadDTO> list)
        {
            this._list = list;
            this._recordCount = list.Count();
        }

        public ResponseListDTO(IEnumerable<ReadDTO> list, int recordCount)
        {
            this._list = list;
            this._recordCount = recordCount;
        }


        public static async Task<ResponseListDTO<ReadDTO>> GetResponseFromQueryAsync(QueryStrategyBase<ReadDTO> queryStrategyBase)
        {
            var list = await queryStrategyBase.GetResultsAsync();
            var recordCount = await queryStrategyBase.CountResultsAsync();

            return new ResponseListDTO<ReadDTO>(list, recordCount);

        }

    }
}
