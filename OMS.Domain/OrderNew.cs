using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain
{
    public class OrderNew : OrderState
    {
        public OrderNew(OrderHeader orderHeader) :base(orderHeader) {}

        public override OrderStates State { get => OrderStates.New; }

        public override void Complete(ref OrderState state)
        {
            throw new InvalidOperationException("A new order must be submitted before it can be completed");
        }

        public override void Reject(ref OrderState state)
        {
            throw new InvalidOperationException("A new order must be submitted before it can be rejected");
        }

        public override void Submit(ref OrderState state)
        {
            //business rule: an order must have at least one order item
            if (!_orderHeader.OrderItems.Any())
            {
                throw new InvalidOperationException("A new order must have at least one item before it can be submitted");
            }
            //Change to state pending
            state = new OrderPending(_orderHeader);
        }
    }
}
