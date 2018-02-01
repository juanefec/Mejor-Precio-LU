using System;
using System.Collections.Generic;
using mejor_precio_2.Models;

namespace ApiViews.Models
{
    public class StoreListViewModel
    {
        private List<Store> storeList;
        public List<Store> StoreList
        {
            get { return storeList;}
            set { storeList = value;}
        }
    }
}