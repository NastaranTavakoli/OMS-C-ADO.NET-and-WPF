using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMS.Controllers;
using OMS.Domain;

namespace OMS.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Initialize()
        {
            OrderController.Instance.ResetDatabase();
        }

        [TestCleanup]
        public void CleanUp()
        {
            OrderController.Instance.ResetDatabase();
        }


        [TestMethod]
        public void CreateNewOrderHeader_ShouldHaveIdGenerated()
        {
            var order = OrderController.Instance.CreateNewOrderHeader();
            Assert.IsTrue(order.Id >0);
        }


        [TestMethod]
        public void CreateNewOrderHeader_ShouldHaveDateTime()
        {
            var order = OrderController.Instance.CreateNewOrderHeader();
            Assert.IsTrue(order.DateTime != null);
        }


        [TestMethod]
        public void CreateNewOrderHeader_OrderStateShouldBeNew()
        {
            var order = OrderController.Instance.CreateNewOrderHeader();
            Assert.IsTrue(order.State == Domain.OrderStates.New);
        }


        [TestMethod]
        public void GetOrderHeaders_ShouldGetOrders()
        {
            var stockItems = StockItemController.Instance.GetStockItems();
            var orders = new List<OrderHeader>();

            foreach(var stockItem in stockItems)
            {
                var order = OrderController.Instance.CreateNewOrderHeader();
                order = OrderController.Instance.UpsertOrderItem(order.Id, stockItem.Id, 1);
                orders.Add(order);
            }

            var testOrders = OrderController.Instance.GetOrderHeaders();

            Assert.AreEqual(orders.Count, testOrders.Count());

        }
    }
}
