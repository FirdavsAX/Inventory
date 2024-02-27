using GalaSoft.MvvmLight;
using Lesson07.TestDataCreator;
using Lesson07.Views;
using MaterialDesignThemes.Wpf;
using MvvmHelpers;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lesson07
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<SampleItem> SampleList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            SampleList = new()
        {
            new SampleItem
            {
                Title = "Home",
                SelectedIcon = PackIconKind.CreditCard,
                UnselectedIcon = PackIconKind.CreditCardOutline,
                Notification = 1
            },
            new SampleItem
            {
                Title = "Products",
                SelectedIcon = PackIconKind.Home,
                UnselectedIcon = PackIconKind.HomeOutline,
            },
            new SampleItem
            {
                Title = "Categories",
                SelectedIcon = PackIconKind.Star,
                UnselectedIcon = PackIconKind.StarOutline,
            },
            new SampleItem
            {
                Title = "Customers",
                SelectedIcon = PackIconKind.Users,
                UnselectedIcon = PackIconKind.UsersOutline,
            },
            new SampleItem
            {
                Title = "Sales",
                SelectedIcon = PackIconKind.Folder,
                UnselectedIcon = PackIconKind.FolderOutline,
            },
            new SampleItem
            {
                Title = "Suppliers",
                SelectedIcon = PackIconKind.Bookshelf,
                UnselectedIcon = PackIconKind.Bookshelf,
            },

            new SampleItem
            {
                Title = "Supplies",
                SelectedIcon = PackIconKind.Bookshelf,
                UnselectedIcon = PackIconKind.Bookshelf,
            },
        };
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
            => SampleList[0].Notification = SampleList[0].Notification is null ? 1 : null;

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
            => SampleList[0].Notification = SampleList[0].Notification is null ? "123+" : null;

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ListBox listBox) return;

            var selectedIndex = listBox.SelectedIndex;

            UserControl content = selectedIndex switch
            {
                0 => new DashboardView(),
                1 => new ProductsView(),
                2 => new CategoriesView(),
                3 => new CustomersView(),
                4 => new SalesView(),
                _ => new DashboardView()
            };

            mainContent.Content = content;
        }

        private void ProductsView_Loaded()
        {

        }
    }

    public class SampleItem : BaseViewModel
    {
        public string? Title { get; set; }
        public PackIconKind SelectedIcon { get; set; }
        public PackIconKind UnselectedIcon { get; set; }
        private object? _notification = null;

        public object? Notification
        {
            get { return _notification; }
            set { SetProperty(ref _notification, value); }
        }

    }
}