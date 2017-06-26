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
    /// Логика взаимодействия для Map.xaml
    /// </summary>
    public partial class Map : Window
    {
        private const double minWidth = 900;
        private const double minHight = 600;

        private double WIDTH = 0;
        private double HEIGHT = 0;
        private double TOP = 0;
        private double LEFT = 0;
        private bool Maximized = false;

        public Map(String place)
        {
            InitializeComponent();
            this.Height = SystemParameters.WorkArea.Height * 0.75;
            this.Width = SystemParameters.WorkArea.Width * 0.75;
            this.Top = SystemParameters.WorkArea.Top + SystemParameters.WorkArea.Height * 0.125;
            this.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width * 0.125;

            webBrowser.Navigate(new Uri("https://maps.google.com/maps?q=" + place.Replace(",", "").Replace("д.", "").Replace("ул.", "").Replace("просп.", "")));
            mainTitle.Content = "Карты - " + place;
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
            max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\max-hover.png", UriKind.Relative)));
        }

        private void HeaderButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.IsActive)
            {
                close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\close-normal.png", UriKind.Relative)));
                min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\min-normal.png", UriKind.Relative)));
                max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\max-normal.png", UriKind.Relative)));
            }
            else
            {
                close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
                min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
                max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
            }
        }

        private void Max_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Maximized)
            {
                SetWindowStage("Normal");
            }
            else
            {
                SetWindowStage("Maximized");
            }

        }

        private void Min_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void SetWindowStage(String _stage)
        {
            switch (_stage)
            {
                case "Maximized":
                    {
                        HEIGHT = this.Height;
                        WIDTH = this.Width;
                        TOP = this.Top;
                        LEFT = this.Left;

                        this.Height = SystemParameters.WorkArea.Height;
                        this.Width = SystemParameters.WorkArea.Width;
                        this.Top = SystemParameters.WorkArea.Top;
                        this.Left = SystemParameters.WorkArea.Left;
                        this.body.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);

                        Maximized = true;
                        break;
                    }

                case "Normal":
                    {
                        this.body.Margin = new Thickness(20.0, 20.0, 20.0, 20.0);
                        this.Height = HEIGHT;
                        this.Width = WIDTH;
                        this.Top = TOP;
                        this.Left = LEFT;

                        Maximized = false;

                        break;
                    }
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                SetWindowStage("Maximized");
                this.WindowState = WindowState.Normal;
            }
        }
        #endregion

        #region Реализация перемещения окна
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (!Maximized)
                {
                    SetWindowStage("Maximized");
                }
                else
                {
                    SetWindowStage("Normal");
                }
            }
            else if (!Maximized) this.DragMove();
        }
        #endregion

        #region Реализация активации и деактивации окна
        private void Window_Activated(object sender, EventArgs e)
        {
            close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\close-normal.png", UriKind.Relative)));
            min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\min-normal.png", UriKind.Relative)));
            max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\max-normal.png", UriKind.Relative)));
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
            max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
            header.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF6F6F6"));
            header.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
            background.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
            mainTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
        }

        #endregion
  
            
    }
}
