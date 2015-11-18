using IServices.Infrastructure;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IServices
{
    public interface IActionListService : IRepository<ActionList>
    {
        List<BasketBall> GetBasketball(DateTime gamedate, string gametype);

        List<Baseball> Getbaseball(DateTime gamedate, string gametype);

        List<IceHockey> GetIceHockey(DateTime gamedate, string gametype);

        List<AFB> GetAFB(DateTime gamedata, string gametype);
    }
}
