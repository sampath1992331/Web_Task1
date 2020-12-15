using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebTask.Common;
using WebTask.Models;
using WebTask.Services;

namespace WebTask.Controller
{
   
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IMain main;
        public MainController(IMain main) 
        {
            this.main = main;
        }
        [HttpGet]
        [Route("GetCustomersNameList")]
        public APIResponse GetCustomersNameList()
        {
            return  main.GetCustomersNameList();
        }
        [HttpGet]
        [Route("GetCustomersDetailsByID")]
        public APIResponse GetCustomersDetailsByID(int customerID)
        {
            return main.GetCustomersDetailsByID(customerID);
        }
        [HttpGet]
        [Route("GetAllItemCode")]
        public APIResponse GetAllItemCode()
        {
            return main.GetAllItemCode();
        }
        [HttpGet]
        [Route("GetItemsDetailsByID")]
        public APIResponse GetItemsDetailsByID(int itemID)
        {
            return main.GetItemsDetailsByID(itemID);
        }
        [HttpPost]
        [Route("SaveOrder")]
        public APIResponse SaveOrder([FromBody] OrderModel orderModel)
        {
            return main.SaveOrder(orderModel); 
        }
        [HttpGet]
        [Route("GetAllOrders")]
        public APIResponse GetAllOrders()
        {
            return main.GetAllOrders();
        }
        [HttpGet]
        [Route("GetOrdersById")]
        public APIResponse GetOrdersById(int id)
        {
            return main.GetOrdersById(id);
        }
    }
}
