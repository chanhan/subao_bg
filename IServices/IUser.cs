using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IUser
    {
        string UserName { get; }
        string UserIP { get; }
        string UserRank { get; }
        string Sid { get; }
    }
}
