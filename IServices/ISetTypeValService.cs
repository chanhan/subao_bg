using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface ISetTypeValService : IRepository<SetTypeVal>
    {
        string GetDataSetTypeVal();
        int EditSetTypeVal(string val1, string val2);
    }
}
