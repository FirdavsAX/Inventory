using Azure.Core;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Lesson07.Models;
using Lesson07.Stores;
using Lesson07.Views;
using MaterialDesignThemes.Wpf;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Lesson07.ViewModels
{
    public class SalesCreateViewModel : BaseViewModel
    {
        #region DataStores
        private readonly CustomersDataStore _customerDataStore;
        private readonly SalesStore _saleDataStore;
        private readonly SaleProductDataStore _saleProductsDataStore;
        private readonly ProductsDataStore _productsDataStore;
        #endregion
        public bool IsEdit = false;

        #region Collections
        public ObservableCollection<SaleProduct> SaleProducts{ get; set; }
        public ObservableCollection<Customer> Customers { get; set; }
        
        #endregion

        #region UI Inputs
        private Customer _selectedCustomer;
        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                SetProperty(ref _selectedCustomer, value);
            }
        }
        private SaleProduct _selectedSaleProduct;
        public SaleProduct SelectedSaleProduct
        {
            get => _selectedSaleProduct;
            set
            {
                SetProperty(ref _selectedSaleProduct, value);
            }
        }
        private decimal _totalPaid;
        public decimal TotalPaid
        {
            get => _totalPaid;
            set
            {
                SetProperty(ref _totalPaid, value);
            }
        }
        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);
            }
        }
        private decimal _totalUnitsPrice=0;
        public decimal TotalUnitsPrice
        {
            get => _totalUnitsPrice;
            set
            {
                SetProperty(ref _totalUnitsPrice, value);
            }
        }
        private decimal _totalDiscount=0;
        public decimal TotalDiscount
        {
            get => _totalDiscount;
            set
            {
                SetProperty(ref _totalDiscount, value);
            }
        }
        private decimal _totalPrice = 0;
        public decimal TotalPrice
        {
            get => _totalPrice;
            set
            {
                SetProperty(ref _totalPrice, value);
            }
        }

        #endregion

        #region Commands
        public ICommand CreateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        #endregion

        public Sale Sale { get; set; }

        public SalesCreateViewModel(Sale? sale = null)
        {

            if (sale is null) 
                Sale = new();
            else
                Sale = sale;

            if (sale is not null) 
            {
                IsEdit = true;
                PopulateData();
            };

            _customerDataStore = new CustomersDataStore();
            _saleDataStore = new SalesStore();
            _saleProductsDataStore = new SaleProductDataStore();
            _productsDataStore = new ProductsDataStore();
            SaleProducts = new ObservableCollection<SaleProduct>();
            Customers = new ObservableCollection<Customer>();

            CreateCommand = new AsyncCommand(OnCreate);
            DeleteCommand = new AsyncCommand(OnDelete);
            SaveCommand = new AsyncCommand(OnSaveCommand);

        }
        public void PopulateData()
        {
            _selectedDate = Sale.SaleDate;
            _totalPaid = Sale.TotalPaid;
            _totalDiscount = Sale.TotalDiscount;
            _totalPrice = Sale.TotalDue; 
        }
        
        public async Task OnDelete()
        {
            try
            {
                if(_selectedSaleProduct is null)
                {
                    return;
                }
                TotalUnitsPrice -= _selectedSaleProduct.UnitPrice;
                TotalDiscount -= _selectedSaleProduct.Discount;
                TotalPrice -= (_selectedSaleProduct.UnitPrice - _selectedSaleProduct.Discount);
                SaleProducts.Remove(_selectedSaleProduct);
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task Load()
        {
            try
            {
                var list = await _customerDataStore.GetCustomersAsync();
                Customers.AddRange(list);

                if (IsEdit)
                {
                    SelectedCustomer = Customers.FirstOrDefault(c => c.Id == Sale.CustomerId) ?? Customers[0];
                      
                    if (Sale.SaleProducts.Count != 0)
                    {
                        foreach(var saleProducts in Sale.SaleProducts)
                        {
                            saleProducts.Product = _productsDataStore.GetProductById(saleProducts.ProductId);
                        }
                        SaleProducts.AddRange(Sale.SaleProducts);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        #region methods ON commands
        public async Task OnCreate()
        {
            try
            {
                var view = new SaleProductCreateView();
                var result = await DialogHost.Show(view, "SaleDialog");

                if(result is not SaleProduct || result is null)
                {
                    return;
                }
                var saleProduct = result as SaleProduct;

                SaleProducts.Add(saleProduct);

                TotalDiscount += saleProduct.Discount;
                TotalUnitsPrice += saleProduct.UnitPrice;
                TotalPrice += saleProduct.UnitPrice - saleProduct.Discount;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnCancelCommand()
        {
            try
            {
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnSaveCommand()
        {
            try
            {
                if (TotalPaid == 0)
                {
                    MessageBox.Show("At least 60% of the payment must be paid to complete the sale!");
                    return;
                }
                if (SaleProducts is null)
                {
                    MessageBox.Show("You have not any products!");
                    return;
                }

                Sale.CustomerId = SelectedCustomer.Id;
                Sale.SaleDate = SelectedDate;
                Sale.TotalDiscount = TotalDiscount;
                Sale.TotalDue = TotalPrice;
                Sale.TotalPaid = TotalPaid;
                Sale.SaleProducts = SaleProducts;

                var result = MessageBox.Show("Are you sure?", "Question", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel) return;
               
                if (!IsEdit)
                {
                    await _saleDataStore.CreateSaleAsync(Sale);

                    await _saleProductsDataStore.CreateSaleProduct(SaleProducts.ToList());
                }
                else
                {
                    var createdSale = await _saleDataStore.UpdateSale(Sale);

                    await _saleProductsDataStore.UpdateSaleProducts(SaleProducts.ToList(),Sale.Id);
                    
                }
                MessageBox.Show("Sale succesfully created!");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
    }
}
