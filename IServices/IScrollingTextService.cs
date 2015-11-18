using IServices.Infrastructure;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IScrollingTextService : IRepository<ScrollingText>
    {

        int EditScrollingText(ScrollingText scrolling);
    }
}
