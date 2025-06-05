namespace DataTransferObjects
{
    public class ResponseList<ReadDTO> where ReadDTO : class
    {
        
        private IEnumerable<ReadDTO> _list;
        private int _recordCount;

        public ResponseList()
        {
        }

        public ResponseList(IEnumerable<ReadDTO> list)
        {
            this.List = list;
            this.RecordCount = list.Count();
        }

        public ResponseList(IEnumerable<ReadDTO> list, int recordCount)
        {
            this.List = list;
            this.RecordCount = recordCount;
        }

        public IEnumerable<ReadDTO> List { get => _list; set => _list = value; }
        public int RecordCount { get => _recordCount; set => _recordCount = value; }
    }
}
