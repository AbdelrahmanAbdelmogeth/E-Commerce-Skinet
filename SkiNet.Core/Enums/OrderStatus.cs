﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EnumsCommerceSkinet.Core.Entities.OrderAggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "PaymentReceived")]
        PaymentReceived,
        [EnumMember(Value = "PaymentDailed")]
        PaymentFailed,
        [EnumMember(Value = "PaymentMismatch")]
        PaymentMismatch,
        [EnumMember(Value = "PaymentRefunded")]
        PaymentRefunded,
    }
}
