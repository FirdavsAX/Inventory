using Lesson07.Data;
using Lesson07.Models;
using Lesson07.Views;
using MaterialDesignThemes.Wpf;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lesson07.ViewModels
{
    internal class CategoriesViewModel : BaseViewModel
    {
        private readonly InventoryDbContext context;

        public ObservableCollection<Category> Categories { get; set; }

        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                SearchCategories();
            }
        }

        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set => SetProperty(ref _categoryName, value);
        }

        public ICommand CreateCommand { get; }
        public ICommand SaveCommand { get; }

        public CategoriesViewModel()
        {
            context = new InventoryDbContext();
            CreateCommand = new Command(OnCreate);
            SaveCommand = new Command(OnSave);

            Load();
        }

        private void Load()
        {
            var loadedCategories = context.Categories.ToList();

            Categories = new ObservableCollection<Category>(loadedCategories);
        }

        private async void OnCreate()
        {
            var view = new CategoryDialog()
            {
                DataContext = this
            };

            var result = await DialogHost.Show(view, "MainDialog");

        }

        private void OnSave()
        {
            var result = MessageBox.Show("Are you sure you want to close form?", "Confirm action", MessageBoxButton.YesNo);
            
            if (result == MessageBoxResult.No)
            {
                return;
            }

            DialogHost.Close("MainDialog");
        }

        private void SearchCategories()
        {
            var filteredCategories = context.Categories
                .Where(x => x.Name.Contains(_search))
                .ToList();

            Categories.Clear();
            
            foreach(var category in filteredCategories)
            {
                Categories.Add(category);
            }
        }
    }
}
