using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Interfaces
{
    public interface IValidate
    {
        bool IsValid { set; get; }
        List<string> MessageList { set; get; }

        void AddError(string message);

        void Clear();

        string ToString();
 
    }
}
