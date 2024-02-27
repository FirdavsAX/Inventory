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
using Lesson07.Models;
using Lesson07.Stores;
using Lesson07.ViewModels;
using MaterialDesignThemes.Wpf;
using MvvmHelpers.Commands;

namespace Lesson07.Views
{
    /// <summary>
    /// Interaction logic for SaleProductCreateView.xaml
    /// </summary>
    public partial class SaleProductCreateView : UserControl   
    {
        private readonly ProductsDataStore dataStore;
        public List<Product> Products { get; set; }
        public ICommand AddCommand { get; }
        public ICommand CancelCommand { get; }

        #region UI Inputs
        public Product SelectedProduct { get; set; }
        public bool IsEnableButton { get; set; }
        public decimal Discount { get; set;}
        public int Quantity { get; set; }
        #endregion

        public SaleProductCreateView() 
        {
            InitializeComponent();
            
            DataContext = this;

            dataStore = new();
            Products = new();
            AddCommand = new AsyncCommand(OnAddCommand);
            CancelCommand = new AsyncCommand(OnCancelCommand);

            Load();
        }
        public async Task Load()
        {
            var totalProducts = await dataStore.GetProductsCountAsync();
            var list = await dataStore.GetProductsAsync(totalProducts, 1);

            Products.AddRange(list);
        }
        public async Task OnCancelCommand()
        {
            try
            {
                DialogHost.Close("SaleDialog");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task OnAddCommand()
        {
            try
            {
                SaleProduct saleProduct = new();
                saleProduct.Product = SelectedProduct;
                saleProduct.Quantity = Quantity;
                saleProduct.UnitPrice = Quantity * SelectedProduct.Price;

                if (Discount >= Quantity * SelectedProduct.Price * 50 / 100)
                {
                    MessageBox.Show("Discount is very large");
                    return;
                }
                saleProduct.Discount = Discount;

                DialogHost.Close("SaleDialog", saleProduct);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
