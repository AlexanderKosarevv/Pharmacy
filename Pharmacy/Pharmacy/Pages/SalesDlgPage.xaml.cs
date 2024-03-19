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

namespace Pharmacy.Pages
{
    /// <summary>
    /// Логика взаимодействия для SalesDlgPage.xaml
    /// </summary>
    public partial class SalesDlgPage : Page
    {
        private Page Page;
        // Редактируемый объект (продажи)
        public Data.Sale Sales { get; set; }
        // Источник данных для комбинированного списка "Препараты"
        public List<Data.Drug> Drugs { get; set; }
        // Источник данных для комбинированного списка "Сотрудники"
        public List<Data.Employee> Employees { get; set; }

        public SalesDlgPage(Page Page, Data.Sale Sales, bool Copying)
        {
            InitializeComponent();
            this.Page = Page;
            LoadDrugs();
            LoadEmployees();
            if (Sales == null)
            {
                CaptionLabel.Content = "Новая продажа";
                this.Sales = new Data.Sale();
                this.Sales.IDSale = Core.VOID;
            }
            else
            {
                if (Copying)
                {
                    CaptionLabel.Content = "Новая продажа на основе выбранной";
                    this.Sales = Core.Database.Sale.AsNoTracking().FirstOrDefault(B => B.IDSale == Sales.IDSale);
                    this.Sales.IDSale = Core.VOID;
                }
                else
                {
                    CaptionLabel.Content = "Изменение информации о продаже";
                    this.Sales = Sales;
                }
            }
            DataContext = this;
        }

        // Метод загрузки списка "Препараты"
        private void LoadDrugs()
        {
            Drugs = new List<Data.Drug>(
                Core.Database.Drug
                .OrderBy(P => P.IDDrug));
            DrugComboBox.ItemsSource = Drugs;
        }

        // Метод загрузки списка "Сотрудники"
        private void LoadEmployees()
        {
            Employees = new List<Data.Employee>(
                Core.Database.Employee
                .OrderBy(P => P.IDEmployee));
            EmployeeComboBox.ItemsSource = Employees;
        }

        // Метод проверки вводимой информации
        private bool CheckInfo()
        {
            if (DrugComboBox.Text.Trim() == "")
            {
                MessageBox.Show("Не указано наименование препарата.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                DrugComboBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(CountTextBox.Text))
            {
                MessageBox.Show("Не указано количество.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                CountTextBox.Focus();
                return false;
            }
            double number;
            if (!double.TryParse(CountTextBox.Text, out number))
            {
                MessageBox.Show("Введите числовое значение.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                CountTextBox.Clear();
                CountTextBox.Focus();
                return false;
            }
            if (EmployeeComboBox.Text.Trim() == "")
            {
                MessageBox.Show("Не указан сотрудник.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                EmployeeComboBox.Focus();
                return false;
            }

            int IDDrug = (DrugComboBox.SelectedItem as Data.Drug).IDDrug;
            int Quantity;
            try
            {
                Quantity = int.Parse(CountTextBox.Text);
            }
            catch
            {
                Quantity = 0;
            }
            int IDEmployee = (EmployeeComboBox.SelectedItem as Data.Employee).IDEmployee;
            Func<Data.Sale, bool> Predicate;
            if (Sales.IDSale == Core.VOID)
            {
                Predicate = B => B.IDDrug == IDDrug
                && B.Quantity == Quantity
                && B.IDEmployee == IDEmployee;
            }
            else
            {
                Predicate = B => B.IDDrug == IDDrug
                && B.Quantity == Quantity
                && B.IDEmployee == IDEmployee
                && B.IDSale != Sales.IDSale;
            }
            int Count = Core.Database.Sale.Where(Predicate).Count();
            if (Count > 0)
            {
                MessageBox.Show("Продажа уже существует.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                DrugComboBox.Focus();
                return false;
            }
            return true;
        }

        // Метод записи информации в базу данных
        private bool SaveInfo()
        {
            try
            {
                if (Sales.IDSale == Core.VOID)
                {
                    Core.Database.Sale.Add(Sales);
                }
                Core.Database.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить изменения в базу данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                Core.CancelChanges(Sales);
                return false;
            }
            return true;
        }

        // Метод закрытия страницы диалога
        private void CloseDialog(bool NeedUpdate)
        {
            if (Page is SalesPage)
            {
                SalesPage SalesPage = Page as SalesPage;
                SalesPage.HideDialog();
                if (NeedUpdate)
                {
                    SalesPage.UpdateSalesDataGrid(Sales);
                }
                else
                {
                    SalesPage.SalesDataGrid.Items.Refresh();
                }
            }
        }

        // Метод обработки события нажатия на кнопку ОК
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            OkButton.Focus();
            if (CheckInfo() && SaveInfo())
            {
                CloseDialog(true);
            }
        }

        // Метод обработки события нажатия на кнопку Отмена
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (Sales.IDSale != Core.VOID) Core.CancelChanges(Sales);   
            CloseDialog(false);
        }
    }
}
