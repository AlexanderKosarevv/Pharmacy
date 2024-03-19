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

namespace Pharmacy.Pages
{
    /// <summary>
    /// Логика взаимодействия для SalesPage.xaml
    /// </summary>
    public partial class SalesPage : Page
    {
        // Компонент для организации работы с данными в таблицах DataGrid
        private CollectionViewSource SalesViewModel { get; set; }
        private int RollID;

        public SalesPage(int RollID)
        {
            InitializeComponent();
            DataContext = this;
            SalesViewModel = new CollectionViewSource();
            SalesViewModel.Filter += FilterSales;
            this.RollID = RollID;
            UpdateSalesDataGrid(null);
        }

        // Метод быстрого поиска (фильтрации данных) в таблице
        private void FilterSales(object sender, FilterEventArgs e)
        {
            Data.Sale Sale = e.Item as Data.Sale;
            string Mask = FilterTextBox.Text.ToLower();
            e.Accepted = Sale.Drug.Name.ToLower().Contains(Mask) ||
                Sale.Quantity.ToString().Contains(Mask) ||
                Sale.Employee.Surname.ToLower().Contains(Mask);
        }

        // Метод установки доступности элементов управления интерфейса страницы
        private void SetControlsEnabled()
        {
            bool DialogHidden = DialogScrollViewer.Visibility == Visibility.Hidden;
            bool ItemSelected = SalesDataGrid.SelectedIndex >= 0;
            AddSaleButton.IsEnabled = DialogHidden && RollID == 0;
            CopySaleButton.IsEnabled = DialogHidden && ItemSelected && RollID == 0;
            EditSaleButton.IsEnabled = DialogHidden && ItemSelected && RollID == 0;
            DeleteSaleButton.IsEnabled = DialogHidden && ItemSelected && RollID == 0;
            FilterSalesButton.IsEnabled = DialogHidden;
            SalesDataGrid.IsEnabled = DialogHidden && RollID == 0;
            Core.AppMainWindow.SetControlsEnabled(DialogHidden);
        }

        // Метод обновления данных в таблице Sales страницы
        public void UpdateSalesDataGrid(Data.Sale Sale)
        {
            if (Sale == null && SalesDataGrid.SelectedItem != null)
            {
                Sale = SalesDataGrid.SelectedItem as Data.Sale;
            }
            ObservableCollection<Data.Sale> Sales =
                new ObservableCollection<Data.Sale>(
                    Core.Database.Sale.Where(B => B.IDSale >= 0));
            SalesViewModel.Source = Sales;
            SalesDataGrid.ItemsSource = SalesViewModel.View;
            if (Sales.Count > 0)
            {
                SalesDataGrid.SelectedItem = Sale;
                if (SalesDataGrid.SelectedIndex < 0)
                {
                    SalesDataGrid.SelectedIndex = 0;
                }
            }
            SetControlsEnabled();
        }

        // Метод для отображения/сокрытия фильтра
        private void FilterSaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterDockPanel.Visibility == Visibility.Collapsed)
            {
                FilterDockPanel.Visibility = Visibility.Visible;
                FilterSalesButton.Content = "Скрыть поиск";
            }
            else
            {
                FilterDockPanel.Visibility = Visibility.Collapsed;
                FilterSalesButton.Content = "Поиск";
            }
        }

        // Метод отображения диалога редактирования данных
        private void ShowDialog(Page Page)
        {
            Grid.SetColumnSpan(PageDataGridDockPanel, 1);
            DialogGridSplitter.Visibility = Visibility.Visible;
            DialogScrollViewer.Visibility = Visibility.Visible;
            DialogFrame.Navigate(Page);
            SetControlsEnabled();
        }

        // Метод сокрытия диалога редактирования данных
        public void HideDialog()
        {
            Grid.SetColumnSpan(PageDataGridDockPanel, 3);
            DialogGridSplitter.Visibility = Visibility.Hidden;
            DialogScrollViewer.Visibility = Visibility.Hidden;
            DialogFrame.Navigate(null);
            while (DialogFrame.CanGoBack)
            {
                DialogFrame.RemoveBackEntry();
            }
            SetControlsEnabled();
        }

        // Добавить новую продажу
        private void AddSaleButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDialog(new SalesDlgPage(this, null, false));
        }

        // Копировать продажу
        private void CopySaleButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Sale Sale = SalesDataGrid.SelectedItem as Data.Sale;
            ShowDialog(new SalesDlgPage(this, Sale, true));
        }

        // Изменить продажу
        private void EditSaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (SalesDataGrid.SelectedIndex >= 0)
            {
                Data.Sale Sale = SalesDataGrid.SelectedItem as Data.Sale;
                ShowDialog(new SalesDlgPage(this, Sale, false));
            }
        }

        // Удалить продажу
        private void DeleteSaleButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Sale DeletingSale = SalesDataGrid.SelectedItem as Data.Sale;
            if (MessageBox.Show("Вы уверены, что хотите удалить выбранную продажу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return;
            } 
            if (SalesDataGrid.SelectedIndex < SalesDataGrid.Items.Count - 1)
            {
                SalesDataGrid.SelectedIndex++;
            }
            else
            {
                if (SalesDataGrid.SelectedIndex > 0)
                {
                    SalesDataGrid.SelectedIndex--;
                }
            }
            Data.Sale SelectingSale = SalesDataGrid.SelectedItem as Data.Sale;
            try
            {
                Core.Database.Sale.Remove(DeletingSale);
                Core.Database.SaveChanges();
                UpdateSalesDataGrid(SelectingSale);
            }
            catch
            {
                MessageBox.Show("Не удалось удалить выбранную продажу из базы данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                Core.CancelChanges(DeletingSale);
                UpdateSalesDataGrid(DeletingSale);
            }
            SalesDataGrid.Focus();
        }

        // Метод вызова фильтрации данных при наборе строки
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SalesViewModel.View.Refresh();
        }
    }
}
