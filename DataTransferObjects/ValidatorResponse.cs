using System.Text.Json;

namespace DataTransferObjects
{
    public class ValidatorResponse
    {
        bool isValid = true;
        List<string> errorList = new List<string>();

        public ValidatorResponse()
        {
        }

        public bool IsValid
        {
            get
            {
                return isValid;
            }
            set
            {
                isValid = value;
            }
        }

        public List<string> MessageList
        {
            get
            {
                return errorList;
            }
            set
            {
                errorList = value;
            }
        }

        public void AddError(string error)
        {
            errorList.Add(error);
            if (IsValid)
                IsValid = false;
        }

        public void Clear()
        {
            MessageList.Clear();
            isValid = true;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

    }

}
