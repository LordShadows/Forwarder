using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Forwarder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Объявление переменных для изменения размеров окна
        bool isRightResize = false;
        bool isLeftResize = false;
        bool isBottomResize = false;
        bool isTopResize = false;
        bool isRightBottomResize = false;
        bool isLeftTopResize = false;
        bool isRightTopResize = false;
        bool isLeftBottomResize = false;
        double positionRightResize = 0;
        double positionLeftResize = 0;
        double positionBottomResize = 0;
        double positionTopResize = 0;
        double positionXRightBottomResize = 0;
        double positionYRightBottomResize = 0;
        double positionXLeftTopResize = 0;
        double positionYLeftTopResize = 0;
        double positionXRightTopResize = 0;
        double positionYRightTopResize = 0;
        double positionXLeftBottomResize = 0;
        double positionYLeftBottomResize = 0;
        #endregion

        private List<ClassResource.User> USERS;
        private List<ClassResource.Engineer> ENGINEERS;
        private List<ClassResource.Forwarder> FORWARDERS;
        private List<ClassResource.Company> COMPANIES;
        private List<ClassResource.Request> REQUESTS;
        private List<ClassResource.Destination> DESTINATIONS;
        private List<ClassResource.Route> ROUTES;

        private ClassResource.User SELECTUSER;
        private ClassResource.Engineer SELECTENGINEER;
        private ClassResource.Forwarder SELECTFORWARDER;
        private ClassResource.Company SELECTCOMPANY;
        private ClassResource.Request SELECTREQUEST;
        private ClassResource.Destination SELECTDESTINATION;
        private ClassResource.Route SELECTROUTE;

        public String USERLOGIN = "";
        public String USERNAME = "";
        public String USERROLE = "";
        public String USERSNAPPING = "";

        private const double minWidth = 900;
        private const double minHeight = 600;

        private const String APPNAME = "Forwarder Tools 1.0";

        public MainWindow()
        {
            InitializeComponent();
            Properties.Settings.Default.Maximized = false;
            Properties.Settings.Default.Minimized = false;
            Properties.Settings.Default.Save();
            Sources.Functions.MAINWINDOW = this;

            this.Height = SystemParameters.WorkArea.Height * 0.8 < minHeight ? minHeight : SystemParameters.WorkArea.Height * 0.8;
            this.Width = SystemParameters.WorkArea.Width * 0.8 < minWidth ? minWidth : SystemParameters.WorkArea.Width * 0.8;

            InitComponent();
        }

        public void InitWindow()
        {
            switch (USERROLE)
            {
                case "Администратор":
                    Sources.Client.SendMessage("UpdateAllData", new String[] { });
                    
                    tcPages.SelectedItem = RequestsPage;

                    bCompanyAdd.IsEnabled = true;
                    bEngineerAdd.IsEnabled = true;
                    bForwarderAdd.IsEnabled = true;
                    bTotalReport.IsEnabled = true;

                    break;
                case "Инженер":
                    Sources.Client.SendMessage("UpdateAllData", new String[] { });

                    engineersPage.Height = 0;
                    tcPages.Items.Remove(EngineersPage);

                    usersPage.Height = 0;
                    tcPages.Items.Remove(UsersPage);

                    bRequestAdd.IsEnabled = true;
                    bCompanyAdd.IsEnabled = true;

                    tcPages.SelectedItem = RequestsPage;
                    break;
                case "Экспедитор":
                    Sources.Client.SendMessage("UpdateAllData", new String[] { });

                    usersPage.Height = 0;
                    tcPages.Items.Remove(UsersPage);

                    forwardersPage.Height = 0;
                    tcPages.Items.Remove(ForwardersPage);

                    tcPages.SelectedItem = RequestsPage;
                    break;
                case "Руководитель экспедиторов":
                    Sources.Client.SendMessage("UpdateAllData", new String[] { });

                    usersPage.Height = 0;
                    tcPages.Items.Remove(UsersPage);

                    bTotalReport.IsEnabled = true;
                    bForwarderAdd.IsEnabled = true;

                    tcPages.SelectedItem = RequestsPage;
                    break;
            }
        }

        private void InitComponent()
        {
            cbUserRole.Items.Clear();
            cbUserRole.Items.Add("Инженер");
            cbUserRole.Items.Add("Экспедитор");
            cbUserRole.Items.Add("Руководитель экспедиторов");
            cbUserRole.Items.Add("Администратор");

            cbRouteCar.Items.Clear();
            cbRouteCar.Items.Add("Легковой");
            cbRouteCar.Items.Add("Фургон");
            cbRouteCar.Items.Add("Платформа");
            cbRouteCar.Items.Add("Тентованный");
        }

        #region Реализация кнопок управления
        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
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
            if(Properties.Settings.Default.Maximized)
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
                        Properties.Settings.Default.Height = this.Height;
                        Properties.Settings.Default.Width = this.Width;
                        Properties.Settings.Default.Top = this.Top;
                        Properties.Settings.Default.Left = this.Left;

                        this.Height = SystemParameters.WorkArea.Height;
                        this.Width = SystemParameters.WorkArea.Width;
                        this.Top = SystemParameters.WorkArea.Top;
                        this.Left = SystemParameters.WorkArea.Left;
                        this.body.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);

                        Properties.Settings.Default.Maximized = true;
                        Properties.Settings.Default.Save();
                        break;
                    }

                case "Normal":
                    {
                        this.body.Margin = new Thickness(20.0, 20.0, 20.0, 20.0);
                        this.Height = Properties.Settings.Default.Height;
                        this.Width = Properties.Settings.Default.Width;
                        this.Top = Properties.Settings.Default.Top;
                        this.Left = Properties.Settings.Default.Left;

                        Properties.Settings.Default.Maximized = false;
                        Properties.Settings.Default.Save();

                        break;
                    }
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
            {
                SetWindowStage("Maximized");
                this.WindowState = WindowState.Normal;
            }
        }
        #endregion

        #region Реализация перемещения окна
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if (!Properties.Settings.Default.Maximized)
                {
                    SetWindowStage("Maximized");
                }
                else
                {
                    SetWindowStage("Normal");
                }
            }
            else if (!Properties.Settings.Default.Maximized) this.DragMove();
        }
        #endregion

        #region Реализация изменения размеров окна
        private void RightResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width + (e.GetPosition(this).X - positionRightResize);
                if (newWidth > minWidth + 40) {
                    this.Width = newWidth;
                    positionRightResize = e.GetPosition(this).X;
                }
            }
        }

        private void RightBottomResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightBottomResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width + (e.GetPosition(this).X - positionXRightBottomResize);
                double newHeight = this.Height + (e.GetPosition(this).Y - positionYRightBottomResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    positionXRightBottomResize = e.GetPosition(this).X;
                }
                if (newHeight > minHeight + 40)
                {
                    this.Height = newHeight;
                    positionYRightBottomResize = e.GetPosition(this).Y;
                }
            }
        }

        private void LeftTopResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftTopResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width - (e.GetPosition(this).X - positionXLeftTopResize);
                double newHeight = this.Height - (e.GetPosition(this).Y - positionYLeftTopResize);
                double newLeft = this.Left + (e.GetPosition(this).X - positionXLeftTopResize);
                double newTop = this.Top + (e.GetPosition(this).Y - positionYLeftTopResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    this.Left = newLeft;
                }
                if (newHeight > minHeight + 40)
                {
                    this.Height = newHeight;
                    this.Top = newTop;
                }
            }
        }

        private void LeftBottomResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftBottomResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width - (e.GetPosition(this).X - positionXLeftBottomResize);
                double newHeight = this.Height + (e.GetPosition(this).Y - positionYLeftBottomResize);
                double newLeft = this.Left + (e.GetPosition(this).X - positionXLeftBottomResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    this.Left = newLeft;
                }
                if (newHeight > minHeight + 40)
                {
                    this.Height = newHeight;
                    positionYLeftBottomResize = e.GetPosition(this).Y;
                }
            }
        }

        private void RightTopResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightTopResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width + (e.GetPosition(this).X - positionXRightTopResize);
                double newHeight = this.Height - (e.GetPosition(this).Y - positionYRightTopResize);
                double newTop = this.Top + (e.GetPosition(this).Y - positionYRightTopResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    positionXRightTopResize = e.GetPosition(this).X;
                }
                if (newHeight > minHeight + 40)
                {
                    this.Height = newHeight;
                    this.Top = newTop;
                }
            }
        }

        private void LeftResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width - (e.GetPosition(this).X - positionLeftResize);
                double newLeft = this.Left + (e.GetPosition(this).X - positionLeftResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    this.Left = newLeft;
                }
            }
        }

        private void BottomResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isBottomResize && !Properties.Settings.Default.Maximized)
            {
                double newHeight = this.Height + (e.GetPosition(this).Y - positionBottomResize);
                if (newHeight > minHeight + 40)
                {
                    this.Height = newHeight;
                    positionBottomResize = e.GetPosition(this).Y;
                }
            }
        }

        private void TopResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTopResize && !Properties.Settings.Default.Maximized)
            {
                double newHeight = this.Height - (e.GetPosition(this).Y - positionTopResize);
                double newTop = this.Top + (e.GetPosition(this).Y - positionTopResize);
                if (newHeight > minHeight + 40)
                {
                    this.Height = newHeight;
                    this.Top = newTop;
                }
            }
        }

        private void Resize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            rect.CaptureMouse();
            switch (rect.Name)
            {
                case "rightResize":
                    isRightResize = true;
                    positionRightResize = e.GetPosition(this).X;
                    break;
                case "leftResize":
                    isLeftResize = true;
                    positionLeftResize = e.GetPosition(this).X;
                    break;
                case "bottomResize":
                    isBottomResize = true;
                    positionBottomResize = e.GetPosition(this).Y;
                    break;
                case "topResize":
                    isTopResize = true;
                    positionTopResize = e.GetPosition(this).Y;
                    break;
                case "rightBottomResize":
                    isRightBottomResize = true;
                    positionXRightBottomResize = e.GetPosition(this).X;
                    positionYRightBottomResize = e.GetPosition(this).Y;
                    break;
                case "leftTopResize":
                    isLeftTopResize = true;
                    positionXLeftTopResize = e.GetPosition(this).X;
                    positionYLeftTopResize = e.GetPosition(this).Y;
                    break;
                case "rightTopResize":
                    isRightTopResize = true;
                    positionXRightTopResize = e.GetPosition(this).X;
                    positionYRightTopResize = e.GetPosition(this).Y;
                    break;
                case "leftBottomResize":
                    isLeftBottomResize = true;
                    positionXLeftBottomResize = e.GetPosition(this).X;
                    positionYLeftBottomResize = e.GetPosition(this).Y;
                    break;
            } 
        }

        private void Resize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
            switch (rect.Name)
            {
                case "rightResize":
                    isRightResize = false;
                    break;
                case "leftResize":
                    isLeftResize = false;
                    break;
                case "bottomResize":
                    isBottomResize = false;
                    break;
                case "topResize":
                    isTopResize = false;
                    break;
                case "rightBottomResize":
                    isRightBottomResize = false;
                    break;
                case "leftTopResize":
                    isLeftTopResize = false;
                    break;
                case "rightTopResize":
                    isRightTopResize = false;
                    break;
                case "leftBottomResize":
                    isLeftBottomResize = false;
                    break;
            }
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

        #region Реализация перехода между страницами
        private void TCPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((TabItem)tcPages.SelectedItem).Name)
            {
                case "RequestsPage":
                    mainTitle.Content = "Заявки";
                    this.Title = APPNAME + " - Заявки";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "RoutesPage":
                    mainTitle.Content = "Маршруты";
                    this.Title = APPNAME + " - Маршруты";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "CompaniesPage":
                    mainTitle.Content = "Фирмы";
                    this.Title = APPNAME + " - Фирмы";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "ForwardersPage":
                    mainTitle.Content = "Экспедиторы";
                    this.Title = APPNAME + " - Экспедиторы";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "EngineersPage":
                    mainTitle.Content = "Инженера";
                    this.Title = APPNAME + " - Инженера";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "UsersPage":
                    mainTitle.Content = "Пользователи";
                    this.Title = APPNAME + " - Пользователи";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "ServerPage":
                    mainTitle.Content = "Сервер";
                    this.Title = APPNAME + " - Сервер";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    break;
            }
        }

        private void SettingMessageBotton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            settingMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
            AdditionalWindows.Settings setting = new AdditionalWindows.Settings();
            setting.Show();
        }

        private void SettingMessageBotton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            settingMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
        }

        private void ExitMessageBotton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            exitMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
            App.Current.Shutdown();
        }

        private void ExitMessageBotton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            exitMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
        }

        private void AboutMessageBotton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            aboutMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
            AdditionalWindows.About about = new AdditionalWindows.About();
            about.Show();
        }

        private void AboutMessageBotton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            aboutMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
        }

        private void RequstsPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = RequestsPage;
        }

        private void RoutesPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = RoutesPage;
        }

        private void CompaniesPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = CompaniesPage;
        }

        private void ForwardersPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = ForwardersPage;
        }

        private void EngineersPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = EngineersPage;
        }

        private void UsersPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = UsersPage;
        }

        private void ServerPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //this.tcPages.SelectedItem = ServerPage;
        }
        #endregion

        #region ПОЛЬЗОВАТЕЛИ Обновление данных на странице Пользователи
        public void UpdateUsersData(List<ClassResource.User> users)
        {
            USERS = users;
            lvUsers.Items.Clear();
            foreach (ClassResource.User user in USERS)
            {
                lvUsers.Items.Add(new UserListItem(user.Login, user.Name, user.Role, ((user.Snapping == "") ? "Не определена" : (user.Engineer != "" ? user.Engineer + " (Инженер)" : user.Forwarder + " (Экспедитор)"))));
            }
            if(lvUsers.Items.Count > 0)
            {
                if (SELECTUSER != null)
                {
                    foreach (var item in lvUsers.Items)
                    {
                        if (((UserListItem)item).Login == SELECTUSER.Login)
                        {
                            lvUsers.SelectedItem = item;
                            break;
                        }
                    }
                    if(lvUsers.SelectedIndex == -1) lvUsers.SelectedIndex = 0;
                }
                else
                    lvUsers.SelectedIndex = 0;
            }
        }

        public void UpdateUsers(List<String> users)
        {
            cbUserSnappingInfo.Items.Clear();
            foreach (var item in ENGINEERS)
            {
                cbUserSnappingInfo.Items.Add(item.Name + "(Инженер)");
            }
            foreach (var item in FORWARDERS)
            {
                cbUserSnappingInfo.Items.Add(item.Name + "(Экспедитор)");
            }
        }
        #endregion

        #region ПОЛЬЗОВАТЕЛИ Обработка выбора пользователя
        private void LVUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvUsers.SelectedIndex >= 0)
            {
                SELECTUSER = USERS.Find(x => x.Login == ((UserListItem)lvUsers.SelectedItem).Login);
                bUserSaveChange.IsEnabled = false;
                bUserDelete.IsEnabled = true;
                tbUserLogin.Text = SELECTUSER.Login;
                tbUserName.Text = SELECTUSER.Name;
                tbUserSnapping.Text = SELECTUSER.Snapping;
                cbUserRole.SelectedItem = SELECTUSER.Role;
            }
            else
            {
                tbUserLogin.Text = "";
                tbUserName.Text = "";
                tbUserSnapping.Text = "";
                cbUserRole.SelectedIndex = -1;
                cbUserSnappingInfo.SelectedIndex = -1;
                bUserSaveChange.IsEnabled = false;
                bUserDelete.IsEnabled = false;
            }
        }
        #endregion

                
       

        #region ПОЛЬЗОВАТЕЛИ Реализация поиска среди пользователей
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbUserSearch.Text != "")
            {
                lvUsers.Items.Clear();
                foreach (ClassResource.User user in USERS)
                {
                    if(user.Login.IndexOf(tbUserSearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || IsInName(user.Name, tbUserSearch.Text) || user.Role.IndexOf(tbUserSearch.Text, StringComparison.OrdinalIgnoreCase) == 0)
                    lvUsers.Items.Add(new UserListItem(user.Login, user.Name, user.Role, ((user.Snapping == "") ? "Не определена" : (user.Engineer != "" ? user.Engineer + " (Инженер)" : user.Forwarder + " (Экспедитор)"))));
                }
                if (lvUsers.Items.Count > 0)
                {
                    lvUsers.SelectedIndex = 0;
                }
            }
            else
            {
                lvUsers.Items.Clear();
                foreach (ClassResource.User user in USERS)
                {
                    lvUsers.Items.Add(new UserListItem(user.Login, user.Name, user.Role, ((user.Snapping == "") ? "Не определена" : (user.Engineer != "" ? user.Engineer + " (Инженер)" : user.Forwarder + " (Экспедитор)"))));
                }
                if (lvUsers.Items.Count > 0)
                {
                    lvUsers.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ИНЖЕНЕРА Обновление данных на странице Инженера
        public void UpdateEngineersData(List<ClassResource.Engineer> engineers)
        {
            ENGINEERS = engineers;
            if (USERROLE != "Инженер")
            {
                lvEngineers.Items.Clear();
                foreach (ClassResource.Engineer engineer in ENGINEERS)
                {
                    lvEngineers.Items.Add(new EngineerListItem(engineer.ID, engineer.Name, engineer.ContactNumber));
                }
                if (lvEngineers.Items.Count > 0)
                {
                    if (SELECTENGINEER != null)
                        foreach (var item in lvEngineers.Items)
                        {
                            if (((EngineerListItem)item).ID == SELECTENGINEER.ID)
                            {
                                lvEngineers.SelectedItem = item;
                                break;
                            }
                        }
                    else
                        lvEngineers.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ИНЖЕНЕРА Обработка выбора инженера
        private void LVEngineers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvEngineers.SelectedIndex >= 0)
            {
                SELECTENGINEER = ENGINEERS.Find(x => x.ID == ((EngineerListItem)lvEngineers.SelectedItem).ID);
                if (USERROLE == "Администратор") bEngineerDelete.IsEnabled = true;
                bEngineerCopyID.IsEnabled = true;
                bEngineerSaveChange.IsEnabled = false;
                tbEngineerName.Text = SELECTENGINEER.Name;
                tbEngineerPhone.Text = SELECTENGINEER.ContactNumber;
                tbEngineerNote.Text = SELECTENGINEER.Note;
            }
            else
            {
                bEngineerDelete.IsEnabled = false;
                bEngineerCopyID.IsEnabled = false;
                bEngineerSaveChange.IsEnabled = false;
                tbEngineerName.Text = "";
                tbEngineerPhone.Text = "";
                tbEngineerNote.Text = "";
            }
        }
        #endregion

        #region ИНЖЕНЕРА Реализация поиска среди инженеров
        private void TBEngineerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbEngineerSearch.Text != "")
            {
                lvEngineers.Items.Clear();
                foreach (ClassResource.Engineer engineer in ENGINEERS)
                {
                    if (engineer.ID.IndexOf(tbEngineerSearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || IsInName(engineer.Name, tbEngineerSearch.Text) || engineer.ContactNumber.Contains(tbEngineerSearch.Text))
                        lvEngineers.Items.Add(new EngineerListItem(engineer.ID, engineer.Name, engineer.ContactNumber));
                }
                if (lvEngineers.Items.Count > 0)
                {
                    lvEngineers.SelectedIndex = 0;
                }
            }
            else
            {
                lvEngineers.Items.Clear();
                foreach (ClassResource.Engineer engineer in ENGINEERS)
                {
                    lvEngineers.Items.Add(new EngineerListItem(engineer.ID, engineer.Name, engineer.ContactNumber));
                }
                if (lvEngineers.Items.Count > 0)
                {
                    lvEngineers.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ЭКСПЕДИТОРЫ Обновление данных на странице Экспедиторы
        public void UpdateForwardersData(List<ClassResource.Forwarder> forwarders)
        {
            FORWARDERS = forwarders;
            lvForwarders.Items.Clear();
            foreach (ClassResource.Forwarder forwarder in FORWARDERS)
            {
                lvForwarders.Items.Add(new ForwarderListItem(forwarder.ID, forwarder.Name, forwarder.ContactNumber));
            }
            if (lvForwarders.Items.Count > 0)
            {
                if (SELECTFORWARDER != null)
                    foreach (var item in lvForwarders.Items)
                    {
                        if (((ForwarderListItem)item).ID == SELECTFORWARDER.ID)
                        {
                            lvForwarders.SelectedItem = item;
                            break;
                        }
                    }
                else
                    lvForwarders.SelectedIndex = 0;
            }

            cbRouteForwarder.Items.Clear();
            foreach (var item in FORWARDERS)
            {
                cbRouteForwarder.Items.Add(item.Name);
            }
        }
        #endregion

        #region ЭКСПЕДИТОРЫ Обработка выбора экспедитора
        private void LVForwarders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvForwarders.SelectedIndex >= 0)
            {
                SELECTFORWARDER = FORWARDERS.Find(x => x.ID == ((ForwarderListItem)lvForwarders.SelectedItem).ID);
                if (USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") bForwarderDelete.IsEnabled = true;
                if (USERROLE == "Администратор") bForwarderCopyID.IsEnabled = true;
                bForwarderSaveChange.IsEnabled = false;
                tbForwarderName.Text = SELECTFORWARDER.Name;
                tbForwarderPhone.Text = SELECTFORWARDER.ContactNumber;
                tbForwarderNote.Text = SELECTFORWARDER.Note;
            }
            else
            {
                bForwarderDelete.IsEnabled = false;
                bForwarderCopyID.IsEnabled = false;
                bForwarderSaveChange.IsEnabled = false;
                tbForwarderName.Text = "";
                tbForwarderPhone.Text = "";
                tbForwarderNote.Text = "";
            }
        }
        #endregion

        #region ЭКСПЕДИТОРЫ Реализация поиска среди экспедиторов
        private void TBForwarderSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbForwarderSearch.Text != "")
            {
                lvForwarders.Items.Clear();
                foreach (ClassResource.Forwarder forwarder in FORWARDERS)
                {
                    if(forwarder.ID.IndexOf(tbForwarderSearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || IsInName(forwarder.Name, tbForwarderSearch.Text) || forwarder.ContactNumber.Contains(tbForwarderSearch.Text))
                    lvForwarders.Items.Add(new ForwarderListItem(forwarder.ID, forwarder.Name, forwarder.ContactNumber));
                }
                if (lvForwarders.Items.Count > 0)
                {
                    lvForwarders.SelectedIndex = 0;
                }
            }
            else
            {
                lvForwarders.Items.Clear();
                foreach (ClassResource.Forwarder forwarder in FORWARDERS)
                {
                    lvForwarders.Items.Add(new ForwarderListItem(forwarder.ID, forwarder.Name, forwarder.ContactNumber));
                }
                if (lvForwarders.Items.Count > 0)
                {
                    lvForwarders.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ФИРМЫ Обновление данных на странице Фирмы
        public void UpdateCompaniesData(List<ClassResource.Company> companies)
        {
            COMPANIES = companies;
            lvCompanies.Items.Clear();
            foreach (ClassResource.Company company in COMPANIES)
            {
                lvCompanies.Items.Add(new CompanyListItem(company.ID, company.Name, company.Country, company.City, company.NameСontactPerson, company.PhoneContactPerson));
            }
            if (lvCompanies.Items.Count > 0)
            {
                if (SELECTCOMPANY != null)
                    foreach (var item in lvCompanies.Items)
                    {
                        if (((CompanyListItem)item).ID == SELECTCOMPANY.ID)
                        {
                            lvCompanies.SelectedItem = item;
                            break;
                        }
                    }
                else
                    lvCompanies.SelectedIndex = 0;
            }
        }
        #endregion

        #region ФИРМЫ Обработка выбора фирмы
        private void LVCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvCompanies.SelectedIndex >= 0)
            {
                SELECTCOMPANY = COMPANIES.Find(x => x.ID == ((CompanyListItem)lvCompanies.SelectedItem).ID);
                if (USERROLE == "Инженер" || USERROLE == "Администратор") bCompanyDelete.IsEnabled = true;
                bCompanyMap.IsEnabled = true;
                bCompanySaveChange.IsEnabled = false;
                tbCompanyName.Text = SELECTCOMPANY.Name;
                tbCompanyCountry.Text = SELECTCOMPANY.Country;
                tbCompanyCity.Text = SELECTCOMPANY.City;
                tbCompanyAddress.Text = SELECTCOMPANY.Address;
                tbCompanyContactName.Text = SELECTCOMPANY.NameСontactPerson;
                tbCompanyContactPhone.Text = SELECTCOMPANY.PhoneContactPerson;
            }
            else
            {
                bCompanyDelete.IsEnabled = false;
                bCompanyMap.IsEnabled = false;
                bCompanySaveChange.IsEnabled = false;
                tbCompanyName.Text = "";
                tbCompanyCountry.Text = "";
                tbCompanyCity.Text = "";
                tbCompanyAddress.Text = "";
                tbCompanyContactName.Text = "";
                tbCompanyContactPhone.Text = "";
            }
        }
        #endregion

        #region ФИРМЫ Реализация поиска среди фирм
        private void TBCompanySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCompanySearch.Text != "")
            {
                lvCompanies.Items.Clear();
                foreach (ClassResource.Company company in COMPANIES)
                {
                    if (company.Name.IndexOf(tbCompanySearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || company.Country.IndexOf(tbCompanySearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || company.City.IndexOf(tbCompanySearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || company.PhoneContactPerson.Contains(tbCompanySearch.Text) || IsInName(company.NameСontactPerson, tbCompanySearch.Text))
                        lvCompanies.Items.Add(new CompanyListItem(company.ID, company.Name, company.Country, company.City, company.NameСontactPerson, company.PhoneContactPerson));
                }
                if (lvCompanies.Items.Count > 0)
                {
                    lvCompanies.SelectedIndex = 0;
                }
            }
            else
            {
                lvCompanies.Items.Clear();
                foreach (ClassResource.Company company in COMPANIES)
                {
                    lvCompanies.Items.Add(new CompanyListItem(company.ID, company.Name, company.Country, company.City, company.NameСontactPerson, company.PhoneContactPerson));
                }
                if (lvCompanies.Items.Count > 0)
                {
                    lvCompanies.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ЗАЯВКИ Обновление данных на странице Заявки
        public void UpdateRequestsData(List<ClassResource.Request> requests)
        {
            REQUESTS = requests;
            lvRequests.Items.Clear();
            foreach (ClassResource.Request request in REQUESTS)
            {
                if(USERROLE != "Инженер" || request.IDEngineer == USERSNAPPING)
                    lvRequests.Items.Add(new RequestListItem(request.ID, request.Number, request.ProductName, request.ProductWeight, request.ProductDimensions, request.Quantity, COMPANIES.Find(x => x.ID == request.IDCompany).Name, request.Note));
            }

            if(ROUTES != null)
            {
                for (int i = 0; i < lvRequests.Items.Count; i++)
                {
                    RequestListItem temp = (RequestListItem)lvRequests.Items[i];
                    if (DESTINATIONS.Find(x => x.IDRequest == temp.ID) != null)
                    {
                        if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Открыт")
                        {
                            temp.Status = "Закреплена";
                        }
                        else if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Закрыт")
                        {
                            temp.Status = "Выполняется";
                        }
                        else if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Завершен")
                        {
                            temp.Status = "Выполнена";
                        }
                    }
                    else
                    {
                        temp.Status = "В обработке";
                    }
                    lvRequests.Items[i] = temp;
                    lvRequests.Items.Refresh();
                }
            }

            if (lvRequests.Items.Count > 0)
            {
                if (SELECTREQUEST != null)
                    foreach (var item in lvRequests.Items)
                    {
                        if(((RequestListItem)item).ID == SELECTREQUEST.ID)
                        {
                            lvRequests.SelectedItem = item;
                            break;
                        }
                    } 
                else
                    lvRequests.SelectedIndex = 0;
            }
        }
        #endregion

        #region ЗАЯВКИ Обработка выбора заявки
        private void LVRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvRequests.SelectedIndex >= 0)
            {
                SELECTREQUEST = REQUESTS.Find(x => x.ID == ((RequestListItem)lvRequests.SelectedItem).ID);
                if ((USERROLE == "Инженер" || USERROLE == "Администратор") && ((RequestListItem)lvRequests.SelectedItem).Status == "В обработке") bRequestDelete.IsEnabled = true;
                else bRequestDelete.IsEnabled = false;
                if ((USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") && ((RequestListItem)lvRequests.SelectedItem).Status == "В обработке") bRequestDistribute.IsEnabled = true;
                else bRequestDistribute.IsEnabled = false;
                bRequestOpen.IsEnabled = true;
                bRequestSaveChange.IsEnabled = false;
                tbRequestNumber.Text = SELECTREQUEST.Number;
                tbRequestProductName.Text = SELECTREQUEST.ProductName;
                tbRequestWeight.Text = SELECTREQUEST.ProductWeight;
                tbRequestDimensions.Text = SELECTREQUEST.ProductDimensions;
                tbRequestQuantity.Text = SELECTREQUEST.Quantity;
                tbRequestCompany.Text = COMPANIES.Find(x => x.ID == SELECTREQUEST.IDCompany).Name;
            }
            else
            {
                bRequestDelete.IsEnabled = false;
                bRequestDistribute.IsEnabled = false;
                bRequestOpen.IsEnabled = false;
                bRequestSaveChange.IsEnabled = false;
                tbRequestNumber.Text = "";
                tbRequestProductName.Text = "";
                tbRequestWeight.Text = "";
                tbRequestDimensions.Text = "";
                tbRequestQuantity.Text = "";
                tbRequestCompany.Text = "";
            }
        }
        #endregion

        #region ЗАЯВКИ Реализация поиска среди заявок
        private void TBRequestSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbRequestSearch.Text != "")
            {
                lvRequests.Items.Clear();
                foreach (ClassResource.Request request in REQUESTS)
                {
                    if (USERROLE != "Инженер" || request.IDEngineer == USERSNAPPING)
                        if (request.Number.IndexOf(tbRequestSearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || request.ProductName.Contains(tbRequestSearch.Text) || COMPANIES.Find(x => x.ID == request.IDCompany).Name.Contains(tbRequestSearch.Text))
                            lvRequests.Items.Add(new RequestListItem(request.ID, request.Number, request.ProductName, request.ProductWeight, request.ProductDimensions, request.Quantity, COMPANIES.Find(x => x.ID == request.IDCompany).Name, request.Note));
                }

                if (ROUTES != null)
                {
                    for (int i = 0; i < lvRequests.Items.Count; i++)
                    {
                        RequestListItem temp = (RequestListItem)lvRequests.Items[i];
                        if (DESTINATIONS.Find(x => x.IDRequest == temp.ID) != null)
                        {
                            if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Открыт")
                            {
                                temp.Status = "Закреплена";
                            }
                            else if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Закрыт")
                            {
                                temp.Status = "Выполняется";
                            }
                            else if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Завершен")
                            {
                                temp.Status = "Выполнена";
                            }
                        }
                        else
                        {
                            temp.Status = "В обработке";
                        }
                        lvRequests.Items[i] = temp;
                        lvRequests.Items.Refresh();
                    }
                }

                if (lvRequests.Items.Count > 0)
                {
                    lvRequests.SelectedIndex = 0;
                }
            }
            else
            {
                lvRequests.Items.Clear();
                foreach (ClassResource.Request request in REQUESTS)
                {
                    if (USERROLE != "Инженер" || request.IDEngineer == USERSNAPPING)
                        lvRequests.Items.Add(new RequestListItem(request.ID, request.Number, request.ProductName, request.ProductWeight, request.ProductDimensions, request.Quantity, COMPANIES.Find(x => x.ID == request.IDCompany).Name, request.Note));
                }

                if (ROUTES != null)
                {
                    for (int i = 0; i < lvRequests.Items.Count; i++)
                    {
                        RequestListItem temp = (RequestListItem)lvRequests.Items[i];
                        if (DESTINATIONS.Find(x => x.IDRequest == temp.ID) != null)
                        {
                            if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Открыт")
                            {
                                temp.Status = "Закреплена";
                            }
                            else if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Закрыт")
                            {
                                temp.Status = "Выполняется";
                            }
                            else if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Завершен")
                            {
                                temp.Status = "Выполнена";
                            }
                        }
                        else
                        {
                            temp.Status = "В обработке";
                        }
                        lvRequests.Items[i] = temp;
                        lvRequests.Items.Refresh();
                    }
                }

                if (lvRequests.Items.Count > 0)
                {
                    lvRequests.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ПУНКТЫ НАЗНАЧЕНИЯ Обновление данных на странице
        public void UpdateDestinationsData(List<ClassResource.Destination> destinations)
        {
            DESTINATIONS = destinations;
        }
        #endregion

        #region ПУНКТЫ НАЗНАЧЕНИЯ Обработка выбора пункта назначения
        private void LVDestinations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvDestinations.SelectedIndex >= 0)
            {
                SELECTDESTINATION = DESTINATIONS.Find(x => x.ID == ((DestinationListItem)lvDestinations.SelectedItem).ID);
                bDestinationSaveChange.IsEnabled = false;
                if ((USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") && SELECTROUTE.RouteStatus == "Открыт") bDestinationDelete.IsEnabled = true;
                else bDestinationDelete.IsEnabled = false;
                bDestinationOpenRequest.IsEnabled = true;
                tbDestinationCity.Text = COMPANIES.Find(x => x.ID == REQUESTS.Find(y => y.ID == SELECTDESTINATION.IDRequest).IDCompany).City;
                tbDestinationNote.Text = SELECTDESTINATION.Note;
                dpDestinationDate.Text = SELECTDESTINATION.ArrivalDate;
            }
            else
            {
                bDestinationSaveChange.IsEnabled = false;
                bDestinationDelete.IsEnabled = false;
                bDestinationOpenRequest.IsEnabled = false;
                tbDestinationCity.Text = "";
                tbDestinationNote.Text = "";
                dpDestinationDate.Text = "";
            }
        }
        #endregion

        #region МАРШРУТЫ Обновление данных на странице Маршруты
        public void UpdateRoutesData(List<ClassResource.Route> routes)
        {
            ROUTES = routes;
            lvRoutes.Items.Clear();
            foreach (ClassResource.Route route in ROUTES)
            {
                lvRoutes.Items.Add(new RouteListItem(route.ID, route.Name, route.RouteStatus));
            }
            if (lvRoutes.Items.Count > 0)
            {
                if (SELECTROUTE != null)
                    foreach (var item in lvRoutes.Items)
                    {
                        if (((RouteListItem)item).ID == SELECTROUTE.ID)
                        {
                            lvRoutes.SelectedItem = item;
                            break;
                        }
                    }
                else
                    lvRoutes.SelectedIndex = 0;
            }

            for (int i = 0; i < lvRequests.Items.Count; i++)
            {
                RequestListItem temp = (RequestListItem)lvRequests.Items[i];
                if (DESTINATIONS.Find(x=>x.IDRequest == temp.ID) != null)
                {
                    if(ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Открыт")
                    {
                        temp.Status = "Закреплена";
                    }
                    else if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Закрыт")
                    {
                        temp.Status = "Выполняется";
                    }
                    else if (ROUTES.Find(x => x.ID == DESTINATIONS.Find(y => y.IDRequest == temp.ID).IDRoute).RouteStatus == "Завершен")
                    {
                        temp.Status = "Выполнена";
                    }
                }
                else
                {
                    temp.Status = "В обработке";
                }
                lvRequests.Items[i] = temp;
                lvRequests.Items.Refresh();
            }

            if (lvRequests.Items.Count > 0)
            {
                if (SELECTREQUEST != null)
                    foreach (var item in lvRequests.Items)
                    {
                        if (((RequestListItem)item).ID == SELECTREQUEST.ID)
                        {
                            lvRequests.SelectedItem = item;
                            break;
                        }
                    }
                else
                    lvRequests.SelectedIndex = 0;
            }
        }
        #endregion

        #region МАРШРУТЫ Обработка выбора маршрута
        private void LVRoutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvRoutes.SelectedIndex >= 0)
            {
                SELECTROUTE = ROUTES.Find(x => x.ID == ((RouteListItem)lvRoutes.SelectedItem).ID);
                if (USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") bRouteAdd.IsEnabled = true;
                if (USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") bRouteDelete.IsEnabled = true;
                if ((USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") && SELECTROUTE.RouteStatus == "Открыт") bRouteClose.IsEnabled = true;
                else bRouteClose.IsEnabled = false;
                if ((USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") && SELECTROUTE.RouteStatus == "Закрыт") bRouteFullClose.IsEnabled = true;
                else bRouteFullClose.IsEnabled = false;
                bRouteSaveChange.IsEnabled = false;
                tbRouteNumber.Text = SELECTROUTE.Name;
                tbRouteNote.Text = SELECTROUTE.Note;
                dpRouteDepartureDate.Text = SELECTROUTE.DepartureDate;
                dpRouteReturnDate.Text = SELECTROUTE.ReturnDate;
                cbRouteCar.Text = SELECTROUTE.CarType;
                if (SELECTROUTE.IDForwarder != "") cbRouteForwarder.SelectedItem = FORWARDERS.Find(x => x.ID == SELECTROUTE.IDForwarder).Name;
                else cbRouteForwarder.Text = "";
                tbRouteRoute.Text = SELECTROUTE.CityCountryDeparture;
                lvDestinations.Items.Clear();
                List<ClassResource.Destination> temp = DESTINATIONS.FindAll(x => x.IDRoute == SELECTROUTE.ID);
                temp.Sort((a, b) => (a.Number.CompareTo(b.Number)));
                foreach (var item in temp)
                {
                    if(tbRouteRoute.Text.LastIndexOf(COMPANIES.Find(x => x.ID == REQUESTS.Find(y => y.ID == item.IDRequest).IDCompany).City) != tbRouteRoute.Text.Length - COMPANIES.Find(x => x.ID == REQUESTS.Find(y => y.ID == item.IDRequest).IDCompany).City.Length)
                        tbRouteRoute.Text += " - " + COMPANIES.Find(x => x.ID == REQUESTS.Find(y => y.ID == item.IDRequest).IDCompany).City;
                    lvDestinations.Items.Add(new DestinationListItem(item.ID, item.Number, REQUESTS.Find(y => y.ID == item.IDRequest).Number, COMPANIES.Find(x => x.ID == REQUESTS.Find(y => y.ID == item.IDRequest).IDCompany).City + "(" + COMPANIES.Find(x => x.ID == REQUESTS.Find(y => y.ID == item.IDRequest).IDCompany).Country + ")", (item.ArrivalDate != "" ? item.ArrivalDate.Substring(0, 10) : "")));
                }
                tbRouteRoute.Text += " - " + SELECTROUTE.CityCountryDeparture;
                if (lvDestinations.Items.Count > 0)
                {
                    if (SELECTDESTINATION != null)
                        foreach (var item in lvDestinations.Items)
                        {
                            if (((DestinationListItem)item).ID == SELECTDESTINATION.ID)
                            {
                                lvDestinations.SelectedItem = item;
                                break;
                            }
                        }
                    else
                        lvDestinations.SelectedIndex = 0;
                }
            }
            else
            {
                bRouteAdd.IsEnabled = false;
                bRouteDelete.IsEnabled = false;
                bRouteClose.IsEnabled = false;
                bRouteSaveChange.IsEnabled = false;
                bRouteFullClose.IsEnabled = false;
                tbRouteNumber.Text = "";
                tbRouteNote.Text = "";
                dpRouteDepartureDate.Text = "";
                dpRouteReturnDate.Text = "";
                cbRouteCar.SelectedItem = "";
                cbRouteForwarder.SelectedItem = "";
                tbRouteRoute.Text = "";
                lvDestinations.Items.Clear();
            }
        }
        #endregion

        #region Классы для ListView
        class UserListItem
        {
            public String Login { get; set; }
            public String Name { get; set; }
            public String Role { get; set; }
            public String Snapping { get; set; }

            public UserListItem(String login, String name, String role, String snapping)
            {
                this.Login = login;
                this.Name = name;
                this.Role = role;
                this.Snapping = snapping;
            }
        }

        class EngineerListItem
        {
            public String ID { get; set; }
            public String Name { get; set; }
            public String СontactNumber { get; set; }

            public EngineerListItem(String id, String name, String contactNumber)
            {
                this.ID = id;
                this.Name = name;
                this.СontactNumber = contactNumber;
            }
        }


        class ForwarderListItem
        {
            public String ID { get; set; }
            public String Name { get; set; }
            public String СontactNumber { get; set; }

            public ForwarderListItem(String id, String name, String contactNumber)
            {
                this.ID = id;
                this.Name = name;
                this.СontactNumber = contactNumber;
            }
        }

        class CompanyListItem
        {
            public String ID { get; set; }
            public String Name { get; set; }
            public String Country { get; set; }
            public String City { get; set; }
            public String NameСontactPerson { get; set; }
            public String PhoneContactPerson { get; set; }

            public CompanyListItem(String id, String name, String country, String city, String nameСontactPerson, String phoneContactPerson)
            {
                this.ID = id;
                this.Name = name;
                this.Country = country;
                this.City = city;
                this.NameСontactPerson = nameСontactPerson;
                this.PhoneContactPerson = phoneContactPerson;
            }
        }

        class RequestListItem
        {
            public String ID { get; set; }
            public String Number { get; set; }
            public String ProductName { get; set; }
            public String ProductWeight { get; set; }
            public String ProductDimensions { get; set; }
            public String Quantity { get; set; }
            public String Company { get; set; }
            public String Note { get; set; }
            public String Status { get; set; }

            public RequestListItem(String id, String number, String productName, String productWeight, String productDimensions, String quantity, String company, String note)
            {
                this.ID = id;
                this.Number = number;
                this.ProductName = productName;
                this.ProductWeight = productWeight;
                this.ProductDimensions = productDimensions;
                this.Quantity = quantity;
                this.Company = company;
                this.Note = note;
            }

        }

        class RouteListItem
        {
            public String ID { get; set; }
            public String Number { get; set; }
            public String Status { get; set; }

            public RouteListItem(String id, String number, String status)
            {
                this.ID = id;
                this.Number = number;
                this.Status = status;
            }
        }

        class DestinationListItem
        {
            public String ID { get; set; }
            public String Number { get; set; }
            public String Request { get; set; }
            public String Address { get; set; }
            public String Date { get; set; }

            public DestinationListItem(String id, String number, String request, String address, String date)
            {
                this.ID = id;
                this.Number = number;
                this.Address = address;
                this.Date = date;
                this.Request = request;
            }
        }


        private bool IsInName(String name, String str)
        {
            foreach (String item in name.Split(' '))
            {
                if (item.IndexOf(str, StringComparison.OrdinalIgnoreCase) == 0) return true;
            }
            return false;
        }
        #endregion

        private void BRequestAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ENGINEERS.Find(x => x.ID == USERSNAPPING) != null)
            {
                AdditionalWindows.Request request = new AdditionalWindows.Request(COMPANIES, USERNAME, ENGINEERS.Find(x => x.ID == USERSNAPPING).ContactNumber);
                request.Show();
            }
            else
            {
                Dialogs.Dialog.ShowWarming("Вы не имеете прав на данное действие.", "Обратитесь к Администратору для получения соответствующих прав.", "Предупреждение: ошибка доступа");
            }
        }

        private void BRequestOpen_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Request request = new AdditionalWindows.Request(COMPANIES, SELECTREQUEST, ENGINEERS.Find(x => x.ID == SELECTREQUEST.IDEngineer).Name, ENGINEERS.Find(x => x.ID == SELECTREQUEST.IDEngineer).ContactNumber);
            request.Show();
        }

        private void BCompanyMap_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Map map = new AdditionalWindows.Map(SELECTCOMPANY.Address);
            map.Show();
        }

        private void BCompanyDelete_Click(object sender, RoutedEventArgs e)
        {
            if(Dialogs.Dialog.ShowYesNoDialog("Вы действительно хотите удалить эту фирму из базы?", "После удаления восстановление невозможно.", "Удаление записи"))
            {
                Sources.Client.SendMessage("DeleteCompany", new String[] { SELECTCOMPANY.ID });
                SELECTCOMPANY = null;
            }
        }

        private void BCompanyAdd_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Company company = new AdditionalWindows.Company();
            company.Show();
        }

        #region IsCompanyDataChange
        private void IsCompanyDataChange()
        {
            if((USERROLE == "Администратор" || USERROLE == "Инженер") && SELECTCOMPANY != null)
            {
                if(tbCompanyName.Text != SELECTCOMPANY.Name || 
                    tbCompanyAddress.Text != SELECTCOMPANY.Address || 
                    tbCompanyCity.Text != SELECTCOMPANY.City || 
                    tbCompanyCountry.Text != SELECTCOMPANY.Country || 
                    tbCompanyContactName.Text != SELECTCOMPANY.NameСontactPerson || 
                    tbCompanyContactPhone.Text != SELECTCOMPANY.PhoneContactPerson)
                {
                    bCompanySaveChange.IsEnabled = true;
                }
                else
                {
                    bCompanySaveChange.IsEnabled = false;
                }
            }
        }

        private void TBCompanyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsCompanyDataChange();
        }

        private void TBCompanyAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsCompanyDataChange();
        }

        private void TBCompanyContactName_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsCompanyDataChange();
        }

        private void TBCompanyCountry_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsCompanyDataChange();
        }

        private void TBCompanyCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsCompanyDataChange();
        }

        private void TBCompanyContactPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsCompanyDataChange();
        }

        #endregion

        private void BCompanySaveChange_Click(object sender, RoutedEventArgs e)
        {
            if(tbCompanyName.Text == "" || tbCompanyAddress.Text == "" || tbCompanyContactName.Text == "" || tbCompanyContactPhone.Text == "" || tbCompanyCountry.Text == "" || tbCompanyCity.Text == "")
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Все поля, отмеченные *, должны быть заполнены.", "Некорректное заполнение");
                return;
            }

            ClassResource.Company company = new ClassResource.Company(SELECTCOMPANY.ID, tbCompanyName.Text, tbCompanyCountry.Text, tbCompanyCity.Text, tbCompanyAddress.Text, tbCompanyContactName.Text, tbCompanyContactPhone.Text);
            Sources.Client.SendMessage("UpdateCompany", new String[] { JsonConvert.SerializeObject(company) });
            bCompanySaveChange.IsEnabled = false;
        }

        #region IsForwarderDataChange
        private void IsForwarderDataChange()
        {
            if ((USERROLE == "Администратор" || USERROLE == "Руководитель экспедиторов") && SELECTFORWARDER != null)
            {
                if (tbForwarderName.Text != SELECTFORWARDER.Name ||
                    tbForwarderPhone.Text != SELECTFORWARDER.ContactNumber ||
                    tbForwarderNote.Text != SELECTFORWARDER.Note)
                {
                    bForwarderSaveChange.IsEnabled = true;
                }
                else
                {
                    bForwarderSaveChange.IsEnabled = false;
                }
            }
        }

        private void TBForwarderName_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsForwarderDataChange();
        }

        private void TBForwarderPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsForwarderDataChange();
        }

        private void TBForwarderNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsForwarderDataChange();
        }

        #endregion

        private void BForwarderSaveChange_Click(object sender, RoutedEventArgs e)
        {
            if (tbForwarderName.Text == "" || tbForwarderPhone.Text == "")
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Все поля, отмеченные *, должны быть заполнены.", "Некорректное заполнение");
                return;
            }

            ClassResource.Forwarder forwarder = new ClassResource.Forwarder(SELECTFORWARDER.ID, tbForwarderName.Text, tbForwarderPhone.Text, tbForwarderNote.Text);
            Sources.Client.SendMessage("UpdateForwarder", new String[] { JsonConvert.SerializeObject(forwarder) });
            bForwarderSaveChange.IsEnabled = false;
        }

        private void BForwarderDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Dialogs.Dialog.ShowYesNoDialog("Вы действительно хотите удалить этого экспедитора из базы?", "После удаления восстановление невозможно.", "Удаление записи"))
            {
                Sources.Client.SendMessage("DeleteForwarder", new String[] { SELECTFORWARDER.ID });
                SELECTFORWARDER = null;
            }
        }

        private void BForwarderCopyID_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)SELECTFORWARDER.ID);
        }

        private void BForwarderAdd_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.ForwarderWindow forwarder = new AdditionalWindows.ForwarderWindow();
            forwarder.Show();
        }

        #region IsEngineerDataChange
        private void IsEngineerDataChange()
        {
            if (USERROLE == "Администратор" && SELECTENGINEER != null)
            {
                if (tbEngineerName.Text != SELECTENGINEER.Name ||
                    tbEngineerPhone.Text != SELECTENGINEER.ContactNumber ||
                    tbEngineerNote.Text != SELECTENGINEER.Note)
                {
                    bEngineerSaveChange.IsEnabled = true;
                }
                else
                {
                    bEngineerSaveChange.IsEnabled = false;
                }
            }
        }

        private void TBEngineerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsEngineerDataChange();
        }

        private void TBEngineerPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsEngineerDataChange();
        }

        private void TBEngineerNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsEngineerDataChange();
        }

        #endregion

        private void BEngineerAdd_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Engineer engineer = new AdditionalWindows.Engineer();
            engineer.Show();
        }

        private void BEngineerSaveChange_Click(object sender, RoutedEventArgs e)
        {
            if (tbEngineerName.Text == "" || tbEngineerPhone.Text == "")
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Все поля, отмеченные *, должны быть заполнены.", "Некорректное заполнение");
                return;
            }

            ClassResource.Engineer engineer = new ClassResource.Engineer(SELECTENGINEER.ID, tbEngineerName.Text, tbEngineerPhone.Text, tbEngineerNote.Text);
            Sources.Client.SendMessage("UpdateEngineer", new String[] { JsonConvert.SerializeObject(engineer) });
            bForwarderSaveChange.IsEnabled = false;
        }

        private void BEngineerCopyID_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, (Object)SELECTENGINEER.ID);
        }

        private void BEngineerDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Dialogs.Dialog.ShowYesNoDialog("Вы действительно хотите удалить этого инженера из базы?", "После удаления восстановление невозможно.", "Удаление записи"))
            {
                Sources.Client.SendMessage("DeleteEngineer", new String[] { SELECTENGINEER.ID });
                SELECTENGINEER = null;
            }
        }

        private void BUserAdd_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.User user = new AdditionalWindows.User(ENGINEERS, FORWARDERS, USERS);
            user.Show();
        }


        #region IsUserDataChange

        Regex regexLogin = new Regex(@"^[A-Za-z][A-Za-z0-9_-]{2,}$");
        Regex regexPassword = new Regex(@"^[A-Za-z0-9_-]{4,}$");

        private void IsUserDataChange()
        {
            if (USERROLE == "Администратор" && SELECTUSER != null && cbUserRole.SelectedItem != null)
            {
                if (tbUserLogin.Text != SELECTUSER.Login ||
                    tbUserName.Text != SELECTUSER.Name ||
                    tbUserSnapping.Text != SELECTUSER.Snapping ||
                    cbUserRole.SelectedItem.ToString() != SELECTUSER.Role)
                {
                    bUserSaveChange.IsEnabled = true;
                }
                else
                {
                    bUserSaveChange.IsEnabled = false;
                }
            }
        }
        private void TBUserLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsUserDataChange();
        }

        private void TBUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsUserDataChange();
        }

        private void TBUserSnapping_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsUserDataChange();
            foreach (var item in ENGINEERS)
            {
                if (item.ID == tbUserSnapping.Text)
                {
                    cbUserSnappingInfo.SelectedItem = item.Name + "(Инженер)";
                    return;
                }
            }
            foreach (var item in FORWARDERS)
            {
                if (item.ID == tbUserSnapping.Text)
                {
                    cbUserSnappingInfo.SelectedItem = item.Name + "(Экспедитор)";
                    return;
                }
            }
            cbUserSnappingInfo.SelectedIndex = -1;
            cbUserSnappingInfo.Text = "";
        }

        private void CBUserRole_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsUserDataChange();
        }

        private void CBUserSnappingInfo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbUserSnappingInfo.SelectedIndex >= 0)
            {
                if (cbUserSnappingInfo.SelectedIndex >= ENGINEERS.Count)
                {
                    if(tbUserSnapping.Text != FORWARDERS[cbUserSnappingInfo.SelectedIndex - ENGINEERS.Count].ID)
                        tbUserSnapping.Text = FORWARDERS[cbUserSnappingInfo.SelectedIndex - ENGINEERS.Count].ID;
                }
                else
                {
                    if(tbUserSnapping.Text != ENGINEERS[cbUserSnappingInfo.SelectedIndex].ID)
                        tbUserSnapping.Text = ENGINEERS[cbUserSnappingInfo.SelectedIndex].ID;
                }
            }
        }
        #endregion

        private void BUserSaveChange_Click(object sender, RoutedEventArgs e)
        {
            if (tbUserLogin.Text == "" || tbUserName.Text == "" || cbUserRole.SelectedIndex < 0)
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Все поля, отмеченные *, должны быть заполнены.", "Некорректное заполнение");
                return;
            }
            if (!regexLogin.IsMatch(tbUserLogin.Text))
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Логин введен некорректно.", "Некорректное заполнение");
                return;
            }
            if (USERS.Find(x => x.Login == tbUserLogin.Text) != null && tbUserLogin.Text != SELECTUSER.Login)
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Пользователь с таким логином уже существует.", "Некорректное заполнение");
                return;
            }
            if (cbUserSnappingInfo.SelectedIndex < 0 && cbUserRole.SelectedItem.ToString() == "Инженер")
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Для инженера обязательна ссылка.", "Некорректное заполнение");
                return;
            }

            ClassResource.User user = new ClassResource.User(tbUserLogin.Text, tbUserName.Text, cbUserRole.SelectedItem.ToString(), tbUserSnapping.Text, SELECTUSER.Login, null, null);
            Sources.Client.SendMessage("UpdateUser", new String[] { JsonConvert.SerializeObject(user) });
            bUserSaveChange.IsEnabled = false;
        }

        private void BUserDelete_Click(object sender, RoutedEventArgs e)
        {
            if (SELECTUSER.Login == USERLOGIN)
            {
                Dialogs.Dialog.ShowWarming("Удаление невозможно!", "Вы не можете удалить себя.", "Удаление невозможно");
                return;
            }
            if (Dialogs.Dialog.ShowYesNoDialog("Вы действительно хотите удалить этого пользователя из базы?", "После удаления восстановление невозможно.", "Удаление записи"))
            {
                Sources.Client.SendMessage("DeleteUser", new String[] { SELECTUSER.Login });
                SELECTUSER = null;
            }
        }

        private void BRouteAdd_Click(object sender, RoutedEventArgs e)
        {
            Sources.Client.SendMessage("AddRoute", new String[] {  });
        }

        private void BRouteDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Dialogs.Dialog.ShowYesNoDialog("Вы действительно хотите удалить этот маршрут из базы?", "После удаления восстановление невозможно.", "Удаление записи"))
            {
                Sources.Client.SendMessage("DeleteRoute", new String[] { SELECTROUTE.ID });
                SELECTROUTE = null;
            }
        }

        private void BDestinationOpenRequest_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Request request = new AdditionalWindows.Request(COMPANIES, REQUESTS.Find(x => x.ID == SELECTDESTINATION.IDRequest), ENGINEERS.Find(x => x.ID == REQUESTS.Find(y => y.ID == SELECTDESTINATION.IDRequest).IDEngineer).Name, ENGINEERS.Find(x => x.ID == REQUESTS.Find(y => y.ID == SELECTDESTINATION.IDRequest).IDEngineer).ContactNumber);
            request.Show();
        }

        #region IsRouteDataChange
        private void IsRouteDataChange()
        {
            if ((USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") && SELECTROUTE != null)
            {
                if ((tbRouteNumber.Text != SELECTROUTE.Name ||
                    tbRouteNote.Text != SELECTROUTE.Note ||
                    dpRouteDepartureDate.Text != (SELECTROUTE.DepartureDate != "" ? SELECTROUTE.DepartureDate.Substring(0, 10) : "") ||
                    dpRouteReturnDate.Text != (SELECTROUTE.ReturnDate != "" ? SELECTROUTE.ReturnDate.Substring(0, 10) : "") ||
                    cbRouteCar.Text != SELECTROUTE.CarType ||
                    (SELECTROUTE.IDForwarder != "" ? (cbRouteForwarder.Text != FORWARDERS.Find(x => x.ID == SELECTROUTE.IDForwarder).Name) : cbRouteForwarder.Text != "")) &&
                    SELECTROUTE.RouteStatus == "Открыт")
                {
                    bRouteSaveChange.IsEnabled = true;
                }
                else
                {
                    bRouteSaveChange.IsEnabled = false;
                }
            }
        }

        private void TBRouteNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsRouteDataChange();
        }

        private void TBRouteRoute_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsRouteDataChange();
        }

        private void CBRouteForwarder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsRouteDataChange();
        }

        private void CBRouteCar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsRouteDataChange();
        }

        private void TBRouteNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsRouteDataChange();
        }

        private void DPRouteDepartureDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            IsRouteDataChange();
        }

        private void DPRouteReturnDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            IsRouteDataChange();
        }
        #endregion

        private void BRouteSaveChange_Click(object sender, RoutedEventArgs e)
        {
            if (tbRouteNumber.Text == "")
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Все поля, отмеченные *, должны быть заполнены.", "Некорректное заполнение");
                return;
            }

            ClassResource.Route route = new ClassResource.Route(SELECTROUTE.ID, tbRouteNumber.Text, dpRouteDepartureDate.Text, cbRouteCar.Text, dpRouteReturnDate.Text, null, null, tbRouteNote.Text, cbRouteForwarder.SelectedIndex >= 0 ? FORWARDERS[cbRouteForwarder.SelectedIndex].ID : null);
            Sources.Client.SendMessage("UpdateRoute", new String[] { JsonConvert.SerializeObject(route) });
            bRouteSaveChange.IsEnabled = false;
        }

        private void BRouteClose_Click(object sender, RoutedEventArgs e)
        {
            if (Dialogs.Dialog.ShowYesNoDialog("Вы действительно хотите закрыть этот маршрут?", "После закрытия восстановление невозможно.", "Закрытие маршрута"))
            {
                Sources.Client.SendMessage("ChangeRouteStatus", new String[] { "Закрыт", SELECTROUTE.ID });
            }
        }

        private void BRouteFullClose_Click(object sender, RoutedEventArgs e)
        {
            if (Dialogs.Dialog.ShowYesNoDialog("Вы действительно хотите завершить этот маршрут?", "После завершения восстановление невозможно.", "Завершение маршрута"))
            {
                Sources.Client.SendMessage("ChangeRouteStatus", new String[] { "Завершен", SELECTROUTE.ID });
            }
        }

        private void BDestinationSaveChange_Click(object sender, RoutedEventArgs e)
        {
            ClassResource.Destination destination = new ClassResource.Destination(SELECTDESTINATION.ID, dpDestinationDate.Text, tbDestinationNote.Text, null, null, null);
            Sources.Client.SendMessage("UpdateDestination", new String[] { JsonConvert.SerializeObject(destination) });
            bDestinationSaveChange.IsEnabled = false;
        }

        #region IsDestinationDataChange
        private void IsDestinationDataChange()
        {
            if ((USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") && SELECTDESTINATION != null)
            {
                if ((dpDestinationDate.Text != (SELECTDESTINATION.ArrivalDate != "" ? SELECTDESTINATION.ArrivalDate.Substring(0, 10) : "") ||
                    tbDestinationNote.Text != SELECTDESTINATION.Note) &&
                    SELECTROUTE.RouteStatus == "Открыт")
                {
                    bDestinationSaveChange.IsEnabled = true;
                }
                else
                {
                    bDestinationSaveChange.IsEnabled = false;
                }
            }
        }

        private void DPDestinationDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            IsDestinationDataChange();
        }

        private void TBDestinationNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsDestinationDataChange();
        }
        #endregion

        private void BDestinationUp_Click(object sender, RoutedEventArgs e)
        {
            if((USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") && lvDestinations.SelectedIndex > 0 && SELECTROUTE.RouteStatus == "Открыт")
            {
                int number = Int32.Parse(SELECTDESTINATION.Number);
                Sources.Client.SendMessage("ChangeDestinationNumber", new String[] { number.ToString(), ((DestinationListItem)lvDestinations.Items[lvDestinations.SelectedIndex - 1]).ID });
                Sources.Client.SendMessage("ChangeDestinationNumber", new String[] { (number - 1).ToString(), SELECTDESTINATION.ID });
            }
        }

        private void BDestinationDown_Click(object sender, RoutedEventArgs e)
        {
            if ((USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") && lvDestinations.SelectedIndex >= 0 && lvDestinations.SelectedIndex < lvDestinations.Items.Count - 1) // && SELECTROUTE.RouteStatus == "Открыт")
            {
                int number = Int32.Parse(SELECTDESTINATION.Number);
                Sources.Client.SendMessage("ChangeDestinationNumber", new String[] { number.ToString(), ((DestinationListItem)lvDestinations.Items[lvDestinations.SelectedIndex + 1]).ID });
                Sources.Client.SendMessage("ChangeDestinationNumber", new String[] { (number + 1).ToString(), SELECTDESTINATION.ID });
            }
        }

        private void BDestinationDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((USERROLE == "Руководитель экспедиторов" || USERROLE == "Администратор") && SELECTROUTE.RouteStatus == "Открыт")
            {
                if (Dialogs.Dialog.ShowYesNoDialog("Вы действительно хотите удалить этот пункт назначения из базы?", "После удаления восстановление невозможно.", "Удаление записи"))
                {
                    for (int i = lvDestinations.SelectedIndex + 1; i < lvDestinations.Items.Count; i++)
                        Sources.Client.SendMessage("ChangeDestinationNumber", new String[] { i.ToString(), ((DestinationListItem)lvDestinations.Items[i]).ID });

                    Sources.Client.SendMessage("DeleteDestination", new String[] { SELECTDESTINATION.ID });
                    SELECTDESTINATION = null;
                }
            }
        }

        private void BRequestDistribute_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Destination destination = new AdditionalWindows.Destination(ROUTES, SELECTREQUEST.ID, SELECTREQUEST.Number);
            destination.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Sources.WorkWithExcel.ForwardersOnBusinessTrips(FORWARDERS, ROUTES, DESTINATIONS, COMPANIES, REQUESTS);
        }

        private void BStatusRequest_Click(object sender, RoutedEventArgs e)
        {
            if(USERROLE == "Инженер")
            {
                Sources.WorkWithExcel.StatusRequest(ROUTES, DESTINATIONS, REQUESTS.FindAll(x => x.IDEngineer == USERSNAPPING));
            }
            else
            {
                Sources.WorkWithExcel.StatusRequest(ROUTES, DESTINATIONS, REQUESTS);
            }
        }

        private void BReportDestination_Click(object sender, RoutedEventArgs e)
        {
            if (lvDestinations.Items.Count > 0)
            {
                Sources.WorkWithExcel.ReportDestination(DESTINATIONS.FindAll(x => x.IDRoute == SELECTROUTE.ID), REQUESTS, COMPANIES);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Report report = new AdditionalWindows.Report(FORWARDERS.FindAll(x => ROUTES.Find(y => y.IDForwarder == x.ID && y.RouteStatus != "Открыт") != null), ROUTES, DESTINATIONS, COMPANIES, REQUESTS);
            report.Show();
        }

        private void BRequestDelete_Click(object sender, RoutedEventArgs e)
        {
            if (Dialogs.Dialog.ShowYesNoDialog("Вы действительно хотите удалить эту заявку из базы?", "После удаления восстановление невозможно.", "Удаление записи"))
            {
                Sources.Client.SendMessage("DeleteRequest", new String[] { SELECTREQUEST.ID });
                SELECTREQUEST = null;
            }
        }

        private void BRequestSaveChange_Click(object sender, RoutedEventArgs e)
        {
            if (tbRequestDimensions.Text == "" || tbRequestWeight.Text == "" || tbRequestQuantity.Text == "")
            {
                Dialogs.Dialog.ShowWarming("Некорректное заполнение!", "Все поля, отмеченные *, должны быть заполнены.", "Некорректное заполнение");
                return;
            }

            ClassResource.Request request = new ClassResource.Request(SELECTREQUEST.ID, null, null, tbRequestWeight.Text, tbRequestDimensions.Text, tbRequestQuantity.Text, null, null, null, null);
            Sources.Client.SendMessage("UpdateRequest", new String[] { JsonConvert.SerializeObject(request) });
            bRequestSaveChange.IsEnabled = false;
        }


        #region IsRequestDataChange
        private void IsRequestDataChange()
        {
            if ((USERROLE == "Инженер" || USERROLE == "Администратор") && SELECTREQUEST != null && lvRequests.SelectedItem != null)
            {
                if ((tbRequestDimensions.Text != SELECTREQUEST.ProductDimensions ||
                    tbRequestWeight.Text != SELECTREQUEST.ProductWeight ||
                    tbRequestQuantity.Text != SELECTREQUEST.Quantity) && ((RequestListItem)lvRequests.SelectedItem).Status == "В обработке")
                {
                    bRequestSaveChange.IsEnabled = true;
                }
                else
                {
                    bRequestSaveChange.IsEnabled = false;
                }
            }
        }
        private void TBRequestWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsRequestDataChange();
        }

        private void TBRequestDimensions_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsRequestDataChange();
        }

        private void TbRequestQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsRequestDataChange();
        }
        #endregion
    }
}
