using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceSkinet.Core.DTO
{
    public class OrderAdminToReturnDto : OrderBaseToReturnDto
    {
        // Add any admin-specific properties here
        public string PaymentIntentId { get; set; } // Example of an admin-only property
    }
}
