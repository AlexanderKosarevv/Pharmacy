using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Pharmacy
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private string Capthca;
        private const int CapthcaLength = 5;
        public RegistrationWindow()
        {
            InitializeComponent();
            CreateCapthca();
        }

        // Метод обработки события нажатия на кнопку Отмена
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow AuthWindow = new AuthWindow();
            AuthWindow.Show();
            Close();
        }

        // Метод обработки события нажатия на кнопку Сменить
        private void ChangeCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            CreateCapthca();
        }

        // Метод создания капчи
        private void CreateCapthca()
        {
            Capthca = "";
            Random Gen = new Random();
            string Alphabet = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < CapthcaLength; i++)
            {
                Capthca += Alphabet[Gen.Next(Alphabet.Length)];
            }
            int W = 310;
            int H = 100;
            Bitmap Bitmap = new Bitmap(W, H);
            Graphics Graphics = Graphics.FromImage(Bitmap);
            Graphics.Clear(System.Drawing.Color.AliceBlue);
            System.Drawing.Color[] Colors = { System.Drawing.Color.Violet, System.Drawing.Color.HotPink, System.Drawing.Color.Black, System.Drawing.Color.Blue, System.Drawing.Color.Green };
            int X = Gen.Next(W / 3);
            int Y = Gen.Next(H / 3);
            Graphics.DrawString(Capthca,
                new Font("Arial", 35),
                new SolidBrush(Colors[Gen.Next(Colors.Length)]),
                new System.Drawing.Point(X, Y));
            // Помехи в виде точек
            for (X = 0; X < W; X++)
            {
                for (Y = 0; Y < H; Y++)
                {
                    if (Gen.Next() % 10 == 0) // Количество точек
                    {
                        Bitmap.SetPixel(X, Y, Colors[Gen.Next(Colors.Length)]);
                    }
                }
            }
            // Помехи в виде линий
            for (int I = 0; I < 20; I++)
            {
                Graphics.DrawLine(new System.Drawing.Pen(Colors[Gen.Next(Colors.Length)]), Gen.Next(W), Gen.Next(H), Gen.Next(W), Gen.Next(H));
            }
            // Преобразование созданного изображения к типу BitmapImage для передачи в CaptchaImage
            CapthcaImage.Source = BitmapToBitmapImage(Bitmap);
        }
        private BitmapImage BitmapToBitmapImage(Bitmap Bitmap)
        {
            MemoryStream Stream = new MemoryStream();
            Bitmap.Save(Stream, System.Drawing.Imaging.ImageFormat.Png); // сохраняем поток
            Stream.Position = 0; // возвращаемся в начало
            BitmapImage Image = new BitmapImage();
            Image.BeginInit();
            Image.StreamSource = Stream;
            Image.CacheOption = BitmapCacheOption.OnLoad; // загружаем поток
            Image.EndInit();
            Image.Freeze(); // нельзя изменять картинку
            return Image;
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

        // Метод обработки события нажатия на кнопку ОК
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckInfo() && SaveInfo())
            {
                MessageBox.Show("Вы успешно зарегистрировались", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                AuthWindow AuthWindow = new AuthWindow();
                AuthWindow.Show();
                Close();
            }
        }

        // Проверка информации
        private bool CheckInfo()
        {
            if (LoginTextBox.Text == "")
            {
                MessageBox.Show("Не указан логин.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                LoginTextBox.Focus();
                return false;
            }

            string Password = PasswordPasswordBox.Visibility == Visibility.Visible ? PasswordPasswordBox.Password : PasswordTextBox.Text;

            if (Password == "")
            {
                MessageBox.Show("Не указан пароль.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                PasswordTextBox.Focus();
                return false;
            }

            if (CaptchaTextBox.Text != Capthca)
            {
                MessageBox.Show("Введенный текст не соответствует капче.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                CaptchaTextBox.Focus();
                return false;
            }
            return true;
        }

        // Сохранение информации о новом пользователе
        private bool SaveInfo()
        {
            try
            {
                Data.Users User = new Data.Users();
                User.UserID = Core.VOID;
                User.Login = LoginTextBox.Text;
                User.Password = PasswordPasswordBox.IsVisible ?
                    PasswordPasswordBox.Password :
                    PasswordTextBox.Text;
                User.RollID = 1;
                Core.Database.Users.Add(User);
                Core.Database.SaveChanges();
                return true;
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить информацию о новом пользователе в базу данных.", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                Core.CancelChanges(Core.Database.Users);
                return false;
            }
        }

        // Метод обработки события нажатия на кнопку Озвучить
        private void PronounceCaptchaButton_Click(object sender, RoutedEventArgs e)
        {
            System.Speech.Synthesis.SpeechSynthesizer Synthesizer = new System.Speech.Synthesis.SpeechSynthesizer();
            Synthesizer.Volume = 100;
            foreach (char C in Capthca)
            {
                Synthesizer.Speak(C.ToString());
            }
        }
    }
}
