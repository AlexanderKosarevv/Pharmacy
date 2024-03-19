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
using System.Windows.Shapes;

namespace Pharmacy
{
    /// <summary>
    /// Логика взаимодействия для AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        // Максимальное количество попыток ввода пароля до блокировки
        private static int MaxAttemps = 3;
        // Количество оставшихся попыток ввода пароля до блокировки
        private int _Attemps = MaxAttemps;
        public int Attemps
        {
            get { return _Attemps; }
            set
            {
                if (_Attemps != value)
                {
                    _Attemps = value;
                    AttemptsLabel.Content = _Attemps;
                    if (_Attemps > 0)
                    {
                        AttemptsDockPanel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        AttemptsDockPanel.Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        public AuthWindow()
        {
            InitializeComponent();
        }

        // Метод обработки события нажатия на кнопку ОК
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            string Login = LoginTextBox.Text;
            string Password = PasswordPasswordBox.Visibility == Visibility.Visible ? PasswordPasswordBox.Password : PasswordTextBox.Text;
            try
            {
                Core.CurrentUser = Core.Database.Users.SingleOrDefault(U => U.Login == Login && U.Password == Password);
                if (Core.CurrentUser != null)
                {
                    MainWindow Window = new MainWindow(Core.CurrentUser.RollID);
                    Window.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Неверно указан логин и/или пароль.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                    Attemps--;
                    if (Attemps == 0)
                    {
                        Close();
                        Properties.Settings.Default.Save();
                        Attemps = MaxAttemps;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к серверу баз данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        // Метод для показа и сокрытия пароля
        private void PasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordPasswordBox.IsVisible)
            {
                PasswordTextBox.Visibility = Visibility.Visible;
                PasswordTextBox.Width = PasswordPasswordBox.ActualWidth;
                PasswordTextBox.Text = PasswordPasswordBox.Password;
                PasswordPasswordBox.Visibility = Visibility.Hidden;
                PasswordPasswordBox.Width = 0;
                PasswordButton.Content = FindResource("Hide");
            }
            else
            {
                PasswordPasswordBox.Visibility = Visibility.Visible;
                PasswordPasswordBox.Width = PasswordTextBox.Width;
                PasswordPasswordBox.Password = PasswordTextBox.Text;
                PasswordTextBox.Visibility = Visibility.Hidden;
                PasswordTextBox.Width = 0;
                PasswordButton.Content = FindResource("Show");
            }
        }

        // Метод обработки события нажатия на кнопку Отмена
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Метод обработки события нажатия на кнопку Регистрация
        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow RegistrationWindow = new RegistrationWindow();
            RegistrationWindow.Show();
            Close();
        }
    }
}
