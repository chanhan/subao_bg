using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
   public interface IMarqueeService: IRepository<Marquee>
    {
        int UpDateMarquee(Marquee marquee);

        int autoClose(int id, string enableYN);

        int DeleteMarquee(int id);
    }
}
