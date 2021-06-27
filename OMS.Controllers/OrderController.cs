using OMS.DAL;
using OMS.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.Controllers
{
    public class OrderController
    {
        //fields
        private readonly OrderRepo _orderRepo = new OrderRepo();
        private readonly StockRepo _stockRepo = new StockRepo();

        //singleton pattern
        private static OrderController _instance;

        private OrderController() { }

        public static OrderController Instance
        {
            get
            {
                if (_instance ==null)
                {
                    _instance = new OrderController();
                }
                return _instance;

            }
        }

        public IEnumerable<OrderHeader> GetOrderHeaders()
        {
            return _orderRepo.GetOrderHeaders();
        }

        public OrderHeader CreateNewOrderHeader()
        {
            return _orderRepo.InsertOrderHeader();
        }

        public OrderHeader UpsertOrderItem(int orderHeaderId, int stockItemId, int quantity)
        {
            //1. find the associated stock item object by id
            var stockItem = _stockRepo.GetStockItem(stockItemId);
            //2. find the associated orderheader object
            var order = _orderRepo.GetOrderHeader(orderHeaderId);
            //3. create or update the order item object
            var orderItem = order.AddOrUpdateOrderItem(stockItemId, stockItem.Price, stockItem.Name, quantity);
            //4. persist the changes to the database
            _orderRepo.UpsertOrderItem(orderItem);
            //return the updated order
            return order;
        }

        public OrderHeader DeleteOrderItem(int orderHeaderId, int stockItemId)
        {
            _orderRepo.DeleteOrderItem(orderHeaderId, stockItemId);
            var order = _orderRepo.GetOrderHeader(orderHeaderId);
            return order;
        }

        public OrderHeader SubmitOrder(int orderHeaderId)
        {
            //retrieve a new (fresh) instance of the order object
            var order = _orderRepo.GetOrderHeader(orderHeaderId);
            //update the state pf the order to pending (in memory)
            order.Submit();
            //persist the changes to the database
            _orderRepo.UpdateOrderState(order);
            //return the updated instance of the order object
            return order;
        }


        public OrderHeader ProcessOrder(int orderHeaderId)
        {
            var order = _orderRepo.GetOrderHeader(orderHeaderId);
            try
            {
                _stockRepo.DecrementOrderStockItemAmount(order);
                order.Complete();
            }
            catch (SqlException ex)
            {
                int exceptionNumber = ex.Number; //547 
                order.Reject();
            }
            _orderRepo.UpdateOrderState(order);
            return order;
        }

        public void DeleteOrderHeaderAndOrderItems(int orderHeaderId)
        {
            _orderRepo.DeleteOrderHeaderAndOrderItems(orderHeaderId);
        }

        public void ResetDatabase()
        {
            _orderRepo.ResetDatabase();
        }

    }
}
