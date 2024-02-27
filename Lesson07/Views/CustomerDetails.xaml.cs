using Lesson07.Models;
using Lesson07.Stores;
using Lesson07.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Identity.Client;
using MvvmHelpers.Commands;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace Lesson07.Views
{
    /// <summary>
    /// Interaction logic for CustomerDetails.xaml
    /// </summary>
    public partial class CustomerDetails : UserControl
    {
        private readonly SalesStore _salesStore;

        #region Pagination
        public int CurrentPage = 1;
        public int TotalPage = 0;
        public int PageList = 10;
        #endregion

        #region UI elements
        public Customer Customer {get; set; }
        public ObservableCollection<Sale> CustomerSales { get; }

        #endregion

        #region Commands
        public ICommand BackCommand { get; }
        public ICommand PrevCommand { get; }
        public ICommand NextCommand { get; }

        #endregion
        public CustomerDetails(Customer customer)
        {
            InitializeComponent();
            
            if(customer is null)
            {
                return;
            }

            _salesStore = new();
            CustomerSales = new();
            
            Customer = customer;
            DataContext = this;

            Load();

            NextCommand = new AsyncCommand(OnNext);
            PrevCommand = new AsyncCommand(OnPrev);
            BackCommand = new DelegateCommand(OnBack);
        }      
        public async Task Load()
        {
            try
            {
                if(Customer is null)
                {
                    return;
                }
                var totalSales = await _salesStore.GetCountSalesAsync(Customer.Id);
                var salesList = await _salesStore.GetSales(PageList,CurrentPage,Customer.Id);

                TotalPage = totalSales / 10 +
                    (totalSales % 10 == 0 ? 0 : 1);

                PageNumberText.Text = $"{CurrentPage} of {TotalPage}";

                CustomerSales.Clear();
                CustomerSales.AddRange(salesList);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region Buttons
        public async Task OnNext()
        {
            try
            {
                if(CurrentPage >= TotalPage)
                {
                    return;
                }

                CurrentPage++;
                await Load();

                PrevButton.IsEnabled = true;

                if (CurrentPage >= TotalPage) NextButton.IsEnabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnPrev()
        {
            try
            {
                if (CurrentPage <= 0)
                {
                    return;
                }

                CurrentPage--;
                await Load();

                NextButton.IsEnabled = true;

                if (CurrentPage <= 1) 
                {
                    PrevButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public void OnBack()
        {
            try 
            {
                DialogHost.Close("MainDialog");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
    }
}
