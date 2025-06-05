using DataTransferObjects;


namespace WebAPI.Client.ViewModels
{

    public class ResponseViewModel<T>
    {
        public ResponseViewModel()
        {
            MessageList = new List<string>();
        }

        public T Content { set; get; }
        public List<string> MessageList { set; get; }

        public ResponseStatus Status { set; get; }
    }

    public enum ResponseStatus
    {
        Success,
        Error,
        Unauthorized
    }
}
