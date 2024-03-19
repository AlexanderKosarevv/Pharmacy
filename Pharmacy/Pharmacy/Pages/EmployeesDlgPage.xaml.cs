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
    /// Логика взаимодействия для EmployeesDlgPage.xaml
    /// </summary>
    public partial class EmployeesDlgPage : Page
    {
        private Page Page;
        // Редактируемый объект (сотрудники)
        public Data.Employee Employee { get; set; }
        // Источник данных для комбинированного списка "Пол"
        public List<Data.Sexes> Sexes { get; set; }

        public EmployeesDlgPage(Page Page, Data.Employee Employee, bool Copying)
        {
            InitializeComponent();
            this.Page = Page;
            LoadSexes();
            if (Employee == null)
            {
                CaptionLabel.Content = "Новый сотрудник";
                this.Employee = new Data.Employee();
                this.Employee.IDEmployee = Core.VOID;
                this.Employee.IDSex = Core.NONE;
            }
            else
            {
                if (Copying)
                {
                    CaptionLabel.Content = "Новый сотрудник на основе выбранного";
                    this.Employee = Core.Database.Employee.AsNoTracking().FirstOrDefault(B => B.IDEmployee == Employee.IDEmployee);
                    this.Employee.IDEmployee = Core.VOID;
                }
                else
                {
                    CaptionLabel.Content = "Изменение информации о сотруднике";
                    this.Employee = Employee;
                }
            }
            DataContext = this;
        }

        // Метод загрузки списка "Пол"
        private void LoadSexes()
        {
            Sexes = new List<Data.Sexes>(
                Core.Database.Sexes
                .OrderBy(P => P.IDSex));
            SexesComboBox.ItemsSource = Sexes;
        }

        // Метод проверки вводимой информации
        private bool CheckInfo()
        {
            if (SurnameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Не указана фамилия сотрудника.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                SurnameTextBox.Focus();
                return false;
            }
            if (NameTextBox.Text.Trim() == "")
            {
                MessageBox.Show("Не указано имя сотрудника.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                NameTextBox.Focus();
                return false;
            }
            if (PatronymicTextBox.Text.Trim() == "")
            {
                if (MessageBox.Show("Не указано отчество сотрудника.\n\nВы уверены, что хотите оставить сотрудника без отчества?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
                {
                    PatronymicTextBox.Focus();
                    return false;
                }
            }
            if (BirthdateDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Не указана дата рождения сотрудника.", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.No);
                BirthdateDatePicker.Focus();
                return false;
            }

            string Surname = SurnameTextBox.Text.ToLower();
            string Name = NameTextBox.Text.ToLower();
            string Patronymic = PatronymicTextBox.Text.ToLower();
            DateTime? Birthdate = BirthdateDatePicker.SelectedDate;
            int IDSex = (SexesComboBox.SelectedItem as Data.Sexes).IDSex;
        
            Func<Data.Employee, bool> Predicate;
            if (Employee.IDEmployee == Core.VOID)
            {
                Predicate = B => B.Surname.ToLower() == Surname
                && B.Name.ToLower() == Name
                && B.Patronymic.ToLower() == Patronymic
                && B.Birthdate == Birthdate
                && B.IDSex == IDSex;
            }
            else
            {
                Predicate = B => B.Surname.ToLower() == Surname
                && B.Name.ToLower() == Name
                && B.Patronymic.ToLower() == Patronymic
                && B.Birthdate == Birthdate
                && B.IDSex == IDSex
                && B.IDEmployee != Employee.IDEmployee;
            }
            int Count = Core.Database.Employee.Where(Predicate).Count();
            if (Count > 0)
            {
                MessageBox.Show("Сотрудник уже существует.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                SurnameTextBox.Focus();
                return false;
            }
            return true;
        }

        // Метод записи информации в базу данных
        private bool SaveInfo()
        {
            try
            {
                if (Employee.IDEmployee == Core.VOID)
                {
                    Core.Database.Employee.Add(Employee);
                }
                Core.Database.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить изменения в базу данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                Core.CancelChanges(Employee);
                return false;
            }
            return true;
        }

        // Метод закрытия страницы диалога
        private void CloseDialog(bool NeedUpdate)
        {
            if (Page is EmployeesPage)
            {
                EmployeesPage EmployeesPage = Page as EmployeesPage;
                EmployeesPage.HideDialog();
                if (NeedUpdate)
                {
                    EmployeesPage.UpdateEmployeesDataGrid(Employee);
                }
                else
                {
                    EmployeesPage.EmployeesDataGrid.Items.Refresh();
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
            if (Employee.IDEmployee != Core.VOID) Core.CancelChanges(Employee);
            CloseDialog(false);
        }
    }
}
