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
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {
        // Компонент для организации работы с данными в таблицах DataGrid
        private CollectionViewSource EmployeesViewModel { get; set; }
        private int RollID;

        public EmployeesPage(int RollID)
        {
            InitializeComponent();
            DataContext = this;
            EmployeesViewModel = new CollectionViewSource();
            EmployeesViewModel.Filter += FilterEmployees;
            this.RollID = RollID;
            UpdateEmployeesDataGrid(null);
        }

        // Метод быстрого поиска (фильтрации данных) в таблице
        private void FilterEmployees(object sender, FilterEventArgs e)
        {
            Data.Employee Employee = e.Item as Data.Employee;
            string Mask = FilterTextBox.Text.ToLower();
            e.Accepted = Employee.Surname.ToLower().Contains(Mask) ||
                Employee.Name.ToLower().Contains(Mask) ||
                Employee.Patronymic.ToLower().Contains(Mask) ||
                Employee.Birthdate.ToString().Contains(Mask) ||
                Employee.Sexes.Name.ToString().Contains(Mask);
        }

        // Метод обновления данных в таблице Employees страницы
        public void UpdateEmployeesDataGrid(Data.Employee Employee)
        {
            if (Employee == null && EmployeesDataGrid.SelectedItem != null)
            {
                Employee = EmployeesDataGrid.SelectedItem as Data.Employee;
            }
            ObservableCollection<Data.Employee> Employees = new ObservableCollection<Data.Employee>(Core.Database.Employee.Where(P => P.IDEmployee >= 0));
            EmployeesViewModel.Source = Employees;
            EmployeesDataGrid.ItemsSource = EmployeesViewModel.View;
            if (Employees.Count > 0)
            {
                EmployeesDataGrid.SelectedItem = Employee;
                if (EmployeesDataGrid.SelectedIndex < 0)
                {
                    EmployeesDataGrid.SelectedIndex = 0;
                }
            }
            SetControlsEnabled();
        }

        // Метод для отображения/сокрытия фильтра
        private void FilterEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterDockPanel.Visibility == Visibility.Collapsed)
            {
                FilterDockPanel.Visibility = Visibility.Visible;
                FilterEmployeesButton.Content = "Скрыть поиск";
            }
            else
            {
                FilterDockPanel.Visibility = Visibility.Collapsed;
                FilterEmployeesButton.Content = "Поиск";
            }
        }

        // Метод установки доступности элементов управления интерфейса страницы
        private void SetControlsEnabled()
        {
            bool DialogHidden = DialogScrollViewer.Visibility == Visibility.Hidden;
            bool ItemSelected = EmployeesDataGrid.SelectedIndex >= 0;
            AddEmployeeButton.IsEnabled = DialogHidden && RollID == 0;
            CopyEmployeeButton.IsEnabled = DialogHidden && ItemSelected && RollID == 0;
            EditEmployeeButton.IsEnabled = DialogHidden && ItemSelected && RollID == 0;
            DeleteEmployeeButton.IsEnabled = DialogHidden && ItemSelected && RollID == 0;
            FilterEmployeesButton.IsEnabled = DialogHidden;
            EmployeesDataGrid.IsEnabled = DialogHidden && RollID == 0;
            Core.AppMainWindow.SetControlsEnabled(DialogHidden);
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

        // Добавить нового сотрудника
        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDialog(new EmployeesDlgPage(this, null, false));
        }

        // Копировать сотрудника
        private void CopyEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Employee Employee = EmployeesDataGrid.SelectedItem as Data.Employee;
            ShowDialog(new EmployeesDlgPage(this, Employee, true));
        }

        // Изменить сотрудника
        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesDataGrid.SelectedIndex >= 0)
            {
                Data.Employee Employee = EmployeesDataGrid.SelectedItem as Data.Employee;
                ShowDialog(new EmployeesDlgPage(this, Employee, false));
            }
        }

        // Удалить сотрудника
        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Employee DeletingEmployee = EmployeesDataGrid.SelectedItem as Data.Employee;
            if (MessageBox.Show("Вы уверены, что хотите удалить выбранного сотрудника?", "Подтверждение",MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return;
            }
            if (EmployeesDataGrid.SelectedIndex < EmployeesDataGrid.Items.Count - 1)
            {
                EmployeesDataGrid.SelectedIndex++;
            } else
            {
                if (EmployeesDataGrid.SelectedIndex > 0)
                {
                    EmployeesDataGrid.SelectedIndex--;
                }
            }
            Data.Employee SelectingEmployee = EmployeesDataGrid.SelectedItem as Data.Employee;
            try
            {
                Core.Database.Employee.Remove(DeletingEmployee);
                Core.Database.SaveChanges();
                UpdateEmployeesDataGrid(SelectingEmployee);
            } catch
            {
                MessageBox.Show("Не удалось удалить выбранного сотрудника из базы данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                Core.CancelChanges(DeletingEmployee);
                UpdateEmployeesDataGrid(DeletingEmployee);
            }
            EmployeesDataGrid.Focus();
        }

        // Метод вызова фильтрации данных при наборе строки
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EmployeesViewModel.View.Refresh();
        }
    }
}
