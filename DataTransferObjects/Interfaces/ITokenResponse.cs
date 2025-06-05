using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Interfaces
{
    public interface ITokenResponse
    {
        string Token { get; }
        DateTime Expires { get; }

    }
}
