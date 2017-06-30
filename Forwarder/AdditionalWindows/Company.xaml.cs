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
using Newtonsoft.Json;

namespace Forwarder.AdditionalWindows
{
    /// <summary>
    /// Логика взаимодействия для Company.xaml
    /// </summary>
    public partial class Company : Window
    {
        public Company()
        {
            InitializeComponent();
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
            if (tbCompanyName.Text == "" || tbCompanyAddress.Text == "" || tbCompanyContactName.Text == "" || tbCompanyContactPhone.Text == "" || tbCompanyCountry.Text == "" || tbCompanyCity.Text == "")
            {
                lErrorMessage.Content = "Заполните все поля, помеченные *";
                return;
            }

            ClassResource.Company company = new ClassResource.Company(null, tbCompanyName.Text, tbCompanyCountry.Text, tbCompanyCity.Text, tbCompanyAddress.Text, tbCompanyContactName.Text, tbCompanyContactPhone.Text);
            Sources.Client.SendMessage("AddCompany", new String[] { JsonConvert.SerializeObject(company) });
            this.Close();
        }
    }
}
