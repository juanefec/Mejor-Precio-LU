using System;
using System.Collections.Generic;
using mejor_precio_2.Models;

namespace ApiViews.Models
{
    public class NotificationViewModel : Notification{

        public NotificationViewModel(Notification notification){
            this.Date = notification.Date;
            this.Id = notification.Id;
            this.OldPrice = notification.OldPrice;
            this.NewPrice = notification.NewPrice;
            this.Product = notification.Product;
            this.Store = notification.Store;
            this.User = notification.User;
            this.State = notification.State;
        }
        
        public NotificationViewModel(){}
    }
}