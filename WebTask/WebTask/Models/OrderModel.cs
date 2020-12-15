using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebTask.Models
{
    public class OrderModel
    {
        public int ItemId { get; set; }
        public int CustomerId { get; set; }
        public string InvoiceNo { get; set; }
        public string ItemCode { get; set; }
        public string ReferenceNo { get; set; }
        public string Note { get; set; }
        public DateTime InvoiceDate { get; set; }
        public List<ItemModel> ItemList { get; set; }
    }
}
