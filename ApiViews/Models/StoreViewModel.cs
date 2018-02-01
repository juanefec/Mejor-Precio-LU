using System;
using System.Collections.Generic;
using mejor_precio_2.Models;

namespace ApiViews.Models
{
    public class StoreViewModel : Store{

        public StoreViewModel(Store store){
            this.Id = store.Id;
            this.Name = store.Name;
            this.Address = store.Address;
            this.Latitude = store.Latitude;
            this.Longitude = store.Longitude;
            this.State = store.State;
            this.Date = store.Date;
        }
        
        public StoreViewModel(){}
    }
}