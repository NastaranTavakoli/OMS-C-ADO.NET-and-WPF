using OMS.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookIn
{
    class Program
    {
        static void Main(string[] args)
        {
            var orderCtrl = OrderController.Instance;

            OrderController.Instance.ResetDatabase();
            var order1 = OrderController.Instance.CreateNewOrderHeader();
            //order1 = OrderController.Instance.UpsertOrderItem(order1.Id, 1, 1);
            //order1 = OrderController.Instance.UpsertOrderItem(order1.Id, 1, 2);
            //order1 = OrderController.Instance.UpsertOrderItem(order1.Id, 2, 3);
            //order1 = OrderController.Instance.UpsertOrderItem(order1.Id, 3, 3);
            //order1 = OrderController.Instance.DeleteOrderItem(order1.Id, 2);

            //var order2 = OrderController.Instance.CreateNewOrderHeader();
            //order2 = OrderController.Instance.UpsertOrderItem(order2.Id, 4, 1);
            //order2 = OrderController.Instance.UpsertOrderItem(order2.Id, 4, 2);
            //order2 = OrderController.Instance.UpsertOrderItem(order2.Id, 5, 3);
            //order2 = OrderController.Instance.UpsertOrderItem(order2.Id, 3, 3);

            //var order3 = OrderController.Instance.CreateNewOrderHeader();
            //order3 = OrderController.Instance.UpsertOrderItem(order3.Id, 5, 1);
            //order3 = OrderController.Instance.UpsertOrderItem(order3.Id, 4, 1);
            //order3 = OrderController.Instance.UpsertOrderItem(order3.Id, 6, 3);
            //order3 = OrderController.Instance.UpsertOrderItem(order3.Id, 3, 3);
        }
    }
}
