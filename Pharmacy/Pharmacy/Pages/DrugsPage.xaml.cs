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
    /// Логика взаимодействия для DrugsPage.xaml
    /// </summary>
    public partial class DrugsPage : Page
    {
        // Компонент для организации работы с данными в таблицах DataGrid
        private CollectionViewSource DrugsViewModel { get; set; }
        private int RollID;

        public DrugsPage(int RollID)
        {
            InitializeComponent();
            DataContext = this;
            DrugsViewModel = new CollectionViewSource();
            DrugsViewModel.Filter += FilterDrugs;
            this.RollID = RollID;
            UpdateDrugsDataGrid(null);
        }

        // Метод быстрого поиска (фильтрации данных) в таблице
        private void FilterDrugs(object sender, FilterEventArgs e)
        {
            Data.Drug Drug = e.Item as Data.Drug;
            string Mask = FilterTextBox.Text.ToLower();
            e.Accepted = Drug.Name.ToLower().Contains(Mask) ||
                Drug.Price.ToString().Contains(Mask) ||
                Drug.Manufacturer.ToLower().Contains(Mask) ||
                Drug.Recipe.Name.ToLower().Contains(Mask) ||
                Drug.Type.Name.ToLower().Contains(Mask);
        }

        // Метод обновления данных в таблице Drugs страницы
        public void UpdateDrugsDataGrid(Data.Drug Drug)
        {
            if (Drug == null && DrugsDataGrid.SelectedItem != null)
            {
                Drug = DrugsDataGrid.SelectedItem as Data.Drug;
            }
            ObservableCollection<Data.Drug> Drugs = new ObservableCollection<Data.Drug>(Core.Database.Drug.Where(P => P.IDDrug >= 0));
            DrugsViewModel.Source = Drugs;
            DrugsDataGrid.ItemsSource = DrugsViewModel.View;
            if (Drugs.Count > 0)
            {
                DrugsDataGrid.SelectedItem = Drug;
                if (DrugsDataGrid.SelectedIndex < 0)
                {
                    DrugsDataGrid.SelectedIndex = 0;
                }
            }
            SetControlsEnabled();
        }

        // Метод для отображения/сокрытия фильтра
        private void FilterDrugsButton_Click(object sender, RoutedEventArgs e)
        {
            if (FilterDockPanel.Visibility == Visibility.Collapsed)
            {
                FilterDockPanel.Visibility = Visibility.Visible;
                FilterDrugsButton.Content = "Скрыть поиск";
            }
            else
            {
                FilterDockPanel.Visibility = Visibility.Collapsed;
                FilterDrugsButton.Content = "Поиск";
            }
        }

        // Метод установки доступности элементов управления интерфейса страницы
        private void SetControlsEnabled()
        {
            bool DialogHidden = DialogScrollViewer.Visibility == Visibility.Hidden;
            bool ItemSelected = DrugsDataGrid.SelectedIndex >= 0;
            AddDrugButton.IsEnabled = DialogHidden && RollID == 0;
            CopyDrugButton.IsEnabled = DialogHidden && ItemSelected && RollID == 0;
            EditDrugButton.IsEnabled = DialogHidden && ItemSelected && RollID == 0;
            DeleteDrugButton.IsEnabled = DialogHidden && ItemSelected && RollID == 0;
            FilterDrugsButton.IsEnabled = DialogHidden;
            DrugsDataGrid.IsEnabled = DialogHidden && RollID == 0;
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

        // Добавить новый препарат
        private void AddDrugButton_Click(object sender, RoutedEventArgs e)
        {
            ShowDialog(new DrugsDlgPage(this, null, false));
        }

        //Копировать препарат
        private void CopyDrugButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Drug Drug = DrugsDataGrid.SelectedItem as Data.Drug;
            ShowDialog(new DrugsDlgPage(this, Drug, true));
        }

        // Изменить препарат
        private void EditDrugButton_Click(object sender, RoutedEventArgs e)
        {
            if (DrugsDataGrid.SelectedIndex >= 0)
            {
                Data.Drug Drug = DrugsDataGrid.SelectedItem as Data.Drug;
                ShowDialog(new DrugsDlgPage(this, Drug, false));
            }
        }

        // Удалить препарат
        private void DeleteDrugButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Drug DeletingDrug = DrugsDataGrid.SelectedItem as Data.Drug;
            if (MessageBox.Show("Вы уверены, что хотите удалить выбранный препарат?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
            {
                return;
            }
            if (DrugsDataGrid.SelectedIndex < DrugsDataGrid.Items.Count - 1)
            {
                DrugsDataGrid.SelectedIndex++;
            }
            else
            {
                if (DrugsDataGrid.SelectedIndex > 0)
                {
                    DrugsDataGrid.SelectedIndex--;
                }
            }
            Data.Drug SelectingDrug = DrugsDataGrid.SelectedItem as Data.Drug;
            try
            {
                Core.Database.Drug.Remove(DeletingDrug);
                Core.Database.SaveChanges();
                UpdateDrugsDataGrid(SelectingDrug);
            }
            catch
            {
                MessageBox.Show("Не удалось удалить выбранный препарат из базы данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.None);
                Core.CancelChanges(DeletingDrug);
                UpdateDrugsDataGrid(DeletingDrug);
            }
            DrugsDataGrid.Focus();
        }

        // Метод вызова фильтрации данных при наборе строки
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DrugsViewModel.View.Refresh();
        }
    }
}
