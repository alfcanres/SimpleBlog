using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Interfaces
{
    public interface IPagerDTO
    {
        int CurrentPage { get; }
        int RecordsPerPage { get; }
        string SearchKeyWord { get; }

        string ToQueryString();
    }
}
