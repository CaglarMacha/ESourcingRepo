using Ordering.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class Order :Entity
    {
        public string AuctionId { get; set; }
        public string SellerName { get; set; }
        public string ProductId { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    } 
}
