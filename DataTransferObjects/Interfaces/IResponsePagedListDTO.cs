using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Interfaces
{
    public interface IResponsePagedListDTO<T>
    {
        IEnumerable<T> List { get; }
        int RecordCount { get; }
        int CurrentPage { get; }
        int PageCount { get; }
    }
}
