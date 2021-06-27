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
    /// Interaction logic for UpdateStockItemView.xaml
    /// </summary>
    public partial class UpdateStockItemView : Page
    {
        private StockItem _stockItem;

        public UpdateStockItemView(StockItem stockItem)
        {
            InitializeComponent();
            try
            {
                _stockItem = stockItem;
                DataContext = _stockItem;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void UpdateOrder_Click(object sender, RoutedEventArgs e)
        {
            var stockItem = (StockItem)DataContext;

            if (stockItem.InStock < 0)
            {
                MessageBox.Show("Quantity must be equal or greater than zero(0)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (stockItem.Price <= 0)
            {
                MessageBox.Show("Price must be greater than zero(0)", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                
                StockItemController.Instance.UpdateStockItem(stockItem.Id, stockItem.InStock, stockItem.Name, stockItem.Price);
                NavigationService.Navigate(new StockView());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void CancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StockView());
        }
    }
}
