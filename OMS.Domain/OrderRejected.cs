using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain
{
    public class OrderRejected : OrderState
    {

        public OrderRejected(OrderHeader orderHeader) : base(orderHeader) { }


        public override OrderStates State => OrderStates.Rejected;

        public override void Complete(ref OrderState state)
        {
            throw new InvalidOperationException("A rejected order cannot be completed");
        }

        public override void Reject(ref OrderState state)
        {
            throw new InvalidOperationException("This order is already rejected");
        }

        public override void Submit(ref OrderState state)
        {
            throw new InvalidOperationException("A rejected order cannot be submitted");
        }
    }
}
