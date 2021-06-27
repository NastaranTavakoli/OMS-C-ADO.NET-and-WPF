using OMS.Controllers;
using OMS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OMS.UI.Views
{
    /// <summary>
    /// Interaction logic for StockView.xaml
    /// </summary>
    public partial class StockView : Page
    {
        public StockView()
        {
            InitializeComponent();
            try
            {
                dgStockItems.ItemsSource = StockItemController.Instance.GetStockItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateStockItem_Click(object sender, RoutedEventArgs e)
        {
            var stockItem = (StockItem)dgStockItems.SelectedItem;
            NavigationService.Navigate(new UpdateStockItemView(stockItem));
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrdersView());
        }
    }
}
