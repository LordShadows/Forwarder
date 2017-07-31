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
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : Window
    {
        private List<ClassResource.Forwarder> FORWARDERS;
        private List<ClassResource.Company> COMPANIES;
        private List<ClassResource.Request> REQUESTS;
        private List<ClassResource.Destination> DESTINATIONS;
        private List<ClassResource.Route> ROUTES;
        private String ID;

        public Report(List<ClassResource.Forwarder> forwarders, List<ClassResource.Route> routes, List<ClassResource.Destination> destinations, List<ClassResource.Company> companies, List<ClassResource.Request> requests)
        {
            FORWARDERS = forwarders;
            COMPANIES = companies;
            REQUESTS = requests;
            DESTINATIONS = destinations;
            ROUTES = routes;
            InitializeComponent();

            cbForwarders.Items.Clear();
            cbForwarders.Items.Add("Все");
            foreach (var item in FORWARDERS)
            {
                cbForwarders.Items.Add(item.Name);
            }
            cbForwarders.SelectedIndex = 0;
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
            mainTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CCFFFFFF"));
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            header.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF6F6F6"));
            header.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
            background.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
            mainTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
        }
        #endregion

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dpStartDate.Text == "" || dpEndDate.Text == "" || dpEndDate.SelectedDate < dpStartDate.SelectedDate)
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Необходимо указать корректную дату начала отчетного периода и дату завершения.", "Некорректное заполнение");
                return;
            }

            if(cbForwarders.SelectedItem.ToString() != "Все")
            {
                Sources.WorkWithExcel.TotalReport(new List<ClassResource.Forwarder> { FORWARDERS[cbForwarders.SelectedIndex - 1] }, ROUTES.FindAll(x => x.DepartureDate == "" ? false : DateTime.Parse(x.DepartureDate) >= dpStartDate.SelectedDate && DateTime.Parse(x.DepartureDate) <= dpEndDate.SelectedDate), DESTINATIONS, COMPANIES, REQUESTS);
            }
            else
            {
                Sources.WorkWithExcel.TotalReport(FORWARDERS, ROUTES.FindAll(x => x.DepartureDate == "" ? false : DateTime.Parse(x.DepartureDate) >= dpStartDate.SelectedDate && DateTime.Parse(x.DepartureDate) <= dpEndDate.SelectedDate), DESTINATIONS, COMPANIES, REQUESTS);
            }
            this.Close();
        }
    }
}
