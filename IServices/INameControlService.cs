using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface INameControlService : IRepository<NameControl>
    {

        List<NameControl> GetNameControls(NameControl nameControl);

        int EditNameControl(NameControl nameControl);

        int DeleteNameControl(int id);

        string GetDataById(int id);
    }
}
