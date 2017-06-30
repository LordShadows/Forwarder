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
using Newtonsoft.Json;

namespace Forwarder.AdditionalWindows
{
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : Window
    {
        List<ClassResource.Engineer> engineers;
        List<ClassResource.Forwarder> forwarders;
        List<ClassResource.User> users;

        Regex regexLogin = new Regex(@"^[A-Za-z][A-Za-z0-9_-]{2,}$");
        Regex regexPassword = new Regex(@"^[A-Za-z0-9_-]{4,}$");

        public User(List<ClassResource.Engineer> engineers, List<ClassResource.Forwarder> forwarders, List<ClassResource.User> users)
        {
            InitializeComponent();

            this.engineers = engineers;
            this.forwarders = forwarders;
            this.users = users;

            cbUserRole.Items.Clear();
            cbUserRole.Items.Add("Инженер");
            cbUserRole.Items.Add("Экспедитор");
            cbUserRole.Items.Add("Руководитель экспедиторов");
            cbUserRole.Items.Add("Администратор");

            cbUserSnappingInfo.Items.Clear();
            foreach (var item in engineers)
            {
                cbUserSnappingInfo.Items.Add(item.Name + "(Инженер)");
            }
            foreach (var item in forwarders)
            {
                cbUserSnappingInfo.Items.Add(item.Name + "(Экспедитор)");
            }
        }

        #region Реализация перемещения окна
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion

        #region Реализация активации и деактивации окна
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
        #endregion

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BGo_Click(object sender, RoutedEventArgs e)
        {
            if (tbUserLogin.Text == "" || tbUserName.Text == "" || tbUserFirstPassword.Password == "" || tbUserSecondPassword.Password == "" || cbUserRole.SelectedIndex < 0)
            {
                lErrorMessage.Content = "Заполните все поля, помеченные *";
                return;
            }
            if (!regexLogin.IsMatch(tbUserLogin.Text))
            {
                lErrorMessage.Content = "Логин введен некорректно";
                return;
            }
            if (users.Find(x=> x.Login == tbUserLogin.Text) != null)
            { 
                lErrorMessage.Content = "Пользователь с таким логином уже существует";
                return;
            }
            if (!regexPassword.IsMatch(tbUserFirstPassword.Password))
            {
                lErrorMessage.Content = "Пароль введен некорректно";
                return;
            }
            if (cbUserSnappingInfo.SelectedIndex < 0 && cbUserRole.SelectedItem.ToString() == "Инженер")
            {
                lErrorMessage.Content = "Для инженера обязательна ссылка";
                return;
            }
            if (tbUserFirstPassword.Password != tbUserSecondPassword.Password)
            {
                lErrorMessage.Content = "Пароли не совпадают";
                return;
            }

            ClassResource.User user = new ClassResource.User(tbUserLogin.Text, tbUserName.Text, cbUserRole.SelectedItem.ToString(), tbUserSnapping.Text, null, null, Sources.Cryptography.GetHash(tbUserFirstPassword.Password));
            Sources.Client.SendMessage("AddUser", new String[] { JsonConvert.SerializeObject(user) });
            this.Close();
        }

        private void CBUserSnappingInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbUserSnappingInfo.SelectedIndex >= 0)
            {
                if(cbUserSnappingInfo.SelectedIndex >= engineers.Count)
                {
                    tbUserSnapping.Text = forwarders[cbUserSnappingInfo.SelectedIndex - engineers.Count].ID;
                    cbUserRole.SelectedItem = "Экспедитор";
                    tbUserName.Text = forwarders[cbUserSnappingInfo.SelectedIndex - engineers.Count].Name;
                }
                else
                {
                    tbUserSnapping.Text = engineers[cbUserSnappingInfo.SelectedIndex].ID;
                    cbUserRole.SelectedItem = "Инженер";
                    tbUserName.Text = engineers[cbUserSnappingInfo.SelectedIndex].Name;
                }
            }
        }

        private void TBUserSnapping_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (var item in engineers)
            {
                if (item.ID == tbUserSnapping.Text)
                {
                    cbUserSnappingInfo.SelectedItem = item.Name + "(Инженер)";
                    cbUserRole.SelectedItem = "Инженер";
                    tbUserName.Text = item.Name;
                    return;
                }
            }
            foreach (var item in forwarders)
            {
                if (item.ID == tbUserSnapping.Text)
                {
                    cbUserSnappingInfo.SelectedItem = item.Name + "(Экспедитор)";
                    cbUserRole.SelectedItem = "Экспедитор";
                    tbUserName.Text = item.Name;
                    return;
                }
            }
            cbUserSnappingInfo.SelectedIndex = -1;
            cbUserSnappingInfo.Text = "";
        }
    }
}
