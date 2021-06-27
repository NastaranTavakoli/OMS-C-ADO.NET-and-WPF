using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using OMS.Domain;


namespace OMS.DAL
{
    public class StockRepo
    {
        string _cs = @"Data Source = localhost\SQLEXPRESS; Initial Catalog =OrderManagementDb; Integrated Security=true";
        public List<StockItem> GetStockItems()
        {
            var items = new List<StockItem>();

            using(var connection =new SqlConnection(_cs))
            using(var command = new SqlCommand("sp_SelectStockItems", connection))
            {
                connection.Open();
                var reader = command.ExecuteReader();
                while(reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    decimal price = reader.GetDecimal(2);
                    int inStock = reader.GetInt32(3);
                    var item = new StockItem(id, name, price, inStock);
                    items.Add(item);
                }
            }
            return items;
        }

        public StockItem GetStockItem(int id)
        {
            StockItem item = null;
            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_SelectStockItemById @id", connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@id", id);
                var reader = command.ExecuteReader(System.Data.CommandBehavior.SingleRow);
                reader.Read();
                string name = reader.GetString(1);
                decimal price = reader.GetDecimal(2);
                int inStock = reader.GetInt32(3);
                item = new StockItem(id, name, price, inStock);
            }
            return item;
        }

        public void DecrementOrderStockItemAmount(OrderHeader order)
        {
            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_UpdateStockItemAmount @id, @amount", connection))
            {
                connection.Open();
                var transaction = connection.BeginTransaction();
                command.Transaction = transaction;

                try
                {
                    foreach (var oi in order.OrderItems)
                    {
                        command.Parameters.AddWithValue("@id", oi.StockItemId);
                        command.Parameters.AddWithValue("@amount", -oi.Quantity);
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }


        public void UpdateStockItem(StockItem stockItem, int? newAmount, string name, decimal? price)
        {
            using (var connection = new SqlConnection(_cs))
            using (var command = new SqlCommand("sp_UpdateStockItem @id, @amount, @name, @price", connection))
            {
                connection.Open();
                try
                {
                    command.Parameters.AddWithValue("@id", stockItem.Id);
                    command.Parameters.AddWithValue("@amount", newAmount);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@price", price);
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
        }

    }
}
