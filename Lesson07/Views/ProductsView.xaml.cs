﻿using Lesson07.ViewModels;
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

namespace Lesson07.Views
{
    /// <summary>
    /// Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class ProductsView : UserControl
    {
        public ProductsView()
        {
            DataContext = new ProductsViewModel();

            InitializeComponent();
        }

        protected override async void OnInitialized(EventArgs e)
        {
            if (DataContext is ProductsViewModel vm)
            {
                await vm.LoadData();
            }

            base.OnInitialized(e);
        }
    }
}