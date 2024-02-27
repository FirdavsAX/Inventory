using Lesson07.Models;
using MaterialDesignThemes.Wpf;
using MvvmHelpers.Commands;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
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
    /// Interaction logic for CustomerAddOrEdit.xaml
    /// </summary>
    public partial class CustomerAddOrEdit : UserControl
    {
        public bool IsEditCustomer { get; set; } = false;
        public Customer Customer { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand message { get; set; }
        public CustomerAddOrEdit(Customer? customer = null)
        {
            InitializeComponent();
            if(customer is null)
            {
                customer = new Customer();
            }
            Customer = customer;
            DataContext = this;

            SaveCommand = new DelegateCommand(OnSaveCommand);
            CancelCommand = new AsyncCommand(OnCancelCommand);
            message = new AsyncCommand(OnMessage);
        }
        public async Task OnMessage() 
        {
            MessageBox.Show("ishlayapti");
        } 
        public void OnSaveCommand()
        {
            try
            {
                if(Customer is null)
                {
                    return;
                }
                
                var phone = Customer.PhoneNumber.Substring(4, 9);
                
                if(phone.Length != 9 || !double.TryParse(phone,out double anyNumber))
                {
                    MessageBox.Show("Phone number is invalid");
                    return;
                }
                DialogHost.Close("MainDialog",Customer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async Task OnCancelCommand()
        {
            try
            {
                DialogHost.Close("MainDialog");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
