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

namespace Forwarder.AdditionalWindows
{
    /// <summary>
    /// Логика взаимодействия для Request.xaml
    /// </summary>
    public partial class Request : Window
    {
        private List<ClassResource.Company> COMPANIES;

        public Request(List<ClassResource.Company> companies, String name, String phone)
        {
            InitializeComponent();
            COMPANIES = companies;
            this.Title = "Добавление заявки - Forwarder Tools 1.0";
            mainTitle.Content = "Добавление заявки";
            tbEngineerName.Text = name;
            tbEngineerPhone.Text = phone;

            cbCompany.Items.Clear();
            foreach (ClassResource.Company item in COMPANIES)
            {
                cbCompany.Items.Add(item.Name);
            }

            DateTime thisDay = DateTime.Today;
            dpDate.Text = thisDay.ToString("d");

            bOK.Visibility = Visibility.Hidden;
        }

        public Request(List<ClassResource.Company> companies, ClassResource.Request selectRequest, String name, String phone)
        {
            InitializeComponent();
            COMPANIES = companies;
            this.Title = "Добавление заявки - Forwarder Tools 1.0";
            mainTitle.Content = "Добавление заявки";
            tbEngineerName.Text = name;
            tbEngineerPhone.Text = phone;

            cbCompany.Items.Clear();
            foreach (ClassResource.Company item in COMPANIES)
            {
                cbCompany.Items.Add(item.Name);
            }

            tbNumber.Text = selectRequest.Number;
            tbProductName.Text = selectRequest.ProductName;
            tbWeight.Text = selectRequest.ProductWeight;
            tbDimensions.Text = selectRequest.ProductDimensions;
            tbQuantity.Text = selectRequest.Quantity;

            cbCompany.SelectedItem = companies.Find(x => x.ID == selectRequest.IDCompany).Name;

            dpDate.Text = selectRequest.Date;
            bAdd.Visibility = Visibility.Hidden;
            bCancel.Visibility = Visibility.Hidden;
        }

        #region Реализация кнопок управления
        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void HeaderButton_MouseEnter(object sender, MouseEventArgs e)
        {
            close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\close-hover.png", UriKind.Relative)));
            min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\min-hover.png", UriKind.Relative)));
        }

        private void HeaderButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.IsActive)
            {
                close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\close-normal.png", UriKind.Relative)));
                min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\min-normal.png", UriKind.Relative)));
            }
            else
            {
                close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
                min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
            }
        }

        private void Min_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        #endregion

        #region Реализация перемещения окна
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion

        #region Реализация активации и деактивации окна
        private void Window_Activated(object sender, EventArgs e)
        {
            close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\close-normal.png", UriKind.Relative)));
            min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\min-normal.png", UriKind.Relative)));
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
            mainTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CCFFFFFF"));
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
            min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
            header.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF6F6F6"));
            header.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
            background.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
            mainTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
        }
        #endregion

        private void CBCompany_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cbCompany.SelectedIndex >= 0)
            {
                ClassResource.Company selectCompany = COMPANIES[cbCompany.SelectedIndex];
                tbCompanyAddress.Text = selectCompany.Address;
                tbCompanyContactName.Text = selectCompany.NameСontactPerson;
                tbCompanyContactPhone.Text = selectCompany.PhoneContactPerson;
            }
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            if(tbNumber.Text == "" || tbProductName.Text == "" || tbWeight.Text == "" || tbDimensions.Text == "" || tbQuantity.Text == "" || cbCompany.SelectedIndex < 0)
            {
                lErrorMessage.Content = "Заполните все поля, помеченные *";
                return;
            }
            if (!Int32.TryParse(tbQuantity.Text, out int temp))
            {
                lErrorMessage.Content = "Неправильно заполнено поле \"Количество\"";
                return;
            }
        }

        private void BOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(tbCompanyAddress.Text != "")
            {
                Map map = new Map(tbCompanyAddress.Text);
                map.Show();
            }
        }
    }
}
