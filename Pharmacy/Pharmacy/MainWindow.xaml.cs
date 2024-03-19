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

namespace Pharmacy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // набор страниц
        private List<Page> ActivePages;
        // индекс текущей страницы
        private int CurrentPageIndex;
        // роль пользователя
        private int RollID;

        public MainWindow(int RollID)
        {
            InitializeComponent();
            ActivePages = new List<Page>();
            CurrentPageIndex = -1;
            Core.AppMainWindow = this;
            this.RollID = RollID;
        }

        // Метод установки доступности элементов управления интерфейса страницы
        public void SetControlsEnabled(bool DialogHidden)
        {
            SalesButton.IsEnabled = DialogHidden;
            EmployeesButton.IsEnabled = DialogHidden;
            DrugsButton.IsEnabled = DialogHidden;

            NextButton.IsEnabled = CurrentPageIndex < ActivePages.Count - 1;
            PreviosButton.IsEnabled = CurrentPageIndex > 0;
            CloseButton.IsEnabled = ActivePages.Count > 0;
            CloseAllButton.IsEnabled = ActivePages.Count > 1;
        }
        // Поиск нужной страницы по типу
        private int GetCurrentPageIndexByType(Type PageType)
        {
            int Index = ActivePages.Count - 1;
            while (Index >= 0 && ActivePages[Index].GetType() != PageType)
            {
                Index--;
            }
            return Index;
        }

        // Отображение страницы
        private void ShowPage(Type PageType)
        {
            if (PageType != null)
            {
                Page Page;
                CurrentPageIndex = GetCurrentPageIndexByType(PageType);
                if (CurrentPageIndex < 0)
                {
                    Page = (Page)Activator.CreateInstance(PageType, RollID);
                    ActivePages.Add(Page);
                    CurrentPageIndex = ActivePages.Count - 1;
                }
                else
                {
                    Page = ActivePages[CurrentPageIndex];
                }
                RootFrame.Navigate(Page);
            }
        }

        // Удаление страницы с фрейма
        private void RemovePagesFromFrame()
        {
            while (RootFrame.CanGoBack)
            {
                RootFrame.RemoveBackEntry();
            }
        }

        // Закрытие страницы
        public void ClosePage()
        {
            if (ActivePages.Count > 0)
            {
                ActivePages.RemoveAt(CurrentPageIndex);
                if (CurrentPageIndex > 0)
                {
                    CurrentPageIndex--;
                }
                else
                {
                    if (CurrentPageIndex >= ActivePages.Count)
                    {
                        CurrentPageIndex--;
                    }
                }
                if (CurrentPageIndex >= 0)
                {
                    RootFrame.Navigate(ActivePages[CurrentPageIndex]);
                }
                else
                {
                    RootFrame.Navigate(null);
                }
            }
        }

        // Закрытие всех страницы
        private void CloseAllPage()
        {
            ActivePages.RemoveAll(p => true);
            CurrentPageIndex = -1;
            RootFrame.Navigate(null);
        }

        // Обработчик события нажатия на кнопку закрытия страницы
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ClosePage();
        }

        // Обработчик события нажатия на кнопку закрытия всех страниц
        private void CloseAllButton_Click(object sender, RoutedEventArgs e)
        {
            CloseAllPage();
        }

        // Обработчик события нажатия на кнопку "следующая страницы"
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageIndex < ActivePages.Count - 1)
            {
                CurrentPageIndex++;
            }
            RootFrame.Navigate(ActivePages[CurrentPageIndex]);
        }

        // Обработчик события нажатия на кнопку "предыдущая страница"
        private void PreviosButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPageIndex > 0)
            {
                CurrentPageIndex--;
            }
            RootFrame.Navigate(ActivePages[CurrentPageIndex]);
        }

        // Обработчик события нажатия на кнопку "продажи"
        private void SalesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.SalesPage));
        }

        // Обработчик события нажатия на кнопку "сотрудники"
        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.EmployeesPage));
        }

        // Обработчик события нажатия на кнопку "препараты"
        private void DrugsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(typeof(Pages.DrugsPage));
        }

        // Обработчик события загрузки окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetControlsEnabled(true);
        }

        // Обработчик события загрузки фрейма
        private void RootFrame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            RemovePagesFromFrame();
            SetControlsEnabled(true);
        }
    }
}
