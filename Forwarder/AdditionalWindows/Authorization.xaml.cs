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
using System.Text.RegularExpressions;

namespace Forwarder.AdditionalWindows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public bool isAuthorization = false;
        private String errorLogin = "Поле логин не заполнено. ";
        private String errorPassword = "Поле пароль не заполнено.";

        public Authorization()
        {
            InitializeComponent();
            Sources.Functions.AUTHORIZATION = this;
        }

        #region Реализация перемещения окна
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion

        private void Window_Activated(object sender, EventArgs e)
        {
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF454545"), 0.0));
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF404040"), 1.0));
            header.Background = linearGradientBrush;
            header.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3C3C3C"));
            background.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3C3C3C"));
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            header.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF6F6F6"));
            header.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
            background.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
        }

        private void TBLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex(@"^[A-Za-z][A-Za-z0-9_-]{2,}$");
            if (tbLogin.Text == "")
            {
                errorLogin = "Поле логин не заполнено. ";
                ChangeErrorLabel();
            }
            else if (!regex.IsMatch(tbLogin.Text))
            {
                errorLogin = "Логин введен не корректно. ";
                ChangeErrorLabel();
            }
            else
            {
                errorLogin = "";
                ChangeErrorLabel();
            }
        }

        private void TBPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"^[A-Za-z][A-Za-z0-9_-]{4,}$");
            if (tbPassword.Password == "")
            {
                errorPassword = "Поле пароль не заполнено.";
                ChangeErrorLabel();
            }
            else if (!regex.IsMatch(tbPassword.Password))
            {
                errorPassword = "Пароль введен не корректно.";
                ChangeErrorLabel();
            }
            else
            {
                errorPassword = "";
                ChangeErrorLabel();
            }
        }

        private void ChangeErrorLabel()
        {
            lError.Content = errorLogin + errorPassword;
            if (errorLogin + errorPassword == "")
                bGo.IsEnabled = true;
            else
                bGo.IsEnabled = false;
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BGo_Click(object sender, RoutedEventArgs e)
        {
            //Sources.Client.SendMessage("Message", new String[] { tbLogin.Text, tbPassword.Password });
            Sources.Client.SendMessage("AuthenticationAttempt", new String[] { tbLogin.Text, tbPassword.Password });
        }
    }
}
