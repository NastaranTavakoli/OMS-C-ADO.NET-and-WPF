using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Domain
{
    public class OrderHeader
    {
        private OrderState _state;

        public OrderHeader(int id, DateTime datetime, int stateId)
        {
            DateTime = datetime;
            Id = id;
            setState(stateId);
        }

        public List<OrderItem> OrderItems { get; } = new List<OrderItem>();

        public OrderItem AddOrUpdateOrderItem(int stockItemId, decimal price, string description, int quantity)
        {
            OrderItem item = null; 
            //check to see if there is already an existing order item for the selected stock item
            foreach(var i in OrderItems)
            {
                if (i.StockItemId == stockItemId)
                {
                    item = i;
                }
            }
            //if there isn't create a new instance and add it to the colletion of order items
            if (item == null)
            {
                item = new OrderItem(this, stockItemId, price, description, quantity);
                OrderItems.Add(item);
            }
            else
            {
                item.Quantity += quantity;
            }
            //return the order item object
            return item;
        }

        public int Id { get; set; }
        public DateTime DateTime { get; set; }

        public int StateId => (int)_state.State;

        public OrderStates State => _state.State;

        public decimal Total { get => OrderItems.Sum(i => i.Total); }
        private void setState(int stateId)
        {
            switch (stateId)
            {
                case 1:
                    _state = new OrderNew(this);
                    break;
                case 2:
                    _state = new OrderPending(this);
                    break;
                case 3:
                    _state = new OrderRejected(this);
                    break;
                case 4:
                    _state = new OrderComplete(this);
                    break;
                default:
                    throw new InvalidOperationException("Invalid StateId Detected");
            }
        }

        //update the state of the order i.e. submit [new> pending], 
        public void Submit()
        {
            _state.Submit(ref _state);
        }

        public void Complete()
        {
            _state.Complete(ref _state);
        }

        public void Reject()
        {
            _state.Reject(ref _state);
        }

    }
}
