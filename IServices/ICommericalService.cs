using IServices.Infrastructure;
using Models;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IServices
{
    public interface ICommericalService : IRepository<Commercial>
    {
        Dictionary<string, Tuple<int, List<CommercialImage>>> GetCommercial(string type);

        int EditCommercial(CommercialImage image, string act, HttpPostedFileBase imagaFile, string oldImageName, string language, string direction);

        int ChangeCommercialSeconds(int seconds, string direction, string language);
    }
}
