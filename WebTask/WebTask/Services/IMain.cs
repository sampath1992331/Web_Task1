using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTask.Common;
using WebTask.Models;

namespace WebTask.Services
{
    public interface IMain
    {
        public APIResponse GetCustomersNameList();
        public APIResponse GetCustomersDetailsByID(int customerid);
        public APIResponse GetAllItemCode();
        public APIResponse GetItemsDetailsByID(int Itemid);
        public APIResponse SaveOrder(OrderModel orderModel);
        public APIResponse GetAllOrders();
        public APIResponse GetOrdersById(int id);

    }
}
