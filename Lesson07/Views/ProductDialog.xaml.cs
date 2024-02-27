using Lesson07.Models;
using Lesson07.Stores;
using MaterialDesignThemes.Wpf;
using MvvmHelpers.Commands;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lesson07.Views
{
    /// <summary>
    /// Interaction logic for ProductDialog.xaml
    /// </summary>
    public partial class ProductDialog : UserControl
    {
        private readonly CategoriesStore _store;

        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }

        public ObservableCollection<Category> Categories { get; set; } = new();
        
        public string Title { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpireDate { get; set; } = DateTime.Now.AddDays(7);
        public Category SelectedCategory { get; set; }
        private Product product;

        public ProductDialog(ObservableCollection<Category> categories)
        {
            ArgumentNullException.ThrowIfNull(categories);
            InitializeComponent();

            _store = new CategoriesStore();
            Categories = categories;
            product = new Product();

            DataContext = this;

            CancelCommand = new Command(OnCancel);
            SaveCommand = new Command(OnSave);
            Title = "Create Product";
        }

        public ProductDialog(ObservableCollection<Category> categories, Product product)
            : this(categories)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));

            Title = "Edit Product";

            this.product = product;

            PopulateData();
        }

        private void PopulateData()
        {
            ProductName = product.Name;
            Description = product.Description;
            Price = product.Price;
            ExpireDate = product.ExpireDate;
            SelectedCategory = Categories.FirstOrDefault(x => x.Id == product.CategoryId) ?? Categories[0];
        }

        private void OnCancel()
        {
            var result = MessageBox.Show("Are you sure you want to close?", "Confirm action", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                DialogHost.Close("MainDialog");
            }
        }

        private void OnSave()
        {
            product.Name = ProductName;
            product.Description = Description;
            product.Price = Price;
            product.ExpireDate = ExpireDate;
            product.CategoryId = SelectedCategory.Id;

            DialogHost.Close("MainDialog", product);
        }

    }
}
