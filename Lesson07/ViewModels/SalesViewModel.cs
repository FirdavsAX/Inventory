using GalaSoft.MvvmLight.Messaging;
using Lesson07.Models;
using Lesson07.Stores;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using Lesson07.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace Lesson07.ViewModels
{
    public class SalesViewModel:BaseViewModel
    {
        private readonly SalesStore _datastore;
        #region Collections
        public ObservableCollection<Sale> Sales { get; set; }
        #endregion

        #region Variables
        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set
            {
                SetProperty(ref _totalPages, value);
            }
        }
        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
            }
        }
        private int _pageList = 15;
        public int PageList
        {
            get => _pageList;
            set
            {
                SetProperty(ref _pageList, value);
            }
        }
        #endregion

        #region Commands
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CreateCommand { get; }
        public ICommand PrimaryPageCommand { get; }
        public ICommand PrevPageCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand SecondPageCommand { get; }
        public ICommand ThirdPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }
        public ICommand FifteenPageCommand { get; }
        public ICommand ThirtyPageCommand { get; }
        public ICommand FiftyPageCommand { get; }

        #endregion

        #region UI elements
        private Sale _selectedSale;
        public Sale SelectedSale
        {
            get => _selectedSale;
            set => SetProperty(ref _selectedSale, value);
        }
        private string _pageString;
        public string PageString
        {
            get => _pageString;
            set
            {
                SetProperty(ref _pageString, value);
            }
        }
        private int _firstPage = 0;
        public int FirstPage 
        {
            get => _firstPage;
            set 
            {
                SetProperty(ref _firstPage, value);
            } 
        }
        private int _secondPage = 1;
        public int SecondPage
        {
            get => _secondPage;
            set
            {
                SetProperty(ref _secondPage, value);
            }
        }
        private int _thirdPage = 2;
        public int ThirdPage
        {
            get => _thirdPage;
            set
            {
                SetProperty(ref _thirdPage, value);
            }
        }
        #endregion

        #region Button Enables
        private bool _isEnablePrimaryPage = false;
        public bool IsEnablePrimaryPage
        {
            get => _isEnablePrimaryPage;
            set
            {
                SetProperty(ref _isEnablePrimaryPage, value);
            }
        }
        private bool _isEnablePrevPage = false;
        public bool IsEnablePrevPage 
        {
            get  => _isEnablePrevPage;
            set
            {
                SetProperty(ref _isEnablePrevPage, value);
            }
        }
        private bool _isEnableFirstPage = false;
        public bool IsEnableFirstPage
        {
            get => _isEnableFirstPage;
            set
            {
                SetProperty(ref _isEnableFirstPage, value);
            }
        }
        private bool _isEnableSecondPage = true;
        public bool IsEnableSecondPage
        {
            get => _isEnableSecondPage;
            set
            {
                SetProperty(ref _isEnableSecondPage, value);
            }
        }
        private bool _isEnableThirdPage = true;
        public bool IsEnableThirdPage
        {
            get => _isEnableThirdPage;
            set
            {
                SetProperty(ref _isEnableThirdPage, value);
            }
        }
        private bool _isEnableNextPage = true;
        public bool IsEnableNextPage
        {
            get => _isEnableNextPage;
            set
            {
                SetProperty(ref _isEnableNextPage, value);
            }
        }
        private bool _isEnableLastPage = true;
        public bool IsEnableLastPage
        {
            get => _isEnableLastPage;
            set
            {
                SetProperty(ref _isEnableLastPage, value);
            }
        }
        #endregion

        public SalesViewModel()
        {
            try
            {
                _datastore = new SalesStore();
                Sales = new();

                PageString = $"{CurrentPage} page of {TotalPages}";

                NextPageCommand = new AsyncCommand(OnNextPage);
                PrevPageCommand = new AsyncCommand(OnPrevPage);
                LastPageCommand = new AsyncCommand(OnLastPage);
                PrimaryPageCommand = new AsyncCommand(OnPrimaryPage);
                FirstPageCommand = new AsyncCommand(OnFirstPage);
                SecondPageCommand = new AsyncCommand(OnSecondPage);
                ThirdPageCommand = new AsyncCommand(OnThirdPage);

                FifteenPageCommand = new AsyncCommand(OnFifteenPage);
                ThirtyPageCommand = new AsyncCommand(OnThirtyPage);
                FiftyPageCommand = new AsyncCommand(OnFiftyPage);

                CreateCommand = new AsyncCommand(OnCreateCommand);
                DeleteCommand = new AsyncCommand(OnDeleteCommand);
                EditCommand = new AsyncCommand(OnEditCommand);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region Work with DataBase 
        public async Task LoadDataAsync()
        {
            await GetTotalPages();

            var listOnePage = await _datastore.GetSales(_pageList,_currentPage);
            Sales.Clear();
            Sales.AddRange(listOnePage);
        }
        public async Task GetTotalPages()
        {
            try
            {
                int salesCount = await _datastore.GetCountSalesAsync();

                if (salesCount <= 0)
                {
                    return;
                }
                
                _totalPages = salesCount / _pageList +
                    (salesCount % _pageList == 0 ? 0 : 1);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region Button reviews
        public async Task OnEditCommand()
        {
            try
            {
                if (SelectedSale is null) return;

                var view = new SaleCreateView(SelectedSale);
                var result = view.ShowDialog();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public async Task OnDeleteCommand()
        {
            try
            {
                var result = MessageBox.Show("Are you sure?", "Question", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel) return;

                _datastore.DeleteSale(SelectedSale);
                Sales.Remove(SelectedSale);
                MessageBox.Show("Sale succesfully deleted");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnCreateCommand()
        {
            try
            {
                var view = new SaleCreateView();
                var result = view.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnNextPage()
        {
            try
            {
                CurrentPage++;

                FirstPage = CurrentPage - 1;
                SecondPage = CurrentPage;
                ThirdPage = CurrentPage + 1;

                await LoadDataAsync();
                await CheckEnables();
                   
                if (TotalPages <= CurrentPage)
                {
                    IsEnableNextPage = false;
                }
                if(CurrentPage > 0)
                {
                    IsEnablePrevPage = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnPrevPage()
        {
            try
            {
                CurrentPage--;

                FirstPage = CurrentPage - 1;
                SecondPage = CurrentPage;
                ThirdPage = CurrentPage + 1;

                await LoadDataAsync();
                await CheckEnables();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnLastPage()
        {
            try
            {
                CurrentPage = TotalPages;

                FirstPage = TotalPages - 1;
                SecondPage = TotalPages;
                ThirdPage = 0;

                await LoadDataAsync();
                await CheckEnables();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnPrimaryPage()
        {
            try
            {
                CurrentPage = 1;

                FirstPage = 0;
                SecondPage = 1;
                ThirdPage = 2;

                await LoadDataAsync();
                await CheckEnables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnFirstPage()
        {
            try
            {
                CurrentPage = FirstPage;
               
                FirstPage--;
                SecondPage = CurrentPage;
                ThirdPage = CurrentPage + 1;

                await LoadDataAsync();
                await CheckEnables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnSecondPage()
        {
            try
            {
                CurrentPage = SecondPage;

                await LoadDataAsync();
                await CheckEnables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnThirdPage()
        {
            try
            {
                CurrentPage = ThirdPage;

                FirstPage++;
                SecondPage = CurrentPage;
                ThirdPage = CurrentPage + 1;

                await LoadDataAsync();
                await CheckEnables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        
        public async Task OnFifteenPage()
        {
            try
            {
                if (PageList > 15 && CurrentPage == TotalPages)
                {
                    PageList = 15;
                    await GetTotalPages();
                    CurrentPage = TotalPages;

                    FirstPage = CurrentPage - 1;
                    SecondPage = CurrentPage;
                    ThirdPage = 0;
                }

                await LoadDataAsync();
                await CheckEnables();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task OnThirtyPage()
        {
            try
            {
                bool UpToDown = CurrentPage == TotalPages
                    && PageList > 30; 

                PageList = 30;
                await GetTotalPages();

                if (UpToDown)
                {
                    CurrentPage = TotalPages;

                    FirstPage = CurrentPage - 1;
                    SecondPage = CurrentPage;
                    ThirdPage = 0;
                }
                
                if (CurrentPage >= TotalPages)
                {
                    CurrentPage = TotalPages;
                    
                    FirstPage = TotalPages - 1;
                    SecondPage = TotalPages;
                    ThirdPage = 0;
                }

                await LoadDataAsync();
                await CheckEnables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task OnFiftyPage()
        {
            try
            {
                PageList = 50;
                
                await GetTotalPages();

                if (CurrentPage >= TotalPages)
                {
                    CurrentPage = TotalPages;

                    FirstPage = TotalPages - 1;
                    SecondPage = TotalPages;
                    ThirdPage = 0;
                }

                await LoadDataAsync();
                await CheckEnables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private async Task CheckEnables()
        {
            try
            {
                PageString = $"{CurrentPage} page of {TotalPages}";

                if (CurrentPage == 1)
                {
                    IsEnablePrevPage = false;
                    IsEnableNextPage = true;
                    IsEnablePrimaryPage = false;
                    IsEnableLastPage = true;
                    IsEnableFirstPage = false;
                    IsEnableSecondPage = true;
                    IsEnableThirdPage = true;
                }
                else if (CurrentPage > 1 && CurrentPage < TotalPages) 
                {
                    IsEnablePrevPage = true;
                    IsEnableNextPage = true;
                    IsEnablePrimaryPage = true;
                    IsEnableLastPage = true;
                    IsEnableFirstPage = true;
                    IsEnableSecondPage = true;
                    IsEnableThirdPage = true;
                }
                else if (CurrentPage == TotalPages)
                {
                    IsEnablePrevPage = true;
                    IsEnableNextPage = false;
                    IsEnablePrimaryPage = true;
                    IsEnableLastPage = false;
                    IsEnableFirstPage = true;
                    IsEnableSecondPage = true;
                    IsEnableThirdPage = false;
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
