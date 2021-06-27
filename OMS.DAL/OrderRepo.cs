using OMS.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMS.DAL
{
    public class OrderRepo
    {
        string _cs = @"Data Source = localhost\SQLEXPRESS; Initial Catalog =OrderManagementDb; Integrated Security=true";

        public OrderHeader InsertOrderHeader()
        {
            OrderHeader order = null;

            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_InsertOrderHeader", connection))
            {
                connection.Open();
                int id = Convert.ToInt32(command.ExecuteScalar());
                command.CommandText = "SELECT * FROM OrderHeaders WHERE Id = @id";
                command.Parameters.AddWithValue("@id", id);
                var reader = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                reader.Read();
                var datetime = reader.GetDateTime(2);
                reader.Close();
                connection.Close();
                order = new OrderHeader(id, datetime, 1);
            }
            return order;

        }

        public List<OrderHeader> GetOrderHeaders()
        {
            var orders = new List<OrderHeader>();
            OrderHeader order = null;

            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_SelectOrderHeaders", connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    //check if this is either the first row or a new order
                    if (order == null || order.Id != id)
                    {
                        int stateId = reader.GetInt32(1);
                        var dateTime = reader.GetDateTime(2);
                        order = new OrderHeader(id, dateTime, stateId);
                        orders.Add(order);
                    }

                    //check if there is an associated stock item i.e. is the value of the stock item id null
                    if (!reader.IsDBNull(3))
                    {
                        int stockItemId = reader.GetInt32(3);
                        string description = reader.GetString(4);
                        decimal price = reader.GetDecimal(5);
                        int quantity = reader.GetInt32(6);
                        order.AddOrUpdateOrderItem(stockItemId, price, description, quantity);
                    }
                }
                return orders;
            }
        }

        public OrderHeader GetOrderHeader(int id)
        {
            OrderHeader order = null;
            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_SelectOrderHeaderById @id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (order == null)
                    {
                        int stateId = reader.GetInt32(1);
                        var dateTime = reader.GetDateTime(2);
                        order = new OrderHeader(id, dateTime, stateId);
                    }

                    //check if there is an associated stock item i.e. the value of the stock item id is null
                    if (!reader.IsDBNull(3))
                    {
                        int stockItemId = reader.GetInt32(3);
                        string description = reader.GetString(4);
                        decimal price = reader.GetDecimal(5);
                        int quantity = reader.GetInt32(6);
                        order.AddOrUpdateOrderItem(stockItemId, price, description, quantity);
                    }
                }
            }
            return order;
        }

        public void UpsertOrderItem(OrderItem orderItem)
        {
            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_UpsertOrderItem @orderHeaderId, @stockItemId, @description, @price, @quantity", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@orderHeaderId", orderItem.OrderHeaderId);
                command.Parameters.AddWithValue("@stockItemId", orderItem.StockItemId);
                command.Parameters.AddWithValue("@description", orderItem.Description);
                command.Parameters.AddWithValue("@price", orderItem.Price);
                command.Parameters.AddWithValue("@quantity", orderItem.Quantity);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteOrderItem(int orderHeaderId, int stockItemId)
        {
            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_DeleteOrderItem @orderHeaderId, @stockItemId", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@orderHeaderId", orderHeaderId);
                command.Parameters.AddWithValue("@stockItemId", stockItemId);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateOrderState(OrderHeader order)
        {
            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_UpdateOrderState @orderHeaderId, @stateId", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@orderHeaderId", order.Id);
                command.Parameters.AddWithValue("@stateId", order.StateId);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteOrderHeaderAndOrderItems(int orderHeaderId)
        {
            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_DeleteOrderHeaderAndOrderItems @orderHeaderId", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@orderHeaderId", orderHeaderId);
                command.ExecuteNonQuery();
            }

        }

        public void ResetDatabase()
        {
            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_ResetDatabase", connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

    }
}
