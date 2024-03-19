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
    /// Логика взаимодействия для DrugsDlgPage.xaml
    /// </summary>
    public partial class DrugsDlgPage : Page
    {
        private Page Page;
        // Редактируемый объект (препараты)
        public Data.Drug Drug { get; set; }
        // Источник данных для комбинированного списка "Тип препарата"
        public List<Data.Type> Types { get; set; }
        // Источник данных для комбинированного списка "Рецепт"
        public List<Data.Recipe> Recipes { get; set; }

        public DrugsDlgPage(Page Page, Data.Drug Drug, bool Copying)
        {
            InitializeComponent();
            this.Page = Page;
            LoadTypes();
            LoadRecipes();
            if (Drug == null)
            {
                CaptionLabel.Content = "Новый препарат";
                this.Drug = new Data.Drug();
                this.Drug.IDDrug = Core.VOID;
            }
            else
            {
                if (Copying)
                {
                    CaptionLabel.Content = "Новый препарат на основе выбранного";
                    this.Drug = Core.Database.Drug.AsNoTracking().FirstOrDefault(B => B.IDDrug == Drug.IDDrug);
                    this.Drug.IDDrug = Core.VOID;
                }
                else
                {
                    CaptionLabel.Content = "Изменение информации о препарате";
                    this.Drug = Drug;
                }
            }
            DataContext = this;
        }
        // Метод загрузки списка "Рецепт"
        private void LoadRecipes()
        {
            Recipes = new List<Data.Recipe>(
                Core.Database.Recipe
                .OrderBy(P => P.IDRecipe));
            RecipeComboBox.ItemsSource = Recipes;
        }

        // Метод загрузки списка "Тип препарата"
        private void LoadTypes()
        {
            Types = new List<Data.Type>(
                Core.Database.Type
                .OrderBy(P => P.IDType));
            TypeComboBox.ItemsSource = Types;
        }

        // Метод проверки вводимой информации
        private bool CheckInfo()
        {
            if (NameTextBox.Text == "")
            {
                MessageBox.Show("Не указано наименование препарата.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                NameTextBox.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(PriceTextBox.Text))
            {
                MessageBox.Show("Не указана стоимость.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                PriceTextBox.Focus();
                return false;
            }
            double number;
            if (!double.TryParse(PriceTextBox.Text, out number))
            {
                MessageBox.Show("Введите числовое значение.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                PriceTextBox.Clear();
                PriceTextBox.Focus();
                return false;
            }
            if (ManufacturerTextBox.Text == "")
            {
                MessageBox.Show("Не указан производитель препарата.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                ManufacturerTextBox.Focus();
                return false;
            }
            if (RecipeComboBox.Text == "")
            {
                MessageBox.Show("Не указан рецепт препарата.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                RecipeComboBox.Focus();
                return false;
            }
            if (TypeComboBox.Text == "")
            {
                MessageBox.Show("Не указан тип препарата.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                TypeComboBox.Focus();
                return false;
            }

            string Name = NameTextBox.Text.ToLower();
            int Price;
            try
            {
                Price = int.Parse(PriceTextBox.Text);
            }
            catch
            {
                Price = 0;
            }
            string Manufacturer = ManufacturerTextBox.Text.ToLower();
            int IDRecipe = (RecipeComboBox.SelectedItem as Data.Recipe).IDRecipe;
            int IDType = (TypeComboBox.SelectedItem as Data.Type).IDType;

            Func<Data.Drug, bool> Predicate;
            if (Drug.IDDrug == Core.VOID)
            {
                Predicate = B => B.Name.ToLower() == Name
                && B.Price == Price
                && B.Manufacturer.ToLower() == Manufacturer
                && B.IDRecipe == IDRecipe
                && B.IDType == IDType;
            }
            else
            {
                Predicate = B => B.Name.ToLower() == Name
                && B.Price == Price
                && B.Manufacturer.ToLower() == Manufacturer
                && B.IDRecipe == IDRecipe
                && B.IDType == IDType
                && B.IDDrug != Drug.IDDrug;
            }
            int Count = Core.Database.Drug.Where(Predicate).Count();
            if (Count > 0)
            {
                MessageBox.Show("Препарат уже существует.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                NameTextBox.Focus();
                return false;
            }
            return true;
        }

        // Метод записи информации в базу данных
        private bool SaveInfo()
        {
            try
            {
                if (Drug.IDDrug == Core.VOID)
                {
                    Core.Database.Drug.Add(Drug);
                }
                Core.Database.SaveChanges();
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить изменения в базу данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None);
                Core.CancelChanges(Drug);
                return false;
            }
            return true;
        }

        // Метод закрытия страницы диалога
        private void CloseDialog(bool NeedUpdate)
        {
            if (Page is DrugsPage)
            {
                DrugsPage DrugsPage = Page as DrugsPage;
                DrugsPage.HideDialog();
                if (NeedUpdate)
                {
                    DrugsPage.UpdateDrugsDataGrid(Drug);
                }
                else
                {
                    DrugsPage.DrugsDataGrid.Items.Refresh();
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
            if (Drug.IDDrug != Core.VOID) Core.CancelChanges(Drug);
            CloseDialog(false);
        }
    }
}
