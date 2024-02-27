using Lesson07.Models;
using Lesson07.Stores;
using Lesson07.Views;
using MaterialDesignThemes.Wpf;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Lesson07.ViewModels
{
    public class ProductsViewModel : BaseViewModel
    {
        private const int minPage = 1;

        private readonly ProductsDataStore _store;
        private readonly CategoriesStore _categoriesStore;

        private readonly TimeSpan requestTimeSpan = new TimeSpan();

        #region Commands
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
        public ICommand CreateCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
        #endregion
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set => SetProperty(ref _selectedProduct, value);
        }


        private string _searchString;
        public string SearchString
        {
            get => _searchString;
            set
            {
                SetProperty(ref _searchString, value);
                FilterProducts();
            }
        }

        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                SetProperty(ref _selectedCategory, value);
                FilterProducts();
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
        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
            }
        }

        private int _totalPages;
        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value);
        }


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
            get => _isEnablePrevPage;
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

        #region PageNumbers
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

        public ProductsViewModel()
        {
            _store = new ProductsDataStore();
            _categoriesStore = new CategoriesStore();

            Products = new ObservableCollection<Product>();
            Categories = new ObservableCollection<Category>();

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


            CreateCommand = new AsyncCommand(OnCreate);
            EditCommand = new AsyncCommand(OnEdit);
            DeleteCommand = new AsyncCommand(OnDelete);
        }
        public async Task LoadData()
        {
            var totalProductsCount = await _store.GetProductsCountAsync();

            TotalPages = (totalProductsCount / PageList) % 10 != 0 ?
                (totalProductsCount / PageList) + 1 :
                (totalProductsCount / PageList); // 50 / 15

            var products = await _store.GetProductsAsync(PageList, CurrentPage);
            var categories = await _categoriesStore.GetCategoriesAsync();

            foreach (var product in products.Take(50))
            {
                Products.Add(product);
            }

            Categories.Add(new Category()
            {
                Id = 0,
                Name = "All Categories"
            });

            foreach (var category in categories)
            {
                Categories.Add(category);
            }

        }
        public async Task GetTotalPages()
        {
            try
            {
                var totalProductsCount = await _store.GetProductsCountAsync(SearchString,SelectedCategory?.Id);

                TotalPages = (totalProductsCount / PageList) % 10 != 0 ?
                    (totalProductsCount / PageList) + 1 :
                    (totalProductsCount / PageList); 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async Task FilterProducts()
        {
            var products = await _store.GetProductsAsync(PageList, CurrentPage, _searchString, _selectedCategory?.Id);
            await GetTotalPages();

            await CheckEnables();

            Products.Clear();

            foreach (var product in products)
            {
                Products.Add(product);
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

                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);

                await CheckEnables();
                
                if (TotalPages <= CurrentPage)
                {
                    IsEnableNextPage = false;
                }
                if (CurrentPage > 0)
                {
                    IsEnablePrevPage = true;
                }
            }
            catch (Exception ex)
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

                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);

                await CheckEnables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public async Task OnLastPage()
        {
            try
            {
                await GetTotalPages();
                CurrentPage = TotalPages;

                FirstPage = TotalPages - 1;
                SecondPage = TotalPages;
                ThirdPage = 0;

                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);

                await CheckEnables();
            }
            catch (Exception ex)
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
               
                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);

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

                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);
                
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

                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);

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
                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);

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
                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);

                await CheckEnables();
            }
            catch (Exception ex)
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
                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);

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

                var products = await _store.GetProductsAsync(PageList, CurrentPage, SearchString, _selectedCategory?.Id);

                Products.Clear();
                Products.AddRange(products);

                await CheckEnables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

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
                if (TotalPages == 1)
                {
                    IsEnablePrevPage = false;
                    IsEnableNextPage = false;
                    IsEnablePrimaryPage = false;
                    IsEnableLastPage = false;
                    IsEnableFirstPage = false;
                    IsEnableSecondPage = true;
                    IsEnableThirdPage = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private async Task OnCreate()
        {
            if (Categories is null)
            {
                return;
            }

            var view = new ProductDialog(Categories);
            var result = await DialogHost.Show(view, "MainDialog");

            if (result is not Product product)
            {
                return;
            }

            try
            {
                _store.CreateProduct(product);
                MessageBox.Show($"Product: {product.Name} was successfully added", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {product.Name} to database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex.Message);
            }

            Products.Insert(0, product);
        }

        private async Task OnEdit()
        {
            if (SelectedProduct is null || Categories is null)
            {
                return;
            }

            var view = new ProductDialog(Categories, SelectedProduct);
            var result = await DialogHost.Show(view, "MainDialog");

            if (result is not Product product)
            {
                return;
            }

            try
            {
                _store.UpdateProduct(product);
                MessageBox.Show($"Product: {product.Name} was successfully updated", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                var index = Products.IndexOf(product);

                if (index == -1)
                {

                }

                Products.Remove(product);
                Products.Insert(index, product);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {product.Name} to database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine(ex.Message);
            }
        }

        private async Task OnDelete()
        {
            var productToDelete = SelectedProduct;
            if (productToDelete is null)
            {
                return;
            }

            var result = MessageBox.Show(
                $"Are you sure you want to delete product: {productToDelete.Name}?",
                "Confirm action",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                int affectedRows = await _store.DeleteProduct(productToDelete.Id);

                if (affectedRows < 1)
                {
                    MessageBox.Show($"Something went wrong while deleting product with id: {productToDelete.Name}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Products.Remove(productToDelete);

                MessageBox.Show($"Successfully deleted product with id: {productToDelete.Name}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Error deleting product with id: {productToDelete.Id}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
