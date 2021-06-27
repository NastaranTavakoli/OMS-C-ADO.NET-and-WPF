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
    public class StockItemController
    {
        private readonly StockRepo _stockRepo = new StockRepo();

        //singleton pattern
        private static StockItemController _instance;

        private StockItemController() { }

        public static StockItemController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StockItemController();
                }
                return _instance;

            }
        }

        public IEnumerable<StockItem> GetStockItems()
        {
            return _stockRepo.GetStockItems();
        }

        public StockItem UpdateStockItem(int stockItemId, int newAmount, string newName, decimal newPrice)
        {
            var stockItem = _stockRepo.GetStockItem(stockItemId);
            _stockRepo.UpdateStockItem(stockItem, newAmount, newName, newPrice);

            return stockItem;
        }
    }
}
