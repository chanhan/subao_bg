using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface ILoginService : IRepository<Employee>
    {
        Employee Login(Employee employee);

        bool IpAccess_Check(Employee employee);
    }
}
