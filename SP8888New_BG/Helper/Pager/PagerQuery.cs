using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helper.Pager
{
    public class PagerQuery<TPager, TEntityList,TQueryModel>
    {
        public PagerQuery(TPager pager, TEntityList entityList,TQueryModel queryModel)
        {
            this.Pager = pager;
            this.EntityList = entityList;
            this.QueryModel = queryModel;
        }
        public TPager Pager { get; set; }
        public TEntityList EntityList { get; set; }
        public TQueryModel QueryModel { get; set; }

    }
}