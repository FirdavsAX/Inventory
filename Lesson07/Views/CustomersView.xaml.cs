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
using Lesson07.ViewModels;
using System.Windows.Shapes;

namespace Lesson07.Views
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class CustomersView : UserControl
    {
        public CustomersView()
        {
            DataContext = new CustomerViewModel();
            InitializeComponent();
        }
        protected override async void OnInitialized(EventArgs e)
        {
            if(DataContext is CustomerViewModel vm)
            {
                await vm.LoadDataAsync();
            }
            base.OnInitialized(e);
        }
    }
}
