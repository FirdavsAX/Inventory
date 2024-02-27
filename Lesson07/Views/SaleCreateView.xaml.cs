using Lesson07.Models;
using Lesson07.ViewModels;
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
    /// Interaction logic for SaleCreateView.xaml
    /// </summary>
    public partial class SaleCreateView : Window
    {
        public SaleCreateView(Sale? sale=null)
        {
            DataContext = new SalesCreateViewModel(sale);
            InitializeComponent();
        }
        protected override async void OnInitialized(EventArgs e)
        {
            if (DataContext is SalesCreateViewModel vm)
            {
                await vm.Load();
            }

            base.OnInitialized(e);
        }

        private void CancelCommand_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
